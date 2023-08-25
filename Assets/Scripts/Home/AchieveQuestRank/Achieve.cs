using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achieve : MonoBehaviour {
    [Header("VALUE")]
    [SerializeField] int id;            public int Id {get => id;} //* 業績
    [SerializeField] int lv;            public int Lv {get => lv;} //* 段階
    [SerializeField] string name;              public string Name {get => base.name;}
    // [HideInInspector] string contentStr;         public string ContentStr {get => contentStr;}
    [SerializeField] int clearCurVal;           public int ClearCurVal {get => clearCurVal; set => clearCurVal = value;}
    [SerializeField] int[] clearMaxVals;           public int[] ClearMaxVals {get => clearMaxVals;}
    [SerializeField] int rewardCoinUnit = 100;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI titleTxt;    public TextMeshProUGUI TitleTxt {get => titleTxt;}
    [SerializeField] Slider statusGauge;        public Slider StatusGauge {get => statusGauge;}
    [SerializeField] TextMeshProUGUI cttTxt;   public TextMeshProUGUI CttTxt {get => cttTxt; set => cttTxt = value;}
    [SerializeField] Button rewardBtn;          public Button RewardBtn {get => rewardBtn;}
    [SerializeField] Image rewardIconImg;          public Image RewardIconImg {get => rewardIconImg;}
    [SerializeField] TextMeshProUGUI priceTxt;          public TextMeshProUGUI PriceTxt {get => priceTxt;}

    void OnEnable() => updateLvAndStatusGauge();

/// -----------------------------------------------------------------------------------------------------------------
#region EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickRewardBtn() => getReward();
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region UPDATE
/// -----------------------------------------------------------------------------------------------------------------
    public bool updateLvAndStatusGauge() {
        bool isAcceptable = false;
        var dt = DB.Dt;
        switch(id) {
            case (int)AchieveManager.ID.CorrectAnswerCnt:
                isAcceptable = setAchievement(dt.AcvCorrectAnswerLv, dt.AcvCorrectAnswerCnt);
                break;
            case (int)AchieveManager.ID.SkinCnt:
                isAcceptable = setAchievement(dt.AcvSkinLv, dt.AcvSkinCnt);
                break;
            case (int)AchieveManager.ID.PetCnt:
                isAcceptable = setAchievement(dt.AcvPetLv, dt.AcvPetCnt);
                break;
            case (int)AchieveManager.ID.CoinAmount:
                isAcceptable = setAchievement(dt.AcvCoinAmountLv, dt.AcvCoinAmount);
                break;
        }
        return isAcceptable;
    }
    private bool setAchievement(int lv, int CurVal) {
        bool isAcceptable = false;
        int maxLv = clearMaxVals.Length + 1;
        lv = Mathf.Clamp(lv, 1, maxLv);
        int ldx = lv - 1;

        //* Done (No More Levvel)
        if(ldx >= clearMaxVals.Length) {
            rewardIconImg.gameObject.SetActive(false);
            rewardBtn.interactable = false;
            rewardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "<color=white>Done</color>";
            rewardBtn.GetComponent<Image>().color = Color.gray;
            cttTxt.text = "";
        }
        else {
            int max = clearMaxVals[ldx];
            clearCurVal = CurVal;
            statusGauge.value = (float)clearCurVal / max;
            isAcceptable = (clearCurVal >= max);

            cttTxt.text = $" {clearCurVal} / {max}";
            priceTxt.text = $"{rewardCoinUnit * lv}";
            rewardBtn.interactable = isAcceptable;
        }
        //* ログ
        string log = $"updateLvAndStatusGauge():: setAchievement():: {name}: lv= {lv}, CurVal= {CurVal}";
        Debug.Log(isAcceptable? $"<color=blue>{log}</color>" : log);
        return isAcceptable;
    }
/// -----------------------------------------------------------------------------------------------------------------
#region REWARD
/// -----------------------------------------------------------------------------------------------------------------
    public void getReward() {
        var dt = DB.Dt;
        switch(id) {
            case (int)AchieveManager.ID.CorrectAnswerCnt:
                setRewardList(dt.AcvCorrectAnswerLv++ * rewardCoinUnit);
                break;
            case (int)AchieveManager.ID.SkinCnt:
                setRewardList(dt.AcvSkinLv++ * rewardCoinUnit);
                break;
            case (int)AchieveManager.ID.PetCnt:
                setRewardList(dt.AcvPetLv++ * rewardCoinUnit);
                break;
            case (int)AchieveManager.ID.CoinAmount:
                setRewardList(dt.AcvCoinAmountLv++ * rewardCoinUnit);
                break;
        }
        updateLvAndStatusGauge(); //* 最新化
    }

    private void setRewardList(int coinVal) {
        //* Add Reward List
        var rwdList = new Dictionary<RewardItemSO, int> {
            { HM._.ui.RwdSOList[(int)Enum.RWD_IDX.Coin], coinVal},
        };
        StartCoroutine(HM._.ui.coActiveRewardPopUp(fame: 5, rwdList));
    }
#endregion
#endregion
}
