using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.U2D.Animation;

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
            if(item is Funiture) {
                var ft = item as Funiture;
                priceTxt.text = ft.Price.ToString();
            }
            else if(item is BgFuniture) {
                var bg = item as BgFuniture;
                priceTxt.text = bg.Price.ToString();
            }
            // priceTxt.text = item.Price.ToString();
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
    [SerializeField] bool isLock;    public bool IsLock {get => isLock; set => isLock = value;}
    [SerializeField] bool isNotify;    public bool IsNotify {get => isNotify; set => isNotify = value;}
    [SerializeField] bool isArranged;   public bool IsArranged {get => isArranged; set => isArranged = value;}

    //* 抽象 : ★★★ 親クラスで、抽象メソッドが呼ばれても、実際に動く場所は「子」クラスだから大丈夫
    public abstract int Price {get; set;} //? 子のpriceがあれば、使う
    public abstract void create();
    //* 仮想
    public virtual void display() {
        create();
        this.IsArranged = true;
        HM._.fUI.onClickShopLeftArrow(); //* ボタン UI最新化
    }
    public virtual void purchase() {
        if(DB.Dt.Coin >= this.Price) {
            Debug.Log("💰購入成功！！");
            DB.Dt.setCoin(-this.Price);
            isLock = false;
            display();
        }
        else {
            Debug.Log("😢 お金がたりない！！");
            HM._.ui.showErrorMsgPopUp("코인이 부족합니다!");
        }
    }
    public virtual void arrange() {
        //* ロック
        if(IsLock) {
            HM._.fUI.InfoDialog.SetActive(true);
            HM._.fUI.InfoDlgItemNameTxt.text = name;
            HM._.fUI.InfoDlgItemImg.sprite = spr;
            HM._.fUI.InfoDlgItemPriceTxt.text = this.Price.ToString();
            Debug.Log($"onClickItemListBtn:: current Category= {HM._.fUI.Category}");
            //*--> onClickInfoDialogPurchaseBtn()でアイテム 購入
        }
        //* 配置
        else {
            if(isArranged) {
                HM._.ui.showErrorMsgPopUp("이미 사용 중입니다.");
                return;
            }
            display();
        }
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Funiture : Item {
    [Header("追加")]
    [SerializeField] int price; public override int Price {get => price; set => price = value;}
    [SerializeField] GameObject prefab;    public GameObject Prefab {get => prefab;}
    [SerializeField] Vector2 pos;   public Vector2 Pos {get => pos; set => pos = value;}
    [SerializeField] bool isFlat;  public bool IsFlat {get => isFlat; set => isFlat = value;}

    Funiture() {
        pos = Vector2.zero;
        isFlat = false;
    }

    public override void create() {
        HM._.state = HM.STATE.DECORATION_MODE;
        int idx = HM._.fUI.CurSelectedItemIdx;

        //* 生成するPrefab 用意
        GameObject pref = (HM._.fUI.Category == Enum.FUNITURE_CATE.Funiture)? DB.Dt.Funitures[idx].Prefab
            : (HM._.fUI.Category == Enum.FUNITURE_CATE.Decoration)? DB.Dt.Decorations[idx].Prefab
            : pref = DB.Dt.Mats[idx].Prefab;

        //* 生成
        GameObject ins = Util.instantiateObj(pref, HM._.ui.RoomObjectGroupTf);
        ins.name = ins.name.Split('(')[0]; //* 名(Clone) 削除
        RoomObject rObj = ins.GetComponent<RoomObject>();

        //* 初期化
        rObj.Start(); 

        //* 選択されて、デコレーションモード 用意
        rObj.IsSelect = true;
        rObj.Sr.material = HM._.outlineAnimMt; //* アウトライン 付き
        HM._.fUI.CurSelectedObj = rObj.gameObject;
        HM._.fUI.InfoDialog.SetActive(false);
        HM._.ui.DecorateModePanel.SetActive(true);

        //* 飾り用のアイテムのZ値が-1のため、この上に配置すると、Z値が０の場合は MOUSE EVENTが出来なくなる。
        const float OFFSET_Z = -1;
        rObj.transform.position = new Vector3(rObj.transform.position.x, rObj.transform.position.y, OFFSET_Z);

        //* 飾りモードの影よりレイヤーを前に配置
        rObj.Sr.sortingOrder = 100;
        Debug.Log($"SORTING AA createFunitureItem:: {rObj.gameObject.name}.sortingOrder= {rObj.Sr.sortingOrder}");
    }

    public override void display() {
        base.display();
        HM._.ui.onClickDecorateModeIconBtn(); //* デコレーションモード
    }
    public override void purchase() => base.purchase();
    public override void arrange() => base.arrange();
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class BgFuniture : Item {
    public enum TYPE {Wall, Floor};
    [Header("追加")]
    [SerializeField] int price; public override int Price {get => price; set => price = value;}
    [SerializeField] TYPE type; public TYPE Type {get => type; set => type = value;}

    public override void create() {
        //* 画像 (タイプによって)
        Transform objTf = setSpriteByType();
        //* 効果
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.delay2);
        //* ホームに戻す
        backHome();
    }
    public override void display() => base.display();
    public override void purchase() => base.purchase();
    public override void arrange() => base.arrange();

/// -----------------------------------------------------------------------------------------------------------------
#region Priavate Func
/// -----------------------------------------------------------------------------------------------------------------
    private Transform setSpriteByType() {
        SpriteRenderer sr = (this.type == TYPE.Wall)? HM._.wallSr : HM._.floorSr;
        BgFuniture[] items = Array.FindAll(DB.Dt.Bgs, item => item.Type == this.type);

        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 画像
        sr.sprite = DB.Dt.Bgs[HM._.fUI.CurSelectedItemIdx].Spr; 
        return sr.transform;
    }
    private void backHome() {
        HM._.fUI.InfoDialog.SetActive(false);
        HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
        HM._.ui.onClickDecorateModeCloseBtn();
        HM._.ui.onClickWoodSignArrowBtn(dirVal: 1); //* プレイヤーが動かないこと対応
        HM._.ui.onClickWoodSignArrowBtn(dirVal: -1);
    }
#endregion
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class PlayerSkin : Item {
    [Header("追加")]
    [SerializeField] SpriteLibraryAsset sprLibraryAsset;    public SpriteLibraryAsset SprLibraryAsset {get => sprLibraryAsset;}
    public override int Price { get => 0; set {} } // 使わない
    public override void create() {
        //TODO
    }

    public override void display() {
        //TODO
    }

    public override void purchase() {
        //TODO
    }

    public override void arrange() {
        //TODO
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