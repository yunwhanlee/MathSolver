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
            }
            else if(col.name == Enum.OBJ_NAME.goldApple.ToString()) {
                MGM._.Score += 3;
                MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.GoldAppleObj);
            }
            else if(col.name == Enum.OBJ_NAME.bomb.ToString()) {
                Mathf.Clamp(MGM._.Score--, 0, 999);
                MGM._.mgem.releaseObj(col.gameObject, (int)MGEM.IDX.BombObj);
            }
        }
    }
}
