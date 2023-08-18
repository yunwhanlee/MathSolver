using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public abstract class TalkManager : MonoBehaviour {
    public enum SPK_IDX { //* Speaker Index
        Empty = -1,
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
    [SerializeField] protected Image[] spkImgs;
    [SerializeField] protected RectTransform nameFrame;
    [SerializeField] protected TextMeshProUGUI spkName;

    protected void Awake() {
        txtTeleType = GetComponent<TextTeleType>();
        talkDialogAnim = talkDialog.GetComponent<Animator>();
        talkDt = new Dictionary<int, string[]>(); //* 対話データ
        generateData();
    }

    /// <summary>
    /// 【対話の書き方】 "{Msg}:{数}:{数}:{数}_FLIP"
    /// 「:」 => Speaker区切り単位 (最大３まで可能)。
    /// 「{数}」　=> Speaker種類 (enum SPK_IDX)。
    /// 「_FLIP」 => 左右反転 Default (Player左)、(Animal右)。
    /// </summary>
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
            string[] splits = rawMsg.Split(":");
            
            string msg = splits[0];
            string[] spkKeys = splits.Skip(1).ToArray(); //* [0]Index除外
            bool[] isFlips = new bool[spkKeys.Length];
            Debug.Log($"msg= {msg}, spkKeys.len= {spkKeys.Length}, isFlips.len= {isFlips.Length}");
            Array.ForEach(spkKeys, key => Debug.Log("key: " + key));

            //* メッセージ
            talkTxt.text = msg;

            //* テレタイプ
            talkDialogAnim.SetTrigger(Enum.ANIM.DoTalk.ToString());
            if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID); //! 以前のコルーチンが生きていたら、停止
            coTxtTeleTypeID = txtTeleType.coTextVisible(talkTxt);
            StartCoroutine(coTxtTeleTypeID);

            //* スピーカー (複数：最大３まで可能)
            int[] DEF_X_ARR = new int[] {-340, -220, 0};
            const int DEF_Y = -500;
            const int DEF_SC = 2;

            //* 初期化
            Array.ForEach(spkImgs, spkImg => spkImg.enabled = false); // 全て非表示
            nameFrame.gameObject.SetActive(true); // 表示

            int i = 0;
            Array.ForEach(spkKeys, spkKey => {
                int DEF_X = DEF_X_ARR[i];
                Image spkImg = spkImgs[i];
                bool isFlip = isFlips[i];

                spkImg.enabled = true; //* 表示

                if(spkKey.Contains("_")) {
                    string[] parts = spkKey.Split("_");
                    if (parts.Length >= 2) {
                        Debug.Log(parts[0] + ", " + parts[1]);
                        spkKey = parts[0];
                        isFlip = parts[1] == "FLIP";
                    }
                }

                int key = int.Parse(spkKey);
                var tf = spkImg.rectTransform;
                tf.anchoredPosition = new Vector2(DEF_X, DEF_Y); // Default Pos
                tf.localScale = new Vector2(DEF_SC, DEF_SC);
                
                switch(key) { // case 0: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_IDLE]; break;   // case 1: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_HAPPY]; break;  // case 2: spkImg.sprite = spkSprDtList[(int)SPK_IDX.PL_SAD]; break;
                    case (int)SPK_IDX.Empty: {
                        spkImg.enabled = false;
                        nameFrame.gameObject.SetActive(false);
                        break;
                    }
                    case (int)SPK_IDX.Pl_Idle: case (int)SPK_IDX.pl_Happy: case (int)SPK_IDX.Pl_Sad: {
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
                    case (int)SPK_IDX.Animal_Idle: case (int)SPK_IDX.Animal_Happy: case (int)SPK_IDX.Animal_Sad: {
                        setOtherPortrait(spkImg, "동물친구", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.DotalMan: {
                        setOtherPortrait(spkImg, "도톨아저씨", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.MoongMom: {
                        setOtherPortrait(spkImg, "뭉이어멈", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.Monkey_Idle: case (int)SPK_IDX.Monkey_Happy: case (int)SPK_IDX.Monkey_Sad: {
                        setOtherPortrait(spkImg, "원숭이", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.Flog_Idle: case (int)SPK_IDX.Flog_Happy: case (int)SPK_IDX.Flog_Sad: {
                        setOtherPortrait(spkImg, "개구리", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.Ant_Idle: case (int)SPK_IDX.Ant_Happy: case (int)SPK_IDX.Ant_Sad: {
                        setOtherPortrait(spkImg, "개미", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.WarriorMonkey_Idle: case (int)SPK_IDX.WarriorMonkey_Happy: case (int)SPK_IDX.WarriorMonkey_Sad:{
                        setOtherPortrait(spkImg, "전사원숭이", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK_IDX.Monkey_God: {
                        tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 150);
                        setOtherPortrait(spkImg, "몽키신", key, tf.anchoredPosition, isFlip);
                        break;
                    }
                }
                i++;
            });
        }
        //* 次の対話準備
        isAction = true;
        talkIdx++;
    }

    private void setOtherPortrait(Image spkImg, string name, int key, Vector3 pos, bool isFlip) {
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
