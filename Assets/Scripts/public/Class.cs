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
public abstract class ItemFrameBtn { //* 親
    [SerializeField] GameObject obj; public GameObject Obj {get => obj; set => obj = value;}
    [SerializeField] Image img; public Image Img {get => img; set => img = value;}
    [SerializeField] GameObject lockFrameObj; public GameObject LockFrameObj {get => lockFrameObj; set => lockFrameObj = value;}
    [SerializeField] GameObject notifyObj; public GameObject NotifyObj {get => notifyObj; set => notifyObj = value;}
    [SerializeField] GameObject arrangeFrameObj; public GameObject ArrangeFrameObj {get => arrangeFrameObj; set => arrangeFrameObj = value;} 
    [SerializeField] GameObject legacyIconObj;  public GameObject LegacyIconObj {get => legacyIconObj; set => legacyIconObj = value;}
    public ItemFrameBtn(GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj, GameObject legacyIconObj) {
        this.obj = obj;
        this.img = img;
        this.lockFrameObj = lockFrameObj;
        this.notifyObj = notifyObj;
        this.arrangeFrameObj = arrangeFrameObj; // ✓ 表示
        this.legacyIconObj = legacyIconObj;
    }
    public virtual void init() {
        img.sprite = null;
        lockFrameObj.SetActive(true);
        notifyObj.SetActive(false);
        arrangeFrameObj.SetActive(false);
        legacyIconObj.SetActive(false);
    }
    public virtual void updateItemFrame(Item item) {
        Img.sprite = item.Spr;
        LockFrameObj.SetActive(item.IsLock);
        NotifyObj.SetActive(item.IsNotify);
        ArrangeFrameObj.SetActive(item.IsArranged);
        LegacyIconObj.SetActive(item.Grade == Item.GRADE.Special);
        Obj.GetComponent<Image>().sprite = HM._.ui.ItemBtnFrameSprs[(int)item.Grade];
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class FunitureShopItemBtn : ItemFrameBtn {
    //* 追加
    const int COIN_ICON = 0, FAME_ICON = 1, EMPTY = 2;
    [SerializeField] TextMeshProUGUI priceTxt; public TextMeshProUGUI PriceTxt {get => priceTxt; set => priceTxt = value;}
    [SerializeField] Image priceIconImg;

    public FunitureShopItemBtn( //* 親 param
    GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj, GameObject legacyIconObj
    ,TextMeshProUGUI priceTxt, Image priceIconImg) //* 子 param
    :base(obj, img, lockFrameObj, notifyObj, arrangeFrameObj, legacyIconObj) { //* 親 コンストラクター 呼出し
        //* 子 要素
        this.priceTxt = priceTxt;
        this.priceIconImg = priceIconImg;
    }

    public override void init() {
        base.init();
        //* 子 要素 (初期化)
        priceTxt.text = "";
        priceIconImg.sprite = HM._.ui.PriceIconSprs[COIN_ICON];
    }

    public override void updateItemFrame(Item item) {
        try {
            base.updateItemFrame(item);
            //* 子 要素
            if(item is Funiture)  // var ft = item as Funiture;
                priceTxt.text = convertPriceTxt(item.Price);
            else if(item is BgFuniture)  // var bg = item as BgFuniture;
                priceTxt.text = convertPriceTxt(item.Price);
            //* priceTxtObj (非)表示
            priceTxt.transform.parent.gameObject.SetActive(item.IsLock);
        }
        catch(NullReferenceException err) {
            Debug.LogError("<color=yellow>DBManagerのInspectorビューに、Nullを確認してください。</color>" + "\n " + err);
        }
    }
    private string convertPriceTxt(string priceTxt) {
        Debug.Log($"convertPriceTxt(priceTxt= {priceTxt})");
        string res = priceTxt;
        if(priceTxt.Contains("quest")) {
            res = "???";
            priceIconImg.sprite = HM._.ui.PriceIconSprs[EMPTY];
        }
        else if(priceTxt.Contains("fame")) {
            res = priceTxt.Split("_")[1];
            priceIconImg.sprite = HM._.ui.PriceIconSprs[FAME_ICON];
        }
        return res;
    }
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class InventoryItemBtn : ItemFrameBtn {

    public InventoryItemBtn( //* 親 param
    GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject arrangeFrameObj, GameObject legacyIconObj)
    :base(obj, img, lockFrameObj, notifyObj, arrangeFrameObj, legacyIconObj) { //* 親 コンストラクター 呼出し
        //* 子 要素
    }
    public override void init() {
        base.init();
    }

    public override void updateItemFrame(Item item) {
        try {
            base.updateItemFrame(item);
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
    public enum GRADE {Normal, Special};
    [SerializeField] string name;   public string Name {get => name;}
    [SerializeField] GRADE grade;   public GRADE Grade {get => grade;}
    [SerializeField] Sprite spr;    public Sprite Spr {get => spr; set => spr = value;}
    // [SerializeField] int id;    public int Id {get => id; set => id = value;}
    [SerializeField] bool isLock;    public bool IsLock {get => isLock; set => isLock = value;}
    [SerializeField] bool isNotify;    public bool IsNotify {get => isNotify; set => isNotify = value;}
    [SerializeField] bool isArranged;   public bool IsArranged {get => isArranged; set => isArranged = value;}

    //* 抽象 : ★★★ 親クラスで、抽象メソッドが呼ばれても、実際に動く場所は「子」クラスだから大丈夫
    public abstract string Price {get; set;} //? 子のpriceがあれば、使う
    public abstract void create();
    //* 仮想
    public virtual void display() {
        if(this is not Funiture) SM._.sfxPlay(SM.SFX.FeatherPop.ToString());
        create();
        this.IsArranged = true;
        this.isNotify = false;
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
    public virtual void purchase(bool isFree = false) {
        if(isFree){
            Debug.Log("🎁イベントリワードで提供");
            isLock = false;
            isNotify = true;
            HM._.ui.activeNewFuniturePopUp(spr, name);
        }
        else if(DB.Dt.Coin >= int.Parse(this.Price)) {
            Debug.Log("💰購入成功！！");
            DB.Dt.setCoin(-int.Parse(this.Price));
            isLock = false;
            isNotify = true;
            HM._.ui.activeNewFuniturePopUp(spr, name);
        }
        else {
            Debug.Log("😢 お金がたりない！！");
            HM._.ui.showErrorMsgPopUp(LM._.localize("Not enough coin!"));
        }
    }
    public virtual void arrange() {
        const int PURCHASE_BTN = 0, MOVE_BTN = 1;
        Debug.Log($"<color=white>Item:: arrange():: name= {name} ,Price= {Price}, isLock= {isLock}</color>");
        var hui = HM._.ui;
        //* ロック
        if(isLock) {
            //* Infoダイアログ 表示
            hui.InfoDialog.SetActive(true);
            switch(this) {
                case Funiture:
                case BgFuniture:
                    //* 家具のみPriceが有るので、活用
                    int index = (this.Price.Contains("quest") || this.Price.Contains("fame"))? MOVE_BTN : PURCHASE_BTN;
                    hui.activeInfoDlgBtn(idx: index);
                    //--> fui.onClickInfoDialogPurchaseBtn()でアイテム 購入
                    break;
                case PlayerSkin:
                case PetSkin:
                    hui.activeInfoDlgBtn(idx: MOVE_BTN);
                    //--> ui.onClickGoClothShop()で、場所移動
                    break;
            }
            hui.setInfoDlgData(this); //* 適用
        }
        //* 配置
        else {
            if(isArranged) {
                hui.showErrorMsgPopUp(LM._.localize("Already in use!"));
            }
            else {
                //* Petの場合、「X」リストクリックしたら、InfoDialog表示しなくすぐ適用
                if(this is PetSkin && name == "X") {
                    display();
                    return;
                }
                hui.InfoDialog.SetActive(true);
                hui.setInfoDlgData(this);
                var arrangeBtn = hui.activeInfoDlgBtn(idx: 2);
                arrangeBtn.onClick.RemoveAllListeners(); //* イベント 初期化
                arrangeBtn.onClick.AddListener(() => display()); //* 家具 配置 イベント
            }
        }
    }
    protected void backHome() {
        Debug.Log("backHome():: " + this);
        HM._.ui.InfoDialog.SetActive(false);
        if(this is Funiture) HM._.ui.onClickDecorateModeIconBtn(); //* FUNITUREモード
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
    [SerializeField] string price; public override string Price {get => price; set => price = value;}
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
        GameObject pref = (HM._.fUI.Category == Enum.FUNITURE_CATE.Funiture)? HM._.fUI.SortFunitures[idx].Prefab //DB.Dt.Funitures[idx].Prefab
            : (HM._.fUI.Category == Enum.FUNITURE_CATE.Decoration)? HM._.fUI.SortDecorations[idx].prefab //DB.Dt.Decorations[idx].Prefab
            : HM._.fUI.SortMats[idx].Prefab; //DB.Dt.Mats[idx].Prefab;

        //* 生成
        GameObject ins = Util.instantiateObj(pref, HM._.ui.RoomObjectGroupTf);
        ins.name = ins.name.Split('(')[0]; //* 名(Clone) 削除
        RoomObject rObj = ins.GetComponent<RoomObject>();

        //* 初期化
        rObj.Start(); 

        //* 選択されて、デコレーションモード 用意
        rObj.IsSelect = false;
        rObj.Sr.material = HM._.outlineAnimMt; //* アウトライン 付き
        HM._.fUI.CurSelectedObj = rObj.gameObject;
        HM._.ui.InfoDialog.SetActive(false);
        HM._.ui.DecorateModePanel.SetActive(true);

        //* 飾り用のアイテムのZ値が-1のため、この上に配置すると、Z値が０の場合は MOUSE EVENTが出来なくなる。
        const float OFFSET_Z = -1;
        rObj.transform.position = new Vector3(rObj.transform.position.x, rObj.transform.position.y, OFFSET_Z);

        //* 飾りモード：一番レイヤーを前に配置
        rObj.Sr.sortingOrder = 100;
        Debug.Log($"SORTING AA createFunitureItem:: {rObj.gameObject.name}.sortingOrder= {rObj.Sr.sortingOrder}");
    }

    public override void display() {
        Debug.Log("display():: " + this);
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
    [SerializeField] string price; public override string Price {get => price; set => price = value;}
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
        BgFuniture[] items = Array.FindAll(HM._.fUI.SortBgs, item => item.Type == this.type); //DB.Dt.Bgs, item => item.Type == this.type);
        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 適用
        sr.sprite = HM._.fUI.SortBgs[HM._.ui.CurSelectedItemIdx].Spr; //DB.Dt.Bgs[HM._.ui.CurSelectedItemIdx].Spr;
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
    public override string Price { get => ""; set {} } // 使わない

    public override void create() {
        //* 画像 (タイプによって)
        Transform objTf = setSpriteLibrary();
        Debug.Log($"objTf.transform.localPositoin= {objTf.transform.localPosition}");
        //* ホームに戻す
        backHome();
        //* 効果
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, objTf.position, Util.time2);        
    }

    #region Private Func
    private Transform setSpriteLibrary() {
        Debug.Log($"PlayerSkin:: setSpriteLibrary():: HM._.ui.CurSelectedItemIdx= {HM._.ui.CurSelectedItemIdx}");
        PlayerSkin[] items = HM._.iUI.SortPlayerSkins;//DB.Dt.PlSkins;
        SpriteLibrary sprLib = HM._.pl.SprLib;
        //* 単一だからInArrange全てFalseに初期化
        Array.ForEach(items, item => item.IsArranged = false); 
        //* 適用
        sprLib.spriteLibraryAsset = items[HM._.ui.CurSelectedItemIdx].SprLibraryAsset;
        HM._.em.createPlayerSkinAuraEF();
        //* Portrait変更
        HM._.pl.IdleSpr = sprLib.spriteLibraryAsset.GetSprite("Idle", "Entry");
        HM._.ui.setMyPortraitsImg(HM._.pl.IdleSpr);
        HM._.htm.setSpkPortrait();
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
    public override string Price { get => ""; set {} } // 使わない

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