using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchControl : MonoBehaviour
{
    const float MAX_DISTANCE = 15f;

    void Update(){
        if(Input.GetMouseButtonDown(0)) {
            if(HM._.state == HM.STATE.DECORATION_MODE) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, transform.forward, MAX_DISTANCE);
            Debug.Log("---------------------------------------");
            for(int i = 0; i < hits.Length; i++) {
                var hit = hits[i];
                Debug.Log($"TouchControl:: Hit.tag= {hit.transform.tag}, Hit.name= {hit.transform.name}, mouseWorldPos= {mouseWorldPos}");
                bool isChair = HM._.isChair(hit.transform.gameObject);
                bool isPlayer = hit.transform.CompareTag(Enum.TAG.Player.ToString());

                if(hit.transform.CompareTag(Enum.TAG.IconBtnGroupArea.ToString())) //* ホームのアイコンボタン領域
                    return; //* プレイヤー移動させない
                else if(hit.transform.CompareTag(Enum.TAG.Pet.ToString())) {
                    HM._.pet.animDance();
                    return; //* プレイヤー移動させない
                }
                else if(isPlayer) {
                    //* isSitTriggerがONなら、プレイヤー選択できないように
                    if(HM._.pl.ColChairObj) continue;
                }
                else if(isChair && HM._.pl.ColChairObj) {
                    if(hit.transform.gameObject != HM._.pl.ColChairObj)
                        return;

                    HM._.pl.setSit(hit.transform);
                }
                else {
                    //* プレイヤー 移動位置
                    HM._.pl.TgPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                }
            }

            Debug.DrawRay(mouseWorldPos, transform.forward * 50, Color.red, 0.3f);
        }
    }
}
