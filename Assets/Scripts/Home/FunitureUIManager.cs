using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;
using TMPro;

public class FunitureUIManager : MonoBehaviour
{
    const int ITEM_BTN_CNT = 9;
    [Header("CATEGORY")]
    [SerializeField] Enum.FUNITURE_CATE category; public Enum.FUNITURE_CATE Category {get => category;}
    [SerializeField] Button[] categoryBtns; public Button[] CategoryBtns {get => categoryBtns; set => categoryBtns = value;}
    [Header("PAGE")]
    [SerializeField] int page;
    [SerializeField] TextMeshProUGUI pageTxt;
    [Header("ITEM")]
    [SerializeField] Vector3 befPos;    public Vector3 BefPos {get => befPos; set => befPos = value;}
    [SerializeField] Transform content; //* 初期化するため、親になるオブジェクト用意 ↓
    [SerializeField] FunitureShopItemBtn[] itemBtns; //* 親になるオブジェクトを通じて、子の要素を割り当てる。
    [SerializeField] GameObject curSelectedObj;    public GameObject CurSelectedObj {get => curSelectedObj; set => curSelectedObj = value;}
    [Header("INFO DIALOG")]
    [SerializeField] int curSelectedItemIdx;    public int CurSelectedItemIdx {get => curSelectedItemIdx; set => curSelectedItemIdx = value;}
    [SerializeField] GameObject infoDialog; public GameObject InfoDialog {get => infoDialog; set => infoDialog = value;}
    [SerializeField] TextMeshProUGUI infoDlgItemNameTxt;
    [SerializeField] Image infoDlgItemImg;
    [SerializeField] TextMeshProUGUI infoDlgItemPriceTxt;

    void Start() {
        //* アイテムボタン 割り当て
        const int IMG = 0, LOCKFRAME = 1, NOTIFY = 2, PRICE = 3, ARRANGE = 4; //* Index 
        page = 0;
        itemBtns = new FunitureShopItemBtn[content.childCount];
        for(int i = 0; i < content.childCount; i++) {
            Transform tf = content.GetChild(i);
            itemBtns[i] = new FunitureShopItemBtn(
                obj: tf.gameObject, 
                img: tf.GetChild(IMG).GetComponent<Image>(),
                lockFrameObj: tf.GetChild(LOCKFRAME).gameObject,
                notifyObj: tf.GetChild(NOTIFY).gameObject,
                //* 子 要素
                priceTxt: tf.GetChild(PRICE).GetComponentInChildren<TextMeshProUGUI>(),
                arrangeFrameObj: tf.GetChild(ARRANGE).gameObject
            );
        }

        //* カテゴリ 初期化
        onClickCategoryBtn((int)Enum.FUNITURE_CATE.Funiture);
    }
/// -----------------------------------------------------------------------------------------------------------------
#region SHOP EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickCategoryBtn(int idx) {
        Debug.Log($"BBB onClickCategoryBtn(idx: {idx})");
        //* 初期化
        page = 0;
        Array.ForEach(itemBtns, ib => ib.init());

        //* カテゴリ IDX
        setCategoryIdx(idx);

        //* カテゴリ 背景色
        for(int i = 0; i < categoryBtns.Length; i++) 
            categoryBtns[i].image.color = (i == idx)? Color.green : Color.white;

        //* アイテム リスト 最新化して並べる
        showItemList();
    }
    public void onClickShopLeftArrow() {
        setPageByArrowBtn(pageDir: -1); //* ページ
        showItemList(); //* アイテムリスト 並べる
    }
    public void onClickShopRightArrow() {
        setPageByArrowBtn(pageDir: +1); //* ページ
        showItemList(); //* アイテムリスト 並べる
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNITURE MODE EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickFunitureModeItemDeleteBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp("삭제할 아이템을 선택해주세요!");
            return;
        }

        Debug.Log($"onClickFunitureModeItemDeleteBtn():: curSelectedObj.tag= {curSelectedObj.tag}");
        Funiture itemDt = getCurObjLayer2FunitureItem(curSelectedObj) as Funiture;

        //* アイテムが無かったら、BUGなので終了
        if(itemDt == null) return;

        itemDt.IsArranged = false; //* 配置トリガー OFF
        Destroy(curSelectedObj); //* オブジェクト 破壊
        HM._.ui.onClickDecorateModeCloseBtn();

        //* 最新化
        showItemList();
    }
    public void onClickFunitureModeItemFlatBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp("반전시킬 아이템을 선택해주세요!");
            return;
        }

        float sx = curSelectedObj.transform.localScale.x * -1;
        Funiture itemDt = getCurObjLayer2FunitureItem(curSelectedObj) as Funiture;
        curSelectedObj.transform.localScale = new Vector2(sx, 1);

    }
    public void onClickFunitureModeItemSetUpBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp("배치할 아이템을 선택해주세요!");
            return;
        }

        Debug.Log($"onClickFunitureModeItemSetUpBtn():: {getCurObjLayer2FunitureItem(curSelectedObj)}");
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, curSelectedObj.transform.position, Util.delay2);
        setUpFunitureModeItem();
        HM._.ui.setDecorationMode(isActive: false);
    }
    public void onClickItemListBtn(int idx) {
        //* ペースも含めた 実際のINDEX
        curSelectedItemIdx = idx + (page * ITEM_BTN_CNT);
        //* Pattern Matching (Child Class)
        Item item = getSelectedItem(curSelectedItemIdx);
        switch(item) {
            case Funiture ft:   setClickItem(ft); break;
            case BgFuniture bg: setClickItem(bg); break;
        }
    }
    public void onClickInfoDialogPurchaseBtn() {
        //* Get Item
        Item item = getSelectedItem(curSelectedItemIdx);

        //* 購入
        if(DB.Dt.Coin >= item.Price) {
            Debug.Log("💰購入成功！！");
            DB.Dt.setCoin(-item.Price);
            item.IsLock = false;
            displayItem(item);
        }
        else {
            Debug.Log("😢 お金がたりない！！");
            HM._.ui.showErrorMsgPopUp("코인이 부족합니다!");
        }
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private void setClickItem(Item item){
        //* ロック
        if(item.IsLock) {
            infoDialog.SetActive(true);
            infoDlgItemNameTxt.text = item.Name;
            infoDlgItemImg.sprite = item.Spr;
            infoDlgItemPriceTxt.text = item.Price.ToString();
            Debug.Log($"onClickItemListBtn:: current Category= {category}");
            //*--> onClickInfoDialogPurchaseBtn()でアイテム 購入
        }
        //* 配置
        else {
            if(item.IsArranged) {
                HM._.ui.showErrorMsgPopUp("이미 사용 중입니다.");
                return;
            }

            displayItem(item);
        }
    }

    private void displayItem(Item item) {
        switch(item) {
            case Funiture ft:
                // createFunitureItem(curSelectedItemIdx); //* 生成
                ft.create();
                HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
                break;
            case BgFuniture bg:
                bg.create();
                break;
        }
        item.IsArranged = true;

        //* ボタン UI最新化
        onClickShopLeftArrow(); 
    }
    private Item getCurObjLayer2FunitureItem(GameObject curSelObj) {
        //* レイヤーで種類を探す
        var tag = curSelObj.tag;
        Item item = null;
        if(tag == Enum.FUNITURE_CATE.Funiture.ToString()) {
            item = Array.Find(DB.Dt.Funitures, item => curSelObj.name == item.Name);
        }
        else if(tag == Enum.FUNITURE_CATE.Decoration.ToString()) {
            item = Array.Find(DB.Dt.Decorations, item => curSelObj.name == item.Name);
        }
        else if(tag == Enum.FUNITURE_CATE.Bg.ToString()) {
            item = Array.Find(DB.Dt.Bgs, item => curSelObj.name == item.Name);
        }
        else if(tag == Enum.FUNITURE_CATE.Mat.ToString()) {
            item = Array.Find(DB.Dt.Mats, item => curSelObj.name == item.Name);
        }

        //* 家具 アイテム
        return item;
    }
    private void setCategoryIdx(int idx) {
        category = (idx == 0)? Enum.FUNITURE_CATE.Funiture
            : (idx == 1)? Enum.FUNITURE_CATE.Decoration
            : (idx == 2)? Enum.FUNITURE_CATE.Bg
            : Enum.FUNITURE_CATE.Mat; //(idx == 3)? Enum.FUNITURE_CATE.Mat
    }
    private int getCategoryItemLenght() {
        return (category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures.Length
            : (category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations.Length
            : (category == Enum.FUNITURE_CATE.Bg)? DB.Dt.Bgs.Length
            : DB.Dt.Mats.Length;
    }
    private Item getSelectedItem(int idx) {
        return (category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures[idx]
            : (category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations[idx]
            : (category == Enum.FUNITURE_CATE.Bg)? DB.Dt.Bgs[idx] as BgFuniture
            : DB.Dt.Mats[idx];
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
            FunitureShopItemBtn itemBtn = itemBtns[i % ITEM_BTN_CNT];
            Item item = getSelectedItem(i);

            //* Parent <-Pattern Mathcing <- Child 
            switch(item) {
                case Funiture ft:   itemBtn.updateItemFrame(ft);   break;
                case BgFuniture bg: itemBtn.updateItemFrame(bg);   break;
            }
        }

        //* 有効なフレームのみ 表示
        Array.ForEach(itemBtns, ib => ib.Obj.SetActive(ib.Img.sprite));
    }
    public void createFunitureItem(int idx) {
        HM._.state = HM.STATE.DECORATION_MODE;

        GameObject pref = (category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures[idx].Prefab
            : (category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations[idx].Prefab
            // : (category == Enum.FUNITURE_CATE.Bg)? DB.Dt.Bgs[idx].Prefab
            : pref = DB.Dt.Mats[idx].Prefab;

        GameObject ins = Instantiate(pref, HM._.ui.RoomObjectGroupTf);
        ins.name = ins.name.Split('(')[0]; //* 名(Clone) 削除
        RoomObject rObj = ins.GetComponent<RoomObject>();
        rObj.Start(); //* 初期化 必要

        rObj.IsSelect = true;
        rObj.Sr.material = HM._.outlineAnimMt; //* アウトライン 付き
        curSelectedObj = rObj.gameObject;
        infoDialog.SetActive(false);
        HM._.ui.DecorateModePanel.SetActive(true);

        //* 飾り用のアイテムのZ値が-1のため、この上に配置すると、Z値が０の場合は MOUSE EVENTが出来なくなる。
        const float OFFSET_Z = -1;
        rObj.transform.position = new Vector3(rObj.transform.position.x, rObj.transform.position.y, OFFSET_Z);

        //* 飾りモードの影よりレイヤーを前に配置
        rObj.Sr.sortingOrder = 100;
        Debug.Log($"SORTING AA createFunitureItem:: {rObj.gameObject.name}.sortingOrder= {rObj.Sr.sortingOrder}");
    }
    public void setUpFunitureModeItem(bool isCancel = false) {
        if(!curSelectedObj) return;

        RoomObject curRoomObject = curSelectedObj.GetComponent<RoomObject>();
        curRoomObject.setSortingOrderByPosY(backBefPos: isCancel);
        curRoomObject.IsSelect = false;
        HM._.ui.DecorateModePanel.SetActive(false);

        //* Z値 ０に戻す
        var tf = curSelectedObj.transform;
        tf.position = new Vector3(tf.position.x, tf.position.y, 0);

        //* 位置データ 保存
        Funiture itemDt = getCurObjLayer2FunitureItem(curSelectedObj) as Funiture;
        float x = (float)Math.Round(tf.position.x, 3);
        float y = (float)Math.Round(tf.position.y, 3);
        itemDt.Pos = new Vector2(x, y);

        //* 反転データ 保存
        itemDt.IsFlat = (tf.localScale.x < 0);

        //* アウトライン 消す
        var sr = curRoomObject.Sr;
        sr.material = HM._.sprUnlitMt;

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;
    }
#endregion
}
