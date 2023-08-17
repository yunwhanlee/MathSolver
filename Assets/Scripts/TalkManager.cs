using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TalkManager : MonoBehaviour {
    public enum SPK_IDX { //* Speaker Index
        Pl_Idle, pl_Happy, Pl_Sad,
        Animal_Idle, Animal_Happy, Animal_Sad,
        DotalMan, MoongMom, 
        Monkey_Idle, Monkey_Happy, Monkey_Sad,
        Flog_Idle, Flog_Happy, Flog_Sad, 
        Ant_Idle, Ant_Happy, Ant_Sad,
        WarriorMonkey_Idle, WarriorMonkey_Happy, WarriorMonkey_Sad,
        Monkey_God,
    };

    IEnumerator coTxtTeleTypeID;
    TextTeleType txtTeleType;
    [SerializeField] protected RectTransform talkFrameTf;

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
    //* スキップ
    public void onClickSkipBtn() {
        //! JUST FOR TEST : 診断評価ができなかったので、システム的によくない。
        if(DB.Dt.IsTutoRoomTrigger) {
            DB.Dt.IsTutoRoomTrigger = false;
            DB.Dt.IsTutoFunitureShopTrigger = false;
            DB.Dt.IsTutoClothShopTrigger = false;
            DB.Dt.IsTutoInventoryTrigger = false;
            DB.Dt.IsTutoGoGameTrigger = false;
            DB.Dt.IsTutoWorldMapTrigger = false;
            DB.Dt.IsTutoFinishTrigger = false;
            DB.Dt.IsTutoDiagChoiceDiffTrigger = false;
            DB.Dt.IsTutoDiagFirstQuizTrigger = false;
            DB.Dt.IsTutoDiagFirstAnswerTrigger = false;
            DB.Dt.IsTutoDiagResultTrigger = false;
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;
            curId = 5;
            talkDialog.SetActive(false);
        }
        else {
            //? これが有っている
            int lastIdx = talkDt[curId].Length - 1;
            Debug.Log($"TalkManager:: onClickSkipBtn():: talkDt[curId({curId})], lastIdx= {lastIdx}-> {talkDt[curId][lastIdx]}");
            if(talkIdx == 1) talkIdx = lastIdx;
            play();
        }
        
    }
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
        Debug.Log($"TalkManager:: action(id= {id})::");
        curId = id;
        play(); //* 最初スタート
    }

    private void play() {
        if(HM._ && HM._.state != HM.STATE.NORMAL) return;
        Debug.Log($"TalkManager:: play()::");
        talk(curId);
        talkDialog.SetActive(isAction);
    }

    private void talk(int id) {
        Debug.Log($"talk(id= {id}):: talkIdx= {talkIdx}");
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
            bool isFlip = false;
            if(spkKey.Contains("_")) {
                string[] spkKeyParts = spkKey.Split("_");
                if (spkKeyParts.Length >= 2) {
                    Debug.Log(spkKeyParts[0] + ", " + spkKeyParts[1]);
                    spkKey = spkKeyParts[0];
                    isFlip = spkKeyParts[1] == "FLIP";
                }
            }
            //* メッセージ
            talkTxt.text = msg;

            //* テレタイプ
            talkDialogAnim.SetTrigger(Enum.ANIM.DoTalk.ToString());
            if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID); //! 以前のコルーチンが生きていたら、停止
            coTxtTeleTypeID = txtTeleType.coTextVisible(talkTxt);
            StartCoroutine(coTxtTeleTypeID);

            //* スピーカー
            const int DEF_X = -340, DEF_Y = -500;
            const int DEF_SC = 2;
            int key = int.Parse(spkKey);
            var tf = spkImg.rectTransform;
            tf.anchoredPosition = new Vector2(DEF_X, DEF_Y); // Default Pos
            tf.localScale = new Vector2(DEF_SC, DEF_SC);
            
            switch(key) { // case 0: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_IDLE]; break;   // case 1: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_HAPPY]; break;  // case 2: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_SAD]; break;
                case (int)SPK_IDX.Pl_Idle: case (int)SPK_IDX.pl_Happy: case (int)SPK_IDX.Pl_Sad: 
                {
                    Debug.Log($"TalkManager:: talk():: spkImg.spr = {spkSprDtList[key]}");
                    //* 画像
                    spkImg.sprite = spkSprDtList[key];
                    spkImg.rectTransform.anchoredPosition = new Vector2(-Mathf.Abs(tf.anchoredPosition.x), tf.anchoredPosition.y);
                    if(key == (int)SPK_IDX.Pl_Sad) {
                        talkDialogAnim.SetTrigger(Enum.ANIM.DoShock.ToString());
                        Camera.main.GetComponent<Animator>().SetTrigger(Enum.ANIM.DoCamShake.ToString());
                    }
                    //* 名前
                    spkName.text = "늑선생"; 
                    setNameCardDir(isLeft: true); // left
                    break;
                }
                case (int)SPK_IDX.Animal_Idle: case (int)SPK_IDX.Animal_Happy: case (int)SPK_IDX.Animal_Sad:
                {
                    setOtherPortrait("동물친구", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.DotalMan: 
                {
                    setOtherPortrait("도톨아저씨", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.MoongMom: 
                {
                    setOtherPortrait("뭉이어멈", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.Monkey_Idle: case (int)SPK_IDX.Monkey_Happy: case (int)SPK_IDX.Monkey_Sad: 
                {
                    setOtherPortrait("원숭이", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.Flog_Idle: case (int)SPK_IDX.Flog_Happy: case (int)SPK_IDX.Flog_Sad: 
                {
                    setOtherPortrait("개구리", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.Ant_Idle: case (int)SPK_IDX.Ant_Happy: case (int)SPK_IDX.Ant_Sad: 
                {
                    setOtherPortrait("개미", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.WarriorMonkey_Idle: case (int)SPK_IDX.WarriorMonkey_Happy: case (int)SPK_IDX.WarriorMonkey_Sad:
                {
                    setOtherPortrait("전사원숭이", key, tf.anchoredPosition, isFlip);
                    break;
                }
                case (int)SPK_IDX.Monkey_God:
                {
                    tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 150);
                    setOtherPortrait("몽키신", key, tf.anchoredPosition, isFlip);
                    break;
                }
            }
        }
        
        //* 次の対話準備
        isAction = true;
        talkIdx++;
    }

    private void setOtherPortrait(string name, int key, Vector3 pos, bool isFlip) {
        //* 画像
        spkImg.sprite = spkSprDtList[key];
        float posX = Mathf.Abs(pos.x) * (isFlip? -1 : 1);
        spkImg.rectTransform.anchoredPosition = new Vector2(posX, pos.y);
        Vector3 sc = spkImg.transform.localScale;
        spkImg.transform.localScale = new Vector3(Mathf.Abs(sc.x) * (isFlip? -1 : 1) , sc.y, sc.z);
        //* 名前
        spkName.text = name;
        setNameCardDir(isLeft: isFlip); // right
    }

    protected string getMsg(int id, int talkIdx) {
        Debug.Log($"getMsg(id= {id}, talkIdx= {talkIdx})::");
        string[] msgs = talkDt[id];
        if(talkIdx == msgs.Length)
            return null;
        else 
            return msgs[talkIdx];
    }

    private void setNameCardDir(bool isLeft) {
        spkName.rectTransform.localScale = new Vector2((isLeft? 1 : -1) * Mathf.Abs(spkName.rectTransform.localScale.x), spkName.rectTransform.localScale.y);
        nameFrame.anchoredPosition = new Vector2((isLeft? -1 : 1) * Mathf.Abs(nameFrame.anchoredPosition.x), nameFrame.anchoredPosition.y);
        nameFrame.localScale = new Vector2((isLeft? 1 : -1) * Mathf.Abs(nameFrame.localScale.x), nameFrame.localScale.y);
    }
#endregion
}
