using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour {
    [Header("OUTSIDE")]
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}

    [Header("VALUE")]
    const int FRONT_Z = -1;
    const int REVERSE_Y = -1;
    const float CHASE_DELAY = 0.5f;
    const float MIN_X = -2.5f, MAX_X = 2.5f, MIN_Y = -3.5f, MAX_Y = 2;
    const float OFFSET_X = 1;
    const float OFFSET_Y = 0;
    const float WALK_STOP_VAL = 0.5f;
    Transform tf;
    Transform plTf;
    Vector2 tgPos;
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr;}
    [SerializeField] float moveSpeed;
    [SerializeField] bool doIdle;   public bool DoIdle {get => doIdle; set => doIdle = value;}
    [SerializeField] bool doWalk;   public bool DoWalk {get => doWalk; set => doWalk = value;}

    void Start() {
        tf = transform;
        plTf = HM._.pl.transform;
    }
    void Update() {
        if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;

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
            bool isMoving = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;

            tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
            tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);

            //* アニメー
            if(isMoving) {setIdle();}
            else {setWalk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setInitSpr() => sr.sprite = idleSpr;
    public void setIdle() { 
        if(!doIdle) { doIdle = true; anim.SetTrigger(Enum.ANIM.DoIdle.ToString());}
        if(doWalk) doWalk = false;
    }
    private void setWalk() {
        if(doIdle) doIdle = false;
        if(!doWalk) { doWalk = true; anim.SetTrigger(Enum.ANIM.DoWalk.ToString());}
    }
#endregion
}
