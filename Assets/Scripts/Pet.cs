using System;
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
    [SerializeField] BoxCollider2D col; public BoxCollider2D Col {get => col;}

    [Header("VALUE")]
    [SerializeField] string name;    public string Name {get => name; set => name = value;}
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr;}
    [SerializeField] bool isChasePlayer = true;    public bool IsChasePlayer {get => isChasePlayer; set => isChasePlayer = value;}
    [SerializeField] SpriteRenderer shadowSr;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos;  public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    [SerializeField] GameObject petShinyAuraEF;

    Transform tf;
    Transform plTf;

    void Start() {
        tf = transform;
        plTf = HM._.pl.transform;
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        var pet = Array.Find(DB.Dt.PtSkins, ptsk => ptsk.IsArranged);
        Debug.Log("pet.name= " + pet.Name);
        name = pet.Name;
        activeShinyAuraEF();
    }

    void Update() {
        if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;

        //* ペットなかったら、非表示
        shadowSr.enabled = sprLib.spriteLibraryAsset; // 影
        if(!sprLib.spriteLibraryAsset) sr.sprite = null; // 画像

        //* レイヤー
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        //* メインゲーム 結果画面なら、プレイヤーより必ず後ろ
        // if(GM._ && GM._.GameStatus == GM.GAME_STT.RESULT)
        //     sr.sortingOrder = Mathf.RoundToInt(GM._.Pl.Sr.sortingOrder -1);
        if(GM._) {
            sr.sortingOrder = Mathf.RoundToInt(GM._.Pl.Sr.sortingOrder -2);
            if(GM._.GameStatus == GM.GAME_STT.RESULT) {
                sr.sortingOrder = Mathf.RoundToInt(GM._.Pl.Sr.sortingOrder -2);
            }
        }

        //* プレイヤー 追いかける
        if(isChasePlayer) {
            bool plFlipX = HM._.pl.Sr.flipX;
            int dir = plFlipX? -1 : 1;
            float x = Mathf.Clamp((plTf.position.x + (OFFSET_X * dir)), MIN_X, MAX_X);
            float y = Mathf.Clamp((plTf.position.y + OFFSET_Y), MIN_Y, MAX_Y);
            tgPos = new Vector2(x, y);
            sr.flipX = plFlipX;
        }

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
    public void animDance() {
        SM._.sfxPlay(SM.SFX.PetClick.ToString());
        anim.SetTrigger(Enum.ANIM.DoDance.ToString());
    }

    public void activeShinyAuraEF() {
        petShinyAuraEF.SetActive(name == "GoldApple Pet" || name == "BabyMonkey Pet" || name == "BabyDragon Pet");
    }

#endregion
}
