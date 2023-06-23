using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class ClothShopUIManager : MonoBehaviour
{
    Animator anim;

    const int REWARD_PET_PER = 30;
    const int GACHA_PRICE = 300;

    Sprite rewardSpr; 

    [Header("PURCHASE BTN")]
    [SerializeField] TextMeshProUGUI priceTxt;  public TextMeshProUGUI PriceBtn {get => priceTxt; set => priceTxt = value;}

    [Header("REWARD ANIM PANEL")]
    [SerializeField] GameObject gachaAnimPanel;    public GameObject GachaRewardAnimPanel {get => gachaAnimPanel; set => gachaAnimPanel = value;}
    [SerializeField] Image rewardImg;    public Image RewardImg {get => rewardImg; set => rewardImg = value;}
    [SerializeField] TextMeshProUGUI rewardNameTxt;   public TextMeshProUGUI RewardNameTxt {get => rewardNameTxt; set => rewardNameTxt = value;}
    [SerializeField] Button tapScreenBtn;   public Button TapScreenBtn {get => tapScreenBtn; set => tapScreenBtn = value;}
    [SerializeField] TextMeshProUGUI tapScreenTxt; public TextMeshProUGUI TapScreenTxt {get => TapScreenTxt; set => TapScreenTxt = value;}

    void Start() {
        anim = gachaAnimPanel.GetComponent<Animator>();
        gachaAnimPanel.SetActive(false);
        priceTxt.text = $"{GACHA_PRICE * DB.Dt.GachaCnt}";
    }

/// -----------------------------------------------------------------------------------------------------------------
#region BTN EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickPurchaseBtn() {
        int price = GACHA_PRICE * DB.Dt.GachaCnt;
        if(DB.Dt.Coin >= price) {
            HM._.ui.playSwitchScreenAnim();
            StartCoroutine(coPlayGachaPanelAnimIdle());
            DB.Dt.setCoin(-price);
            //* 上がる値段 最新化
            DB.Dt.GachaCnt++;
            priceTxt.text = $"{GACHA_PRICE * DB.Dt.GachaCnt}";
        }
        else {
            HM._.ui.showErrorMsgPopUp("코인이 부족합니다!");
        }

        //* GachaAnimPanel 表示
        HM._.ui.TopGroup.SetActive(false);
    }
    public void onClickTapScreenBtn() {
        //* カーテン開ける アニメーション
        anim.SetBool("IsShowGachaReward", true);

        if(!rewardSpr) {
            //* ランダムのリワードスプライト 習得
            int rand = Random.Range(0, 100);
            if(rand < REWARD_PET_PER) {
                PetSkin[] items = Array.FindAll(DB.Dt.PtSkins, pet => pet.IsLock);
                Debug.Log($"REWARD PET: Lenght= {items.Length}");
                rand = Random.Range(0, items.Length);
                var reward = items[rand];
                reward.IsLock = false;
                rewardSpr = reward.Spr;
                rewardNameTxt.text = reward.Name;

            }
            else {
                PlayerSkin[] items = Array.FindAll(DB.Dt.PlSkins, pl => pl.IsLock);
                Debug.Log($"REWARD PLAYER: Lenght= {items.Length}");
                rand = Random.Range(0, items.Length);
                var reward = items[rand];
                reward.IsLock = false;
                rewardSpr = reward.Spr;
                rewardNameTxt.text = reward.Name;
            }

            //* スプライト 適用
            Debug.Log($"Reward Sprite= {rewardSpr}");
            rewardImg.sprite = rewardSpr;
        }
        //* ホームに戻る
        else {
            anim.SetBool("IsShowGachaReward", false);
            rewardSpr = null;
            HM._.ui.TopGroup.SetActive(true);
            gachaAnimPanel.SetActive(false);
        }
    }
#endregion

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
IEnumerator coPlayGachaPanelAnimIdle() {
    yield return Util.time0_5;
    gachaAnimPanel.SetActive(true); 
}
#endregion
}
