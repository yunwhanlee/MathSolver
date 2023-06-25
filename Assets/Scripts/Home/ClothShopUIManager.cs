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
        PlayerSkin[] lockedPlSks = Array.FindAll(DB.Dt.PlSkins, pl => pl.IsLock);
        PetSkin[] lockedPtSks = Array.FindAll(DB.Dt.PtSkins, pet => pet.IsLock);
        if(lockedPlSks.Length == 0 && lockedPtSks.Length == 0) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Nothing more to buy."));
            return;
        } 

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
            HM._.ui.showErrorMsgPopUp(LM._.localize("Not enough coin!"));
        }

        //* GachaAnimPanel 表示
        HM._.ui.TopGroup.SetActive(false);
    }
    public void onClickTapScreenBtn() {
        //* カーテン開ける アニメーション
        anim.SetBool(Enum.ANIM.IsShowGachaReward.ToString(), true);
        if(!rewardSpr) {
            PetSkin[] lockedPtSks = Array.FindAll(DB.Dt.PtSkins, pet => pet.IsLock);
            PlayerSkin[] lockedPlSks = Array.FindAll(DB.Dt.PlSkins, pl => pl.IsLock);

            //* 残る数が有るか確認
            bool isPlayerSkin = (lockedPlSks.Length == 0)? false : (lockedPtSks.Length == 0)? true : Random.Range(0, 100) < REWARD_PET_PER;
            bool isPetSkin = !isPlayerSkin;

            //* ガチャー
            if (isPlayerSkin && lockedPlSks.Length > 0) {
                int rand = Random.Range(0, lockedPlSks.Length);
                setReward(reward: lockedPlSks[rand]);
            }
            else if (isPetSkin && lockedPtSks.Length > 0) {
                int rand = Random.Range(0, lockedPtSks.Length);
                setReward(reward: lockedPtSks[rand]);
            }

            //* スプライト 適用
            Debug.Log($"Reward Sprite= {rewardSpr}");
            rewardImg.sprite = rewardSpr;
        }
        //* ホームに戻る
        else {
            anim.SetBool(Enum.ANIM.IsShowGachaReward.ToString(), false);
            rewardSpr = null;
            HM._.ui.TopGroup.SetActive(true);
            gachaAnimPanel.SetActive(false);
        }
    }
#endregion

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private void setReward(PlayerSkin reward) {
        reward.IsLock = false;
        rewardSpr = reward.Spr;
        rewardNameTxt.text = LM._.localize(reward.Name);
    }
    private void setReward(PetSkin reward) {
        reward.IsLock = false;
        rewardSpr = reward.Spr;
        rewardNameTxt.text = LM._.localize(reward.Name);
    }
    IEnumerator coPlayGachaPanelAnimIdle() {
        yield return Util.time0_5;
        gachaAnimPanel.SetActive(true); 
    }
#endregion
}
