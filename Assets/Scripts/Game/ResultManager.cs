using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {
    const float RESULT_PANEL_START_DIST = 3.0f;
    const float RESULT_PANEL_PET_DANCE_POS_X = 1.75f;
    
    [Header("VALUE")]
    float rotSpeed;
    [SerializeField] int rewardExp;     public int RewardExp {get => rewardExp; set => rewardExp = value;}
    [SerializeField] int rewardCoin;    public int RewardCoin {get => rewardCoin; set => rewardCoin = value;}

    [Header("SPRITE")]
    [SerializeField] Sprite starSpr;
    [SerializeField] SpriteRenderer resultBGSr;
    [SerializeField] Sprite[] resultBGSprs;

    [Header("OBJECT")]
    [SerializeField] GameObject worldSpaceResultGroup;
    [SerializeField] Transform resPlSpot;
    [SerializeField] Transform resPetSpot;

    [Header("UI")]
    [SerializeField] Transform starGroupTf;
    [SerializeField] TextMeshProUGUI msgAnimTxt;
    [SerializeField] TextMeshProUGUI topCoinTxt;    public TextMeshProUGUI TopCoinTxt {get => topCoinTxt;}
    [SerializeField] TextMeshProUGUI expTxt;    public TextMeshProUGUI ExpTxt {get => expTxt; set => expTxt = value;}
    [SerializeField] TextMeshProUGUI coinTxt;    public TextMeshProUGUI CoinTxt {get => coinTxt; set => coinTxt = value;}
    [SerializeField] TextMeshProUGUI lvTxt;    public TextMeshProUGUI LvTxt {get => lvTxt; set => lvTxt = value;}
    [SerializeField] GameObject lvBonusMsg;
    [SerializeField] GameObject answerCntBonusMsg;
    [SerializeField] GameObject legacyBonusMsg;
    [SerializeField] Image expFilledCircleBar;  public Image ExpFilledCircleBar {get => expFilledCircleBar; set => expFilledCircleBar = value;}
    [SerializeField] GameObject goHomePanelBtn;  public GameObject GoHomePanelBtn {get => goHomePanelBtn; set => goHomePanelBtn = value;}

    [Header("EF")]
    [SerializeField] GameObject coinAttractionEF;
    [SerializeField] GameObject expAttractionEF;

    void Start() {
        //* ReaultBG
        resultBGSr.sprite = resultBGSprs[DB._.SelectMapIdx];

        //* 以前のコイン量 表示
        topCoinTxt.text = DB.Dt.Coin.ToString();
        lvTxt.text = DB.Dt.Lv.ToString();
        expFilledCircleBar.fillAmount = DB.Dt.getExpPer();

        //* Init
        rewardExp = 0;
        rewardCoin = 0;
        msgAnimTxt.gameObject.SetActive(false);
        goHomePanelBtn.SetActive(false);
    }

    void Update() {
        if(!worldSpaceResultGroup) return;
        rotSpeed = 10 * Time.deltaTime;
        starGroupTf.Rotate(Vector3.back * rotSpeed);
    }

//-------------------------------------------------------------------------------------------------------------
#region EVENT
//-------------------------------------------------------------------------------------------------------------
    public void onClickGoHomePanelBtn() => StartCoroutine(coGoHome());
#endregion
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private IEnumerator coGoHome() {
        SM._.sfxPlay(SM.SFX.Transition.ToString());
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(true);
        GM._.gui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackIn.ToString());
        yield return Util.time0_5;
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public IEnumerator coDisplayResultPanel(WJ_Connector connector = null) {
        GM._.GameStatus = GM.GAME_STT.RESULT;
        GM._.gui.ExitBtn.SetActive(false);
        if(GM._.Pl.transform.localScale.x > 1) {
            GM._.Pl.transform.localScale = Vector3.one;
        }

        //* チュートリアル 結果
        if(DB.Dt.IsTutoDiagResultTrigger) {
            GM._.gtm.action((int)GameTalkManager.ID.TUTO_DIAG_RESULT);
        }

        //* レベール ボーナス
        float lvBonus = GM._.Pl.calcLvBonusPer();

        //* 正解率 ボーナス (Learning専用)
        float answerCntBonus = 0;
        if(connector) {
            var resSts = connector.cLearnProg.data.lrnPrgsStsCd;
            answerCntBonus = (resSts == "LPS01")? 0
                : (resSts == "LPS02")? 0.1f
                : (resSts == "LPS03")? 0.25f
                : 0.5f; //(resSts == "LPS04")
        }
        // TODO Regacy Item Bonus
        //* 遺物 ボーナス
        float legacyBonus = GM._.Pl.calcLegacyBonusPer();

        //* 総合
        const float OFFSET_100PER = 1.0f;
        float totalBonus = OFFSET_100PER + lvBonus + answerCntBonus + legacyBonus;

        //* Result
        rewardExp = (int)(rewardExp * totalBonus);
        rewardCoin = (int)(rewardCoin * totalBonus);

        //* UI
        expTxt.text = $"+{rewardExp}";
        coinTxt.text = $"+{rewardCoin}";

        //* Bonus UI
        lvBonusMsg.SetActive(lvBonus != 0);
        lvBonusMsg.GetComponentInChildren<TextMeshProUGUI>().text = $"{LM._.localize("Level")}{LM._.localize("Bonus")} <color=green><size=60>+{lvBonus * 100}%</size></color>";
        answerCntBonusMsg.SetActive(answerCntBonus != 0);
        answerCntBonusMsg.GetComponentInChildren<TextMeshProUGUI>().text = $"{LM._.localize("Answer")}{LM._.localize("Bonus")} <color=green><size=60>+{answerCntBonus * 100}%</size></color>";
        legacyBonusMsg.SetActive(legacyBonus != 0);
        legacyBonusMsg.GetComponentInChildren<TextMeshProUGUI>().text = $"{LM._.localize("Lecagy")}{LM._.localize("Bonus")} <color=green><size=60>+{legacyBonus * 100}%</size></color>";

        SM._.sfxPlay(SM.SFX.Transition.ToString());
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(true);
        GM._.gui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        yield return Util.time1;

        SM._.sfxPlay(SM.SFX.Result.ToString());
        StartCoroutine(coPlayObjAnim(GM._.Pl, GM._.Pet));

        yield return coPlayAnswerProgressFrameStarAnim(GM._.qm.QuizAnswerResultArr); //* 下のテーブル★
        yield return coPlayStarAndMsgAnim(GM._.qm.QuizAnswerResultArr); //* 空の回る★
        StartCoroutine(coPlayCoinCollectAnim());
        yield return Util.time0_5;
        StartCoroutine(coPlayExpCollectionAnim());

        GM._.gui.SwitchScreenAnim.gameObject.SetActive(false);
        goHomePanelBtn.SetActive(true);

        yield return Util.time0_2;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_1;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_5;
        yield return Util.time0_2;
        SM._.sfxPlay(SM.SFX.GetExp.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetExp.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetExp.ToString());
        yield return Util.time0_1;
        SM._.sfxPlay(SM.SFX.GetExp.ToString());
    }
    public void setReward(int exp, int coin) {
        rewardExp += exp;
        rewardCoin += coin;
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region ANIM
//-------------------------------------------------------------------------------------------------------------
    IEnumerator coPlayCoinCollectAnim() {
        bool isCoinUP = true;
        int coinVal = 0;

        coinAttractionEF.SetActive(true);
        // yield return Util.time1;
        int myCoin = int.Parse(topCoinTxt.text);
        while(isCoinUP) {
            coinVal += 10;
            if(coinVal <= rewardCoin) topCoinTxt.text = $"{coinVal + myCoin}";
            else    isCoinUP = false;

            yield return Util.time0_005;
        }

        //* Add DataBase Coin
        DB.Dt.setCoin(rewardCoin);

        
    }

    IEnumerator coPlayExpCollectionAnim() {
        Debug.Log("ResultManager:: coPlayExpCollectionAnim():: rewardExp= " + rewardExp);

        bool isExpUp = true;
        int expVal = 0;

        expAttractionEF.SetActive(true);
        while(isExpUp) {
            expVal++;
            if(expVal <= rewardExp) {
                DB.Dt.Exp++;
                expFilledCircleBar.fillAmount = DB.Dt.getExpPer();
                if(expFilledCircleBar.fillAmount == 1) {
                    Debug.Log("ResultManager:: coPlayExpCollectionAnim():: LevelUp!");
                    StartCoroutine(GM._.Pl.coLevelUpEF());
                    lvTxt.text = DB.Dt.Lv.ToString();
                }
            }
            else {
                isExpUp = false;
            }
            yield return Util.time0_01;
        }
    }

    IEnumerator coPlayObjAnim(Player pl, Pet pet) {
        bool isIncreasing = false;
        //* On
        worldSpaceResultGroup.SetActive(true);
        GM._.gui.ResultPanel.SetActive(true);

        //* Off
        GM._.WorldSpaceQuizGroup.SetActive(false);
        GM._.gui.QuizPanel.SetActive(false);

        //* Player Move To TargetPos
        pl.transform.SetParent(resPlSpot);
        pl.transform.position = new Vector2(resPlSpot.position.x - RESULT_PANEL_START_DIST, resPlSpot.position.y);
        pl.TgPos = resPlSpot.position;

        //* Pet Move To TargetPos
        pet.transform.SetParent(resPetSpot);
        pet.transform.position = new Vector2(resPetSpot.position.x + RESULT_PANEL_START_DIST, resPetSpot.position.y);
        pet.TgPos = resPetSpot.position;

        //* Anim
        yield return Util.time1;
        pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pl.transform));
        pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pet.transform));

        //* Anim Repeat
        while(true) {
            if(!isIncreasing) {
                yield return Util.time2; yield return Util.time1;
                pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
                StartCoroutine(Util.coPlayBounceAnim(pl.transform));
                
                pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
                pet.TgPos = new Vector2(-RESULT_PANEL_PET_DANCE_POS_X, resPetSpot.position.y);
                isIncreasing = true;
            }
            else {
                yield return Util.time2; yield return Util.time1;
                pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
                StartCoroutine(Util.coPlayBounceAnim(pl.transform));

                pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
                pet.TgPos = new Vector2(RESULT_PANEL_PET_DANCE_POS_X, resPetSpot.position.y);
                isIncreasing = false;
            }
        }
    }
    IEnumerator coPlayAnswerProgressFrameStarAnim(string[] quizAnswerResultArr) {
        Transform starIcons =  GM._.qm.AnswerProgressFrameTf;
        for(int i = 0; i < starIcons.childCount; i++) {
            Transform starIcon = starIcons.GetChild(i);
            starIcon.GetChild(0).gameObject.SetActive(quizAnswerResultArr[i] == "Y");
        }
        yield return Util.time1;
        yield return Util.time0_3;
    }
    IEnumerator coPlayStarAndMsgAnim(string[] quizAnswerResultArr) {
        //* Stars
        yield return Util.time0_5;
        const int STAR_EF_IDX = 0;
        for(int i = 0; i < starGroupTf.childCount; i++) {
            var star = starGroupTf.GetChild(i);
            var effectObj = star.GetChild(STAR_EF_IDX).gameObject;
            if(quizAnswerResultArr[i] == "Y") {
                effectObj.SetActive(true);
                yield return Util.time0_2;
                SM._.sfxPlay(SM.SFX.CorrectAnswer.ToString());
                StartCoroutine(coEnableStarImg(i));
            }
        }

        //* Set MsgTxt
        int correctCnt = 0;
        Array.ForEach(quizAnswerResultArr, arr => {if(arr == "Y") correctCnt++;});
        msgAnimTxt.text = (correctCnt >= 8)? $"{LM._.localize("Fantastic")}!"
            : (correctCnt >= 6)? $"{LM._.localize("Great")}!"
            : (correctCnt >= 4)? $"{LM._.localize("Fine")}!"
            : (correctCnt >= 2)? $"{LM._.localize("Good Job")}!"
            : $"{LM._.localize("That`s right")}!";

        yield return Util.time0_5;
        SM._.sfxPlay(SM.SFX.Fanfare.ToString());
        msgAnimTxt.gameObject.SetActive(true); //* 結果メッセージアニメー 表示
    }
    private IEnumerator coEnableStarImg(int idx) {
        yield return Util.time1;
        yield return Util.time0_5;
        starGroupTf.GetChild(idx).GetComponent<Image>().enabled = true;
        starGroupTf.GetChild(idx).GetComponent<Image>().sprite = starSpr;
        starGroupTf.GetChild(idx).GetComponent<Image>().color = new Color(1, 1, 1, 1);// Color.white;
    }
#endregion
}
