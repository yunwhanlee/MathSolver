using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MGUI : MonoBehaviour {
    [SerializeField] Button startBtn;
    [SerializeField] TextMeshProUGUI scoreTxt;   public TextMeshProUGUI ScoreTxt {get => scoreTxt;}
    [SerializeField] TextMeshProUGUI playTimerTxt;  public TextMeshProUGUI PlayTimerTxt {get => playTimerTxt;}
    [SerializeField] TextMeshProUGUI titleTxt;      public TextMeshProUGUI TitleTxt {get => titleTxt;}
    [SerializeField] TextMeshProUGUI contentTxt;    public TextMeshProUGUI ContentTxt {get => contentTxt;}

    void Start() {
        scoreTxt.text = "";
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickStartBtn() {
        Debug.Log("onClickStartBtn():: Status: READY ➝ PLAY");
        MGM._.Status = MGM.STATUS.PLAY;
        startBtn.gameObject.SetActive(false);
        titleTxt.gameObject.SetActive(false);
        contentTxt.gameObject.SetActive(false);
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
#endregion
}
