using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;
using TMPro;
public class InventoryUIManager : MonoBehaviour
{
    const int ITEM_BTN_CNT = 9;
    [Header("CATEGORY")]
    [SerializeField] Enum.INV_CATE category; public Enum.INV_CATE Category {get => category;}
    [SerializeField] Button[] categoryBtns; public Button[] CategoryBtns {get => categoryBtns; set => categoryBtns = value;}
    [Header("PAGE")]
    [SerializeField] int page;
    [SerializeField] TextMeshProUGUI pageTxt;
    [Header("ITEM")]
    [SerializeField] Transform content; //* 初期化するため、親になるオブジェクト用意 ↓
    [SerializeField] InventoryItemBtn[] itemBtns; //* 親になるオブジェクトを通じて、子の要素を割り当てる。
    [SerializeField] GameObject curSelectedObj;    public GameObject CurSelectedObj {get => curSelectedObj; set => curSelectedObj = value;}
    [Header("INFO DIALOG")]
    [SerializeField] int curSelectedItemIdx;    public int CurSelectedItemIdx {get => curSelectedItemIdx; set => curSelectedItemIdx = value;}
    [SerializeField] GameObject infoDialog; public GameObject InfoDialog {get => infoDialog; set => infoDialog = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemNameTxt;    public TextMeshProUGUI InfoDlgItemNameTxt {get => infoDlgItemNameTxt; set => infoDlgItemNameTxt = value;}
    [SerializeField] Image infoDlgItemImg;    public Image InfoDlgItemImg {get => infoDlgItemImg; set => infoDlgItemImg = value;}

    void Start() {
        //* アイテムボタン 割り当て
        const int IMG = 0, LOCKFRAME = 1, NOTIFY = 2, ARRANGE = 3; //* Index

        itemBtns = new InventoryItemBtn[content.childCount];
        for(int i = 0; i < content.childCount; i++) {
            Transform tf = content.GetChild(i);
            itemBtns[i] = new InventoryItemBtn(
                obj: tf.gameObject, 
                img: tf.GetChild(IMG).GetComponent<Image>(),
                lockFrameObj: tf.GetChild(LOCKFRAME).gameObject,
                notifyObj: tf.GetChild(NOTIFY).gameObject,
                arrangeFrameObj: tf.GetChild(ARRANGE).gameObject
                //* 子 要素
                //TODO アウトラインするかしないか？毛メルコと。-> arrangeFrameObj「✓」なので、しなくてもいいかも。
            );
        }

        //* カテゴリ 初期化
        onClickCategoryBtn((int)Enum.INV_CATE.Player);
    }

/// -----------------------------------------------------------------------------------------------------------------
#region INV EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickCategoryBtn(int idx) {
        //* 初期化
        page = 0;
        Array.ForEach(itemBtns, itemBtn => itemBtn.init());

        //* カテゴリ IDX
        setCategoryIdx(idx);

        //* カテゴリ 背景色
        for(int i = 0; i < categoryBtns.Length; i++) 
            categoryBtns[i].image.color = (i == idx)? Color.green : Color.white;

        //* アイテム リスト 最新化して並べる
        showItemList();
    }
    public void onClickInvLeftArrow() {
        setPageByArrowBtn(pageDir: -1); //* ページ
        showItemList(); //* アイテムリスト 並べる
    }
    public void onClickInvRightArrow() {
        setPageByArrowBtn(pageDir: +1); //* ページ
        showItemList(); //* アイテムリスト 並べる
    }
    public void onClickItemListBtn(int idx) {
        //* ペースも含めた 実際のINDEX
        curSelectedItemIdx = idx + (page * ITEM_BTN_CNT);
        //* Pattern Matching (Child Class)
        Item item = getSelectedItem(curSelectedItemIdx);
        switch(item) {
            case PlayerSkin plsk:   plsk.arrange(); break;
            case PetSkin ptsk:   ptsk.arrange(); break;
        }
    }
    public void onClickInfoDialogGoToClothShop() {
        // TODO
    }
#endregion

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private void setCategoryIdx(int idx) {
        category = (idx == 0)? Enum.INV_CATE.Player : Enum.INV_CATE.Pet;
    }
    private int getCategoryItemLenght() {
        return (category == Enum.INV_CATE.Player)? DB.Dt.PlSkins.Length : DB.Dt.PetSkins.Length;
    }
    private Item getSelectedItem(int idx) {
        return (category == Enum.INV_CATE.Player)? DB.Dt.PlSkins[idx] : DB.Dt.PetSkins[idx];
    }
    private void setPageByArrowBtn(int pageDir) { // @param pageDir : -1(Left) or 1(Right)
        //* 初期化
        for(int i = 0; i < itemBtns.Length; i++) itemBtns[i].init();

        //* ページ
        int len = getCategoryItemLenght();
        page += pageDir;
        page = Mathf.Clamp(page, 0, (len - 1) / ITEM_BTN_CNT);
        Debug.Log($"onClickPageArrowBtn():: category= {category}, len= {len}, page= {page}");
    }
    private void showItemList() {
        int len = getCategoryItemLenght();
        int start = page * ITEM_BTN_CNT;
        int end = Mathf.Clamp(start + ITEM_BTN_CNT, min: start, max: len);
        Debug.Log($"showItemList():: cate={category}, getCategoryItemLenght= {len}");

        //* ページ 表示
        const int PG_IDX_OFFSET = 1;
        pageTxt.text = $"{page + PG_IDX_OFFSET} / {((len - 1) / ITEM_BTN_CNT) + PG_IDX_OFFSET}";

        //* 画像 表示
        for(int i = start; i < end; i++) {
            InventoryItemBtn itemBtn = itemBtns[i % ITEM_BTN_CNT];
            Item item = getSelectedItem(i);

            //* Parent <-Pattern Mathcing <- Child 
            switch(item) {
                case PlayerSkin plSk:   itemBtn.updateItemFrame(plSk);   break;
                case PetSkin ptSk: itemBtn.updateItemFrame(ptSk);   break;
            }
        }

        //* 有効なフレームのみ 表示
        Array.ForEach(itemBtns, ib => ib.Obj.SetActive(ib.Img.sprite));
    }
#endregion
}
