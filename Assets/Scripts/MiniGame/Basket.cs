using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.Obj.ToString())) {
            Debug.Log($"Basket():: col.name= {col.name}");
            if(col.name == Enum.OBJ_NAME.apple.ToString()) {
                MGM._.Score++;
                MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.AppleObj);
                StartCoroutine(Util.coPlayBounceAnim(transform));
                StartCoroutine(Util.coPlayBounceAnim(MGM._.Pl.transform));
                MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            }
            else if(col.name == Enum.OBJ_NAME.goldApple.ToString()) {
                MGM._.Score += 3;
                MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.GoldAppleObj);
                StartCoroutine(Util.coPlayBounceAnim(transform));
                StartCoroutine(Util.coPlayBounceAnim(MGM._.Pl.transform));
                MGM._.mgem.showEF((int)MGEM.IDX.BasketCatchEF, transform.position, Util.time2);
            }
            else if(col.name == Enum.OBJ_NAME.bomb.ToString()) {
                Mathf.Clamp(MGM._.Score--, 0, 999);
                MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.BombObj);
                StartCoroutine(Util.coPlayBounceAnim(transform));
                StartCoroutine(Util.coPlayBounceAnim(MGM._.Pl.transform));
                MGM._.Pl.Anim.SetTrigger(Enum.ANIM.DoFail.ToString());
                MGM._.mgem.showEF((int)MGEM.IDX.StunEF, transform.position, Util.time2);
                MGM._.mgem.showEF((int)MGEM.IDX.ExplosionBombEF, transform.position, Util.time2);
                StartCoroutine(MGM._.coSetPlayerStun());

            }
        }
    }
}
