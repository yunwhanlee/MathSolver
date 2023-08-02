using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour {
    //* Value
    [SerializeField] string qName;              public string QName {get => qName;}
    [SerializeField] int clearCurVal;           public int ClearCurVal {get => clearCurVal; set => clearCurVal = value;}
    [SerializeField] int clearMaxVal;           public int ClearMaxVal {get => clearMaxVal;}

    [SerializeField] Image iconFrameImg;        public Image IconFrameImg {get => iconFrameImg;}
    [SerializeField] Image iconImg;             public Image IconImg {get => iconImg;}
    [SerializeField] TextMeshProUGUI titleTxt;  public TextMeshProUGUI TitleTxt {get => titleTxt;}
    [SerializeField] Slider statusGauge;        public Slider StatusGauge {get => statusGauge;}
    [SerializeField] TextMeshProUGUI cttTxt;    public TextMeshProUGUI CttTxt {get => cttTxt;}
    [SerializeField] Button acceptBtn;          public Button AcceptBtn {get => acceptBtn;}
    [SerializeField] Button rewardBtn;          public Button RewardBtn {get => rewardBtn;}

    void Awake() {
        //* Init
        qName = this.gameObject.name;
        statusGauge.maxValue = clearMaxVal;
    }

    void OnEnable() {
        Debug.Log($"Quest:: qName={qName} OnEnable()");
        if(qName == QuestManager.MQ_ID.Tutorial.ToString()) {
            clearCurVal = setTutorialClearVal();
            statusGauge.value = clearCurVal;
            //* リワードボタン 活性化
            if(clearCurVal == clearMaxVal) rewardBtn.interactable = true;
        }
        else if(qName == QuestManager.MQ_ID.Tutorial.ToString()) {
            //TODO
        }
    }
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void acceptQuest() {
        acceptBtn.gameObject.SetActive(false);
        rewardBtn.gameObject.SetActive(true);
        rewardBtn.interactable = false;
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region QUEST ID (CLEAR VALUE)
/// -----------------------------------------------------------------------------------------------------------------
    private int setTutorialClearVal() {
        int res = 0;
        var dt = DB.Dt;
        if(!dt.IsTutoRoomTrigger) res++;
        if(!dt.IsTutoFunitureShopTrigger) res++;
        if(!dt.IsTutoClothShopTrigger) res++;
        if(!dt.IsTutoInventoryTrigger) res++;
        if(!dt.IsTutoGoGameTrigger) res++;
        if(!dt.IsTutoWorldMapTrigger) res++;
        if(!dt.IsTutoFinishTrigger) res++;
        if(!dt.IsTutoDiagChoiceDiffTrigger) res++;
        if(!dt.IsTutoDiagFirstQuizTrigger) res++;
        if(!dt.IsTutoDiagFirstAnswerTrigger) res++;
        if(!dt.IsTutoDiagResultTrigger) res++;

        return res;
    }
#endregion
}
