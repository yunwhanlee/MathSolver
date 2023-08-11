using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MGResultManager : MonoBehaviour {
    [Header("VALUE")]
    [SerializeField] int rewardExp;     public int RewardExp {get => rewardExp; set => rewardExp = value;}
    [SerializeField] int rewardCoin;    public int RewardCoin {get => rewardCoin; set => rewardCoin = value;}

    [SerializeField] TextMeshProUGUI msgAnimTxt;
    [SerializeField] TextMeshProUGUI topCoinTxt;    public TextMeshProUGUI TopCoinTxt {get => topCoinTxt;}
    [SerializeField] TextMeshProUGUI expTxt;    public TextMeshProUGUI ExpTxt {get => expTxt; set => expTxt = value;}
    [SerializeField] TextMeshProUGUI coinTxt;    public TextMeshProUGUI CoinTxt {get => coinTxt; set => coinTxt = value;}
    [SerializeField] TextMeshProUGUI lvTxt;    public TextMeshProUGUI LvTxt {get => lvTxt; set => lvTxt = value;}

    [SerializeField] Image expFilledCircleBar;  public Image ExpFilledCircleBar {get => expFilledCircleBar; set => expFilledCircleBar = value;}
    [SerializeField] GameObject goHomePanelBtn;  public GameObject GoHomePanelBtn {get => goHomePanelBtn; set => goHomePanelBtn = value;}

    [Header("EF")]
    [SerializeField] GameObject coinAttractionEF;
    [SerializeField] GameObject expAttractionEF;

    public void onClickResultGoHomePanelBtn() => StartCoroutine(coGoHome());

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private IEnumerator coGoHome() {
        MGM._.ui.SwitchScreenAnim.gameObject.SetActive(true);
        MGM._.ui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackIn.ToString());
        yield return Util.time0_5;
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public IEnumerator coDisplayResultPanel() {
        yield return Util.time1;
        //* レベール ボーナス
        float lvBonus = 0;

        //* 総合
        const float OFFSET_100PER = 1.0f;
        float totalBonus = OFFSET_100PER + lvBonus; // + answerCntBonus;

        //* Result
        rewardExp = (int)(rewardExp * totalBonus);
        rewardCoin = (int)(rewardCoin * totalBonus);

        //* UI
        expTxt.text = $"+{rewardExp}";
        coinTxt.text = $"+{rewardCoin}";
        MGM._.ui.LeftArrowBtn.gameObject.SetActive(false);
        MGM._.ui.RightArrowBtn.gameObject.SetActive(false);
        
        MGM._.ui.SwitchScreenAnim.gameObject.SetActive(true);
        MGM._.ui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        yield return Util.time1;

        MGM._.ui.ResultPanel.SetActive(true);
        StartCoroutine(coRepeatPlayerSuccessAnim());
        yield return coPlayCoinCollectAnim();
        yield return Util.time0_5;
        yield return coPlayExpCollectionAnim();

        MGM._.ui.SwitchScreenAnim.gameObject.SetActive(false);
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
    IEnumerator coRepeatPlayerSuccessAnim() {
        while(true) {
            MGM._.Pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
            StartCoroutine(Util.coPlayBounceAnim(MGM._.Pl.transform));
            yield return Util.time1;
            yield return Util.time2;
        }
    }

    IEnumerator coPlayCoinCollectAnim() {
        bool isCoinUP = true;
        int coinVal = 0;

        coinAttractionEF.SetActive(true);
        // yield return Util.time1;
        int myCoin = DB.Dt.Coin;
        while(isCoinUP) {
            coinVal += 5;
            if(coinVal <= rewardCoin) topCoinTxt.text = $"{coinVal + myCoin}";
            else    isCoinUP = false;

            yield return Util.time0_005;
        }

        //* Add DataBase Coin
        DB.Dt.setCoin(rewardCoin);
    }

    IEnumerator coPlayExpCollectionAnim() {
        bool isExpUp = true;
        int expVal = 0;

        expAttractionEF.SetActive(true);
        while(isExpUp) {
            expVal++;
            if(expVal <= rewardExp) {
                DB.Dt.Exp++;
                expFilledCircleBar.fillAmount = DB.Dt.getExpPer();
                if(expFilledCircleBar.fillAmount == 1)
                    StartCoroutine(GM._.Pl.coLevelUpEF());
            }
            else
                isExpUp = false;
            yield return Util.time0_01;
        }
    }
#endregion
}
