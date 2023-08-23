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

    [SerializeField] bool isGachaOn;    public bool IsGachaOn {get => isGachaOn; set => isGachaOn = value;}
    [SerializeField] int price;     public int Price {get => price; set => price = value;}

    Sprite rewardSpr; 

    [Header("PURCHASE BTN")]
    [SerializeField] TextMeshProUGUI priceTxt;  public TextMeshProUGUI PriceBtn {get => priceTxt; set => priceTxt = value;}
    [SerializeField] GameObject purchaseNotifyIcon;  public GameObject PurchaseNotifyIcon {get => purchaseNotifyIcon;}

    [Header("REWARD ANIM PANEL")]
    [SerializeField] GameObject gachaAnimPanel;    public GameObject GachaRewardAnimPanel {get => gachaAnimPanel; set => gachaAnimPanel = value;}
    [SerializeField] Image rewardImg;    public Image RewardImg {get => rewardImg; set => rewardImg = value;}
    [SerializeField] TextMeshProUGUI rewardNameTxt;   public TextMeshProUGUI RewardNameTxt {get => rewardNameTxt; set => rewardNameTxt = value;}
    [SerializeField] Button tapScreenBtn;   public Button TapScreenBtn {get => tapScreenBtn; set => tapScreenBtn = value;}
    [SerializeField] TextMeshProUGUI tapScreenTxt; public TextMeshProUGUI TapScreenTxt {get => TapScreenTxt; set => TapScreenTxt = value;}

    void Start() {
        anim = gachaAnimPanel.GetComponent<Animator>();
        gachaAnimPanel.SetActive(false);
        setPrice();
    }

/// -----------------------------------------------------------------------------------------------------------------
#region BTN EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickPurchaseBtn() {
        if(isGachaOn) return;

        PlayerSkin[] lockedPlSks = Array.FindAll(DB.Dt.PlSkins, pl => pl.IsLock && pl.Grade == Item.GRADE.Normal);
        PetSkin[] lockedPtSks = Array.FindAll(DB.Dt.PtSkins, pet => pet.IsLock && pet.Grade == Item.GRADE.Normal);
        if(lockedPlSks.Length == 0 && lockedPtSks.Length == 0) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Nothing more to buy."));
            return;
        } 

        if(DB.Dt.Coin >= price) {
            SM._.sfxPlay(SM.SFX.BubblePop.ToString());
            isGachaOn = true;
            HM._.ui.playSwitchScreenAnim();
            StartCoroutine(coPlayGachaPanelAnimIdle());
            DB.Dt.setCoin(-price);
            //* 上がる値段 最新化
            DB.Dt.GachaCnt++;
            StartCoroutine(coDelayUpdatePriceTxt());
            //* GachaAnimPanel 表示
            HM._.ui.TopGroup.SetActive(false);
        }
        else {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Not enough coin!"));
        }
    }
    private IEnumerator coDelayUpdatePriceTxt() {
        yield return Util.time1;
        setPrice();
    }
    private void setPrice() {
        price = Config.CLOTH_PRICE_UNIT * DB.Dt.GachaCnt;
        priceTxt.text = price.ToString();
    }
    public void onClickTapScreenBtn() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
        //* カーテン開ける アニメーション
        anim.SetBool(Enum.ANIM.IsShowGachaReward.ToString(), true);
        if(!rewardSpr) {
            SM._.sfxPlay(SM.SFX.Grinding.ToString(), delay: 0.45f);
            SM._.sfxPlay(SM.SFX.Tada.ToString(), delay: 2.5f);

            PetSkin[] lockedPtSks = Array.FindAll(DB.Dt.PtSkins, pet => pet.IsLock && pet.Grade == Item.GRADE.Normal);
            PlayerSkin[] lockedPlSks = Array.FindAll(DB.Dt.PlSkins, pl => pl.IsLock && pl.Grade == Item.GRADE.Normal);

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
            isGachaOn = false;
            anim.SetBool(Enum.ANIM.IsShowGachaReward.ToString(), false);
            rewardSpr = null;
            HM._.ui.TopGroup.SetActive(true);
            gachaAnimPanel.SetActive(false);
            //* 最新化 (習得したアイテムをアンロック)
            HM._.iUI.showItemList();
        }
    }
#endregion

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void setReward(PlayerSkin reward) {
        DB.Dt.AcvPetCnt++;
        reward.IsLock = false;
        reward.IsNotify = true;
        rewardSpr = reward.Spr;
        rewardNameTxt.text = LM._.localize(reward.Name);
    }
    public void setReward(PetSkin reward) {
        DB.Dt.AcvSkinCnt++;
        reward.IsLock = false;
        reward.IsNotify = true;
        rewardSpr = reward.Spr;
        rewardNameTxt.text = LM._.localize(reward.Name);
    }
    IEnumerator coPlayGachaPanelAnimIdle() {
        yield return Util.time0_5;
        gachaAnimPanel.SetActive(true); 
    }
#endregion
}
