using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TalkManager : MonoBehaviour {
    public enum SPK_IDX { //* Speaker Index
        PL_IDLE, PL_HAPPY, PL_SAD
    };

    IEnumerator coTxtTeleTypeID;
    TextTeleType txtTeleType;

    //* DATA
    protected Dictionary<int, string[]> talkDt;
    [SerializeField] protected List<Sprite> spkSprDtList; //* Inspector viewで

    //* Value
    [SerializeField] protected bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] protected int curId;
    [SerializeField] protected int talkIdx;
    [SerializeField] protected GameObject talkDialog;
    private Animator talkDialogAnim;
    [SerializeField] protected TextMeshProUGUI talkTxt;
    [SerializeField] protected Image spkImg;
    [SerializeField] protected RectTransform nameFrame;
    [SerializeField] protected TextMeshProUGUI spkName;

    protected void Awake() {
        txtTeleType = GetComponent<TextTeleType>();
        talkDialogAnim = talkDialog.GetComponent<Animator>();
        talkDt = new Dictionary<int, string[]>(); //* 対話データ
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

            //* テレタイプ
            talkDialogAnim.SetTrigger(Enum.ANIM.DoTalk.ToString());
            if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID); //! 以前のコルーチンが生きていたら、停止
            coTxtTeleTypeID = txtTeleType.coTextVisible(talkTxt);
            StartCoroutine(coTxtTeleTypeID);

            //* スピーカー
            int key = int.Parse(spkKey);
            var pos = spkImg.rectTransform.anchoredPosition;
            //* 画像
            switch(key) { // case 0: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_IDLE]; break;   // case 1: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_HAPPY]; break;  // case 2: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_SAD]; break;
                case 0: case 1: case 2: 
                    spkImg.sprite = spkSprDtList[key];
                    spkImg.rectTransform.anchoredPosition = new Vector2(-Mathf.Abs(pos.x), pos.y);
                    break;
                case 3: 
                    spkImg.sprite = GM._.Anm.SprLib.GetSprite("Idle", "Entry"); 
                    spkImg.rectTransform.anchoredPosition = new Vector2(Mathf.Abs(pos.x), pos.y);
                    break;
            }
            //* 名前
            switch(key) {
                case 0: case 1: case 2: 
                    spkName.text = "송백 늑선생"; 
                    setNameFrameDirection(isLeft: true); // left
                    break;
                case 3:
                    spkName.text = "동물친구1";
                    setNameFrameDirection(isLeft: false); // right
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

    private void setNameFrameDirection(bool isLeft) {
        spkName.rectTransform.localScale = new Vector2((isLeft? 1 : -1) * Mathf.Abs(spkName.rectTransform.localScale.x), spkName.rectTransform.localScale.y);
        nameFrame.anchoredPosition = new Vector2((isLeft? -1 : 1) * Mathf.Abs(nameFrame.anchoredPosition.x), nameFrame.anchoredPosition.y);
        nameFrame.localScale = new Vector2((isLeft? 1 : -1) * Mathf.Abs(nameFrame.localScale.x), nameFrame.localScale.y);
    }
#endregion
}
