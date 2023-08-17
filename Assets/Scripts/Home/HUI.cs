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
    UnityAction onAcceptRewardPopUp;   public UnityAction OnAcceptRewardPopUp {get => onAcceptRewardPopUp; set => onAcceptRewardPopUp = value;}
    UnityAction onDisplayNewItemPopUp; public UnityAction OnDisplayNewItemPopUp {get => onDisplayNewItemPopUp; set => onDisplayNewItemPopUp = value;}
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
    [SerializeField] Canvas canvasWorldMap;
    [SerializeField] GameObject topGroup; public GameObject TopGroup {get => topGroup; set => topGroup = value;}
    [SerializeField] GameObject[] homeScenePanelArr;    public GameObject[] HomeScenePanelArr {get => homeScenePanelArr; set => homeScenePanelArr = value;}
    [SerializeField] GameObject achiveRankPanel; public GameObject AchiveRankPanel {get => achiveRankPanel; set => achiveRankPanel = value;}
    [SerializeField] GameObject decorateModePanel; public GameObject DecorateModePanel {get => decorateModePanel; set => decorateModePanel = value;}
    [SerializeField] GameObject roomPanel; public GameObject RoomPanel {get => roomPanel; set => roomPanel = value;}
    [SerializeField] GameObject ikeaShopPanel; public GameObject IkeaShopPanel {get => ikeaShopPanel; set => ikeaShopPanel = value;}
    [SerializeField] GameObject clothShopPanel; public GameObject ClothShopPanel {get => clothShopPanel; set => clothShopPanel = value;}
    [SerializeField] GameObject inventoryPanel; public GameObject InventoryPanel {get => inventoryPanel; set => inventoryPanel = value;}
    [SerializeField] GameObject settingPanel;   public GameObject SettingPanel {get => settingPanel; set => settingPanel = value;}

    [Header("MAIN QUEST BOX (AT HOME)")]
    [SerializeField] Button mainQuestBoxBtn;    public Button MainQuestBoxBtn {get => mainQuestBoxBtn;}
    [SerializeField] Image mainQuestBoxIconImg;     public Image MainQuestBoxIconImg {get => mainQuestBoxIconImg;}
    [SerializeField] TextMeshProUGUI mainQuestBoxTitleTxt;     public TextMeshProUGUI MainQuestBoxTitleTxt {get => mainQuestBoxTitleTxt;}
    [SerializeField] Slider mainQuestBoxSlider;    public Slider MainQuestBoxSlider {get => mainQuestBoxSlider;}

    [Header("MENU TAP")]
    [SerializeField] RectTransform menuTapFrame;    public RectTransform MenuTapFrame {get => menuTapFrame;}
    [SerializeField] GameObject menuTapFrameObjCollider;

    [Header("SETTING")]
    [SerializeField] GameObject selectLangDialog;   public GameObject SelectLangDialog {get => selectLangDialog; set => selectLangDialog = value;}

    [SerializeField] TextMeshProUGUI nickName;      public TextMeshProUGUI NickName {get => nickName;}
    [SerializeField] TextMeshProUGUI[] levelTxts;   public TextMeshProUGUI[] LevelTxts {get => levelTxts; set => levelTxts = value;}
    [SerializeField] TextMeshProUGUI fameValTxt;   public TextMeshProUGUI FameValTxt {get => fameValTxt; set => fameValTxt = value;}
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

    [Header("SHOP & INV INFO DIALOG")]
    [SerializeField] int curSelectedItemIdx;    public int CurSelectedItemIdx {get => curSelectedItemIdx; set => curSelectedItemIdx = value;}
    [SerializeField] Sprite[] infoMoveBtnIconSprs;
    [SerializeField] Sprite[] priceIconSprs;    public Sprite[] PriceIconSprs {get => priceIconSprs;}
    [SerializeField] Sprite[] itemBtnFrameSprs; public Sprite[] ItemBtnFrameSprs {get => itemBtnFrameSprs;} // [0] : NORMAL, [1] : Special

    [SerializeField] GameObject infoDialog; public GameObject InfoDialog {get => infoDialog; set => infoDialog = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemNameTxt;    public TextMeshProUGUI InfoDlgItemNameTxt {get => infoDlgItemNameTxt; set => infoDlgItemNameTxt = value;}
    [SerializeField] Image infoDlgItemImg;    public Image InfoDlgItemImg {get => infoDlgItemImg; set => infoDlgItemImg = value;}
    
    [SerializeField] Transform infoDlgBtnGroup;    public Transform InfoDlgBtnGroup {get => infoDlgBtnGroup;}
    [SerializeField] Button infoDlgPurchaseBtn;    public Button InfoDlgPurchaseBtn {get => infoDlgPurchaseBtn; set => infoDlgPurchaseBtn = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemPriceTxt;    public TextMeshProUGUI InfoDlgItemPriceTxt {get => infoDlgItemPriceTxt; set => infoDlgItemPriceTxt = value;}
    [SerializeField] Button infoDlgMoveBtn;    public Button InfoDlgMoveBtn {get => infoDlgMoveBtn; set => infoDlgMoveBtn = value;}
    [SerializeField] Image infoDlgMoveIconImg;
    [SerializeField] TextMeshProUGUI infoDlgMoveCttTxt;    public TextMeshProUGUI InfoDlgMoveCttTxt {get => infoDlgMoveCttTxt; set => infoDlgMoveCttTxt = value;}
    [SerializeField] Button infoDlgArrangeBtn;    public Button InfoDlgArrangeBtn {get => infoDlgArrangeBtn; set => infoDlgArrangeBtn = value;}

    [Header("CANVAS ANIM")]
    [SerializeField] Animator switchScreenAnim; public Animator SwitchScreenAnim {get => switchScreenAnim;}
    [SerializeField] RectTransform handFocusTf;   public RectTransform HandFocusTf {get => handFocusTf;}

    

    [Header("POP UP")]
    [SerializeField] GameObject goGamePopUp; public GameObject GoGamePopUp {get => goGamePopUp; set => goGamePopUp = value;}
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
    [SerializeField] Image mapUnlockImg;    public Image MapUnlockImg {get => mapUnlockImg;}
    [SerializeField] TextMeshProUGUI mapUnlockPopUpNameTxt;    public TextMeshProUGUI MapUnlockPopUpNameTxt {get => mapUnlockPopUpNameTxt;}
    [SerializeField] TextMeshProUGUI mapUnlockPopUpCttTxt;    public TextMeshProUGUI MapUnlockPopUpCttTxt {get => mapUnlockPopUpCttTxt;}
    [Space(10)]
    [SerializeField] GameObject goMapPopUp;   public GameObject GoMapPopUp {get => goMapPopUp; set => goMapPopUp = value;}
    [SerializeField] Image goMapPopUpMapImg;     public Image GoMapPopUpMapImg {get => goMapPopUpMapImg; set => goMapPopUpMapImg = value;}
    [SerializeField] TextMeshProUGUI goMapPopUpTitleTxt;   public TextMeshProUGUI GoMapPopUpTitleTxt {get => goMapPopUpTitleTxt; set => goMapPopUpTitleTxt = value;}
    [Space(10)]
    [SerializeField] GameObject newFuniturePopUp;   public GameObject NewFuniturePopUp {get => newFuniturePopUp;}
    [SerializeField] Image newFuniturePopUpImg;   public Image NewFuniturePopUpImg {get => newFuniturePopUpImg;}
    [SerializeField] TextMeshProUGUI newFuniturePopUpTitleTxt;   public TextMeshProUGUI NewFuniturePopUpTitleTxt {get => newFuniturePopUpTitleTxt;}

    void Start() {
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
        const int QUEST = 1;
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == QUEST)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == QUEST);
        }
        achiveRankTitleTxt.text = LM._.localize("Achivement");//Enum.ACHIVERANK.Achivement.ToString();
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
        if(HM._.qm.IsFinishMainQuest) {
            showErrorMsgPopUp("먼저 메인퀘스트 달성을 완료해주세요.");
            return;
        }

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
        if(canvasWorldMap.gameObject.activeSelf) {
            Debug.Log("onClickGoRoom():: From Select World Map");
            Time.timeScale = 1;
            HM._.state = HM.STATE.NORMAL;
            canvasStatic.gameObject.SetActive(true);
            canvasWorldMap.gameObject.SetActive(false);
            goGamePopUp.SetActive(false);
        }
        else {
            Debug.Log("onClickGoRoom():: From ClothShop");
            while(curHomeSceneIdx > (int)Enum.HOME.Room) {
                onClickWoodSignArrowBtn(dirVal: -1);
            }
        }

    }
    public void onClickGoClothShop() {
        Debug.Log("onClickGoClothShop()::");
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
            Debug.Log("onClickAchiveRankIconBtn()::");
            topGroup.SetActive(false);
            infoDialog.SetActive(false);
            roomPanel.SetActive(true);
            achiveRankPanel.SetActive(true);
            HM._.qm.updateMainQuestList();
            handFocusTf.gameObject.SetActive(false);
        }
        public void onClickAchiveRankCloseBtn() {
            topGroup.SetActive(true);
            achiveRankPanel.SetActive(false);
            // woodSignObj.SetActive(true);
        }
        public void onClickDecorateModeIconBtn() {
            if(HM._.qm.IsFinishMainQuest) {
                showErrorMsgPopUp("먼저 메인퀘스트 달성을 완료해주세요.");
                return;
            }
            if(roomObjectGroupTf.childCount == 0) {
                HM._.ui.showErrorMsgPopUp("조정할 가구가 없습니다.");
                return;
            }
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
            : (idx == 1)? LM._.localize("Quest")//Enum.ACHIVERANK.Mission.ToString()
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
        Debug.Log("onClickPortraitBtn()::");
        HM._.state = HM.STATE.SETTING;
        topGroup.SetActive(false);
        infoDialog.SetActive(false);
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
    public void onClickResetBtn() {
        // DB.Dt.IsTutoRoomTrigger = true; DB.Dt.IsTutoFunitureShopTrigger = true; DB.Dt.IsTutoClothShopTrigger = true; DB.Dt.IsTutoInventoryTrigger = true; DB.Dt.IsTutoGoGameTrigger = true; DB.Dt.IsTutoWorldMapTrigger = true; DB.Dt.IsTutoFinishTrigger = true; DB.Dt.IsTutoDiagChoiceDiffTrigger = true; DB.Dt.IsTutoDiagFirstQuizTrigger = true; DB.Dt.IsTutoDiagFirstAnswerTrigger = true; DB.Dt.IsTutoDiagResultTrigger = true;
        DB._.reset();
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
        Debug.Log("onClickRewardPopUpAcceptBtn():: ");
        HM._.state = HM.STATE.NORMAL;
        rewardPopUp.SetActive(false);

        checkLevelUp();
        onAcceptRewardPopUp?.Invoke();
        onAcceptRewardPopUp = null; //* 初期化
    }

    #region SELECT MAP
    public void onClickGoGameDialogYesBtn() { // Choose Map
        Debug.Log("onClickGoGameDialogYesBtn()::");
        // if(HM._.qm.IsFinishMainQuest) {
            // showErrorMsgPopUp("먼저 메인퀘스트 달성을 완료해주세요.");
            // return;
        // }

        HM._.state = HM.STATE.NORMAL;
        canvasWorldMap.gameObject.SetActive(true);
        canvasStatic.gameObject.SetActive(false);
        goGamePopUp.gameObject.SetActive(false);

        //* Quest
        if(DB.Dt.IsTutoWorldMapTrigger)
            HM._.htm.action((int)HomeTalkManager.ID.TUTO_WORLDMAP);

        //* もし、メインクエストのACCEPTしなかったら、自動受託
        onClickAchiveRankIconBtn();
        var mq = HM._.qm.MainQuests[DB.Dt.MainQuestID];
        if(mq.AcceptBtn.gameObject.activeSelf){
            Debug.Log($"onClickGoGameDialogYesBtn():: MainQuest.name= {mq.name}, Auto Accept!");
            mq.acceptQuest();
        }
        onClickAchiveRankCloseBtn();
    }
    public void onClickGoGameDialogNoBtn() {
        HM._.state = HM.STATE.NORMAL;
        goGamePopUp.SetActive(false);
    }
    public void onClickBgArea(int idx) {
        DB._.SelectMapIdx = idx;
        var map = HM._.wmm.getMap(idx);
        Array.ForEach(map.BgBtns, bgBtn => {
            bgBtn.GetComponent<Image>().color = Color.yellow;
        });
        StartCoroutine(map.coBounceAnim());
        displayGoMapPupUp(map.MapName);
    }

    public void onClickGoMapPupUpYesBtn() {
        Time.timeScale = 1;
        StartCoroutine(HM._.GoToLoadingScene(Enum.SCENE.Loading.ToString()));
    }
    public void onClickGoMapPupUpCloseBtn() {
        var map = HM._.wmm.getMap(DB._.SelectMapIdx);
        Array.ForEach(map.BgBtns, bgBtn => {
            bgBtn.GetComponent<Image>().color = Color.white;
        });
        goMapPopUp.SetActive(false);
    }
    #endregion
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void activeHandFocus(Vector2 pos) {
        handFocusTf.gameObject.SetActive(true);
        handFocusTf.anchoredPosition = pos;
    }
    IEnumerator coShowTutorialFinish() {
        yield return Util.time1;
        if(!DB.Dt.IsTutoRoomTrigger
        && !DB.Dt.IsTutoFunitureShopTrigger
        && !DB.Dt.IsTutoClothShopTrigger
        && !DB.Dt.IsTutoInventoryTrigger
        && !DB.Dt.IsTutoGoGameTrigger
        && !DB.Dt.IsTutoWorldMapTrigger
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
                fameValTxt.text = DB.Dt.Fame.ToString();//$"+{HM._.pl.calcLvBonusPer() * 100}%";
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
    public void setInfoDlgData(Item item) {
        Debug.Log($"setInfoDlgData(item):: item.Name= {item.Name}, item.Price= {item.Price}");
        const int CLOTH_ICON = 0, QUEST_ICON = 1, MINIGAME_ICON = 2, FAME_ICON = 3;
        string moveBtncontent = "";
        Sprite moveBtnIconSpr = null;
        UnityAction onClickMoveBtn = () => {};
        infoDlgMoveBtn.onClick.RemoveAllListeners();

        //* 例外1（スキン）
        switch(item.Name) {
            case "GoldApple Pet":
                moveBtncontent = "Minigame1 Clear";
                moveBtnIconSpr = infoMoveBtnIconSprs[MINIGAME_ICON];
                onClickMoveBtn = onClickGoGameDialogYesBtn;
                
                break;
            default: 
                moveBtncontent = "Go Clothshop";
                moveBtnIconSpr = infoMoveBtnIconSprs[CLOTH_ICON];
                onClickMoveBtn = onClickGoClothShop;
                break;
        }

        //* 例外2（家具）
        if(item.Price.Split("_").Length >= 2) {
            string keyword = item.Price.Split("_")[0];
            string conditionCtt = item.Price.Split("_")[1];

            //* キーワードで、アイコンとイベント適用
            switch(keyword) {
                case "quest":
                    moveBtnIconSpr = infoMoveBtnIconSprs[QUEST_ICON];
                    onClickMoveBtn = onClickAchiveRankIconBtn;
                    break;
                case "fame": 
                    moveBtnIconSpr = infoMoveBtnIconSprs[FAME_ICON];
                    onClickMoveBtn = onClickPortraitBtn;
                    break;
            }

            //* 条件内容
            moveBtncontent = conditionCtt;
        }

        infoDlgMoveBtn.onClick.AddListener(onClickMoveBtn);

        infoDlgItemNameTxt.text = LM._.localize(item.Name);
        infoDlgItemImg.sprite = item.Spr;
        infoDlgItemPriceTxt.text = item.Price.ToString();

        //* MoveBtn
        infoDlgMoveIconImg.sprite = moveBtnIconSpr;
        infoDlgMoveCttTxt.text = moveBtncontent;
    }

    /// <summary>
    /// Infoダイアログ 表示
    /// </summary>
    /// <param name="idx">[0]: Purchase, [1]: MoveBtn, [2]: Arrange</param>
    public Button activeInfoDlgBtn(int idx) {
        Button resBtn = null;
        for(int i = 0; i < infoDlgBtnGroup.childCount; i++) {
            var btn = infoDlgBtnGroup.GetChild(i).GetComponent<Button>();
            btn.gameObject.SetActive(i == idx);
            if(i == idx) resBtn = btn;
        }
        return resBtn;
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
        goMapPopUp.SetActive(true);
        goMapPopUpMapImg.sprite = 
            //* Map
            (mapName == Enum.MAP.Forest.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Forest].MapSpr
            : (mapName == Enum.MAP.Jungle.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Jungle].MapSpr
            : (mapName == Enum.MAP.Tundra.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Tundra].MapSpr
            //* MiniGame
            : (mapName == Enum.MG.Minigame1.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Forest].MiniGameSpr
            : (mapName == Enum.MG.Minigame2.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Jungle].MiniGameSpr
            : (mapName == Enum.MG.Minigame3.ToString())? HM._.wmm.Maps[(int)Enum.MAP.Tundra].MiniGameSpr
            : null;
        if(goMapPopUpMapImg.sprite == null) {
            Debug.LogError("<color=red>Null Error : 探したMap名と一致する画像がないです。</color>");
            return;
        }
        goMapPopUpTitleTxt.text = mapName;
    }
    private void displayNewItemPopUp(Item item) {
        Time.timeScale = 0;
        item.purchase(isFree: true);
    }
    private void checkLevelUp() {
        if(DB._.LvUpCnt > 0) {
            Debug.Log($"checkLevelUp():: LvUpCnt= {DB._.LvUpCnt}");
            DB._.LvUpCnt = 0; //TODO Double Levelの場合対応
            StartCoroutine(coActiveLevelUpPopUp( new Dictionary<RewardItemSO, int>() {
                {rwdSOList[(int)Enum.RWD_IDX.Coin], DB.Dt.Lv * 100},
            }));
        }
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
        int curBonus = (int)(HM._.pl.calcLvBonusPer() * 100);
        int addBonus = (int)(Config.LV_BONUS_PER * 100);
        
        lvUpPopUpBonusTxt.text = $"{LM._.localize("Coin")} & {LM._.localize("Exp")} {LM._.localize("Bonus")} {curBonus}%\n<color=green>(+{addBonus}%)</color>";
        yield return coCreateRewardItemList(rewardDic, lvUpItemGroup);
    }
    public IEnumerator coActiveRewardPopUp(int fame, Dictionary<RewardItemSO, int> rewardDic) {
        HM._.state = HM.STATE.SETTING;
        rewardPopUp.SetActive(true);
        rewardPopUpFameValTxt.text = $"+{fame}";

        yield return coCreateRewardItemList(rewardDic, rewardItemGroup);
    }
    public void activeNewFuniturePopUp(Sprite spr, string name) {
        newFuniturePopUp.SetActive(true);
        newFuniturePopUpImg.sprite = spr;
        newFuniturePopUpTitleTxt.text = name;
    }
    private IEnumerator coCreateRewardItemList(Dictionary<RewardItemSO, int> rewardDic, Transform itemGroupTf) {
        const int SEPCIAL_EF = 0, SPRITE = 1, VAL = 2;
        var dt = DB.Dt;
        var funiArr = dt.Funitures;
        var decoArr = dt.Decorations;

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
                dt.setCoin(val);
            else if(rwdInfo.name == Enum.RWD_IDX.Exp.ToString()) {
                dt.Exp += val;
                dt.getExpPer();
            }
            //* Tutorial ➝ Reward Popupを閉じたら、すぐ読み出し
            else if(rwdInfo.name == Enum.RWD_IDX.WoodChair.ToString()) {
                int idx = Array.FindIndex(funiArr, item => item.Spr.name.Contains(Enum.RWD_IDX.WoodChair.ToString()));
                onAcceptRewardPopUp += () => displayNewItemPopUp(funiArr[idx]);
            }
            //* MainQuest Unlock Map ➝ WorldMapManager:: init():: onActionList.Add()として、順番通り読み出し
            else if(rwdInfo.name == Enum.RWD_IDX.FrogChair.ToString()) {
                int idx = Array.FindIndex(funiArr, item => item.Spr.name.Contains(Enum.RWD_IDX.FrogChair.ToString()));
                onDisplayNewItemPopUp = () => displayNewItemPopUp(funiArr[idx]); //-> WorldMapManager:: init():: onActionList.Add()へ使う
            }
            else if(rwdInfo.name == Enum.RWD_IDX.WoodenWolfStatue.ToString()) {
                int idx = Array.FindIndex(decoArr, item => item.Spr.name.Contains(Enum.RWD_IDX.WoodenWolfStatue.ToString()));
                onDisplayNewItemPopUp = () => displayNewItemPopUp(decoArr[idx]); //-> WorldMapManager:: init():: onActionList.Add()へ使う
            }
            else if(rwdInfo.name == Enum.RWD_IDX.GoldenMonkeyStatue.ToString()) {
                int idx = Array.FindIndex(decoArr, item => item.Spr.name.Contains(Enum.RWD_IDX.GoldenMonkeyStatue.ToString()));
                onDisplayNewItemPopUp = () => displayNewItemPopUp(decoArr[idx]); //-> WorldMapManager:: init():: onActionList.Add()へ使う
            }
            else if(rwdInfo.name == Enum.RWD_IDX.IceDragonStatue.ToString()) {
                int idx = Array.FindIndex(decoArr, item => item.Spr.name.Contains(Enum.RWD_IDX.IceDragonStatue.ToString()));
                onDisplayNewItemPopUp = () => displayNewItemPopUp(decoArr[idx]); //-> WorldMapManager:: init():: onActionList.Add()へ使う
            }
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
