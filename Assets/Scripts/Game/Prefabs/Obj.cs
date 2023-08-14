using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour {

    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rigid; public Rigidbody2D Rigid {get => rigid;}
    [SerializeField] SpriteRenderer sprRdr; public SpriteRenderer SprRdr {get => sprRdr;}
    [SerializeField] bool isDisappear;   public bool IsDisappear {get => isDisappear; set => isDisappear = value;}

    [SerializeField] Coroutine coroutineID = null;  public Coroutine CoroutineID {get => coroutineID; set => coroutineID = value;}

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
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
        if(col.CompareTag(Enum.TAG.Player.ToString())) {
            Debug.Log($"<b>Obj:: OnTriggerExit2D(col= {col.tag}):: Obj.name= {this.name}, Player.velocity.dir= {MGM._.Pl.Rigid.velocity.normalized}</b>");
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            this.GetComponent<SpriteRenderer>().color = Color.red;

            //* 上から下へ落ちる時、橋場とぶつかったらTriggerExit()なので、PlayerへCollisionEnter2Dが有っても、ぶつからない問題があり、
            //* 動く方向を把握して、下向きならジャンプを直接させる。
            bool isDirDown = MGM._.Pl.Rigid.velocity.normalized.y < 0;
            if(isDirDown) {
                MGM._.Pl.jump();
                MGM._.mgem.releaseObj(this.gameObject, (int)MGEM.IDX.JumpingPadObj);
            }
        }
    }
#endregion
#endregion
}
