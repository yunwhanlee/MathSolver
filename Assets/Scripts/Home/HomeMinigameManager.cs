using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeMinigameManager : MonoBehaviour {
    [SerializeField] GameObject minigameLvPopUp;   public GameObject MinigameLvPopUp {get => minigameLvPopUp;}
    [SerializeField] GameObject[] infoIcons;          public GameObject[] InfoIcons {get => infoIcons;}

    [SerializeField] Button[] lvBtns;  public Button[] LvBtns {get => lvBtns;}
    [SerializeField] GameObject[] lvBtnFocusLines;
    [SerializeField] GameObject[] lvBtnLockFrames;
    [SerializeField] Button playBtn;   public Button PlayBtn {get => playBtn;}

    [SerializeField] Button[] rewardIconBtns;      public Button[] RewardIconBtns {get => rewardIconBtns;}
    [SerializeField] TextMeshProUGUI[] rewardIconBtnValTxts;      public TextMeshProUGUI[] RewardIconBtnValTxts {get => rewardIconBtnValTxts;}
    [SerializeField] Image[] rewardCheckIcons;      public Image[] RewardCheckIcons {get => rewardCheckIcons;}

    [SerializeField] Slider bestScoreSlider;   public Slider BestScoreSlider {get => bestScoreSlider;}
    [SerializeField] TextMeshProUGUI bestScoreSliderValTxt;


    void Start() {
        #region MINIGAME 1
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
        
        int mg1BestScore = DB.Dt.Minigame1BestScore;
        int easyScore = Config.MINIGAME1_REWARD_SCORES[(int)Enum.MINIGAME_LV.Easy];
        int normalScore = Config.MINIGAME1_REWARD_SCORES[(int)Enum.MINIGAME_LV.Normal];
        int hardScore = Config.MINIGAME1_REWARD_SCORES[(int)Enum.MINIGAME_LV.Hard];

        //* LockFrame 表示
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Easy].SetActive(false);
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Normal].SetActive(!(mg1BestScore >= easyScore));
        lvBtnLockFrames[(int)Enum.MINIGAME_LV.Hard].SetActive(!(mg1BestScore >= normalScore));

        //* Slider Reward IconBtn 活性化
        if(mg1BestScore >= easyScore) {
            bool isTrigger = !DB.Dt.Minigame1RewardTriggers[0];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Easy].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Easy].gameObject.SetActive(!isTrigger);
        }
        if(mg1BestScore >= normalScore && !DB.Dt.Minigame1RewardTriggers[1]) {
            bool isTrigger = !DB.Dt.Minigame1RewardTriggers[1];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Normal].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Normal].gameObject.SetActive(!isTrigger);
        }
        if(mg1BestScore >= hardScore  && !DB.Dt.Minigame1RewardTriggers[2]) {
            bool isTrigger = !DB.Dt.Minigame1RewardTriggers[2];
            rewardIconBtns[(int)Enum.MINIGAME_LV.Hard].interactable = isTrigger;
            rewardCheckIcons[(int)Enum.MINIGAME_LV.Hard].gameObject.SetActive(!isTrigger);
        }

        //* Best Score Slider
        bestScoreSlider.value = mg1BestScore;
        bestScoreSliderValTxt.text = $"<color=white>{mg1BestScore}</color> / {Config.MINIGAME1_MAX_VAL}";
        #endregion
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    #region MINIGAME LEVEL POPUP
    public void onClickMinigameExclamationMarkBtn(int idx) { //* [3]:Minigame1, [4]:Minigame2, [5]:Minigame3
        DB._.SelectMapIdx = idx; 
        minigameLvPopUp.SetActive(true);
    }
    public void onClickMinigameLvPopUpLvBtn(int difficultyLvIdx) {
        const int EASY = 0, NORMAL = 1, HARD = 2;
        DB._.MinigameLv = difficultyLvIdx;

        //* ロックしたら、解禁条件のお知らせ
        if(lvBtnLockFrames[difficultyLvIdx].activeSelf) {
            HM._.ui.showErrorMsgPopUp($"Achieve {Config.MINIGAME1_REWARD_SCORES[difficultyLvIdx]} Best Score!");
            return;
        }

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
        StartCoroutine(HM._.GoToLoadingScene(Enum.SCENE.MiniGame.ToString()));
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
                //TODO Unlock GoldApple Pet
                DB.Dt.Minigame1RewardTriggers[2] = true;
                rewardIconBtns[2].interactable = false;
                rewardCheckIcons[2].gameObject.SetActive(true);
                break;
        }
    }
    #endregion
#endregion
}
