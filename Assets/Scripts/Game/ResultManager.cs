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
    [SerializeField] GameObject goHomePanelBtn;  public GameObject GoHomePanelBtn {get => goHomePanelBtn; set => goHomePanelBtn = value;}

    [Header("EF")]
    [SerializeField] GameObject coinAttractionEF;

    void Start() {
        //* 以前のコイン量 表示
        topCoinTxt.text = DB.Dt.Coin.ToString();

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
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(true);
        GM._.gui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackIn.ToString());
        yield return Util.time0_5;
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public IEnumerator coDisplayResultPanel() {
        //* チュートリアル 結果
        if(DB.Dt.IsTutoDiagResultTrigger) {
            GM._.gtm.action((int)GameTalkManager.TALK_ID_IDX.TUTORIAL_DIAG_RESULT);
        }
        
        expTxt.text = $"+{rewardExp}";
        coinTxt.text = $"+{rewardCoin}";

        GM._.gui.SwitchScreenAnim.gameObject.SetActive(true);
        GM._.gui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        yield return Util.time1;

        StartCoroutine(coPlayObjAnim(GM._.Pl, GM._.Pet));
        yield return coPlayStarAndMsgAnim(GM._.qm.QuizAnswerResultArr);
        yield return coPlayCoinCollectAnim();
        yield return coCheckLevelUp();

        yield return Util.time1;
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(false);
        goHomePanelBtn.SetActive(true);
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
        yield return Util.time1;
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

    IEnumerator coCheckLevelUp() {
        DB.Dt.Exp += rewardExp;
        yield return GM._.Pl.coExpUpEF();
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
                StartCoroutine(coEnableStarImg(i));
            }
        }

        //* Set MsgTxt
        int correctCnt = 0;
        Array.ForEach(quizAnswerResultArr, arr => {if(arr == "Y") correctCnt++;});
        msgAnimTxt.text = (correctCnt >= 8)? "판타스틱!"
            : (correctCnt >= 6)? "대단해요!"
            : (correctCnt >= 4)? "훌륭해요!"
            : (correctCnt >= 2)? "잘했어요!"
            : "괜찮아요!";

        yield return Util.time0_5;
        msgAnimTxt.gameObject.SetActive(true); //* 結果メッセージアニメー 表示
    }
    private IEnumerator coEnableStarImg(int idx) {
        yield return Util.time1;
        yield return Util.time0_5;
        starGroupTf.GetChild(idx).GetComponent<Image>().enabled = true;
        starGroupTf.GetChild(idx).GetComponent<Image>().sprite = starSpr;
        starGroupTf.GetChild(idx).GetComponent<Image>().color = Color.white;
    }
#endregion
}
