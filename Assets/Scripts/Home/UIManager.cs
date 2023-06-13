using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour {
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
    [SerializeField] GameObject infoDialog; public GameObject InfoDialog {get => infoDialog; set => infoDialog = value;}

    [Header("SPACE")]
    [SerializeField] GameObject room; public GameObject Room {get => room; set => room = value;}
    [SerializeField] Transform roomObjectGroupTf; public Transform RoomObjectGroupTf {get => roomObjectGroupTf; set => roomObjectGroupTf = value;}
    [SerializeField] GameObject inventorySpace; public GameObject InventorySpace {get => inventorySpace;}
    [SerializeField] Vector3 roomDefPetPos;
    [SerializeField] Vector3 invSpacePlayerPos;
    [SerializeField] Vector3 invSpacePetPos;

    [Header("DIALOG")]
    [SerializeField] GameObject goGameDialog; public GameObject GoGameDialog {get => goGameDialog; set => goGameDialog = value;}
    [Header("POP UP")]
    [SerializeField] GameObject errorMsgPopUp;  public GameObject ErrorMsgPopUp {get => errorMsgPopUp; set => errorMsgPopUp = value;}
    [SerializeField] TextMeshProUGUI errorMsgTxt;   public TextMeshProUGUI ErrorMsgTxt {get => errorMsgTxt; set => errorMsgTxt = value;}


    void Start() {
        StartCoroutine(coUpdateUI());

        //* ホームシーンのパンネル配列 初期化
        homeScenePanelArr = new GameObject[] {
            roomPanel, ikeaShopPanel, clothShopPanel, inventoryPanel
        };

        //* ルームオブジェクト
        room.SetActive(true);
        decorateModePanel.SetActive(false);
        inventorySpace.SetActive(false);

        //* 看板
        woodSignTxt.text = "서재";//Enum.HOME.Room.ToString();

        //* 業績・ランク
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == 0)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == 0);
        }
        achiveRankTitleTxt.text = "업적";//Enum.ACHIVERANK.Achivement.ToString();

        //* インベントリー
        for(int i = 0; i < invTypeBtns.Length; i++) {
            invTypeBtns[i].GetComponent<Image>().color = (i == 0)? selectedTypeBtnClr : Color.white;
            invListFrames[i].SetActive(i == 0);
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

        //* パンネル 表示・非表示
        for(int i = 0; i < homeScenePanelArr.Length; i++)
            homeScenePanelArr[i].SetActive(curHomeSceneIdx == i);
        //* タッチの動き
        HM._.touchCtr.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);
        HM._.pl.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);

        //* 看板 テキスト
        woodSignTxt.text = (curHomeSceneIdx == 0)? "서재"//Enum.HOME.Room.ToString()
            : (curHomeSceneIdx == 1)? "가구점"//Enum.HOME.IkeaShop.ToString()
            : (curHomeSceneIdx == 2)? "의류점"//Enum.HOME.ClothShop.ToString()
            : "인벤토리";//Enum.HOME.Inventory.ToString();

        //* RoomとInventoryスペース 表示。
        if(curHomeSceneIdx == (int)Enum.HOME.Inventory){
            HM._.pl.Anim.enabled = false;
            HM._.pet.Anim.enabled = false;
            //* ポーズを初期化
            HM._.pl.setInitSpr();
            HM._.pet.setInitSpr();

            HM._.pl.transform.position = invSpacePlayerPos;
            HM._.pet.transform.position = invSpacePetPos;
            room.SetActive(false);
            inventorySpace.SetActive(true);
        }
        else {
            HM._.pl.Anim.enabled = true;
            HM._.pet.Anim.enabled = true;
            //* アイドルに初期化
            HM._.pl.setIdle();
            HM._.pet.setIdle();

            HM._.pl.transform.position = HM._.pl.TgPos;
            HM._.pet.transform.position = roomDefPetPos;
            room.SetActive(true);
            inventorySpace.SetActive(false);
        }
    }
    public void onClickGoRoom() {
        while(curHomeSceneIdx > (int)Enum.HOME.Room) {
            onClickWoodSignArrowBtn(dirVal: -1);
        }
    }
    public void onClickGoClothShop() {
        while(curHomeSceneIdx > (int)Enum.HOME.ClothShop) {
            onClickWoodSignArrowBtn(dirVal: -1);
        }
    }
//---------------------------------------------------------------------------------------------------------------------------------------------------
#region HOME ICON EVENT
//---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickAchiveRankIconBtn() {
        woodSignObj.SetActive(false);
        achiveRankPanel.SetActive(true);
    }
    public void onClickAchiveRankCloseBtn() {
        woodSignObj.SetActive(true);
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
//---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickGoGameDialogYesBtn() {
        HM._.GoToLoadingScene();
    }
    public void onClickAchiveRankTypeBtn(int idx) {
        //* Title
        achiveRankTitleTxt.text = (idx == 0)? "업적"//Enum.ACHIVERANK.Achivement.ToString()
            : (idx == 1)? "임무"//Enum.ACHIVERANK.Mission.ToString()
            : "랭킹";//Enum.ACHIVERANK.Rank.ToString(); // idx == 2

        //* Display
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == idx);
        }
    }
    public void onClickinventoryTypeBtn(int idx) {
        //* Display
        for(int i = 0; i < invTypeBtns.Length; i++) {
            invTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
            invListFrames[i].SetActive(i == idx);
        }
    }
    public void onClickInventoryItemListBtn() { //TODO Just Unlock Test
        infoDialog.SetActive(true);
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
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
        woodSignTxt.text = "서재";
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
    yield return Util.delay1;

    errorMsgPopUp.SetActive(false);
    errorMsgTxt.text = "";
}
#endregion
}
