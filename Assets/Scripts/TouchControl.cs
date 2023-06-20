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

                    const float OFFSET_SIT_X = 0.15f;
                    const float OFFSET_SIT_Y = 0.825f;
                    const int FRONT_SIT_SORTING_ORDER = 3;
                    var plTf = HM._.pl.transform;
                    var hitTf = hit.transform;

                    //* 座る
                    if(!HM._.pl.IsSit) {
                        Debug.Log($"Player Sit! hit.name= {hit.transform.name}");
                        HM._.pl.IsSit = true;
                        HM._.pl.Anim.SetBool(Enum.ANIM.IsSit.ToString(), true);
                        plTf.localPosition = new Vector2(hitTf.localPosition.x + OFFSET_SIT_X, hitTf.localPosition.y + OFFSET_SIT_Y);
                        plTf.localScale = new Vector2(hitTf.localScale.x, plTf.localScale.y);
                        //* レイヤー 椅子より +1前に
                        HM._.pl.Sr.sortingOrder = HM._.pl.Sr.sortingOrder + FRONT_SIT_SORTING_ORDER;
                        //* 全て椅子のアウトライン 初期化
                        HM._.clearAllChairOutline();
                    }
                    //* 立つ
                    else {
                        Debug.Log($"Player Stand Up! hit.name= {hit.transform.name}");
                        HM._.pl.IsSit = false;
                        HM._.pl.Anim.SetBool(Enum.ANIM.IsSit.ToString(), false);
                        plTf.localPosition = new Vector2(hitTf.localPosition.x, hitTf.localPosition.y);
                        return;
                    }
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
