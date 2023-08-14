using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

/// -----------------------------------------------------------------------------------------------------------------
#region COLLIDER
/// -----------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log($"Basket():: col.name= {col.name}");
        //* Minigame 1
        if(col.CompareTag(Enum.TAG.Apple.ToString())) {
            MGM._.Score += MGM._.ApplePoint;
            MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            releaseAndBounce(col.gameObject, (int)MGEM.IDX.AppleObj);
        }
        else if(col.CompareTag(Enum.TAG.Bomb.ToString())) {
            MGM._.Score = Mathf.Clamp((int)(MGM._.Score * 0.9f), 0, 999);
            MGM._.cam.Anim.SetTrigger(Enum.ANIM.DoCamShake.ToString());
            MGM._.Pl.Anim.SetTrigger(Enum.ANIM.DoFail.ToString());
            MGM._.mgem.showEF((int)MGEM.IDX.ExplosionBombEF, transform.position, Util.time2);
            MGM._.mgem.showEF((int)MGEM.IDX.StunEF, transform.position, Util.time1_5);
            releaseAndBounce(col.gameObject, (int)MGEM.IDX.BombObj);
            StartCoroutine(MGM._.coSetPlayerStun());
        }
        else if(col.CompareTag(Enum.TAG.GoldApple.ToString())) {
            MGM._.Score += MGM._.GoldApplePoint;
            MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            MGM._.mgem.showEF((int)MGEM.IDX.ShineSpoutGoldEF, transform.position, Util.time2);
            releaseAndBounce(col.gameObject, (int)MGEM.IDX.GoldAppleObj);
        }
        else if(col.CompareTag(Enum.TAG.Diamond.ToString())) {
            MGM._.Score += MGM._.DiamondPoint;
            MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            MGM._.mgem.showEF((int)MGEM.IDX.ShineSpoutGoldEF, transform.position, Util.time2);
            releaseAndBounce(col.gameObject, (int)MGEM.IDX.DiamondObj);
        }
        //* Minigame 2
        else if(col.gameObject.CompareTag(Enum.TAG.Banana.ToString())) {
            MGM._.Score += 1;
            MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, col.transform.position, Util.time999);
            MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.BananaObj);
        }
        else if(col.gameObject.CompareTag(Enum.TAG.GoldBanana.ToString())) {
            MGM._.Score += 3;
            MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            MGM._.mgem.showEF((int)MGEM.IDX.ShineSpoutGoldEF, transform.position, Util.time2);
            MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.GoldBananaObj);
        }
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private void releaseAndBounce(GameObject obj, int idx) {
        MGM._.mgem.releaseObj(obj, idx);
        StartCoroutine(Util.coPlayBounceAnim(this.transform));
        StartCoroutine(Util.coPlayBounceAnim(MGM._.Pl.transform));
    }
#endregion
}
