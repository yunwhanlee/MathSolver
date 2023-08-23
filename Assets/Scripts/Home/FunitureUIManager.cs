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
    const int CATE_ON = 0, CATE_OFF = 1;
    [Header("CATEGORY")]
    [SerializeField] Enum.FUNITURE_CATE category; public Enum.FUNITURE_CATE Category {get => category;}
    [SerializeField] Sprite[] cateBgSprs;
    [SerializeField] Sprite[] cateDecorationSprs;
    [SerializeField] Sprite[] cateFurnitureSprs;
    [SerializeField] Sprite[] cateMatSprs;
    [SerializeField] Button[] categoryBtns; public Button[] CategoryBtns {get => categoryBtns; set => categoryBtns = value;}
    [SerializeField] Image[] categoryBtnIcons;
    [Header("PRICE ICONS")]
    [SerializeField] Sprite[] priceIconSprs;    public Sprite[] PriceIconSprs {get => priceIconSprs;}
    [Header("PAGE")]
    [SerializeField] int page;
    [SerializeField] TextMeshProUGUI pageTxt;

    [Header("ITEM")]
    [SerializeField] Vector3 befPos;    public Vector3 BefPos {get => befPos; set => befPos = value;}
    [SerializeField] Transform content; //* 初期化するため、親になるオブジェクト用意 ↓
    [SerializeField] FunitureShopItemBtn[] itemBtns; //* 親になるオブジェクトを通じて、子の要素を割り当てる。
    [SerializeField] GameObject curSelectedObj;    public GameObject CurSelectedObj {get => curSelectedObj; set => curSelectedObj = value;}

    void Start() {
        //* アイテムボタン 割り当て
        const int IMG = 0, LOCKFRAME = 1, NOTIFY = 2, ARRANGE = 3, PRICE = 4, LEGACY = 5; //* Index
        page = 0;
        itemBtns = new FunitureShopItemBtn[content.childCount];
        for(int i = 0; i < content.childCount; i++) {
            Transform tf = content.GetChild(i);
            //* コンストラクタ（初期化）
            itemBtns[i] = new FunitureShopItemBtn(
                obj: tf.gameObject, 
                img: tf.GetChild(IMG).GetComponent<Image>(),
                lockFrameObj: tf.GetChild(LOCKFRAME).gameObject,
                notifyObj: tf.GetChild(NOTIFY).gameObject,
                arrangeFrameObj: tf.GetChild(ARRANGE).gameObject,
                legacyIconObj: tf.GetChild(LEGACY).gameObject,
                //* 子（追加） 要素
                priceTxt: tf.GetChild(PRICE).GetComponentInChildren<TextMeshProUGUI>(),
                priceIconImg: tf.GetChild(PRICE).GetComponentInChildren<Image>()
            );
        }

        //* カテゴリ 初期化
        onClickCategoryBtn((int)Enum.FUNITURE_CATE.Funiture);
    }
/// -----------------------------------------------------------------------------------------------------------------
#region SHOP EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickCategoryBtn(int idx) {
        Debug.Log($"onClickCategoryBtn(idx: {idx})");
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        //* 初期化
        page = 0;
        Array.ForEach(itemBtns, itemBtn => itemBtn.init());

        //* カテゴリ IDX
        setCategoryIdx(idx);

        //* カテゴリ アイコン 表示
        categoryBtnIcons[(int)Enum.FUNITURE_CATE.Funiture].sprite = (idx == 0)? cateFurnitureSprs[CATE_ON] : cateFurnitureSprs[CATE_OFF];
        categoryBtnIcons[(int)Enum.FUNITURE_CATE.Decoration].sprite = (idx == 1)? cateDecorationSprs[CATE_ON] : cateDecorationSprs[CATE_OFF];
        categoryBtnIcons[(int)Enum.FUNITURE_CATE.Bg].sprite = (idx == 2)? cateBgSprs[CATE_ON] : cateBgSprs[CATE_OFF];
        categoryBtnIcons[(int)Enum.FUNITURE_CATE.Mat].sprite = (idx == 3)? cateMatSprs[CATE_ON] : cateMatSprs[CATE_OFF];

        //* カテゴリ 背景色
        for(int i = 0; i < categoryBtns.Length; i++)
            categoryBtns[i].image.color = (i == idx)? Config.CATE_SELECT_COLOR : Color.white;

        //* アイテム リスト 最新化して並べる
        updateItemList();
    }
    public void onClickShopLeftArrow() {
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        setPageByArrowBtn(pageDir: -1); //* ページ
        updateItemList(); //* アイテムリスト 並べる
    }
    public void onClickShopRightArrow() {
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        setPageByArrowBtn(pageDir: +1); //* ページ
        updateItemList(); //* アイテムリスト 並べる
    }
    public void onClickItemListBtn(int idx) {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
        //* ペースも含めた 実際のINDEX
        HM._.ui.CurSelectedItemIdx = idx + (page * ITEM_BTN_CNT);
        //* Pattern Matching (Child Class)
        Item item = getSelectedItem(HM._.ui.CurSelectedItemIdx);
        switch(item) {
            case Funiture ft:   ft.arrange(); break;
            case BgFuniture bg: bg.arrange(); break;
        }
    }
    public void onClickInfoDialogPurchaseBtn() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
        Item item = getSelectedItem(HM._.ui.CurSelectedItemIdx); //* Get Item
        item.purchase(); //* 購入
    }
    public void onClickNewFuniturePopUpCloseBtn() {
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        HM._.ui.NewFuniturePopUp.SetActive(false);
        //* 最新化
        updateItemList();

        //* 次のアクション 読込 (WorldMapNamager:: init()イベントで使われる場合)
        HM._.wmm.callbackOnActionList();
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNITURE MODE EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickFunitureModeItemDeleteBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Please select an item!"));
            return;
        }

        Debug.Log($"onClickFunitureModeItemDeleteBtn():: curSelectedObj.tag= {curSelectedObj.tag}");
        SM._.sfxPlay(SM.SFX.FeatherPop.ToString());
        Funiture itemDt = getCurSelectObjToItem(curSelectedObj) as Funiture;

        //* アイテムが無かったら、BUGなので終了
        if(itemDt == null) return;

        itemDt.IsArranged = false; //* 配置トリガー OFF
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, curSelectedObj.transform.position, Util.time2);
        Destroy(curSelectedObj); //* オブジェクト 破壊
        HM._.ui.onClickDecorateModeCloseBtn();

        //* 最新化
        updateItemList();
    }
    public void onClickFunitureModeItemFlatBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Please select an item!"));
            return;
        }
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        var sprRdr = curSelectedObj.GetComponent<SpriteRenderer>();
        sprRdr.flipX = !sprRdr.flipX;

    }
    public void onClickFunitureModeItemSetUpBtn() {
        if(!curSelectedObj) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Please select an item!"));
            return;
        }

        Debug.Log($"onClickFunitureModeItemSetUpBtn():: {getCurSelectObjToItem(curSelectedObj)}");
        SM._.sfxPlay(SM.SFX.FeatherPop.ToString());
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, curSelectedObj.transform.position, Util.time2);
        setUpFunitureModeItem();
        HM._.ui.setDecorationMode(isActive: false);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private Item getCurSelectObjToItem(GameObject curSelObj) {
        Debug.Log($"getCurSelectObjToItem(curSelObj= {curSelObj.name})::");
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
    private void updateItemList() {
        int len = getCategoryItemLenght();
        int start = page * ITEM_BTN_CNT;
        int end = Mathf.Clamp(start + ITEM_BTN_CNT, min: start, max: len);
        Debug.Log($"showItemList():: cate={category}, getCategoryItemLenght= {len}");

        HM._.ui.InfoDialog.gameObject.SetActive(false);

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

        //* カテゴリのNEWお知らせアイコン 表示
        activeCategoryNewNofityIcon();

        //* 有効なフレームのみ 表示
        Array.ForEach(itemBtns, ib => ib.Obj.SetActive(ib.Img.sprite));
    }

    private void activeCategoryNewNofityIcon() {
        var cateFunitureNotifyObj = categoryBtns[0].transform.GetChild(1).gameObject;
        var cateDecoNotifyObj = categoryBtns[1].transform.GetChild(1).gameObject;
        var cateBgNotifyObj = categoryBtns[2].transform.GetChild(1).gameObject;
        var cateMatNotifyObj = categoryBtns[3].transform.GetChild(1).gameObject;
        cateFunitureNotifyObj.SetActive(Array.Exists(DB.Dt.Funitures, fn => fn.IsNotify));
        cateDecoNotifyObj.SetActive(Array.Exists(DB.Dt.Decorations, dc => dc.IsNotify));
        cateBgNotifyObj.SetActive(Array.Exists(DB.Dt.Bgs, bg => bg.IsNotify));
        cateMatNotifyObj.SetActive(Array.Exists(DB.Dt.Mats, mt => mt.IsNotify));
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
        HM._.ui.InfoDialog.SetActive(false);
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

        //* アウトライン 消す
        var sr = curRoomObject.Sr;
        sr.material = HM._.sprUnlitMt;

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;
    }

    public void saveFunitureItemPos() {
        //* Z値 ０に戻す
        var curObjtf = curSelectedObj.transform;
        curObjtf.position = new Vector3(curObjtf.position.x, curObjtf.position.y, 0);

        //* ★位置データ 保存
        Funiture itemDt = getCurSelectObjToItem(curSelectedObj) as Funiture;
        float x = (float)Math.Round(curObjtf.position.x, 3);
        float y = (float)Math.Round(curObjtf.position.y, 3);
        itemDt.Pos = new Vector2(x, y);

        //* 反転データ 保存
        itemDt.IsFlat = (curObjtf.localScale.x < 0);
    }
#endregion
}
