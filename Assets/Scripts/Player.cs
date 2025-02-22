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
    // const float OFFSET_SIT_X = 0.15f;
    // const float OFFSET_SIT_Y = 0.5f;
    const int OFFSET_SIT_FRONT_SORTING = 3;
    const float TARGET_POS_Y_MAX = 1.3f;


    [Header("OUTSIDE")]
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr; set => sr = value;}
    [SerializeField] Collider2D col; public Collider2D Col {get => col; set => col = value;}
    [SerializeField] Rigidbody2D rigid; public Rigidbody2D Rigid {get => rigid;}
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}

    [Header("ACTIVE EF")]
    [SerializeField] GameObject roarEF;
    [SerializeField] GameObject levelUpEF;    public GameObject LevelUpEF {get => levelUpEF;}
    [SerializeField] Transform auraEFGroup;   public Transform AuraEFGroup {get => auraEFGroup;}

    [Header("VALUE")]    
    [SerializeField] SpriteLibrary sprLib;  public SpriteLibrary SprLib {get => sprLib; set => sprLib = value;}
    
    [SerializeField] Sprite idleSpr;    public Sprite IdleSpr {get => idleSpr; set => idleSpr = value;}
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 tgPos; public Vector2 TgPos {get => tgPos; set => tgPos = value;}
    //* Sit Chair
    [SerializeField] bool isSit; public bool IsSit {get => isSit; set => isSit = value;}
    [SerializeField] GameObject colChairObj;    public GameObject ColChairObj {get => colChairObj;}
    [SerializeField] int tbDecoAreaSortingAddVal = 0;   public int TbDecoAreaSortingAddVal {get => tbDecoAreaSortingAddVal; set => tbDecoAreaSortingAddVal = value;}

    Transform tf;

    void Start() {
        tf = transform;
        tgPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        sprLib = GetComponent<SpriteLibrary>();
    }

    void Update() {
        if(HM._ != null && HM._.state != HM.STATE.NORMAL) return;
        if(MGM._) return; //* MiniGame時には以下処理しない
        if(isSit) return;
        //* レイヤー
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y + tbDecoAreaSortingAddVal;

        if(tgPos.x != tf.position.x || tgPos.y != tf.position.y) {
            //* 家具クリックで、壁まで行くバグ対応
            if(tgPos.y >= TARGET_POS_Y_MAX) tgPos.y = TARGET_POS_Y_MAX;
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
    public void initPos() {
        idle();
        Vector2 INIT_POS = new Vector2(0, -4);
        tgPos = INIT_POS; //* 玄関の前に位置させる
        transform.position = INIT_POS;
    }
    public void jump() {
        Debug.Log($"Player:: jump():: ");
        SM._.sfxPlay(SM.SFX.Jump.ToString());
        float val = MGM._.JumpPower * Time.fixedDeltaTime;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * val, ForceMode2D.Impulse);
    }
    public void setSit(Transform hitTf) {
        //* 座る
        if(!isSit 
        && colChairObj // 衝突した椅子がなかったリターン
        && hitTf.gameObject == colChairObj) { // 衝突した椅子と違ったらリターン
            animSit(true);
            tf.localPosition = hitTf.GetComponent<RoomObject>().SitSpot.position;
            sr.flipX = hitTf.GetComponent<SpriteRenderer>().flipX;
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
    public void flyByhitObstacle(int power) {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        float xDir = (transform.localPosition.x < 0)? -1 : 1;
        var dir = new Vector2(xDir, 1);
        dir = dir.normalized;
        int rotPower = 500;
        rigid.AddForce(dir * power * Time.fixedDeltaTime, ForceMode2D.Impulse);
        rigid.AddTorque(rotPower * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    public float calcLvBonusPer() {
        return DB.Dt.Lv * Config.LV_BONUS_PER - Config.LV_BONUS_PER; // 2 * 0.1f - 0.1f = 0.1f
    }
    public float calcLegacyBonusPer() {
        float res = DB._.LegacyCnt * Config.LEGACY_BONUS_PER;
        Debug.Log($"<color=yellow>calcLegacyBonusPer():: res= {res}</color>");
        return res;
    }
    private void collideWithChair(bool isTrigger, Collider2D col) {
        if(HM._.isChair(col.gameObject)) {
            if(isTrigger) {
                if(isSit) return;
                colChairObj = col.gameObject;
                //* 衝突した一つのみアウトライン表示
                HM._.clearAllChairOutline();
                var obj = colChairObj.GetComponent<RoomObject>();
                obj.Sr.material = HM._.outlineMt;
            }
            else {
                //* アウトライン解除
                colChairObj = null;
                HM._.clearAllChairOutline();
            }
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
    public IEnumerator coLevelUpEF(float delay = 0) {
        // yield return delay == 1? Util.time1 : null;
        // Debug.Log($"coLevelUpEF():: levelUpEF.name= {levelUpEF.name}");
        SM._.sfxPlay(SM.SFX.LevelUp.ToString());
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
        if(_isSit) {
            SM._.sfxPlay(SM.SFX.TinyBubblePop.ToString());
            StartCoroutine(Util.coPlayBounceAnim(this.transform));
        }
    }
#endregion
///------------------------------------------------------------------------------------------
#region COLLIDER (Collision)
///------------------------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D col) {
        //* Minigame 2
        if(col.gameObject.CompareTag(Enum.TAG.JumpingPad.ToString())) {
            jump();
            MGM._.mgem.showEF((int)MGEM.IDX.DecalWoodEF, col.transform.position, Util.time1);
            MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.JumpingPadObj);
            col.transform.GetComponent<Collider2D>().isTrigger = true;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
#endregion
///------------------------------------------------------------------------------------------
#region COLLIDER (Trigger)
///------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.GoGame.ToString())) {
            HM._.state = HM.STATE.POPUP;
            HM._.ui.GoGamePopUp.SetActive(true);
        }
        else if(col.gameObject.CompareTag(Enum.TAG.EraseObjLine.ToString())) {
            SM._.sfxPlay(SM.SFX.Fall.ToString());
            MGM._.Status = MGM.STATUS.FINISH;
        }
        else if(col.gameObject.CompareTag(Enum.TAG.PlayerNoCollideArea.ToString())) {
            Debug.Log("<color=red>Player:: OnTriggerEnter2D():: PlayerNoCollideArea</color>");
            col.isTrigger = true;
        }

    }
    private void OnTriggerStay2D(Collider2D col) {
        if(HM._) {
            collideWithChair(true, col); //* Sit Trigger ON
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(HM._) {
            collideWithChair(false, col); //* Sit Trigger OFF
        }
        else if(col.gameObject.CompareTag(Enum.TAG.PlayerNoCollideArea.ToString())) {
            Debug.Log("<color=red>Player:: OnTriggerExit2D():: PlayerNoCollideArea</color>");
            col.isTrigger = false;
        }
    }
#endregion

}
