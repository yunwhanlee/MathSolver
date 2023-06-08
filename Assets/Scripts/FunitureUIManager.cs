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
    [SerializeField] Enum.FUNITURE_CATE category; public Enum.FUNITURE_CATE Category {get => category;}

    [SerializeField] Button[] categoryBtns; public Button[] CategoryBtns {get => categoryBtns; set => categoryBtns = value;}
    [SerializeField] int page;
    [SerializeField] TextMeshProUGUI pageTxt;
    [SerializeField] Transform content; //* 初期化するため、親になるオブジェクト用意 ↓
    [SerializeField] FunitureShopItemBtn[] itemBtns; //* 親になるオブジェクトを通じて、子の要素を割り当てる。
    [SerializeField] GameObject curSelectedObj;    public GameObject CurSelectedObj {get => curSelectedObj; set => curSelectedObj = value;}

    void Start() {
        //* アイテムボタン 割り当て
        const int IMG = 0, LOCKFRAME = 1, NOTIFY = 2, PRICE = 3; //* Index 
        page = 0;
        itemBtns = new FunitureShopItemBtn[content.childCount];
        for(int i = 0; i < content.childCount; i++) {
            Transform tf = content.GetChild(i);
            itemBtns[i] = new FunitureShopItemBtn(
                obj: tf.gameObject, 
                img: tf.GetChild(IMG).GetComponent<Image>(),
                lockFrameObj: tf.GetChild(LOCKFRAME).gameObject,
                notifyObj: tf.GetChild(NOTIFY).gameObject,
                priceTxt: tf.GetChild(PRICE).GetComponentInChildren<TextMeshProUGUI>()
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
        Array.ForEach(itemBtns, ib => ib.init());
        page = 0;

        //* カテゴリ IDX
        setCategoryIdx(idx);

        //* カテゴリ 背景色
        int i=0;
        Array.ForEach(categoryBtns, btn => {
            btn.image.color = (i == idx)? Color.green : Color.white;
            i++;
        });

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
        Destroy(curSelectedObj);
        HM._.ui.onClickDecorateModeCloseBtn();
    }
    public void onClickFunitureModeItemFlatBtn() {
        float sx = curSelectedObj.transform.localScale.x * -1;
        curSelectedObj.transform.localScale = new Vector2(sx, 1);
    }
    public void onClickFunitureModeItemSetUpBtn() {
        RoomObject curRoomObject = curSelectedObj.GetComponent<RoomObject>();
        curRoomObject.setSortingOrderByPosY();
        curRoomObject.IsSelect = false;
        HM._.ui.DecorateModePanel.SetActive(false);

        //* Z値 ０に戻す
        var tf = curSelectedObj.transform;
        tf.position = new Vector3(tf.position.x, tf.position.y, 0);

        //* アウトライン 消す
        var sr = curRoomObject.Sr;
        sr.material = HM._.sprUnlitMt;

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;

        HM._.ui.onClickDecorateModeCloseBtn();
    }
    public void onClickItemListBtn(int idx) {
        createFunitureItem(idx); //* 生成
        HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
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
        Debug.Log($"showItemList():: getCategoryItemLenght= {len}");

        //* ページ 表示
        const int PG_IDX_OFFSET = 1;
        pageTxt.text = $"{page + PG_IDX_OFFSET} / {((len - 1) / ITEM_BTN_CNT) + PG_IDX_OFFSET}";

        //* 画像 表示
        for(int i = start; i < end; i++) {
            FunitureShopItemBtn itemBtn = itemBtns[i % ITEM_BTN_CNT];
            itemBtn.updateItemFrame(category, i);
        }

        //* 有効なフレームのみ 表示
        Array.ForEach(itemBtns, ib => ib.Obj.SetActive(ib.Img.sprite));
    }
    public void createFunitureItem(int idx) {
        HM._.state = HM.STATE.DECORATION_MODE;

        GameObject ins = (category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures[idx].Prefab
            : (category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations[idx].Prefab
            : (category == Enum.FUNITURE_CATE.Bg)? DB.Dt.Bgs[idx].Prefab
            : ins = DB.Dt.Mats[idx].Prefab;

        GameObject item = Instantiate(ins, HM._.ui.RoomObjectGroupTf);
        RoomObject rObj = item.GetComponent<RoomObject>();
        rObj.Start(); //* 初期化 必要

        rObj.IsSelect = true;
        rObj.Sr.material = HM._.outlineAnimMt; //* アウトライン 付き
        curSelectedObj = rObj.gameObject;
        HM._.ui.DecorateModePanel.SetActive(true);

        //* 飾り用のアイテムのZ値が-1のため、この上に配置すると、Z値が０の場合は MOUSE EVENTが出来なくなる。
        const float OFFSET_Z = -1;
        rObj.transform.position = new Vector3(rObj.transform.position.x, rObj.transform.position.y, OFFSET_Z);

        //* 飾りモードの影よりレイヤーを前に配置
        rObj.Sr.sortingOrder = 100;
        Debug.Log($"SORTING AA createFunitureItem:: {rObj.gameObject.name}.sortingOrder= {rObj.Sr.sortingOrder}");
    }
#endregion
}
