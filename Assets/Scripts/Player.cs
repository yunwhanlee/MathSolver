using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour {
    const int FRONT_Z = -1;
    const int REVERSE_Y = -1;
    const float WALK_STOP_VAL = 0.05f;
    const float MIN_Y = -5;
    const float MAX_Y = 2;
    const float OFFSET_SIT_X = 0.15f;
    const float OFFSET_SIT_Y = 0.5f;
    const int OFFSET_SIT_FRONT_SORTING = 3;

    [Header("OUTSIDE")]
    [SerializeField] Collider2D col; public Collider2D Col {get => col; set => col = value;}
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}

    [Header("ACTIVE EF")]
    [SerializeField] GameObject roarEF;
    [SerializeField] GameObject levelUpEF;

    [Header("VALUE")]
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr; set => idleSpr = value;}
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos; public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    //* Sit Chair
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
        if(isSit) return;

        //* レイヤー
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            //* プレイヤー方向
            Vector2 dir = new Vector2(tgPos.x - tf.position.x, tgPos.y - tf.position.y).normalized;
            bool isLeft = ((dir.x < 0 && 0 <= dir.y) || (dir.x < 0 && dir.y < 0));
            //* (BUG) playBounceAnimするとき、ScaleでFlipすると-xの場合エラーになるので、SpriteRenderのFlipX属性を活用！
            sr.flipX = isLeft;

            //* 追いかける & アニメー
            float distX = Mathf.Abs(tgPos.x - tf.position.x);
            float distY = Mathf.Abs(tgPos.y - tf.position.y);
            bool isWalkStop = distX < WALK_STOP_VAL && distY < WALK_STOP_VAL;

            if(isWalkStop) {idle();}
            else {walk();}
        }
    }

///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setSit(Transform hitTf) {
        //* 座る
        if(!isSit 
        && colChairObj // 衝突した椅子がなかったリターン
        && hitTf.gameObject == colChairObj) { // 衝突した椅子と違ったらリターン
            animSit(true);
            tf.localPosition = new Vector2(hitTf.localPosition.x + OFFSET_SIT_X, hitTf.localPosition.y + OFFSET_SIT_Y);
            tf.localScale = new Vector2(hitTf.localScale.x, tf.localScale.y);
            //* レイヤー 椅子より +1前に
            sr.sortingOrder = sr.sortingOrder + OFFSET_SIT_FRONT_SORTING;
            //* 全て椅子のアウトライン 初期化
            HM._.clearAllChairOutline();
        }
        //* 立つ
        else {
            animSit(false);
        }
    }
#endregion
///------------------------------------------------------------------------------------------
#region EFFECT
///------------------------------------------------------------------------------------------
    public IEnumerator coRoarEF() {
        roarEF.SetActive(true);
        yield return Util.time2;
        roarEF.SetActive(false);
    }
    public IEnumerator coLevelUpEF() {
        levelUpEF.SetActive(true);
        yield return Util.time1;
        levelUpEF.SetActive(false);
    }
#endregion

///------------------------------------------------------------------------------------------
#region ANIM
///------------------------------------------------------------------------------------------
    public void idle() {
        tf.position = new Vector3(tgPos.x, tgPos.y, FRONT_Z);
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
    }
    public void walk() {
        tf.position = Vector2.Lerp(tf.position, tgPos, moveSpeed * Time.deltaTime);
        tf.position = new Vector3(tf.position.x, Mathf.Clamp(tf.position.y, MIN_Y, MAX_Y), FRONT_Z);
        anim.SetBool(Enum.ANIM.IsWalk.ToString(), true);
    }
    public void animSit(bool _isSit) {
        isSit = _isSit;
        HM._.pl.Anim.SetBool(Enum.ANIM.IsSit.ToString(), _isSit);
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
        collideWithChair(true, col);
    }

    private void OnTriggerExit2D(Collider2D col) {
        //* Sit Trigger OFF
        collideWithChair(false, col);
    }
#endregion
    private void collideWithChair(bool isTrigger, Collider2D col) {
        if(HM._.isChair(col.gameObject)) {
            if(isTrigger) {
                colChairObj = col.gameObject;
                if(isSit) return;
                var sr = colChairObj.GetComponent<SpriteRenderer>();
                sr.material = HM._.outlineMt;
            }
            else {
                var sr = colChairObj.GetComponent<SpriteRenderer>();
                sr.material = HM._.sprUnlitMt;
                colChairObj = null;
            }
        }
    }
}
