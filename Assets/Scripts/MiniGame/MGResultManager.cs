using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

public class MGResultManager : MonoBehaviour {
    [Header("VALUE")]
    [SerializeField] int rewardExp;     public int RewardExp {get => rewardExp; set => rewardExp = value;}
    [SerializeField] int rewardCoin;    public int RewardCoin {get => rewardCoin; set => rewardCoin = value;}

    // [SerializeField] TextMeshProUGUI msgAnimTxt;
    [SerializeField] TextMeshProUGUI topCoinTxt;    public TextMeshProUGUI TopCoinTxt {get => topCoinTxt;}
    [SerializeField] TextMeshProUGUI expTxt;    public TextMeshProUGUI ExpTxt {get => expTxt; set => expTxt = value;}
    [SerializeField] TextMeshProUGUI coinTxt;    public TextMeshProUGUI CoinTxt {get => coinTxt; set => coinTxt = value;}
    [SerializeField] TextMeshProUGUI lvTxt;    public TextMeshProUGUI LvTxt {get => lvTxt; set => lvTxt = value;}

    [SerializeField] Image expFilledCircleBar;  public Image ExpFilledCircleBar {get => expFilledCircleBar; set => expFilledCircleBar = value;}
    [SerializeField] GameObject goHomePanelBtn;  public GameObject GoHomePanelBtn {get => goHomePanelBtn; set => goHomePanelBtn = value;}
    [SerializeField] Image portraitImg;

    [Header("EF")]
    [SerializeField] GameObject coinAttractionEF;
    [SerializeField] GameObject expAttractionEF;

    public void onClickResultGoHomePanelBtn() => StartCoroutine(coGoHome());


    void Start() {
        topCoinTxt.text = DB.Dt.Coin.ToString();
        lvTxt.text = DB.Dt.Lv.ToString();
        expFilledCircleBar.fillAmount = DB.Dt.getExpPer();

        //* Init
        rewardExp = 0;
        rewardCoin = 0;
        setMyPortraitsImg(MGM._.Pl.IdleSpr);
    }
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private void setMyPortraitsImg(Sprite spr) {
        portraitImg.sprite = spr;
    }
    public IEnumerator coGoHome() {
        SM._.sfxPlay(SM.SFX.Transition.ToString());
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
        
        SM._.sfxPlay(SM.SFX.Transition.ToString());
        MGM._.ui.SwitchScreenAnim.gameObject.SetActive(true);
        MGM._.ui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        yield return Util.time1;
        SM._.sfxPlay(SM.SFX.Result.ToString());

        MGM._.ui.ResultPanel.SetActive(true);
        StartCoroutine(coRepeatPlayerSuccessAnim());
        StartCoroutine(coPlayCoinCollectAnim());
        yield return Util.time0_8;
        StartCoroutine(coPlayExpCollectionAnim());

        MGM._.ui.SwitchScreenAnim.gameObject.SetActive(false);
        goHomePanelBtn.SetActive(true);

        yield return Util.time0_2;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_05;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_1;
        SM._.sfxPlay(SM.SFX.GetCoin.ToString());
        yield return Util.time0_8;
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
            coinVal += 5; //* Counting Unit
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
                Debug.Log($"coPlayExpCollectionAnim():: expVal / rewardExp = {expVal} / {rewardExp}, fill= {expFilledCircleBar.fillAmount}");
                if(expFilledCircleBar.fillAmount == 1) {
                    Debug.Log($"coPlayExpCollectionAnim():: LevelUp! MGM._.Pl.name= {MGM._.Pl.name}");
                    StartCoroutine(MGM._.Pl.coLevelUpEF(delay: 1));
                    lvTxt.text = DB.Dt.Lv.ToString(); //* レベルアップ 反映
                }
            }
            else {
                isExpUp = false;
            }
            yield return Util.time0_01;
        }
    }
#endregion
}
