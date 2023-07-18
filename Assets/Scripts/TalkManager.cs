using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TalkManager : MonoBehaviour {
    public enum SPK_IDX { //* Speaker Index
        PL_IDLE, PL_HAPPY, PL_SAD
    };

    //* DATA
    protected Dictionary<int, string[]> talkDt;
    [SerializeField] protected List<Sprite> spkSprDtList; //* Inspector viewで

    //* Value
    [SerializeField] protected bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] protected int curId;
    [SerializeField] protected int talkIdx;
    [SerializeField] protected GameObject talkDialog;
    [SerializeField] protected TextMeshProUGUI talkTxt;
    [SerializeField] protected Image spkImg;

    protected void Awake() {
        //* 対話データ
        talkDt = new Dictionary<int, string[]>();
        generateData();
    }

    public abstract void generateData();

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    //* TalkDialogのPlayActionBtnへ張り付ける
    public void onClickPlayActionBtn() => play();
    //* 対話開始をボタンイベントでする時、使います
    public void onClickRegistActionBtn(int id) => action(id);
#endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected virtual string setEvent(int id) { //* イベント 処理
        Debug.Log("processMsg:: setEvent:: override必要！");
        return getMsg(id, talkIdx);
    }
    protected abstract void endSwitchProccess(int id); //* 終了 処理

    public void action(int id) {
        curId = id;
        play(); //* 最初スタート
    }

    private void play() {
        if(HM._.state != HM.STATE.NORMAL) return;
        talk(curId);
        talkDialog.SetActive(isAction);
    }

    private void talk(int id) {
        //* イベント 処理
        string rawMsg = setEvent(id);

        //* 対話 終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 終了 処理
            endSwitchProccess(id);
            return;
        }
        //* 対話 続き
        else {
            Time.timeScale = 0;
            //* 分析 :「メッセージ」と「スピーカー画像」
            string msg = rawMsg.Split(":")[0];
            string spkKey = rawMsg.Split(":")[1];
            //* メッセージ
            talkTxt.text = msg;

            //* スピーカー画像
            switch(int.Parse(spkKey)) {
                case 0: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_IDLE]; break;
                case 1: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_HAPPY]; break;
                case 2: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_SAD]; break;
            }
        }
        
        //* 次の対話準備
        isAction = true;
        talkIdx++;
    }

    protected string getMsg(int id, int talkIdx) {
        string[] msgs = talkDt[id];
        if(talkIdx == msgs.Length)
            return null;
        else 
            return msgs[talkIdx];
    }
#endregion
}
