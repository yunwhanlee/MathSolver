using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class HomeMinigameManager : MonoBehaviour {
    UnityAction[] onInits = new UnityAction[2];

    [SerializeField] GameObject minigameLvPopUp;   public GameObject MinigameLvPopUp {get => minigameLvPopUp;}
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI titleTxt;

    [SerializeField] GameObject[] infoIcons;          public GameObject[] InfoIcons {get => infoIcons;}

    [SerializeField] Button[] lvBtns;  public Button[] LvBtns {get => lvBtns;}
    [SerializeField] GameObject[] lvBtnFocusLines;
    [SerializeField] GameObject[] lvBtnLockFrames;
    [SerializeField] Button playBtn;   public Button PlayBtn {get => playBtn;}
    [SerializeField] TextMeshProUGUI playPriceTxt;   public TextMeshProUGUI PlayPriceTxt {get => playPriceTxt;}

    [SerializeField] Button[] rewardIconBtns;      public Button[] RewardIconBtns {get => rewardIconBtns;}
    [SerializeField] TextMeshProUGUI[] rewardIconBtnValTxts;      public TextMeshProUGUI[] RewardIconBtnValTxts {get => rewardIconBtnValTxts;}
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

        onInits[0] = () => init(Enum.MAP.Minigame1.ToString(), "Catch Falling apples!"
                            , DB.Dt.Minigame1BestScore, Config.MINIGAME1_REWARD_SCORES
                            , DB.Dt.Minigame1RewardTriggers, Config.MINIGAME1_MAX_VAL
        );
        onInits[1] = () => init(Enum.MAP.Minigame2.ToString(), "Jump to the sky!"
                            , DB.Dt.Minigame2BestScore, Config.MINIGAME2_REWARD_SCORES
                            , DB.Dt.Minigame2RewardTriggers, Config.MINIGAME2_MAX_VAL
        );
    }


///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    void init(string name, string title, int bestScore, int[] rewardScores, bool[] rewardTrigger, int maxVal) {
        nameTxt.text = name;
        titleTxt.text = title;

        HM._.wmm.displayUnlockPopUp(null, name, true);

        if(bestScore == 0) {
            playPriceTxt.text = "Free";
            lvBtns[0].interactable = false;
        }
        else 
            playPriceTxt.text = Config.MINIGMAE_PLAY_PRICES[0].ToString();

        int easyScore = rewardScores[(int)Enum.MINIGAME_LV.Easy];
        int normalScore = rewardScores[(int)Enum.MINIGAME_LV.Normal];
        int hardScore = rewardScores[(int)Enum.MINIGAME_LV.Hard];

        //* LockFrame 表示
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Easy].SetActive(false);
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Normal].SetActive(!(bestScore >= easyScore));
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Hard].SetActive(!(bestScore >= normalScore));

        //* Slider Reward IconBtn 活性化
        if(bestScore >= easyScore) {
            bool isTrigger = !rewardTrigger[0];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Easy].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Easy].gameObject.SetActive(!isTrigger);
        }
        if(bestScore >= normalScore && !rewardTrigger[1]) {
            bool isTrigger = !rewardTrigger[1];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Normal].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Normal].gameObject.SetActive(!isTrigger);
        }
        if(bestScore >= hardScore  && !rewardTrigger[2]) {
            bool isTrigger = !rewardTrigger[2];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Hard].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Hard].gameObject.SetActive(!isTrigger);
        }

        //* Best Score Slider
        bestScoreSlider.value = bestScore;
        bestScoreSliderValTxt.text = $"<color=white>{bestScore}</color> / {maxVal}";
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    #region MINIGAME LEVEL POPUP
    public void onClickMinigameExclamationMarkBtn(int idx) { //* [3]:Minigame1, [4]:Minigame2, [5]:Minigame3
        Debug.Log($"onClickMinigameExclamationMarkBtn(idx={idx}):: ");
        DB._.SelectMapIdx = idx;
        minigameLvPopUp.SetActive(true);

        //* Init
        int offsetIdx = (idx == 3)? 0 : (idx == 4)? 1 : 2;
        onInits[offsetIdx].Invoke();
    }
    public void onClickMinigameLvPopUpLvBtn(int difficultyLvIdx) {
        //* ロックしたら、解禁条件のお知らせ
        if(lvBtnLockFrames[difficultyLvIdx].activeSelf) {
            HM._.ui.showErrorMsgPopUp($"Achieve {Config.MINIGAME1_REWARD_SCORES[difficultyLvIdx]} Best Score!");
            return;
        }

        const int EASY = 0, NORMAL = 1, HARD = 2;
        DB._.MinigameLv = difficultyLvIdx;

        //* Play Price
        playPriceTxt.text = Config.MINIGMAE_PLAY_PRICES[DB._.MinigameLv].ToString();

        //* 選択 枠
        for(int i = 0; i< lvBtns.Length; i++)
            lvBtnFocusLines[i].SetActive(i == difficultyLvIdx);

        //* 登場するアイテムの情報表示欄
        //* Minigame 1
        const int APPLE = 0, GOLDAPPLE = 1, DIAMOND = 2;
        switch(difficultyLvIdx) {
            case EASY:
                infoIcons[APPLE].SetActive(true);
                infoIcons[APPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_EASY_OBJ_DATA[APPLE]}";
                infoIcons[GOLDAPPLE].SetActive(true);
                infoIcons[GOLDAPPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_EASY_OBJ_DATA[GOLDAPPLE]}";
                infoIcons[DIAMOND].SetActive(false);
                break;
            case NORMAL:
                infoIcons[APPLE].SetActive(true);
                infoIcons[APPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_NORMAL_OBJ_DATA[APPLE]}";
                infoIcons[GOLDAPPLE].SetActive(true);
                infoIcons[GOLDAPPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_NORMAL_OBJ_DATA[GOLDAPPLE]}";
                infoIcons[DIAMOND].SetActive(true);
                infoIcons[DIAMOND].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_NORMAL_OBJ_DATA[DIAMOND]}";
                break;
            case HARD:
                infoIcons[APPLE].SetActive(true);
                infoIcons[APPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_HARD_OBJ_DATA[APPLE]}";
                infoIcons[GOLDAPPLE].SetActive(true);
                infoIcons[GOLDAPPLE].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_HARD_OBJ_DATA[GOLDAPPLE]}";
                infoIcons[DIAMOND].SetActive(true);
                infoIcons[DIAMOND].GetComponentInChildren<TextMeshProUGUI>().text = $"+{Config.MINIGAME1_HARD_OBJ_DATA[DIAMOND]}";
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
