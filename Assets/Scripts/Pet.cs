using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Pet : MonoBehaviour {
    const int FRONT_Z = -1;
    const int REVERSE_Y = -1;
    const float CHASE_DELAY = 0.5f;
    const float MIN_X = -2.5f, MAX_X = 2.5f, MIN_Y = -3.5f, MAX_Y = 2;
    const float OFFSET_X = 1;
    const float OFFSET_Y = 0;
    const float WALK_STOP_VAL = 0.5f;

    [Header("OUTSIDE")]
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}


    [Header("VALUE")]
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] SpriteRenderer shadowSr;
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr;}
    [SerializeField] float moveSpeed;

    Transform tf;
    Transform plTf;
    Vector2 tgPos;
    
    void Start() {
        tf = transform;
        plTf = HM._.pl.transform;
    }
    void Update() {
        if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;

        //* ペットなかったら、影 非表示
        shadowSr.enabled = sprLib.spriteLibraryAsset;

        //* レイヤー
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        //* プレイヤー 追いかける
        float x = Mathf.Clamp((plTf.position.x + OFFSET_X), MIN_X, MAX_X);
        float y = Mathf.Clamp((plTf.position.y + OFFSET_Y), MIN_Y, MAX_Y);
        tgPos = new Vector2(x, y);

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            float distX = Mathf.Abs(tgPos.x - tf.position.x);
            float distY = Mathf.Abs(tgPos.y - tf.position.y);
            bool isWalkStop = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;

            tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
            tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);

            //* アニメー
            if(isWalkStop) {setIdle();}
            else {setWalk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setInitSpr() => sr.sprite = idleSpr;

    public void setIdle() {
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
    }

    public void setWalk() {
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), true);
    }
#endregion
}
