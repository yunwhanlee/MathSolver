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
    const float OFFSET_Y = -0.275f;
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

        //* ペットなかったら、非表示
        shadowSr.enabled = sprLib.spriteLibraryAsset; // 影
        if(!sprLib.spriteLibraryAsset) sr.sprite = null; // 画像

        //* レイヤー
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        //* プレイヤー 追いかける
        int dir = (HM._.pl.transform.localScale.x < 0)? -1 : 1;
        float x = Mathf.Clamp((plTf.position.x + (OFFSET_X * dir)), MIN_X, MAX_X);
        float y = Mathf.Clamp((plTf.position.y + OFFSET_Y), MIN_Y, MAX_Y);
        tgPos = new Vector2(x, y);

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            float distX = Mathf.Abs(tgPos.x - tf.position.x);
            float distY = Mathf.Abs(tgPos.y - tf.position.y);
            bool isWalkStop = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;

            tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
            //* 適用
            tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);

            //* アニメー
            if(isWalkStop) {animIdle();}
            else {animWalk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setInitSpr() => sr.sprite = idleSpr;
    public void animIdle() => anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);    
    public void animWalk() => anim.SetBool(Enum.ANIM.IsWalk.ToString(), true);
    public void animDance() => anim.SetTrigger(Enum.ANIM.DoDance.ToString());
#endregion
}
