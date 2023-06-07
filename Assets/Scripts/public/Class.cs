using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region UI アイテム フレーム ボタン
///---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public abstract class ItemFrameBtn {
    [SerializeField] GameObject obj; public GameObject Obj {get => obj; set => obj = value;}
    [SerializeField] Image img; public Image Img {get => img; set => img = value;}
    [SerializeField] GameObject lockFrameObj; public GameObject LockFrameObj {get => lockFrameObj; set => lockFrameObj = value;}
    [SerializeField] GameObject notifyObj; public GameObject NotifyObj {get => notifyObj; set => notifyObj = value;}

    // public ItemFrameBtn(GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject priceFrameObj)
    // {
    //     this.obj = obj;
    //     this.img = img;
    //     this.lockFrameObj = lockFrameObj;
    //     this.notifyObj = notifyObj;
    // }

    public abstract void initInvFrame();
    // {
        // img.sprite = null;
        // lockFrameObj.SetActive(true);
        // notifyObj.SetActive(false);
    // }

    // public void updateInvFrame(Enum.InvCategory cate, int i) {
        // img.sprite = (cate == Enum.InvCategory.Chara)? DB.Dt.Players[i].IdleSpr: DB.Dt.Pets[i].IdleSpr; //TODO :Theme
        // lockFrameObj.SetActive((cate == Enum.InvCategory.Chara)? DB.Dt.Players[i].IsLock : DB.Dt.Pets[i].IsLock); //TODO :Theme
        // notifyObj.SetActive((cate == Enum.InvCategory.Chara)? DB.Dt.Players[i].IsNotify : DB.Dt.Pets[i].IsNotify); //TODO :Theme
        // outlineObj.SetActive((cate == Enum.InvCategory.Chara)? i == DB.Dt.PlayerId : i == DB.Dt.PetId); //TODO :Theme
    // }
}

[System.Serializable]
public class FunitureShopItemBtn : ItemFrameBtn {
    [SerializeField] GameObject priceFrameObj; public GameObject PriceFrameObj {get => priceFrameObj; set => priceFrameObj = value;}

    public FunitureShopItemBtn(GameObject obj, Image img, GameObject lockFrameObj, GameObject notifyObj, GameObject priceFrameObj) {
        this.Obj = obj;
        this.Img = img;
        this.LockFrameObj = lockFrameObj;
        this.NotifyObj = notifyObj;
        this.PriceFrameObj = priceFrameObj;
    }

    public override void initInvFrame() {
        Img.sprite = null;
        LockFrameObj.SetActive(true);
        PriceFrameObj.SetActive(true);
        NotifyObj.SetActive(false);
    }
}

#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region アイテム
///---------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public abstract class Item {
    [SerializeField] string name;   public string Name {get => name;}
    [SerializeField] GameObject prefab;    public GameObject Prefab {get => prefab;}
    [SerializeField] int id;    public int Id {get => id; set => id = value;}
    [SerializeField] int price; public int Price {get => price; set => price = value;}
    [SerializeField] bool isLock;    public bool IsLock {get => isLock; set => isLock = value;}
    [SerializeField] bool isNotify;    public bool IsNotify {get => isNotify; set => isNotify = value;}

    public abstract void updateItem();
}

[System.Serializable]
public class Funiture : Item {
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