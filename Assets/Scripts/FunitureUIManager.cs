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
    [SerializeField] Enum.FUNITURE_CATE category;

    [SerializeField] Button[] categoryBtns; public Button[] CategoryBtns {get => categoryBtns; set => categoryBtns = value;}
    [SerializeField] int page;
    [SerializeField] TextMeshProUGUI pageTxt;
    [SerializeField] Transform content; //* 初期化するため、親になるオブジェクト用意 ↓
    [SerializeField] FunitureShopItemBtn[] itemBtns; //* 親になるオブジェクトを通じて、子の要素を割り当てる。

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
#region BTN CLICK EVENT
/// -----------------------------------------------------------------------------------------------------------------
    public void onClickCategoryBtn(int idx) {
        //* 初期化
        Array.ForEach(itemBtns, ib => ib.init());

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
    private void showItemList() {
        int len = getCategoryItemLenght();
        int start = page * ITEM_BTN_CNT;
        int end = Mathf.Clamp(start + ITEM_BTN_CNT, min: start, max: len);

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
#endregion
}
