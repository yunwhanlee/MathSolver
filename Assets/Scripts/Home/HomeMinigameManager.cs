using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

/// <summary>
/// ミニーゲームPopUp アイテム情報
/// </summary>
[System.Serializable]
public class minigameInfo {

    [SerializeField] Color frameColor;  public Color FrameColor {get => frameColor;}
    [SerializeField] Sprite minigameSpr;   public Sprite MinigameSpr {get => minigameSpr;}
    [SerializeField] Sprite labelSpr;   public Sprite LabelSpr {get => labelSpr;}
    [SerializeField] Sprite finalRewardSpr;  public Sprite FinalRewardSpr {get => finalRewardSpr;}
    [SerializeField] Sprite[] iconSpr;  public Sprite[] IconSpr {get => iconSpr;}
    [SerializeField] int[] easyVals;  public int[] EasyVals {get => easyVals;}
    [SerializeField] int[] normalVals;  public int[] NormalVals {get => normalVals;}
    [SerializeField] int[] hardVals;  public int[] HardVals {get => hardVals;}
}

public class HomeMinigameManager : MonoBehaviour {
    UnityAction[] onInits = new UnityAction[2];

    [SerializeField] GameObject minigameLvPopUp;   public GameObject MinigameLvPopUp {get => minigameLvPopUp;}
    [SerializeField] Image topFrame1Img, topFrame2Img, labelImg;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] minigameInfo mg1InfoData;
    [SerializeField] minigameInfo mg2InfoData;
    [SerializeField] minigameInfo mg3InfoData;
    [SerializeField] GameObject[] infoIconList;          public GameObject[] InfoIconList {get => infoIconList;}

    [SerializeField] Button[] lvBtns;  public Button[] LvBtns {get => lvBtns;}
    [SerializeField] GameObject[] lvBtnFocusLines;
    [SerializeField] GameObject[] lvBtnLockFrames;
    [SerializeField] Button playBtn;   public Button PlayBtn {get => playBtn;}
    [SerializeField] TextMeshProUGUI playPriceTxt;   public TextMeshProUGUI PlayPriceTxt {get => playPriceTxt;}
    
    [Header("BEST SCORE SLIDER")]
    [SerializeField] Button[] rewardIconBtns;      public Button[] RewardIconBtns {get => rewardIconBtns;}
    [SerializeField] TextMeshProUGUI[] rewardIconBtnValTxts;      public TextMeshProUGUI[] RewardIconBtnValTxts {get => rewardIconBtnValTxts;}
    [SerializeField] TextMeshProUGUI[] unlockScoreTxts;      public TextMeshProUGUI[] UnlockScoreTxts {get => unlockScoreTxts;}
    [SerializeField] Image[] rewardCheckIcons;      public Image[] RewardCheckIcons {get => rewardCheckIcons;}

    [SerializeField] Slider bestScoreSlider;   public Slider BestScoreSlider {get => bestScoreSlider;}
    [SerializeField] TextMeshProUGUI bestScoreSliderValTxt;


    void Start() {
        //* Frame 初期化
        for(int i = 0; i < lvBtns.Length; i++) {
            int last = lvBtns[i].transform.childCount;
            lvBtnFocusLines[i] = lvBtns[i].transform.GetChild(last - 1).gameObject;
            lvBtnLockFrames[i] = lvBtns[i].transform.GetChild(last - 2).gameObject;
        }

        //* RewardBtn 初期化
        for(int i = 0; i < rewardIconBtns.Length; i++) {
            rewardIconBtnValTxts[i] = rewardIconBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            rewardCheckIcons[i] = rewardIconBtns[i].GetComponentsInChildren<Image>(true)[1];
        }

        onInits[0] = () => init(Enum.MG.Minigame1.ToString(), "Catch Falling apples!"
                            , DB.Dt.Minigame1BestScore, Config.MINIGAME1_UNLOCK_SCORES
                            , DB.Dt.Minigame1RewardTriggers, Config.MINIGAME1_MAX_VAL, mg1InfoData
        );
        onInits[1] = () => init(Enum.MG.Minigame2.ToString(), "Jump to the sky!"
                            , DB.Dt.Minigame2BestScore, Config.MINIGAME2_UNLOCK_SCORES
                            , DB.Dt.Minigame2RewardTriggers, Config.MINIGAME2_MAX_VAL, mg2InfoData
        );
        //TODO Tundra onInits[2]
    }


///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private void init(string name, string title, int bestScore, int[] unlockScores, bool[] rewardTrigger, int maxVal, minigameInfo infoData) {
        const int EASY = (int)Enum.MINIGAME_LV.Easy, 
            NORMAL = (int)Enum.MINIGAME_LV.Normal, 
            HARD = (int)Enum.MINIGAME_LV.Hard;

        topFrame1Img.color = infoData.FrameColor;
        topFrame2Img.color = infoData.FrameColor;
        labelImg.sprite = infoData.LabelSpr;
        rewardIconBtns[2].GetComponent<Image>().sprite = infoData.FinalRewardSpr;

        Debug.Log("name--> " + name);
        nameTxt.text = name;
        titleTxt.text = title;

        HM._.wmm.displayUnlockPopUp(null, name, true);

        if(bestScore == 0) {
            playPriceTxt.text = "Free";
        }
        else 
            playPriceTxt.text = Config.MINIGMAE_PLAY_PRICES[0].ToString();

        int easyScore = unlockScores[EASY];
        int normalScore = unlockScores[NORMAL];
        int hardScore = unlockScores[HARD];

        //* LockFrame 表示
        lvBtnLockFrames[EASY].SetActive(false);
        lvBtnLockFrames[NORMAL].SetActive(!(bestScore >= easyScore));
        lvBtnLockFrames[HARD].SetActive(!(bestScore >= normalScore));

        //* Slider Data
        unlockScoreTxts[EASY].text = easyScore.ToString() + "\nNormal";
        unlockScoreTxts[NORMAL].text = normalScore.ToString() + "\nHard";
        unlockScoreTxts[HARD].text = hardScore.ToString();

        onClickMinigameLvPopUpLvBtn(0);

        //* Slider Reward IconBtn 活性化
        if(bestScore >= easyScore)
            activeRewardIcon(EASY, rewardTrigger[0]);
        if(bestScore >= normalScore && !rewardTrigger[1])
            activeRewardIcon(NORMAL, rewardTrigger[1]);
        if(bestScore >= hardScore  && !rewardTrigger[2])
            activeRewardIcon(HARD, rewardTrigger[2]);

        //* Best Score Slider
        bestScoreSlider.value = bestScore;
        bestScoreSliderValTxt.text = $"<color=white>{bestScore}</color> / {maxVal}";
    }
    private void activeRewardIcon(int idx, bool rewardTrigger) {
        bool isTrigger = !rewardTrigger;
        rewardIconBtns[idx].interactable = isTrigger;
        rewardCheckIcons[idx].gameObject.SetActive(!isTrigger);
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    #region MINIGAME LEVEL POPUP
    public void onClickMinigameExclamationMarkBtn(int idx) {
        Debug.Log($"onClickMinigameExclamationMarkBtn(idx={idx}):: ");
        DB._.SelectMinigameIdx = idx;
        minigameLvPopUp.SetActive(true);
        onInits[idx].Invoke(); //* Init
    }
    public void onClickMinigameLvPopUpLvBtn(int difficultyLvIdx) {
        //* ロックしたら、解禁条件のお知らせ、以下処理しない
        if(lvBtnLockFrames[difficultyLvIdx].activeSelf) {
            int[] unlockScores = (DB._.SelectMinigameIdx == (int)Enum.MG.Minigame1)? Config.MINIGAME1_UNLOCK_SCORES
                : (DB._.SelectMinigameIdx == (int)Enum.MG.Minigame2)? Config.MINIGAME2_UNLOCK_SCORES
                : null;
            HM._.ui.showErrorMsgPopUp($"Achieve {unlockScores[difficultyLvIdx]} Score!");
            return;
        }

        //* 選択した難易度
        DB._.MinigameLv = difficultyLvIdx;

        //* Play Price
        if(playPriceTxt.text != "Free")
            playPriceTxt.text = Config.MINIGMAE_PLAY_PRICES[DB._.MinigameLv].ToString();

        //* 選択 枠
        for(int i = 0; i< lvBtns.Length; i++)
            lvBtnFocusLines[i].SetActive(i == difficultyLvIdx);

        //* 選択したミニーゲーム データ
        minigameInfo infoData = (DB._.SelectMinigameIdx == (int)Enum.MG.Minigame1)? mg1InfoData
            : (DB._.SelectMinigameIdx == (int)Enum.MG.Minigame2)? mg2InfoData
            : mg3InfoData;

        //* 登場するアイテムの情報表示欄
        //* Minigame 1
        const int EASY = 0, NORMAL = 1, HARD = 2;
        switch(difficultyLvIdx) {
            case EASY:
                infoIconList[0].SetActive(true);
                infoIconList[0].GetComponent<Image>().sprite = infoData.IconSpr[0];
                infoIconList[0].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.EasyVals[0]}";
                infoIconList[1].SetActive(true);
                infoIconList[1].GetComponent<Image>().sprite = infoData.IconSpr[1];
                infoIconList[1].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.EasyVals[1]}";
                infoIconList[2].SetActive(false);
                break;
            case NORMAL:
                infoIconList[0].SetActive(true);
                infoIconList[0].GetComponent<Image>().sprite = infoData.IconSpr[0];
                infoIconList[0].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.NormalVals[0]}";
                infoIconList[1].SetActive(true);
                infoIconList[1].GetComponent<Image>().sprite = infoData.IconSpr[1];
                infoIconList[1].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.NormalVals[1]}";
                infoIconList[2].SetActive(true);
                infoIconList[2].GetComponent<Image>().sprite = infoData.IconSpr[2];
                infoIconList[2].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.NormalVals[2]}";
                break;
            case HARD:
                infoIconList[0].SetActive(true);
                infoIconList[0].GetComponent<Image>().sprite = infoData.IconSpr[0];
                infoIconList[0].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.HardVals[0]}";
                infoIconList[1].SetActive(true);
                infoIconList[1].GetComponent<Image>().sprite = infoData.IconSpr[1];
                infoIconList[1].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.HardVals[1]}";
                infoIconList[2].SetActive(true);
                infoIconList[2].GetComponent<Image>().sprite = infoData.IconSpr[2];
                infoIconList[2].GetComponentInChildren<TextMeshProUGUI>().text = $"+{infoData.HardVals[2]}";
                break;
        }
    }
    public void onClickMinigamePlayBtn() {
        int price = Config.MINIGMAE_PLAY_PRICES[DB._.MinigameLv]; 
        if(DB.Dt.Minigame1BestScore == 0) { //* 最初は無料
            StartCoroutine(HM._.GoToLoadingScene(Enum.SCENE.MiniGame.ToString()));
        }
        else if(DB.Dt.Coin >= price) {
            DB.Dt.setCoin(-price);
            StartCoroutine(HM._.GoToLoadingScene(Enum.SCENE.MiniGame.ToString()));
        }
        else {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Not enough coin!"));
        }
    }
    public void onClickSliderRewardIconBtn(int idx) {
        switch(idx) {
            case 0: 
                DB.Dt.Fame += 20;
                DB.Dt.Minigame1RewardTriggers[0] = true;
                rewardIconBtns[0].interactable = false;
                rewardCheckIcons[0].gameObject.SetActive(true);
                break;
            case 1: 
                DB.Dt.Fame += 40;
                DB.Dt.Minigame1RewardTriggers[1] = true;
                rewardIconBtns[1].interactable = false;
                rewardCheckIcons[1].gameObject.SetActive(true);
                break;
            case 2: 
                PetSkin goldApplePet = Array.Find(DB.Dt.PtSkins, pet => pet.Name == "GoldApple Pet");
                HM._.cUI.setReward(goldApplePet);
                HM._.ui.activeNewFuniturePopUp(goldApplePet.Spr, goldApplePet.Name);
                DB.Dt.Minigame1RewardTriggers[2] = true;
                rewardIconBtns[2].interactable = false;
                rewardCheckIcons[2].gameObject.SetActive(true);
                break;
        }
    }
    #endregion
#endregion
}
