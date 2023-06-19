using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour {
    const int FRONT_Z = -1;
    const int REVERSE_Y = -1;
    const float WALK_STOP_VAL = 0.05f;
    const float MIN_Y = -4.5f;
    const float MAX_Y = 2.0f;

    [Header("OUTSIDE")]
    [SerializeField] Collider2D col; public Collider2D Col {get => col; set => col = value;}
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}

    [Header("VALUE")]
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr;}
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos; public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    [SerializeField] bool isSitTrigger; public bool IsSitTrigger {get => isSitTrigger; set => isSitTrigger = value;}
    [SerializeField] bool isSit; public bool IsSit {get => isSit; set => isSit = value;}
    [SerializeField] GameObject colChairObj;    public GameObject ColChairObj {get => colChairObj;}

    Transform tf;

    void Start() {
        tf = transform;
        tgPos = transform.position;
        col = GetComponent<Collider2D>();
        sprLib = GetComponent<SpriteLibrary>();
    }

    void Update() {
        if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;
        if(HM._.state != HM.STATE.NORMAL) return;
        if(isSit) return;

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
            bool isWalkStop = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;

            if(isWalkStop) {setIdle();}
            else {setWalk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setInitSpr() => sr.sprite = idleSpr;

    public void setIdle() {
        tf.position = new Vector3(tgPos.x, tgPos.y, FRONT_Z);
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
    }

    public void setWalk() {
        tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
        tf.position = new Vector3(tf.position.x, Mathf.Clamp(tf.position.y, MIN_Y, MAX_Y), FRONT_Z);
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), true);
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

    private void OnTriggerStay2D(Collider2D col) {
        //* Sit Trigger ON
        if(col.CompareTag(Enum.TAG.Funiture.ToString()) 
        && col.gameObject.layer == LayerMask.NameToLayer(Enum.LAYER.Chair.ToString())) {
            Debug.Log("Player:: isSitTrigger ON");
            isSitTrigger = true;
            colChairObj = col.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        //* Sit Trigger OFF
        if(col.CompareTag(Enum.TAG.Funiture.ToString()) 
        && col.gameObject.layer == LayerMask.NameToLayer(Enum.LAYER.Chair.ToString())) {
            Debug.Log("Player:: isSitTrigger OFF");
            isSitTrigger = false;
            colChairObj = null;
        }
    }
#endregion
}
