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
    public abstract void updateItemFrame(Funiture item);
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

    public override void updateItemFrame(Funiture item) {
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
            Debug.LogError("<color=yellow>DBManagerのInspectorビューに、Nullが有るかを確認してください。</color>" + "\n " + err);
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
    [SerializeField] GameObject prefab;    public GameObject Prefab {get => prefab;}
    [SerializeField] Sprite spr;    public Sprite Spr {get => spr; set => spr = value;}
    [SerializeField] int id;    public int Id {get => id; set => id = value;}
    [SerializeField] int price; public int Price {get => price; set => price = value;}
    [SerializeField] bool isLock;    public bool IsLock {get => isLock; set => isLock = value;}
    [SerializeField] bool isNotify;    public bool IsNotify {get => isNotify; set => isNotify = value;}

    public abstract void updateItem();
}
//---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Funiture : Item {
    [SerializeField] bool isArranged;   public bool IsArranged {get => isArranged; set => isArranged = value;}
    [SerializeField] Vector2 pos;   public Vector2 Pos {get => pos; set => pos = value;}
    [SerializeField] bool isFlat;  public bool IsFlat {get => isFlat; set => isFlat = value;}

    Funiture() {
        isArranged = false;
        pos = Vector2.zero;
        isFlat = false;
    }
    public override void updateItem() {

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