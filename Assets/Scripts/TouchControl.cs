using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    const float MAX_DISTANCE = 15f;

    void Update(){
        if(Input.GetMouseButtonDown(0)) {
            if(HM._.state == HM.STATE.DECORATION_MODE) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, transform.forward, MAX_DISTANCE);
            if(hit) {
                Debug.Log($"TouchControl:: Hit.Layer= {LayerMask.LayerToName(hit.transform.gameObject.layer)}, mouseWorldPos= {mouseWorldPos}");

                //* ホームのアイコンボタン領域なら、プレイヤー移動させない
                if(hit.transform.CompareTag(Enum.LAYER.IconBtnGroupArea.ToString())) return;
                HM._.pl.TgPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            }
            Debug.DrawRay(mouseWorldPos, transform.forward * 50, Color.red, 0.3f);
        }
    }
}
