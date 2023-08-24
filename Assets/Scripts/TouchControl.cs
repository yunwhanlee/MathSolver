using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchControl : MonoBehaviour
{
    const float MAX_DISTANCE = 15f;

    void Update(){
        if(Input.GetMouseButtonDown(0)) {
            if(HM._.state != HM.STATE.NORMAL) return;
            if(HM._.ui.CurHomeSceneIdx != (int)Enum.HOME.Room) return;
            if(HM._.htm.IsAction) return;
            if(HM._.funitureModeShadowFrameObj.activeSelf) //* NewFuniturePopUpが LevelUpPopUpと重なったら、STATE.NORMALになるバグ対応。
                HM._.state = HM.STATE.DECORATION_MODE;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, transform.forward, MAX_DISTANCE);
            Debug.Log("---------------------------------------");
            for(int i = 0; i < hits.Length; i++) {
                var hit = hits[i];
                Debug.Log($"TouchControl:: Hit.tag= {hit.transform.tag}, Hit.name= {hit.transform.name}, mouseWorldPos= {mouseWorldPos}");

                bool isIconUIArea = hit.transform.CompareTag(Enum.TAG.IconBtnGroupArea.ToString());
                bool isChair = HM._.isChair(hit.transform.gameObject);
                bool isPlayer = hit.transform.CompareTag(Enum.TAG.Player.ToString());
                bool isPet = hit.transform.CompareTag(Enum.TAG.Pet.ToString());
                //* ホームのアイコンボタン領域
                if(isIconUIArea)
                    return; // プレイヤー移動させない
                //* プレイヤー
                else if(isPlayer) {
                    // isSitTriggerがONなら、プレイヤー選択できないように
                    if(HM._.pl.ColChairObj) continue;
                }
                //* ペット
                else if(isPet) {
                    HM._.pet.animDance(); // 踊る
                    return; // プレイヤー移動させない
                }
                //* 椅子(家具)
                else if(isChair) {
                    // 一旦、椅子の方にも移動できるように
                    HM._.pl.TgPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                    HM._.pl.setSit(hit.transform); // 座る・立つ
                    return;
                }
                //* その以外 歩く
                else {
                    SM._.sfxPlay(SM.SFX.BtnClick.ToString());
                    HM._.pl.animSit(false); // 座る状態なら、立つ
                    // プレイヤー 移動位置
                    HM._.pl.TgPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                    // return;
                }
            }
            Debug.DrawRay(mouseWorldPos, transform.forward * 50, Color.red, 0.3f);
        }
    }
}
