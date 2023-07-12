using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TalkManager : MonoBehaviour {
    public enum SPEAKER_IDX {PLAYER};

    //* DATA
    protected Dictionary<int, string[]> talkDt;
    protected List<Sprite> speakerSprDtList;

    //* Speaker Spr

    //* Value
    [SerializeField] protected bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] protected int curId;
    [SerializeField] protected int talkIdx;
    [SerializeField] protected GameObject talkDialog;
    [SerializeField] protected TextMeshProUGUI talkTxt;
    [SerializeField] protected Image speakerImg;

    protected void Awake() {
        //* 対話データ
        talkDt = new Dictionary<int, string[]>();
        generateData();
    }

    protected void Start() {
        //* キャラの画像データ
        speakerSprDtList = new List<Sprite>();
        speakerSprDtList.Add(HM._.pl.IdleSpr);
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
    //* 途中 処理
    protected virtual string processMsg(int id) {
        Debug.Log("processMsg:: TalkManager::");
        return getMsg(id, talkIdx);
    }
    //* 終了 処理
    protected abstract void endSwitchProccess(int id); 

    public void action(int id) {
        curId = id;
        play(); //* 最初スタート
    }

    private void play() {
        talk(curId);
        talkDialog.SetActive(isAction);
    }

    private void talk(int id) {
        //* 途中 処理
        string rawMsg = processMsg(id);

        //* 終了 処理
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 追加処理
            endSwitchProccess(id);
            return;
        }
        //* 対話 表示
        else {
            Time.timeScale = 0;
            //* 分析 「メッセージ」と「スピーカー画像」
            string msg = rawMsg.Split(":")[0];
            string spkKey = rawMsg.Split(":")[1];
            //* メッセージ
            talkTxt.text = msg;

            //* スピーカー画像
            switch(int.Parse(spkKey)) {
                case (int)SPEAKER_IDX.PLAYER: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.PLAYER]; 
                    break;
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
