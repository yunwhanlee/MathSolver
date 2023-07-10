using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] GameObject topGroup; public GameObject TopGroup {get => topGroup; set => topGroup = value;}
    [SerializeField] GameObject[] homeScenePanelArr;    public GameObject[] HomeScenePanelArr {get => homeScenePanelArr; set => homeScenePanelArr = value;}
    [SerializeField] GameObject achiveRankPanel; public GameObject AchiveRankPanel {get => achiveRankPanel; set => achiveRankPanel = value;}
    [SerializeField] GameObject decorateModePanel; public GameObject DecorateModePanel {get => decorateModePanel; set => decorateModePanel = value;}
    [SerializeField] GameObject roomPanel; public GameObject RoomPanel {get => roomPanel; set => roomPanel = value;}
    [SerializeField] GameObject ikeaShopPanel; public GameObject IkeaShopPanel {get => ikeaShopPanel; set => ikeaShopPanel = value;}
    [SerializeField] GameObject clothShopPanel; public GameObject ClothShopPanel {get => clothShopPanel; set => clothShopPanel = value;}
    [SerializeField] GameObject inventoryPanel; public GameObject InventoryPanel {get => inventoryPanel; set => inventoryPanel = value;}
    [SerializeField] GameObject settingPanel;   public GameObject SettingPanel {get => settingPanel; set => settingPanel = value;}

    [Header("SETTING")]
    [SerializeField] GameObject selectLangDialog;   public GameObject SelectLangDialog {get => selectLangDialog; set => selectLangDialog = value;}
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
    [SerializeField] GameObject errorMsgPopUp;  public GameObject ErrorMsgPopUp {get => errorMsgPopUp; set => errorMsgPopUp = value;}
    [SerializeField] TextMeshProUGUI errorMsgTxt;   public TextMeshProUGUI ErrorMsgTxt {get => errorMsgTxt; set => errorMsgTxt = value;}

    void Start() {
        switchScreenAnim.SetTrigger(Enum.ANIM.BlackOut.ToString());
        StartCoroutine(coShowTutorialFinish());
        StartCoroutine(coUpdateUI());

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
            HM._.htm.action((int)HomeTalkManager.TALK_ID_IDX.TUTORIAL_GOGAME);
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
        HM._.pl.animIdle();
        HM._.pet.animIdle();
        // 位置
        HM._.pl.transform.position = isInv? invSpacePlayerTf.position : HM._.pl.TgPos;
        HM._.pet.transform.position = isInv? invSpacePetTf.position : roomDefPetPos;
        // スペース 表示
        room.SetActive(!isInv);
        inventorySpace.SetActive(isInv);
    }
    public void onClickGoRoom() {
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

    public void onClickGoGameDialogYesBtn() { //* Go Game!
        StartCoroutine(HM._.GoToLoadingScene());
    }
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
    public void onClickSettingIconBtn() {
        HM._.state = HM.STATE.SETTING;
        settingPanel.SetActive(true);
        topGroup.SetActive(false);
    }
    public void onClickSettingPanelCloseBtn() {
        HM._.state = HM.STATE.NORMAL;
        settingPanel.SetActive(false);
        topGroup.SetActive(true);
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
        HM._.htm.action((int)HomeTalkManager.TALK_ID_IDX.TUTORIAL_FINISH);
    }
}

IEnumerator coUpdateUI() {
    while(true) {
        try {
            //* コイン
            coinTxt.text = DB.Dt.Coin.ToString();
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
    decorateModePanel.SetActive(isActive);
    HM._.funitureModeShadowFrameObj.SetActive(isActive);
    HM._.pl.gameObject.SetActive(!isActive);
    HM._.pet.gameObject.SetActive(!isActive);
}

public void showErrorMsgPopUp(string msg) => StartCoroutine(coShowErrorMsgPopUp(msg));
IEnumerator coShowErrorMsgPopUp(string msg) {
    errorMsgPopUp.SetActive(true);
    errorMsgTxt.text = msg;
    yield return Util.time1;

    errorMsgPopUp.SetActive(false);
    errorMsgTxt.text = "";
}
public void test_GetCoinFromFrontouthBoy() {
        DB.Dt.setCoin(10000);
        HM._.em.showEF((int)HEM.IDX.CoinBurstTopEF, HM._.pl.transform.position, Util.time1);
        HM._.pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
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


#endregion
}
