using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("WOOD SIGN")]
    [SerializeField] int curHomeSceneIdx = 0;
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

    [Header("SPACE")]
    [SerializeField] GameObject room; public GameObject Room {get => room; set => room = value;}
    [SerializeField] GameObject inventorySpace; public GameObject InventorySpace {get => inventorySpace;}
    [SerializeField] Vector3 roomDefPetPos;
    [SerializeField] Vector3 invSpacePlayerPos;
    [SerializeField] Vector3 invSpacePetPos;

    [Header("DIALOG")]
    [SerializeField] GameObject goGameDialog; public GameObject GoGameDialog {get => goGameDialog; set => goGameDialog = value;}


    void Start() {
        woodSignTxt.text = Enum.HOME.Room.ToString();
        
        homeScenePanelArr = new GameObject[] {
            roomPanel, ikeaShopPanel, clothShopPanel, inventoryPanel
        };
    }

    void Update() {
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
        }

        //* 看板 テキスト
        woodSignTxt.text = (curHomeSceneIdx == 0)? Enum.HOME.Room.ToString()
            : (curHomeSceneIdx == 1)? Enum.HOME.IkeaShop.ToString()
            : (curHomeSceneIdx == 2)? Enum.HOME.ClothShop.ToString()
            : Enum.HOME.Inventory.ToString();

        //* RoomとInventoryスペース 表示。
        if(curHomeSceneIdx == (int)Enum.HOME.Room) {
            GM._.pl.enabled = true; // 動ける
            GM._.pl.transform.position = GM._.pl.MoveTargetPos;
            GM._.pet.transform.position = roomDefPetPos;
            room.SetActive(true);
            inventorySpace.SetActive(false);
        }
        else if(curHomeSceneIdx == (int)Enum.HOME.Inventory){
            GM._.pl.enabled = false; // 動かない
            GM._.pl.transform.position = invSpacePlayerPos;
            GM._.pet.transform.position = invSpacePetPos;
            room.SetActive(false);
            inventorySpace.SetActive(true);
        }
    }
    public void onClickAchiveRankIconBtn() {
        achiveRankPanel.SetActive(true);
    }
    public void onClickDecorateModeIconBtn() {
        decorateModePanel.SetActive(true);
    }
    public void onClickGoGameDialogYesBtn() {
        GM._.GoToLoadingScene();
    }
#endregion
}
