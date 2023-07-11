using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TalkManager : MonoBehaviour {
    public enum SPEAKER_IDX {FRONTOOTH_BOY, PLAYER};

    //* DATA
    Dictionary<int, string[]> talkDt;
    [SerializeField] List<Sprite> speakerSprDtList;

    //* Speaker Spr
    [SerializeField] Sprite frontoothBoySpr;

    //* Value
    [SerializeField] bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] int curId;
    [SerializeField] int talkIdx;
    [SerializeField] GameObject talkDialog;
    [SerializeField] TextMeshProUGUI talkTxt;
    [SerializeField] Image speakerImg;

    protected void Awake() {
        //* 対話データ
        talkDt = new Dictionary<int, string[]>();
        generateData();
    }

    protected void Start() {
        //* キャラの画像データ
        speakerSprDtList = new List<Sprite>();
        speakerSprDtList.Add(frontoothBoySpr);
        speakerSprDtList.Add(HM._.pl.IdleSpr);
    }

    protected abstract void generateData();

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
    public void action(int id) {
        curId = id;
        play(); //* 最初スタート
    }
    public void play() {
        talk(curId);
        talkDialog.SetActive(isAction);
    }
    private string getMsg(int id, int talkIdx) {
        string[] msgs = talkDt[id];
        if(talkIdx == msgs.Length)
            return null;
        else 
            return msgs[talkIdx];
    }

    private void talk(int id) {
        string rawMsg = getMsg(id, talkIdx);

        //* 対話 終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 追加処理
            endSwitchProccess(id);
            return;
        }
        //* 対話表示
        else {
            Time.timeScale = 0;
            //* 分析 「メッセージ」と「スピーカー画像」
            string msg = rawMsg.Split(":")[0];
            string spkKey = rawMsg.Split(":")[1];
            //* メッセージ
            talkTxt.text = msg;

            //* スピーカー画像
            switch(int.Parse(spkKey)) {
                case (int)SPEAKER_IDX.FRONTOOTH_BOY: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.FRONTOOTH_BOY]; 
                    break;
                case (int)SPEAKER_IDX.PLAYER: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.PLAYER]; 
                    break;
            }
        }
        
        //* 次の対話準備
        isAction = true;
        talkIdx++;
    }

    public abstract void endSwitchProccess(int id);



#endregion
}
