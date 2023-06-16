using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour {
    
    [Header("OUTSIDE")]
    [SerializeField] Collider2D col; public Collider2D Col {get => col; set => col = value;}
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}
    const int FRONT_Z = -1;
    const int REVERSE_Y = -1;
    const float WALK_STOP_VAL = 0.05f;
    const float MIN_Y = -4.5f;
    const float MAX_Y = 2.0f;

    [Header("VALUE")]
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr;}
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos; public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    [SerializeField] bool doIdle;   public bool DoIdle {get => doIdle; set => doIdle = value;}
    [SerializeField] bool doWalk;   public bool DoWalk {get => doWalk; set => doWalk = value;}
    Transform tf;

    void Start() {
        tf = transform;
        tgPos = transform.position;
        col = GetComponent<Collider2D>();
        sprLib = GetComponent<SpriteLibrary>();
        // sprLib.spriteLibraryAsset = DB.Dt.PlSkins[0].SprLibraryAsset;
    }

    void Update() {
        
        if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;
        if(HM._.state != HM.STATE.NORMAL) return;

        //* レイヤー
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            //* プレイヤー方向
            Vector2 dir = new Vector2(tgPos.x - tf.position.x, tgPos.y - tf.position.y).normalized;
            bool isLeft = ((dir.x < 0 && 0 <= dir.y) || (dir.x < 0 && dir.y < 0));
            float flipX = (isLeft)? -1 : 1;
            tf.localScale = new Vector2(flipX, tf.localScale.y);

            //* 追いかける & アニメー
            float distX = Mathf.Abs(tgPos.x - tf.position.x);
            float distY = Mathf.Abs(tgPos.y - tf.position.y);
            bool isMoving = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;
            Debug.Log($"setPosAndAnim:: isMoving= {isMoving}");

            if(isMoving) {setIdle();}
            else {setWalk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setInitSpr() => sr.sprite = idleSpr;
    public void setIdle() {
        tf.position = new Vector3(tgPos.x, tgPos.y, FRONT_Z);
        if(!doIdle) { doIdle = true; anim.SetTrigger(Enum.ANIM.DoIdle.ToString());}
        if(doWalk) doWalk = false;
    }
    private void setWalk() {
        tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
        tf.position = new Vector3(tf.position.x, Mathf.Clamp(tf.position.y, MIN_Y, MAX_Y), FRONT_Z);
        if(doIdle) doIdle = false;
        if(!doWalk) { doWalk = true; anim.SetTrigger(Enum.ANIM.DoWalk.ToString());}
    }
#endregion
///------------------------------------------------------------------------------------------
#region COLLIDER
///------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.GoGame.ToString())) {
            HM._.ui.GoGameDialog.SetActive(true);
        }
    }
#endregion
}
