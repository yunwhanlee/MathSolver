using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region (UI) アイテム フレーム ボタン
///---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public abstract class ItemFrameBtn {
    [SerializeField] GameObject obj; public GameObject Obj {get => obj; set => obj = value;}
    [SerializeField] Image img; public Image Img {get => img; set => img = value;}
    [SerializeField] GameObject lockFrameObj; public GameObject LockFrameObj {get => lockFrameObj; set => lockFrameObj = value;}
    [SerializeField] GameObject notifyObj; public GameObject NotifyObj {get => notifyObj; set => notifyObj = value;}

    public abstract void init();
    public abstract void updateItemFrame(Item item);
}

//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class FunitureShopItemBtn : ItemFrameBtn {
    [SerializeField] TextMeshProUGUI priceTxt; public TextMeshProUGUI PriceTxt {get => priceTxt; set => priceTxt = value;}
    [SerializeField] GameObject arrangeFrameObj; public GameObject ArrangeFrameObj {get => arrangeFrameObj; set => arrangeFrameObj = value;} 

    public FunitureShopItemBtn(GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, TextMeshProUGUI priceTxt, GameObject arrangeFrameObj) {
        this.Obj = obj;
        this.Img = img;
        this.LockFrameObj = lockFrameObj;
        this.NotifyObj = notifyObj;
        //* 子 要素
        this.priceTxt = priceTxt;
        this.arrangeFrameObj = arrangeFrameObj; // ✓ 表示
    }

    public override void init() {
        Img.sprite = null;
        LockFrameObj.SetActive(true);
        NotifyObj.SetActive(false);
        //* 子 要素
        priceTxt.text = "";
        arrangeFrameObj.SetActive(false);
    }

    public override void updateItemFrame(Item item) {
        try {
            Img.sprite = item.Spr;
            LockFrameObj.SetActive(item.IsLock);
            NotifyObj.SetActive(item.IsNotify);
            //* 子 要素
            priceTxt.text = item.Price.ToString();
            arrangeFrameObj.SetActive(item.IsArranged);
            //* priceTxtObj (非)表示
            if(!item.IsLock)
                priceTxt.transform.parent.gameObject.SetActive(false);
        }
        catch(NullReferenceException err) {
            Debug.LogError("<color=yellow>DBManagerのInspectorビューに、Nullを確認してください。</color>" + "\n " + err);
        }
    }
}

#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region (OBJ) アイテム
///---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public abstract class Item {
    [SerializeField] string name;   public string Name {get => name;}
    [SerializeField] Sprite spr;    public Sprite Spr {get => spr; set => spr = value;}
    // [SerializeField] int id;    public int Id {get => id; set => id = value;}
    [SerializeField] int price; public int Price {get => price; set => price = value;}
    [SerializeField] bool isLock;    public bool IsLock {get => isLock; set => isLock = value;}
    [SerializeField] bool isNotify;    public bool IsNotify {get => isNotify; set => isNotify = value;}
    [SerializeField] bool isArranged;   public bool IsArranged {get => isArranged; set => isArranged = value;}

    public abstract void create();
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Funiture : Item {
    [Header("追加")]
    [SerializeField] GameObject prefab;    public GameObject Prefab {get => prefab;}
    [SerializeField] Vector2 pos;   public Vector2 Pos {get => pos; set => pos = value;}
    [SerializeField] bool isFlat;  public bool IsFlat {get => isFlat; set => isFlat = value;}

    Funiture() {
        pos = Vector2.zero;
        isFlat = false;
    }

    public override void create() {
        HM._.state = HM.STATE.DECORATION_MODE;
        var fui = HM._.fUI;
        int idx = fui.CurSelectedItemIdx;

        GameObject pref = (fui.Category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures[idx].Prefab
            : (fui.Category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations[idx].Prefab
            : pref = DB.Dt.Mats[idx].Prefab;

        //! GameObject ins = Instantiate(pref, HM._.ui.RoomObjectGroupTf);
        GameObject ins = Util.instantiateObj(pref, HM._.ui.RoomObjectGroupTf);
        ins.name = ins.name.Split('(')[0]; //* 名(Clone) 削除
        RoomObject rObj = ins.GetComponent<RoomObject>();
        rObj.Start(); //* 初期化 必要

        rObj.IsSelect = true;
        rObj.Sr.material = HM._.outlineAnimMt; //* アウトライン 付き
        fui.CurSelectedObj = rObj.gameObject;
        fui.InfoDialog.SetActive(false);
        HM._.ui.DecorateModePanel.SetActive(true);

        //* 飾り用のアイテムのZ値が-1のため、この上に配置すると、Z値が０の場合は MOUSE EVENTが出来なくなる。
        const float OFFSET_Z = -1;
        rObj.transform.position = new Vector3(rObj.transform.position.x, rObj.transform.position.y, OFFSET_Z);

        //* 飾りモードの影よりレイヤーを前に配置
        rObj.Sr.sortingOrder = 100;
        Debug.Log($"SORTING AA createFunitureItem:: {rObj.gameObject.name}.sortingOrder= {rObj.Sr.sortingOrder}");
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class BgFuniture : Item {
    public enum TYPE {Wall, Floor};
    [Header("追加")]
    [SerializeField] TYPE type; public TYPE Type {get => type; set => type = value;}

    public override void create() {
        //* 画像 (タイプによって)
        Transform objTf = setSpriteByType();
        //* 効果
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.delay2);
        //* ホームに戻す
        HM._.fUI.InfoDialog.SetActive(false);
        HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
        HM._.ui.onClickDecorateModeCloseBtn();
        HM._.ui.onClickWoodSignArrowBtn(dirVal: 1); //* プレイヤーが動かないこと対応
        HM._.ui.onClickWoodSignArrowBtn(dirVal: -1);
    }
    private Transform setSpriteByType() {
        SpriteRenderer sr = (this.type == TYPE.Wall)? HM._.wallSr : HM._.floorSr;
        BgFuniture[] items = Array.FindAll(DB.Dt.Bgs, item => item.Type == this.type);

        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 画像
        sr.sprite = DB.Dt.Bgs[HM._.fUI.CurSelectedItemIdx].Spr; 
        return sr.transform;
    }

}

#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region 問題
///---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Problem {
    public float n1;
    public float n2;
    public Enum.OPERATION op1;
    public float res;
    public float[] answers;
    public string sentence;

    public Problem(float n1, float n2, Enum.OPERATION op1, float res, float[] answers) {
        this.n1 = n1;
        this.n2 = n2;
        this.op1 = op1;
        this.res = res;
        this.answers = answers;
    }
#endregion

}