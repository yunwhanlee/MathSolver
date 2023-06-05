using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    const int FRONT_Z = -1;
    Transform tf;
    public Collider2D col;
    public Animator anim;

    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr;}
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos; public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    [SerializeField] bool doIdle;
    [SerializeField] bool doWalk;
    void Start() {
        tf = transform;
        col = GetComponent<Collider2D>();
        tgPos = transform.position;
    }

    void Update() {
        //* レイヤー
        const int REVERSE_Y = -1;
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            float distX = Mathf.Abs(tgPos.x - tf.position.x);
            float distY = Mathf.Abs(tgPos.y - tf.position.y);

            //* プレイヤー方向
            Vector2 dir = new Vector2(tgPos.x - tf.position.x, tgPos.y - tf.position.y).normalized;
            bool isLeft = ((dir.x < 0 && 0 <= dir.y) || (dir.x < 0 && dir.y < 0));
            float flipX = (isLeft)? -1 : 1;
            tf.localScale = new Vector2(flipX, tf.localScale.y);
            
            const float WALK_STOP_VAL = 0.05f;
            if(distX < WALK_STOP_VAL && distY < WALK_STOP_VAL) {
                tf.position = new Vector3(tgPos.x, tgPos.y, FRONT_Z);
                if(!doIdle) { doIdle = true; anim.SetTrigger(Enum.ANIM.DoIdle.ToString());}
                if(doWalk) doWalk = false;
            }
            else {
                tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
                tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);
                if(doIdle) doIdle = false;
                if(!doWalk) { doWalk = true; anim.SetTrigger(Enum.ANIM.DoWalk.ToString());}
            }

            Debug.Log($"pl.transform.position -> isLeft= {isLeft}, distance x= {distX}, y= {distY}");
        }
    }

///------------------------------------------------------------------------------------------
#region Collider
///------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.GoGame.ToString())) {
            HM._.ui.GoGameDialog.SetActive(true);
        }
    }
#endregion
}
