using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public abstract class TalkManager : MonoBehaviour {
    public enum SPK { //* Speaker Index
        Empty = -1,
        Pl_Idle, Pl_Happy, Pl_Sad,
        Mole_Idle, Mole_Happy, Mole_Sad,
        Bear_Idle, Bear_Happy, Bear_Sad,
        Duck_Happy, DotalMan, MoongMom,
        Monkey_Idle, Monkey_Happy, Monkey_Sad,
        Frog_Idle, Frog_Happy, Frog_Sad, 
        Ant_Idle, Ant_Happy, Ant_Sad,
        WarriorMonkey_Idle, WarriorMonkey_Happy, WarriorMonkey_Sad,
        Monkey_God,
        Seal_Idle, Seal_Happy, Seal_Sad,
        TundraBear_Idle, TundraBear_Happy, TundraBear_Sad,
        SnowRabbit_Idle, SnowRabbit_Happy, SnowRabbit_Sad,
        BabyDragon,
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
    [SerializeField] protected Image bgImg;
    
    private Animator talkDialogAnim;
    [SerializeField] protected TextMeshProUGUI talkTxt;
    [SerializeField] protected Image[] spkImgs;
    [SerializeField] protected RectTransform nameFrame;
    [SerializeField] protected TextMeshProUGUI spkName;

    [SerializeField] TMP_FontAsset notoSansKR;
    [SerializeField] TMP_FontAsset notoSansSC;

    protected void Awake() {
        txtTeleType = GetComponent<TextTeleType>();
        talkDialogAnim = talkDialog.GetComponent<Animator>();
        talkDt = new Dictionary<int, string[]>(); //* 対話データ
        generateData();
    }
    void Start() {
        //* Update Player Portraite Sprites
        // Debug.Log("Pl_Idle= " + HM._.pl.SprLib.spriteLibraryAsset.GetSprite("Idle", "Entry"));
        // Debug.Log("Pl_Success= " + HM._.pl.SprLib.spriteLibraryAsset.GetSprite("Success", "Entry"));
        // Debug.Log("Pl_Fail= " + HM._.pl.SprLib.spriteLibraryAsset.GetSprite("Fail", "Entry"));
        setSpkPortrait();
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
            if(talkIdx < lastIdx) talkIdx = lastIdx;
            play();
        }
        
    }
#endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected abstract void setBG();

    protected virtual string setEvent(int id) { //* イベント 処理
        Debug.Log("processMsg:: setEvent:: override必要！");
        return getMsg(id, talkIdx);
    }
    protected abstract void endSwitchProccess(int id); //* 終了 処理

    public void setSpkPortrait() {
        var plSprLibAsset = HM._.pl.SprLib.spriteLibraryAsset;
        if(HM._) plSprLibAsset = HM._.pl.SprLib.spriteLibraryAsset;
        else if(GM._) plSprLibAsset = GM._.Pl.SprLib.spriteLibraryAsset;
        spkSprDtList[(int)SPK.Pl_Idle] = plSprLibAsset.GetSprite("Idle", "Entry");
        spkSprDtList[(int)SPK.Pl_Happy] = plSprLibAsset.GetSprite("Success", "Entry");
        spkSprDtList[(int)SPK.Pl_Sad] = plSprLibAsset.GetSprite("Fail", "Entry");
    }

    public void action(int id) {
        Debug.Log($"TalkManager:: action(id= {id})::");
        curId = id;
        setBG();
        play(); //* 最初スタート
    }

    private void play() {
        if(HM._ && HM._.state != HM.STATE.NORMAL) return;
        Debug.Log($"TalkManager:: play():: talkTxt.font= " + talkTxt.font);
        
        if(LM._.curLangIndex == (int)LM.LANG_IDX.JP) {
            if(talkTxt.font != notoSansSC)  talkTxt.font = notoSansSC;
        }
        else {
            if(talkTxt.font != notoSansKR)  talkTxt.font = notoSansKR;
        }

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
            // int spkId = int.Parse(spkKeys[0]);
            // var enumValArr = (SPK[])System.Enum.GetValues(typeof(SPK));
            // SPK spk = enumValArr[int.Parse(spkKeys[0])];
            Debug.Log($"talk():: msg= {msg}, spkKeys.len= {spkKeys.Length}, isFlips.len= {isFlips.Length}");
            Array.ForEach(spkKeys, key => Debug.Log("talk():: spkKey: " + key));

            //* メッセージ
            if(msg.Contains("NICKNAME")) {//* 例外
                string nickname = msg.Replace("NICKNAME", "");
                talkTxt.text = nickname;
            }
            else {
                talkTxt.text = LM._.localize(msg, (int)LM.LANG_IDX.KR); //? 僕は韓国人だから、クエストは韓国語を基準で言語変換する。
                // talkTxt.text = msg;
            }

            //* テレタイプ
            talkDialogAnim.SetTrigger(Enum.ANIM.DoTalk.ToString());
            if(coTxtTeleTypeID != null) {
                Debug.Log("TalkManager:: STOP Before talk -> StopCoroutine(coTxtTeleTypeID)");
                SM._.disableTalk();
                StopCoroutine(coTxtTeleTypeID); //! 以前のコルーチンが生きていたら、停止
            } 
            // 転換
            string[] spkIDStrs = spkKeys.Select(str => str.Contains("_") ? str.Split('_')[0] : str).ToArray();
            int[] spkIDs = Array.ConvertAll(spkIDStrs, int.Parse);
            Array.ForEach(spkIDs, spk => Debug.Log("talk():: spkID: " + spk));
            int spk = spkIDs[0];
            // bool isPlayer = Array.Exists(spkIDs, id => (id <= (int)TalkManager.SPK.Pl_Sad));
            //*適用
            string voice = (spk == (int)SPK.Empty
                || spk == (int)SPK.Pl_Idle || spk == (int)SPK.Pl_Happy || spk == (int)SPK.Pl_Sad)? 
                    SM.SFX.Talk.ToString()
                : (spk == (int)SPK.DotalMan
                || spk == (int)SPK.WarriorMonkey_Idle || spk == (int)SPK.WarriorMonkey_Happy || spk == (int)SPK.WarriorMonkey_Sad
                || spk == (int)SPK.SnowRabbit_Idle || spk == (int)SPK.SnowRabbit_Happy || spk == (int)SPK.SnowRabbit_Sad)? 
                    SM.SFX.Talk3.ToString()
                : (spk == (int)SPK.Monkey_God)?
                    SM.SFX.Talk4.ToString()
                : (spk == (int)SPK.Mole_Idle || spk == (int)SPK.Mole_Happy || spk == (int)SPK.Mole_Sad
                || spk == (int)SPK.Ant_Idle || spk == (int)SPK.Ant_Happy || spk == (int)SPK.Ant_Sad
                || spk == (int)SPK.BabyDragon)?
                    SM.SFX.Talk5.ToString() 
                : SM.SFX.Talk2.ToString(); //* 残りは全て
            SM._.enabledTalk(voice);
            coTxtTeleTypeID = txtTeleType.coTextVisible(talkTxt, voice);
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
                    case (int)SPK.Empty: {
                        spkImg.enabled = false;
                        nameFrame.gameObject.SetActive(false);
                        break;
                    }
                    case (int)SPK.Pl_Idle: case (int)SPK.Pl_Happy: case (int)SPK.Pl_Sad: {
                        Debug.Log($"TalkManager:: talk():: spkImg.spr = {spkSprDtList[key]}");
                        //* 画像
                        spkImg.sprite = spkSprDtList[key];
                        spkImg.rectTransform.anchoredPosition = new Vector2(-Mathf.Abs(tf.anchoredPosition.x), tf.anchoredPosition.y);
                        if(key == (int)SPK.Pl_Sad) {
                            SM._.sfxPlay(SM.SFX.WrongAnswer.ToString());
                            talkDialogAnim.SetTrigger(Enum.ANIM.DoShock.ToString());
                            Camera.main.GetComponent<Animator>().SetTrigger(Enum.ANIM.DoCamShake.ToString());
                        }
                        //* 名前
                        setOtherPortrait(spkImg, LM._.localize("Teacher Wolf"), key, tf.anchoredPosition, !isFlip, isPlayer: true);
                        break;
                    }
                    case (int)SPK.Mole_Idle: case (int)SPK.Mole_Happy: case (int)SPK.Mole_Sad: {
                        setOtherPortrait(spkImg, LM._.localize("Mole"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Bear_Idle: case (int)SPK.Bear_Happy: case (int)SPK.Bear_Sad: {
                        setOtherPortrait(spkImg, LM._.localize("Bear"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Duck_Happy: {
                        setOtherPortrait(spkImg, LM._.localize("Duck"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.DotalMan: {
                        setOtherPortrait(spkImg, LM._.localize("Uncle Dotol"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.MoongMom: {
                        setOtherPortrait(spkImg, LM._.localize("Moong Mom"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Monkey_Idle: case (int)SPK.Monkey_Happy: case (int)SPK.Monkey_Sad: {
                        setOtherPortrait(spkImg, LM._.localize("Monkey"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Frog_Idle: case (int)SPK.Frog_Happy: case (int)SPK.Frog_Sad: {
                        setOtherPortrait(spkImg, LM._.localize("Frog"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Ant_Idle: case (int)SPK.Ant_Happy: case (int)SPK.Ant_Sad: {
                        tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 50);
                        setOtherPortrait(spkImg, LM._.localize("Ant"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.WarriorMonkey_Idle: case (int)SPK.WarriorMonkey_Happy: case (int)SPK.WarriorMonkey_Sad:{
                        setOtherPortrait(spkImg, LM._.localize("Warroir Monkey"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Monkey_God: {
                        tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 150);
                        setOtherPortrait(spkImg, LM._.localize("God Monkey"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.Seal_Idle: case (int)SPK.Seal_Happy: case (int)SPK.Seal_Sad: {
                        tf.anchoredPosition = new Vector2(DEF_X + 50, DEF_Y);
                        setOtherPortrait(spkImg, LM._.localize("Seal"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.TundraBear_Idle: case (int)SPK.TundraBear_Happy: case (int)SPK.TundraBear_Sad: {
                        setOtherPortrait(spkImg, LM._.localize("Bear"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.SnowRabbit_Idle: case (int)SPK.SnowRabbit_Happy: case (int)SPK.SnowRabbit_Sad: {
                        tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 50);
                        setOtherPortrait(spkImg, LM._.localize("Snow Rabbit"), key, tf.anchoredPosition, isFlip);
                        break;
                    }
                    case (int)SPK.BabyDragon: {
                        tf.anchoredPosition = new Vector2(DEF_X, DEF_Y + 100);
                        setOtherPortrait(spkImg, LM._.localize("Baby Dragon"), key, tf.anchoredPosition, isFlip);
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

    private void setOtherPortrait(Image spkImg, string name, int key, Vector3 pos, bool isFlip, bool isPlayer = false) {
        //* 画像
        spkImg.sprite = spkSprDtList[key];
        float posX = Mathf.Abs(pos.x) * (isFlip? -1 : 1);
        spkImg.rectTransform.anchoredPosition = new Vector2(posX, pos.y);
        Vector3 sc = spkImg.transform.localScale;
        spkImg.transform.localScale = new Vector3(Mathf.Abs(sc.x) * (isFlip? -1 : 1) * ( isPlayer? -1 : 1) , sc.y, sc.z);
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
