using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour {
    public enum TYPE {MainQuest, RepeatQuest};

    //* Value
    [SerializeField] TYPE type;                 public TYPE Type {get => type;}
    [SerializeField] int id;                    public int Id {get => id;}
    [SerializeField] string qName;              public string QName {get => qName;}
    [SerializeField] int clearCurVal;           public int ClearCurVal {get => clearCurVal; set => clearCurVal = value;}
    [SerializeField] int clearMaxVal;           public int ClearMaxVal {get => clearMaxVal;}
    

    //* UI
    [SerializeField] Image iconFrameImg;        public Image IconFrameImg {get => iconFrameImg;}
    [SerializeField] Image iconImg;             public Image IconImg {get => iconImg;}
    [SerializeField] TextMeshProUGUI titleTxt;  public TextMeshProUGUI TitleTxt {get => titleTxt;}
    [SerializeField] Slider statusGauge;        public Slider StatusGauge {get => statusGauge;}
    [SerializeField] TextMeshProUGUI cttTxt;    public TextMeshProUGUI CttTxt {get => cttTxt;}
    [SerializeField] Button acceptBtn;          public Button AcceptBtn {get => acceptBtn;}
    [SerializeField] Button rewardBtn;          public Button RewardBtn {get => rewardBtn;}

    void Awake() {
        //* Init
        // foreach (QuestManager.MQ_ID mqID in System.Enum.GetValues(typeof(QuestManager.MQ_ID))) // オブジェクト名とMQ_IDのリスト名と同じくすること
        //     if(this.name == mqID.ToString()) id = (int)mqID;
        statusGauge.maxValue = clearMaxVal;
        rewardBtn.onClick.AddListener(() => onClickRewardBtn(id));
    }

    void OnEnable() {
        updateStatusGauge();
    }
/// -----------------------------------------------------------------------------------------------------------------
#region EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickAcceptBtn() => acceptQuest();
    public void onClickRewardBtn(int id) => HM._.qm.getReward(id);
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------


    public void acceptQuest() {
        acceptBtn.gameObject.SetActive(false);
        rewardBtn.gameObject.SetActive(true);
        // rewardBtn.interactable = false;
        updateStatusGauge();
    }
    private void setStatusGauge(int val) {
        clearCurVal = val;
        statusGauge.value = clearCurVal;
        //* リワードボタン 活性化
        rewardBtn.interactable = clearCurVal >= clearMaxVal;
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region QUEST ID : STATUS
/// -----------------------------------------------------------------------------------------------------------------
    private void updateStatusGauge() {
        Debug.Log($"Quest:: OnEnable():: qName={qName} == QuestManager.MQ_ID.Tutorial.ToString()={QuestManager.MQ_ID.Tutorial.ToString()}");
        switch(DB.Dt.MainQuestID) {
            case (int)QuestManager.MQ_ID.Tutorial:
                setStatusGauge(getTutoClearVal());
                break;
            case (int)QuestManager.MQ_ID.UnlockMap1Windmill:
            case (int)QuestManager.MQ_ID.UnlockMap1Orchard:
            case (int)QuestManager.MQ_ID.OpenJungleMap2:
            case (int)QuestManager.MQ_ID.UnlockMap2Bush:
            case (int)QuestManager.MQ_ID.UnlockMap2MoneyWat:
            case (int)QuestManager.MQ_ID.OpenTundraMap3:
            case (int)QuestManager.MQ_ID.UnlockMap3SnowMountain:
            case (int)QuestManager.MQ_ID.UnlockMap3IceDragon:
                setStatusGauge(DB.Dt.Lv);
                break;
        }
    }
    private int getTutoClearVal() {
        int res = 0;
        var dt = DB.Dt;
        if(!dt.IsTutoRoomTrigger) res++;
        if(!dt.IsTutoFunitureShopTrigger) res++;
        if(!dt.IsTutoClothShopTrigger) res++;
        if(!dt.IsTutoInventoryTrigger) res++;
        if(!dt.IsTutoGoGameTrigger) res++;
        if(!dt.IsTutoWorldMapTrigger) res++;
        if(!dt.IsTutoDiagChoiceDiffTrigger) res++;
        if(!dt.IsTutoDiagFirstQuizTrigger) res++;
        if(!dt.IsTutoDiagFirstAnswerTrigger) res++;
        if(!dt.IsTutoDiagResultTrigger) res++;
        // if(!dt.IsTutoFinishTrigger) res++;
        Debug.Log($"getTutoClearVal():: res= {res}");
        return res;
    }
#endregion
}
