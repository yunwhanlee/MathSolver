using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Color selectedTypeBtnClr;

    [Header("WOOD SIGN")]
    [SerializeField] int curHomeSceneIdx = 0;
    [SerializeField] GameObject woodSignObj;  public GameObject WoodSignObj {get => woodSignObj; set => woodSignObj = value;}
    [SerializeField] TextMeshProUGUI woodSignTxt;  public TextMeshProUGUI WoodSignTxt {get => woodSignTxt; set => woodSignTxt = value;}
    [SerializeField] Button woodSignArrowLeftBtn;
    [SerializeField] Button woodSignArrowRightBtn;

    [Header("HOME PANEL")]
    [SerializeField] GameObject[] homeScenePanelArr;
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
    [SerializeField] Button[] funitureTypeBtns; public Button[] FunitureTypeBtns {get => funitureTypeBtns; set => funitureTypeBtns = value;}


    [Header("INVENTORY")]
    [SerializeField] Button[] invTypeBtns; public Button[] InvTypeBtns {get => invTypeBtns; set => invTypeBtns = value;}
    [SerializeField] GameObject[] invListFrames; public GameObject[] InvListFrames {get => invListFrames; set => invListFrames = value;}

    [Header("SPACE")]
    [SerializeField] GameObject room; public GameObject Room {get => room; set => room = value;}
    [SerializeField] GameObject inventorySpace; public GameObject InventorySpace {get => inventorySpace;}
    [SerializeField] Vector3 roomDefPetPos;
    [SerializeField] Vector3 invSpacePlayerPos;
    [SerializeField] Vector3 invSpacePetPos;

    [Header("DIALOG")]
    [SerializeField] GameObject goGameDialog; public GameObject GoGameDialog {get => goGameDialog; set => goGameDialog = value;}


    void Start() {
        //* ホームシーンのパンネル配列 初期化
        homeScenePanelArr = new GameObject[] {
            roomPanel, ikeaShopPanel, clothShopPanel, inventoryPanel
        };

        //* ルームオブジェクト
        room.SetActive(true);
        inventorySpace.SetActive(false);

        //* 看板
        woodSignTxt.text = Enum.HOME.Room.ToString();

        //* 業績・ランク
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == 0)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == 0);
        }
        achiveRankTitleTxt.text = Enum.ACHIVERANK.Achivement.ToString();

        //* 家具店
        for(int i = 0; i < funitureTypeBtns.Length; i++) {
            funitureTypeBtns[i].GetComponent<Image>().color = (i == 0)? selectedTypeBtnClr : Color.white;
        }

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

        //* 表示・非表示
        for(int i = 0; i < homeScenePanelArr.Length; i++) {
            homeScenePanelArr[i].SetActive(curHomeSceneIdx == i);
            //* タッチの動き
            GM._.touchCtr.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);
            GM._.pl.enabled = (curHomeSceneIdx == (int)Enum.HOME.Room);
        }

        //* 看板 テキスト
        woodSignTxt.text = (curHomeSceneIdx == 0)? Enum.HOME.Room.ToString()
            : (curHomeSceneIdx == 1)? Enum.HOME.IkeaShop.ToString()
            : (curHomeSceneIdx == 2)? Enum.HOME.ClothShop.ToString()
            : Enum.HOME.Inventory.ToString();

        //* RoomとInventoryスペース 表示。
        if(curHomeSceneIdx == (int)Enum.HOME.Room) {
            GM._.pl.transform.position = GM._.pl.TargetPos;
            GM._.pet.transform.position = roomDefPetPos;
            room.SetActive(true);
            inventorySpace.SetActive(false);
        }
        else if(curHomeSceneIdx == (int)Enum.HOME.Inventory){
            GM._.pl.transform.position = invSpacePlayerPos;
            GM._.pet.transform.position = invSpacePetPos;
            room.SetActive(false);
            inventorySpace.SetActive(true);
        }
    }
    public void onClickAchiveRankIconBtn() {
        woodSignObj.SetActive(false);
        achiveRankPanel.SetActive(true);
    }
    public void onClickAchiveRankCloseBtn() {
        woodSignObj.SetActive(true);
        achiveRankPanel.SetActive(false);
    }
    public void onClickDecorateModeIconBtn() {
        woodSignObj.SetActive(false);
        decorateModePanel.SetActive(true);
        GM._.pl.gameObject.SetActive(false);
        GM._.pet.gameObject.SetActive(false);
    }
    public void onClickDecorateModeCloseBtn() {
        woodSignObj.SetActive(true);
        decorateModePanel.SetActive(false);
        GM._.pl.gameObject.SetActive(true);
        GM._.pet.gameObject.SetActive(true);
    }
    public void onClickGoGameDialogYesBtn() {
        GM._.GoToLoadingScene();
    }
    public void onClickAchiveRankTypeBtn(int idx) {
        //* Title
        achiveRankTitleTxt.text = (idx == 0)? Enum.ACHIVERANK.Achivement.ToString()
            : (idx == 1)? Enum.ACHIVERANK.Mission.ToString()
            : Enum.ACHIVERANK.Rank.ToString(); // idx == 2

        //* Display
        for(int i = 0; i < achiveRankTypeBtns.Length; i++) {
            achiveRankTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
            achiveRankScrollFrames[i].SetActive(i == idx);
        }
    }
    public void onClickFunitureShopTypeBtn(int idx) {
        //* Display
        for(int i = 0; i < funitureTypeBtns.Length; i++) {
            funitureTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
        }
    }
    public void onClickinventoryTypeBtn(int idx) {
        //* Display
        for(int i = 0; i < invTypeBtns.Length; i++) {
            invTypeBtns[i].GetComponent<Image>().color = (i == idx)? selectedTypeBtnClr : Color.white;
            invListFrames[i].SetActive(i == idx);
        }
    }
#endregion
}
