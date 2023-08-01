using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class HUI : MonoBehaviour {
    [SerializeField] Color selectedTypeBtnClr;
    [SerializeField] TextMeshProUGUI coinTxt; public TextMeshProUGUI CoinTxt {get => coinTxt; set => coinTxt = value;}

    [Header("WOOD SIGN")]
    [SerializeField] int curHomeSceneIdx = 0; public int CurHomeSceneIdx {get => curHomeSceneIdx; set => curHomeSceneIdx = value;}
    [SerializeField] GameObject woodSignObj;  public GameObject WoodSignObj {get => woodSignObj; set => woodSignObj = value;}
    [SerializeField] TextMeshProUGUI woodSignTxt;  public TextMeshProUGUI WoodSignTxt {get => woodSignTxt; set => woodSignTxt = value;}
    [SerializeField] Button woodSignArrowLeftBtn;
    [SerializeField] Button woodSignArrowRightBtn;

    [Header("HOME PANEL")]
    [SerializeField] Canvas canvasStatic; 
    [SerializeField] Canvas canvasSelectMap;

    [SerializeField] GameObject topGroup; public GameObject TopGroup {get => topGroup; set => topGroup = value;}
    [SerializeField] GameObject[] homeScenePanelArr;    public GameObject[] HomeScenePanelArr {get => homeScenePanelArr; set => homeScenePanelArr = value;}
    [SerializeField] GameObject achiveRankPanel; public GameObject AchiveRankPanel {get => achiveRankPanel; set => achiveRankPanel = value;}
    [SerializeField] GameObject decorateModePanel; public GameObject DecorateModePanel {get => decorateModePanel; set => decorateModePanel = value;}
    [SerializeField] GameObject roomPanel; public GameObject RoomPanel {get => roomPanel; set => roomPanel = value;}
    [SerializeField] GameObject ikeaShopPanel; public GameObject IkeaShopPanel {get => ikeaShopPanel; set => ikeaShopPanel = value;}
    [SerializeField] GameObject clothShopPanel; public GameObject ClothShopPanel {get => clothShopPanel; set => clothShopPanel = value;}
    [SerializeField] GameObject inventoryPanel; public GameObject InventoryPanel {get => inventoryPanel; set => inventoryPanel = value;}
    [SerializeField] GameObject settingPanel;   public GameObject SettingPanel {get => settingPanel; set => settingPanel = value;}

    [Header("MENU TAP")]
    [SerializeField] RectTransform menuTapFrame;
    [SerializeField] GameObject menuTapFrameObjCollider;

    [Header("SETTING")]
    [SerializeField] GameObject selectLangDialog;   public GameObject SelectLangDialog {get => selectLangDialog; set => selectLangDialog = value;}

    [SerializeField] TextMeshProUGUI nickName;      public TextMeshProUGUI NickName {get => nickName;}
    [SerializeField] TextMeshProUGUI[] levelTxts;   public TextMeshProUGUI[] LevelTxts {get => levelTxts; set => levelTxts = value;}
    [SerializeField] TextMeshProUGUI lvBonusValTxt;   public TextMeshProUGUI LvBonusValTxt {get => lvBonusValTxt; set => lvBonusValTxt = value;}
    [SerializeField] TextMeshProUGUI legacyBonusValTxt;   public TextMeshProUGUI LegacyBonusValTxt {get => legacyBonusValTxt; set => legacyBonusValTxt = value;}
    [SerializeField] Image expFilledCircleBar;      public Image ExpFilledCircleBar {get => expFilledCircleBar; set => expFilledCircleBar = value;}
    [SerializeField] Slider settingExpSliderBar;    public Slider SettingExpSliderBar {get => settingExpSliderBar; set => settingExpSliderBar = value;}
    [SerializeField] TextMeshProUGUI settingExpSliderTxt;    public TextMeshProUGUI SettingExpSliderTxt {get => settingExpSliderTxt; set => settingExpSliderTxt = value;}

    [SerializeField] TextMeshProUGUI curLangTxt;  public TextMeshProUGUI CurLangTxt {get => curLangTxt; set => curLangTxt = value;}
    [SerializeField] Image curLangImg;  public Image CurLangImg {get => curLangImg; set => curLangImg = value;}
    [SerializeField] Button[] langBtns; public Button[] LangBtns {get => langBtns; set => langBtns = value;}

    [Header("ACHIVE & RANK")]
    [SerializeField] TextMeshProUGUI achiveRankTitleTxt; public TextMeshProUGUI AchiveRankTitleTxt {get => achiveRankTitleTxt; set => achiveRankTitleTxt = value;}
    [SerializeField] Button[] achiveRankTypeBtns; public Button[] AchiveRankTypeBtns {get => achiveRankTypeBtns; set => achiveRankTypeBtns = value;}
    [SerializeField] GameObject[] achiveRankScrollFrames; public GameObject[] AchiveRankScrollFrames {get => achiveRankScrollFrames; set => achiveRankScrollFrames = value;}

    [Header("FUNITURE SHOP")] //* スクロールではなく、９個のリストをタイプによって切り替えるだけ
    [SerializeField] GameObject[] funitureListFrames; public GameObject[] FunitureListFrames {get => funitureListFrames; set => funitureListFrames = value;}
    [SerializeField] GameObject funitureItemPf;

    [Header("INVENTORY")]
    [SerializeField] Button[] invTypeBtns; public Button[] InvTypeBtns {get => invTypeBtns; set => invTypeBtns = value;}
    [SerializeField] GameObject[] invListFrames; public GameObject[] InvListFrames {get => invListFrames; set => invListFrames = value;}

    [Header("SPACE")]
    [SerializeField] GameObject room; public GameObject Room {get => room; set => room = value;}
    [SerializeField] Transform roomObjectGroupTf; public Transform RoomObjectGroupTf {get => roomObjectGroupTf; set => roomObjectGroupTf = value;}
    [SerializeField] GameObject inventorySpace; public GameObject InventorySpace {get => inventorySpace;}
    [SerializeField] Vector3 roomDefPetPos;
    [SerializeField] Transform invSpacePlayerTf;
    [SerializeField] Transform invSpacePetTf;

    [Header("GO GAME DIALOG")]
    [SerializeField] GameObject goGameDialog; public GameObject GoGameDialog {get => goGameDialog; set => goGameDialog = value;}

    [Header("SHOP & INV INFO DIALOG")]
    [SerializeField] int curSelectedItemIdx;    public int CurSelectedItemIdx {get => curSelectedItemIdx; set => curSelectedItemIdx = value;}
    [SerializeField] GameObject infoDialog; public GameObject InfoDialog {get => infoDialog; set => infoDialog = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemNameTxt;    public TextMeshProUGUI InfoDlgItemNameTxt {get => infoDlgItemNameTxt; set => infoDlgItemNameTxt = value;}
    [SerializeField] Image infoDlgItemImg;    public Image InfoDlgItemImg {get => infoDlgItemImg; set => infoDlgItemImg = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemPriceTxt;    public TextMeshProUGUI InfoDlgItemPriceTxt {get => infoDlgItemPriceTxt; set => infoDlgItemPriceTxt = value;}
    [SerializeField] Button infoDlgPurchaseBtn;    public Button InfoDlgPurchaseBtn {get => infoDlgPurchaseBtn; set => infoDlgPurchaseBtn = value;}
    [SerializeField] Button infoDlgMoveBtn;    public Button InfoDlgMoveBtn {get => infoDlgMoveBtn; set => infoDlgMoveBtn = value;}

    [Header("CANVAS ANIM")]
    [SerializeField] Animator switchScreenAnim; public Animator SwitchScreenAnim {get => switchScreenAnim;}

    [Header("POP UP")]
    UnityAction onRewardPopUpAction;

    [SerializeField] GameObject errorMsgPopUp;  public GameObject ErrorMsgPopUp {get => errorMsgPopUp; set => errorMsgPopUp = value;}
    [SerializeField] TextMeshProUGUI errorMsgTxt;   public TextMeshProUGUI ErrorMsgTxt {get => errorMsgTxt; set => errorMsgTxt = value;}
    [Space(10)]
    [SerializeField] GameObject successMsgPopUp;  public GameObject SuccessMsgPopUp {get => successMsgPopUp; set => successMsgPopUp = value;}
    [SerializeField] TextMeshProUGUI successMsgTxt;   public TextMeshProUGUI SuccessMsgTxt {get => successMsgTxt; set => successMsgTxt = value;}
    [Space(10)]
    [SerializeField] GameObject nickNamePopUp;  public GameObject NickNamePopUp {get => nickNamePopUp; set => nickNamePopUp = value;}
    [SerializeField] TMP_InputField nickNameInputField;  public TMP_InputField NickNameInputField {get => nickNameInputField; set => nickNameInputField = value;}
    [Space(10)]
    [SerializeField] GameObject lvUpPopUp;   public GameObject LvUpPopUp {get => lvUpPopUp; set => lvUpPopUp = value;}
    [SerializeField] Transform lvUpItemGroup; public Transform LvUpItemGroup {get => lvUpItemGroup; set => lvUpItemGroup = value;}

    [SerializeField] TextMeshProUGUI lvUpPopUpValTxt;   public TextMeshProUGUI LvUpPopUpValTxt {get => lvUpPopUpValTxt; set => lvUpPopUpValTxt = value;}
    [SerializeField] TextMeshProUGUI lvUpPopUpBonusTxt;   public TextMeshProUGUI LvUpPopUpBonusTxt {get => lvUpPopUpBonusTxt; set => lvUpPopUpBonusTxt = value;}
    [Space(10)]
    [SerializeField] GameObject rewardPopUp;    public GameObject RewardPopUp {get => rewardPopUp; set => rewardPopUp = value;}
    [SerializeField] Transform rewardItemGroup; public Transform RewardItemGroup {get => rewardItemGroup; set => rewardItemGroup = value;}
    [SerializeField] TextMeshProUGUI rewardPopUpFameValTxt;    public TextMeshProUGUI RewardPopUpFameValTxt {get => rewardPopUpFameValTxt; set => rewardPopUpFameValTxt = value;}
    [SerializeField] GameObject rwdPf;   public GameObject RwdPf {get => rwdPf; set => rwdPf = value;}
    [SerializeField] List<RewardItemSO> rwdSOList;  public List<RewardItemSO> RwdSOList {get => rwdSOList;}
    [Space(10)]
    [SerializeField] GameObject mapUnlockPopUp;   public GameObject MapUnlockPopUp {get => mapUnlockPopUp; set => mapUnlockPopUp = value;}
    [SerializeField] RectTransform mapImageOutlineFrame;   public RectTransform MapImageOutlineFrame {get => mapImageOutlineFrame;}
    [SerializeField] TextMeshProUGUI mapUnlockPopUpNameTxt;    public TextMeshProUGUI MapUnlockPopUpNameTxt {get => mapUnlockPopUpNameTxt;}
    [SerializeField] TextMeshProUGUI mapUnlockPopUpCttTxt;    public TextMeshProUGUI MapUnlockPopUpCttTxt {get => mapUnlockPopUpCttTxt;}
    [Space(10)]
    [SerializeField] GameObject goMapPupUp;   public GameObject GoMapPupUp {get => goMapPupUp; set => goMapPupUp = value;}
    [SerializeField] TextMeshProUGUI goMapPupUpTitleTxt;   public TextMeshProUGUI GoMapPupUpTitleTxt {get => goMapPupUpTitleTxt; set => goMapPupUpTitleTxt = value;}
    [Space(10)]
    [SerializeField] GameObject newFuniturePopUp;   public GameObject NewFuniturePopUp {get => newFuniturePopUp;}
    [SerializeField] Image newFuniturePopUpImg;   public Image NewFuniturePopUpImg {get => newFuniturePopUpImg;}
    [SerializeField] TextMeshProUGUI newFuniturePopUpTitleTxt;   public TextMeshProUGUI NewFuniturePopUpTitleTxt {get => newFuniturePopUpTitleTxt;}


    void Start() {
        onRewardPopUpAction = () => {};
        switchScreenAnim.SetTrigger(Enum.ANIM.BlackOut.ToString());
        StartCoroutine(coShowTutorialFinish());
        StartCoroutine(coUpdateUI());

        //* Level Up Check
        checkLevelUp();

        //* Setting Add Event Listener
        const int EN = 0, KR = 1, JP = 2;
        langBtns[EN].onClick.AddListener(() => LM._.onClickLanguageSettingBtn(EN));
        langBtns[KR].onClick.AddListener(() => LM._.onClickLanguageSettingBtn(KR));
        langBtns[JP].onClick.AddListener(() => LM._.onClickLanguageSettingBtn(JP));

        //* Current Country Txt
        // curLangTxt.text = (LM._.curLangIndex == EN)? "English"
        //     :(LM._.curLangIndex == KR)? "한국어"
        //     : "日本語";
        //* Current Country Icon
        curLangImg.sprite = (LM._.curLangIndex == EN)? HM._.conturiesIcons[EN] 
            :(LM._.curLangIndex == KR)? HM._.conturiesIcons[KR]
            : HM._.conturiesIcons[JP];


        //* ホームシーンのパンネル配列 初期化
        homeScenePanelArr = new GameObject[] {
            roomPanel, ikeaShopPanel, clothShopPanel, inventoryPanel
        };

        //* ルームオブジェクト
        room.SetActive(true);
        decorateModePanel.SetActive(false);
        inventorySpace.SetActive(false);

        //* 看板
        woodSignTxt.text = LM._.localize("Home");//"서재";//Enum.HOME.Room.ToString();

        //* 業績・ランク
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == 0)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == 0);
        }
        achiveRankTitleTxt.text = LM._.localize("Achivement");;//Enum.ACHIVERANK.Achivement.ToString();
    }

    void Update() {
        //! TEST
        if(Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("GetKeyDown(KeyCode.Q)");
            StartCoroutine(coActiveLevelUpPopUp( new Dictionary<RewardItemSO, int>() {
                {rwdSOList[(int)Enum.RWD_IDX.Coin], 100},
            }));
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            Debug.Log("GetKeyDown(KeyCode.W)");
            StartCoroutine(coActiveRewardPopUp(fame: 5, new Dictionary<RewardItemSO, int>() {
                {rwdSOList[(int)Enum.RWD_IDX.Coin], 100},
                {rwdSOList[(int)Enum.RWD_IDX.Exp], 300},
            }));
        }
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickWoodSignArrowBtn(int dirVal) { //* directionValue : -1 or 1にすること。
        //* 現在シーン パンネル INDEX
        if(dirVal == -1 || dirVal == 1) {
            curHomeSceneIdx += dirVal; // Left or Right
            curHomeSceneIdx %= homeScenePanelArr.Length; // Index 範囲

            //* マイナスの場合、Maxに戻す
            if(curHomeSceneIdx < 0)
                curHomeSceneIdx = homeScenePanelArr.Length - 1;
        }
        else{
            Debug.LogWarning("onClickWoodSignArrowBtn()イベントメソッドのパラメータ値を -1 とか1に設定してください。");
        }

        Debug.Log($"onClickWoodSignArrowBtn():: curHomeSceneIdx= {curHomeSceneIdx}");
        
        if(!DB.Dt.IsTutoRoomTrigger
        && !DB.Dt.IsTutoFunitureShopTrigger
        && !DB.Dt.IsTutoClothShopTrigger
        && !DB.Dt.IsTutoInventoryTrigger
        && DB.Dt.IsTutoGoGameTrigger) {
            HM._.htm.action((int)HomeTalkManager.ID.TUTO_GOGAME);
        }

        //* パンネル 表示・非表示
        for(int i = 0; i < homeScenePanelArr.Length; i++)
            homeScenePanelArr[i].SetActive(curHomeSceneIdx == i);
        //* ダイアログ 非表示
        infoDialog.SetActive(false);
        //* タッチの動き
        HM._.touchCtr.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);
        HM._.pl.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);

        //* 看板 テキスト
        woodSignTxt.text = (curHomeSceneIdx == 0)? LM._.localize("Home")
            : (curHomeSceneIdx == 1)? LM._.localize("Funitureshop")
            : (curHomeSceneIdx == 2)? LM._.localize("Clothshop")
            : LM._.localize("Inventory");

        //* InventoryとRoomのスペース 調整
        bool isInv = curHomeSceneIdx == (int)Enum.HOME.Inventory;
        // アニメー初期化
        HM._.pl.idle();
        HM._.pet.animIdle();
        // 位置
        HM._.pl.transform.position = isInv? invSpacePlayerTf.position : HM._.pl.TgPos;
        HM._.pet.transform.position = isInv? invSpacePetTf.position : roomDefPetPos;
        // スペース 表示
        room.SetActive(!isInv);
        inventorySpace.SetActive(isInv);
    }

    public void onClickGoRoom() {
        //* From Select Map
        if(canvasSelectMap) {
            Time.timeScale = 1;
            HM._.state = HM.STATE.NORMAL;
            canvasStatic.gameObject.SetActive(true);
            canvasSelectMap.gameObject.SetActive(false);
            goGameDialog.SetActive(false);
            return;
        }

        //* From Wood Arrow
        while(curHomeSceneIdx > (int)Enum.HOME.Room) {
            onClickWoodSignArrowBtn(dirVal: -1);
        }
    }
    public void onClickGoClothShop() {
        infoDialog.SetActive(false);
        while(curHomeSceneIdx > (int)Enum.HOME.ClothShop) {
            onClickWoodSignArrowBtn(dirVal: -1);
        }
    }

    #region HOME ICON EVENT
        public void onClickMenuToogleBtn() {
            bool isToggle = menuTapFrame.anchoredPosition.x == 0;
            //* プレイヤー移動できないコライダー移動
            menuTapFrameObjCollider.transform.localPosition = new Vector2(isToggle? 2.25f: 1.1f, menuTapFrameObjCollider.transform.position.y);
            //* UI メニューFrame移動アニメー
            StartCoroutine(coPlayMenuToogleBtnMoveAnim(isToggle)); // menuTapFrame.anchoredPosition = new Vector2(isToggle? 250 : 0, 0);
        }
        public void onClickAchiveRankIconBtn() {
            topGroup.SetActive(false);
            // woodSignObj.SetActive(false);
            achiveRankPanel.SetActive(true);
        }
        public void onClickAchiveRankCloseBtn() {
            topGroup.SetActive(true);
            // woodSignObj.SetActive(true);
            achiveRankPanel.SetActive(false);
        }
        public void onClickDecorateModeIconBtn() {
            setDecorationMode(isActive: true);
        }
        public void onClickDecorateModeCloseBtn() {
            HM._.fUI.setUpFunitureModeItem(isCancel: true);
            setDecorationMode(isActive: false);
        }
    #endregion

    public void onClickAchiveRankTypeBtn(int idx) {
        //* Title
        achiveRankTitleTxt.text = (idx == 0)? LM._.localize("Achivement")
            : (idx == 1)? LM._.localize("Mission")//Enum.ACHIVERANK.Mission.ToString()
            : LM._.localize("Rank");//Enum.ACHIVERANK.Rank.ToString(); // idx == 2

        //* Display
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == idx);
        }
    }
    public void onClickInventoryItemListBtn() {
        infoDialog.SetActive(true);
    }
    public void onClickPortraitBtn() {
        HM._.state = HM.STATE.SETTING;
        topGroup.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void onClickSettingPanelCloseBtn() {
        HM._.state = HM.STATE.NORMAL;
        topGroup.SetActive(true);
        settingPanel.SetActive(false);
    }
    public void onClickLanguageBtn() {
        selectLangDialog.SetActive(true);
    }
    public void onClickTutorialSettingBtn() {
        DB.Dt.IsTutoRoomTrigger = true;
        DB.Dt.IsTutoFunitureShopTrigger = true;
        DB.Dt.IsTutoClothShopTrigger = true;
        DB.Dt.IsTutoInventoryTrigger = true;
        DB.Dt.IsTutoGoGameTrigger = true;
        DB.Dt.IsTutoFinishTrigger = true;
        DB.Dt.IsTutoDiagChoiceDiffTrigger = true;
        DB.Dt.IsTutoDiagFirstQuizTrigger = true;
        DB.Dt.IsTutoDiagFirstAnswerTrigger = true;
        DB.Dt.IsTutoDiagResultTrigger = true;
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public void onLimitLengthNickNameInputTxt() {
        int maxChars = isEnglish(nickNameInputField.text) ? 12 : 6;
        if (nickNameInputField.text.Length > maxChars) {
            nickNameInputField.text = nickNameInputField.text.Substring(0, maxChars);
        }
    }
    public void onClickChangeNickNameBtn() {
        showNickNamePopUp(isActive: true);
    }
    public void onClickNickNameOkBtn() {
        if(nickNameInputField.text.Length == 0) {
            showErrorMsgPopUp("Please Input Nickname!");
            return;
        }
        //* 処理
        showNickNamePopUp(isActive: false);
        DB.Dt.NickName = nickNameInputField.text;
        nickName.text = DB.Dt.NickName;
        if(!HM._.htm.IsAction) showSuccessMsgPopUp("NickName Change Success!");
    }
    public void onClickNickNamePopUpExitBtn() {
        showNickNamePopUp(isActive: false);
    }
    public void onClickLevelUpPopUpAcceptBtn() {
        HM._.state = HM.STATE.NORMAL;
        lvUpPopUp.SetActive(false);
    }
    public void onClickRewardPopUpAcceptBtn() {
        HM._.state = HM.STATE.NORMAL;
        rewardPopUp.SetActive(false);

        checkLevelUp();
        onRewardPopUpAction();
    }

    #region SELECT MAP
        public void onClickGoGameDialogYesBtn() { // Choose Map
            HM._.state = HM.STATE.NORMAL;
            canvasSelectMap.gameObject.SetActive(true);
            canvasStatic.gameObject.SetActive(false);
            HM._.htm.action((int)HomeTalkManager.ID.TUTO_WORLDMAP);
        }
        public void onClickGoGameDialogNoBtn() {
            HM._.state = HM.STATE.NORMAL;
            goGameDialog.SetActive(false);
        }
        public void onClickBgArea(int idx) {
            DB._.SelectMapIdx = idx;
            var map = HM._.wmm.Maps[idx];
            Array.ForEach(map.BgBtns, bgBtn => {
                bgBtn.GetComponent<Image>().color = Color.yellow;
            });
            StartCoroutine(map.coPlayBounce());
            displayGoMapPupUp(map.MapName);
        }
        public void onClickGoMapPupUpYesBtn() {
            Time.timeScale = 1;
            StartCoroutine(HM._.GoToLoadingScene());
        }
        public void onClickGoMapPupUpCloseBtn() {
            var map = HM._.wmm.Maps[DB._.SelectMapIdx];
            Array.ForEach(map.BgBtns, bgBtn => {
                bgBtn.GetComponent<Image>().color = Color.white;
            });
            goMapPupUp.SetActive(false);
        }
    #endregion
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator coShowTutorialFinish() {
        yield return Util.time1;
        if(!DB.Dt.IsTutoRoomTrigger
        && !DB.Dt.IsTutoFunitureShopTrigger
        && !DB.Dt.IsTutoClothShopTrigger
        && !DB.Dt.IsTutoInventoryTrigger
        && !DB.Dt.IsTutoGoGameTrigger
        && !DB.Dt.IsTutoDiagChoiceDiffTrigger
        && !DB.Dt.IsTutoDiagFirstQuizTrigger
        && !DB.Dt.IsTutoDiagFirstAnswerTrigger
        && !DB.Dt.IsTutoDiagResultTrigger
        && DB.Dt.IsTutoFinishTrigger) {
            HM._.htm.action((int)HomeTalkManager.ID.TUTO_FINISH);
        }
    }

    IEnumerator coUpdateUI() {
        while(true) {
            try {
                //* コイン
                coinTxt.text = DB.Dt.Coin.ToString();

                //* 
                Array.ForEach(levelTxts, lvTxt => lvTxt.text = DB.Dt.Lv.ToString());
                lvBonusValTxt.text = $"+{HM._.pl.calcLvBonusPer() * 100}%";
                legacyBonusValTxt.text = $"+{HM._.pl.calcLegacyBonusPer() * 100}%";

                //* 
                const int MAX_UNIT = 100;
                expFilledCircleBar.fillAmount = DB.Dt.getExpPer();
                settingExpSliderBar.value = DB.Dt.getExpPer();
                settingExpSliderTxt.text = $"{DB.Dt.Exp} / {MAX_UNIT * DB.Dt.Lv}";
            }
            catch(Exception err) {
                Debug.LogWarning($"ERROR: {err}");
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void setDecorationMode(bool isActive) {
        HM._.state = isActive? HM.STATE.DECORATION_MODE : HM.STATE.NORMAL;

        //* UI 表示・非表示
        if(isActive) {
            for(int i = 0; i < homeScenePanelArr.Length; i++) homeScenePanelArr[i].SetActive(!isActive);
        }
        else {
            HM._.fUI.CurSelectedObj = null;
            curHomeSceneIdx = 0;
            woodSignTxt.text = LM._.localize("Home");
            roomPanel.SetActive(true);
        }

        topGroup.SetActive(!isActive);
        HM._.ui.menuTapFrameObjCollider.SetActive(!isActive);
        decorateModePanel.SetActive(isActive);
        HM._.funitureModeShadowFrameObj.SetActive(isActive);
        HM._.pl.gameObject.SetActive(!isActive);
        HM._.pet.gameObject.SetActive(!isActive);
    }

    private bool isEnglish(string text) {
        foreach (char c in text) {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                return true;
        }
        return false;
    }

    private void displayGoMapPupUp(string mapName) {
        goMapPupUp.SetActive(true);
        goMapPupUpTitleTxt.text = mapName;

    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region POP UP
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void showErrorMsgPopUp(string msg) => StartCoroutine(coErrorMsgPopUp(msg));
    IEnumerator coErrorMsgPopUp(string msg) {
        errorMsgPopUp.SetActive(true);
        errorMsgTxt.text = msg;
        yield return Util.realTime1;

        errorMsgPopUp.SetActive(false);
        errorMsgTxt.text = "";
    }
    public void showSuccessMsgPopUp(string msg) => StartCoroutine(coSuccessMsgPopUp(msg));
    IEnumerator coSuccessMsgPopUp(string msg) {
        successMsgPopUp.SetActive(true);
        successMsgTxt.text = msg;
        yield return Util.realTime1;

        successMsgPopUp.SetActive(false);
        successMsgTxt.text = "";
    }
    public void showNickNamePopUp(bool isActive) {
        HM._.state = (isActive)? HM.STATE.SETTING : HM.STATE.NORMAL;
        nickNamePopUp.SetActive(isActive);
        settingPanel.SetActive(!isActive); //* BUG なぜかこらが表示されたら、InputFeild入力できない
        if(HM._.htm.IsAction) settingPanel.SetActive(false);
        if(isActive) nickNameInputField.text = DB.Dt.NickName;
    }
    IEnumerator coActiveLevelUpPopUp(Dictionary<RewardItemSO, int> rewardDic) {
        yield return Util.time1;
        HM._.state = HM.STATE.SETTING;
        lvUpPopUp.SetActive(true);
        lvUpPopUpValTxt.text = DB.Dt.Lv.ToString();
        lvUpPopUpBonusTxt.text = $"Bonus\nCoin & Exp +{HM._.pl.calcLvBonusPer() * 100}%";

        yield return coCreateRewardItemList(rewardDic, lvUpItemGroup);
    }
    public IEnumerator coActiveRewardPopUp(int fame, Dictionary<RewardItemSO, int> rewardDic) {
        HM._.state = HM.STATE.SETTING;
        rewardPopUp.SetActive(true);
        rewardPopUpFameValTxt.text = $"+{fame}";

        yield return coCreateRewardItemList(rewardDic, rewardItemGroup);
    }
    public void activeNewFuniturePupUp(Sprite spr, string name) {
        newFuniturePopUp.SetActive(true);
        newFuniturePopUpImg.sprite = spr;
        newFuniturePopUpTitleTxt.text = name;
    }
    private IEnumerator coCreateRewardItemList(Dictionary<RewardItemSO, int> rewardDic, Transform itemGroupTf) {
        const int SEPCIAL_EF = 0, SPRITE = 1, VAL = 2;

        //* init ItemGroup
        foreach(Transform chd in itemGroupTf) Destroy(chd.gameObject);

        //* Instantiate
        foreach(var pair in rewardDic) {
            yield return Util.time0_2;
            RewardItemSO rwdInfo = pair.Key;
            int val = pair.Value;
            Debug.Log($"coCreateRewardItemList():: rwdInfo= {rwdInfo}");

            //* UI
            Transform ins = Instantiate(rwdPf, itemGroupTf).transform;
            ins.GetChild(SEPCIAL_EF).gameObject.SetActive(rwdInfo.IsSpecial);
            ins.GetChild(SPRITE).GetComponent<Image>().sprite = rwdInfo.Spr;
            ins.GetChild(SPRITE).GetComponent<Image>().color = rwdInfo.Clr;
            ins.GetChild(VAL).GetComponent<TextMeshProUGUI>().text = val.ToString();

            //* Set Data
            if(rwdInfo.name == Enum.RWD_IDX.Coin.ToString())
                DB.Dt.setCoin(val);
            else if(rwdInfo.name == Enum.RWD_IDX.Exp.ToString()) {
                DB.Dt.Exp += val;
                DB.Dt.getExpPer();
            }
            else if(rwdInfo.name == Enum.RWD_IDX.WoodChair.ToString()) {
                //* アイテムリワードPopUp CallBack
                const int WOOD_CHAIR = 0;
                onRewardPopUpAction += () => setRewardItemObj(DB.Dt.Funitures[WOOD_CHAIR]);
            }
        }
    }
    private void setRewardItemObj(Item item) {
        item.purchase(isFree: true);
    }
    private void checkLevelUp() {
        if(DB._.LvUpCnt > 0) {
            DB._.LvUpCnt = 0; //TODO Double Levelの場合対応
            StartCoroutine(coActiveLevelUpPopUp( new Dictionary<RewardItemSO, int>() {
                {rwdSOList[(int)Enum.RWD_IDX.Coin], DB.Dt.Lv * 100},
            }));
        }
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region CANVAS ANIM
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void playSwitchScreenAnim() {
        switchScreenAnim.SetTrigger(Enum.ANIM.BlackInOut.ToString());
        // StartCoroutine(coPlaySwitchScreenAnim());
    }
    IEnumerator coPlaySwitchScreenAnim() {
        switchScreenAnim.gameObject.SetActive(true);
        yield return Util.time1;
        yield return Util.time0_5;
        switchScreenAnim.gameObject.SetActive(false);
    }
    IEnumerator coPlayMenuToogleBtnMoveAnim(bool isToggle) {
        float targetX = isToggle ? 250f : 0f; // 移動する位置
        float duration = 0.2f; // 掛かる時間
        float elapsed = 0f; // 結果時間

        Vector2 startPos = menuTapFrame.anchoredPosition;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Vector2 newPosition = Vector2.Lerp(startPos, new Vector2(targetX, startPos.y), t);
            menuTapFrame.anchoredPosition = newPosition;
            yield return null;
        }
        //* Lerpなので、最後に正確な位置に調整
        menuTapFrame.anchoredPosition = new Vector2(targetX, startPos.y);

        yield break;
    }
#endregion
}
