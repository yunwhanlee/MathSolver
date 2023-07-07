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
    [SerializeField] GameObject arrangeFrameObj; public GameObject ArrangeFrameObj {get => arrangeFrameObj; set => arrangeFrameObj = value;} 
    public ItemFrameBtn(GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj) {
        this.obj = obj;
        this.img = img;
        this.lockFrameObj = lockFrameObj;
        this.notifyObj = notifyObj;
        this.arrangeFrameObj = arrangeFrameObj; // ✓ 表示
    }
    public virtual void init() {
        img.sprite = null;
        lockFrameObj.SetActive(true);
        notifyObj.SetActive(false);
        arrangeFrameObj.SetActive(false);
    }
    public abstract void updateItemFrame(Item item);
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class FunitureShopItemBtn : ItemFrameBtn {
    [SerializeField] TextMeshProUGUI priceTxt; public TextMeshProUGUI PriceTxt {get => priceTxt; set => priceTxt = value;}

    public FunitureShopItemBtn( //* 親 param
    GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj
    ,TextMeshProUGUI priceTxt) //* 子 param
    :base(obj, img, lockFrameObj, notifyObj, arrangeFrameObj) { //* 親 コンストラクター 呼出し
        //* 子 要素
        this.priceTxt = priceTxt;
    }

    public override void init() {
        base.init();
        //* 子 要素
        priceTxt.text = "";
    }

    public override void updateItemFrame(Item item) {
        try {
            Img.sprite = item.Spr;
            LockFrameObj.SetActive(item.IsLock);
            NotifyObj.SetActive(item.IsNotify);
            ArrangeFrameObj.SetActive(item.IsArranged);
            //* 子 要素
            if(item is Funiture) {
                var ft = item as Funiture;
                priceTxt.text = ft.Price.ToString();
            }
            else if(item is BgFuniture) {
                var bg = item as BgFuniture;
                priceTxt.text = bg.Price.ToString();
            }
            //* priceTxtObj (非)表示
            if(!item.IsLock)
                priceTxt.transform.parent.gameObject.SetActive(false);
        }
        catch(NullReferenceException err) {
            Debug.LogError("<color=yellow>DBManagerのInspectorビューに、Nullを確認してください。</color>" + "\n " + err);
        }
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class InventoryItemBtn : ItemFrameBtn {

    public InventoryItemBtn( //* 親 param
    GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj)
    :base(obj, img, lockFrameObj, notifyObj, arrangeFrameObj) { //* 親 コンストラクター 呼出し
        //* 子 要素
    }
    public override void init() {
        base.init();
    }

    public override void updateItemFrame(Item item) {
        try {
            Img.sprite = item.Spr;
            LockFrameObj.SetActive(item.IsLock);
            NotifyObj.SetActive(item.IsNotify);
            ArrangeFrameObj.SetActive(item.IsArranged);
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
        //* ボタン Funiture UI最新化
        if(this is Funiture || this is BgFuniture)
            HM._.fUI.onClickShopLeftArrow();
        //* ボタン Inventory UI最新化
        if(this is PlayerSkin || this is PetSkin) {
            HM._.pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
            HM._.pet.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
            HM._.iUI.onClickInvLeftArrow();
        }
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
            HM._.ui.showErrorMsgPopUp(LM._.localize("Not enough coin!"));
        }
    }
    public virtual void arrange() {
        //* ロック
        if(IsLock) {
            HM._.ui.InfoDialog.SetActive(true);
            HM._.ui.InfoDlgItemNameTxt.text = LM._.localize(name);
            HM._.ui.InfoDlgItemImg.sprite = spr;
            HM._.ui.InfoDlgItemPriceTxt.text = this.Price.ToString();
            switch(this) {
                case Funiture: case BgFuniture:
                    HM._.ui.InfoDlgPurchaseBtn.gameObject.SetActive(true);
                    HM._.ui.InfoDlgMoveBtn.gameObject.SetActive(false);
                    //*--> fui.onClickInfoDialogPurchaseBtn()でアイテム 購入
                    break;
                case PlayerSkin: case PetSkin:
                    HM._.ui.InfoDlgPurchaseBtn.gameObject.SetActive(false);
                    HM._.ui.InfoDlgMoveBtn.gameObject.SetActive(true);
                    //*--> ui.onClickGoClothShop()で、場所移動
                    break;
            }
        }
        //* 配置
        else {
            if(isArranged) {
                HM._.ui.showErrorMsgPopUp(LM._.localize("Already in use!"));
                return;
            }
            else {
                display();
            }
        }
    }
    protected void backHome() {
        HM._.ui.InfoDialog.SetActive(false);
        HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
        HM._.ui.onClickDecorateModeCloseBtn();
        HM._.ui.onClickWoodSignArrowBtn(dirVal: 1); //* プレイヤーが動かないこと対応
        HM._.ui.onClickWoodSignArrowBtn(dirVal: -1);
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Funiture : Item {
    [Header("追加")]
    [SerializeField] int price; public override int Price {get => price; set => price = value;}
    [SerializeField] GameObject prefab;    public GameObject Prefab {get => prefab; set => prefab = value;}
    [SerializeField] Vector2 pos;   public Vector2 Pos {get => pos; set => pos = value;}
    [SerializeField] bool isFlat;  public bool IsFlat {get => isFlat; set => isFlat = value;}

    Funiture() {
        pos = Vector2.zero;
        isFlat = false;
    }

    public override void create() {
        HM._.state = HM.STATE.DECORATION_MODE;
        int idx = HM._.ui.CurSelectedItemIdx;

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
        HM._.ui.InfoDialog.SetActive(false);
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
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.time2);
        //* ホームに戻す
        backHome();
    }

    #region Priavate Func
    private Transform setSpriteByType() {
        SpriteRenderer sr = (this.type == TYPE.Wall)? HM._.wallSr : HM._.floorSr;
        BgFuniture[] items = Array.FindAll(DB.Dt.Bgs, item => item.Type == this.type);
        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 適用
        sr.sprite = DB.Dt.Bgs[HM._.ui.CurSelectedItemIdx].Spr; 
        //* EFに位置を与えるため、リターン
        return sr.transform;
    }
    #endregion
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class PlayerSkin : Item {
    [Header("追加")]
    [SerializeField] SpriteLibraryAsset sprLibraryAsset;    public SpriteLibraryAsset SprLibraryAsset {get => sprLibraryAsset; set => sprLibraryAsset = value;}
    public override int Price { get => 0; set {} } // 使わない

    public override void create() {
        //* 画像 (タイプによって)
        Transform objTf = setSpriteLibrary();
        Debug.Log($"objTf.transform.localPositoin= {objTf.transform.localPosition}");
        //* ホームに戻す
        backHome();
        //* 効果
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.time2);
    }

    #region Priavate Func
    private Transform setSpriteLibrary() {
        Debug.Log($"PlayerSkin:: setSpriteLibrary():: HM._.ui.CurSelectedItemIdx= {HM._.ui.CurSelectedItemIdx}");
        PlayerSkin[] items = DB.Dt.PlSkins;
        SpriteLibrary sprLib = HM._.pl.GetComponent<SpriteLibrary>();
        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 適用
        sprLib.spriteLibraryAsset = items[HM._.ui.CurSelectedItemIdx].SprLibraryAsset;
        //* EFに位置を与えるため、リターン
        return sprLib.transform;
    }
    #endregion
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class PetSkin : Item {
    [Header("追加")]
    [SerializeField] SpriteLibraryAsset sprLibraryAsset;    public SpriteLibraryAsset SprLibraryAsset {get => sprLibraryAsset; set => sprLibraryAsset = value;}
    public override int Price { get => 0; set {} } // 使わない

    public override void create() {
        //* 画像 (タイプによって)
        Transform objTf = setSpriteLibrary();
        //* ホームに戻す
        backHome();
        //* 効果
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.time2);
    }

    #region Priavate Func
    private Transform setSpriteLibrary() {
        PetSkin[] items = DB.Dt.PtSkins;
        SpriteLibrary sprLib = HM._.pet.GetComponent<SpriteLibrary>();
        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false);
        //* 適用
        sprLib.spriteLibraryAsset = items[HM._.ui.CurSelectedItemIdx].SprLibraryAsset;
        //* EFに位置を与えるため、リターン
        return sprLib.transform;
    }
    #endregion
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