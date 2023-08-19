using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour {

    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rigid; public Rigidbody2D Rigid {get => rigid;}
    [SerializeField] SpriteRenderer sprRdr; public SpriteRenderer SprRdr {get => sprRdr;}
    [SerializeField] bool isDisappear;   public bool IsDisappear {get => isDisappear; set => isDisappear = value;}

    [SerializeField] Coroutine coroutineID = null;  public Coroutine CoroutineID {get => coroutineID; set => coroutineID = value;}

    [Header("MINIGAME 3")]
    [SerializeField] bool isMoving;  public bool IsMoving {get => isMoving; set => isMoving = value;}
    [SerializeField] float movingSpeed;   public float MovingSpeed {get => movingSpeed; set => movingSpeed = value;}

    void OnDisable() {
        isMoving = false;
        movingSpeed = 0;
    }

    void Update() {
        if(isMoving) {
            transform.transform.Translate(0, movingSpeed * Time.deltaTime, 0);
        }
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void activeMoving(float spd) {
        isMoving = true;
        movingSpeed = spd;
    }
    public void addForce(Vector2 dir) {
        // Debug.Log($"Obj:: addForce(dir= {dir})");
        int burstPower = Random.Range(250, 400);
        float power = burstPower * Time.fixedDeltaTime;
        rigid.AddForce(dir * power, ForceMode2D.Impulse);
    }
    public IEnumerator coDisappear() {
        float spd = 5 * Time.deltaTime;
        col.enabled = false;
        SpriteRenderer sprRdr = GetComponent<SpriteRenderer>();
        while(sprRdr.color.a > 0) {
            sprRdr.color = new Color(1,1,1,sprRdr.color.a - spd);
            yield return null;
        }
        Destroy(this);
    }
///------------------------------------------------------------------------------------------
#region COLLIDER (Trigger)
///------------------------------------------------------------------------------------------
    private void OnTriggerExit2D(Collider2D col) {
        //* 自分が🍌オブジェクトなら、処理しない
        if(CompareTag(Enum.TAG.Banana.ToString())) return;
        if(CompareTag(Enum.TAG.GoldBanana.ToString())) return;
        if(CompareTag(Enum.TAG.Obstacle.ToString())) return;
        if(CompareTag(Enum.TAG.Blueberry.ToString())) return;
        if(CompareTag(Enum.TAG.GoldBlueberry.ToString())) return;

        #region MINIGAME 2
        if(col.CompareTag(Enum.TAG.Player.ToString())) {
            Debug.Log($"<b>Obj:: OnTriggerExit2D(col= {col.tag}):: Obj.name= {name}, Player.velocity.dir= {MGM._.Pl.Rigid.velocity.normalized}</b>");
            GetComponent<BoxCollider2D>().isTrigger = false;
            // GetComponent<SpriteRenderer>().color = Color.red;
            //* 上から下へ落ちる時、橋場とぶつかったらTriggerExit()なので、PlayerへCollisionEnter2Dが有っても、ぶつからない問題があり、
            //* 動く方向を把握して、下向きならジャンプを直接させる。
            bool isDirDown = MGM._.Pl.Rigid.velocity.normalized.y < 0;
            if(isDirDown) {
                MGM._.Pl.jump();
                MGM._.mgem.showEF((int)MGEM.IDX.DecalWoodEF, gameObject.transform.position, Util.time1);
                MGM._.mgem.releaseObj(gameObject, (int)MGEM.IDX.JumpingPadObj);
                this.col.isTrigger = true;
                // GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        //* 削除 JumpingPad
        else if(col.CompareTag(Enum.TAG.EraseObjLine.ToString())) {
            Debug.Log($"<b>Obj:: OnTriggerExit2D(col= {col.tag}):: Obj.name= {name}</b>");
            col.isTrigger = true;
            // GetComponent<SpriteRenderer>().color = Color.white;
            MGM._.mgem.releaseObj(this.gameObject, (int)MGEM.IDX.JumpingPadObj);
        }
        #endregion
    }
#endregion
#endregion
}
