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
            
            Array.ForEach(hits, hit => {
                if(hit) {
                Debug.Log($"TouchControl:: Hit.tag= {hit.transform.tag}, Hit.name= {hit.transform.name}, mouseWorldPos= {mouseWorldPos}");
                bool isChair = hit.transform.CompareTag(Enum.TAG.Funiture.ToString()) && hit.transform.gameObject.layer == LayerMask.NameToLayer(Enum.LAYER.Chair.ToString());
                bool isPlayer = hit.transform.CompareTag(Enum.TAG.Player.ToString());

                //* ホームのアイコンボタン領域なら、プレイヤー移動させない
                if(hit.transform.CompareTag(Enum.TAG.IconBtnGroupArea.ToString()))
                    return;
                else if(hit.transform.CompareTag(Enum.TAG.Pet.ToString())) {
                    HM._.pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
                    return;
                }
                else if(isChair && HM._.pl.IsSitTrigger) {
                    if(hit.transform.gameObject != HM._.pl.ColChairObj) return;
                    Debug.Log($"Player Sit! hit.name= {hit.transform.name}");
                    const float OFFSET_SIT_X = 0.15f;
                    const float OFFSET_SIT_Y = 0.825f;
                    var plTf = HM._.pl.transform;
                    var hitTf = hit.transform;
                    if(!HM._.pl.IsSit) {
                        HM._.pl.IsSit = true;
                        HM._.pl.Anim.SetBool(Enum.ANIM.IsSit.ToString(), true);
                        plTf.localPosition = new Vector2(hitTf.localPosition.x + OFFSET_SIT_X, hitTf.localPosition.y + OFFSET_SIT_Y);
                        plTf.localScale = new Vector2(hitTf.localScale.x, plTf.localScale.y);
                        //* レイヤー 椅子より +1前に
                        HM._.pl.Sr.sortingOrder = HM._.pl.Sr.sortingOrder + 2;
                    }
                    else {
                        HM._.pl.IsSit = false;
                        HM._.pl.Anim.SetBool(Enum.ANIM.IsSit.ToString(), false);
                        plTf.localPosition = new Vector2(hitTf.localPosition.x, hitTf.localPosition.y);
                    }
                }
                else if(isPlayer) {
                    //* isSitTrigger ONなら、プレイヤー選択できないように
                    if(HM._.pl.IsSitTrigger) return;
                }

                //* プレイヤー 移動位置
                HM._.pl.TgPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            }
            });
            Debug.DrawRay(mouseWorldPos, transform.forward * 50, Color.red, 0.3f);
        }
    }
}
