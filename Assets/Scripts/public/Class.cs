using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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