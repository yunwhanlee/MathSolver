using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField] TextMeshProUGUI msgTxt;

    void Start() {
        //* Init
        rewardExp = 0;
        rewardCoin = 0;
        msgTxt.gameObject.SetActive(false);
    }

    void Update() {
        if(!worldSpaceResultGroup) return;
        rotSpeed = 10 * Time.deltaTime;
        starGroupTf.Rotate(Vector3.back * rotSpeed);
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public IEnumerator coDisplayResultPanel() {
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(true);
        GM._.gui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        yield return Util.time1;

        StartCoroutine(coPlayRewardUICountingAnim());
        StartCoroutine(coPlayObjAnim(GM._.Pl, GM._.Pet));
        StartCoroutine(coPlayStarAndMsgAnim(GM._.qm.QuizAnswerResultArr));

        yield return Util.time1;
        GM._.gui.SwitchScreenAnim.gameObject.SetActive(false);
    }
    public void setReward(int exp, int coin) {
        rewardExp += exp;
        rewardCoin += coin;
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region ANIM
//-------------------------------------------------------------------------------------------------------------
    IEnumerator coPlayRewardUICountingAnim() {
        bool isExpUP = true;
        bool isCoinUP = true;
        int expVal = 0;
        int coinVal = 0;

        while(isExpUP || isCoinUP) {
            if(expVal < rewardExp) GM._.gui.ExpTxt.text = $"+{++expVal}";
            else    isExpUP = false;

            if(coinVal < rewardCoin) GM._.gui.CoinTxt.text = $"+{++coinVal}";
            else    isCoinUP = false;

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
        msgTxt.text = (correctCnt >= 8)? "환타스틱!"
            : (correctCnt >= 6)? "대단코요!"
            : (correctCnt >= 4)? "훌륭해요!"
            : (correctCnt >= 2)? "잘했어요!"
            : "괜찮아요!";

        //* MsgTxt Scale In-Out Anim
        yield return Util.time0_5;
        msgTxt.gameObject.SetActive(true);
        msgTxt.transform.localScale = Vector2.zero;
        const float MAX_SC = 1.2f;
        const float LERP_SC_OFFSET = 0.0325f;
        bool isDecreasing = false;
        float spd = 20 * Time.deltaTime;
        var sc = msgTxt.transform.localScale;
        while(true) {
            if(!isDecreasing) {
                msgTxt.transform.localScale = new Vector2(
                    Mathf.Lerp(msgTxt.transform.localScale.x, MAX_SC, spd), 
                    Mathf.Lerp(msgTxt.transform.localScale.y, MAX_SC, spd)
                );
                if(msgTxt.transform.localScale.x > MAX_SC - LERP_SC_OFFSET)
                    isDecreasing = true;
            }
            else {
                msgTxt.transform.localScale = new Vector2(
                    msgTxt.transform.localScale.x - spd,
                    msgTxt.transform.localScale.y - spd
                );
                if(msgTxt.transform.localScale.x <= 1) break;
            }
            yield return Util.time0_01;
        }
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
