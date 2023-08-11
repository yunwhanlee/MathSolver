using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MGEM : MonoBehaviour {
    public enum IDX {
        //* Obj
        AppleObj, GoldAppleObj, BombObj, DiamondObj
        //* EF
        ,StunEF, BasketCatchEF, ExplosionBombEF, ShineSpoutGoldEF,
    }

    //* MiniGame1
    [SerializeField] GameObject appleObj;
    [SerializeField] GameObject goldAppleObj;
    [SerializeField] GameObject bombObj;
    [SerializeField] GameObject diamondObj;

    [SerializeField] GameObject stunEF;
    [SerializeField] GameObject basketCatchEF;
    [SerializeField] GameObject explosionBombEF;
    [SerializeField] GameObject shineSpoutGoldEF;

    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();
    [SerializeField] Transform objectGroup;
    [SerializeField] Transform effectGroup;

    void Awake() {
        //! enum IDX 順番に合わせること!
        pool.Add(initEF(appleObj, max: 10, objectGroup));
        pool.Add(initEF(goldAppleObj, max: 3, objectGroup));
        pool.Add(initEF(bombObj, max: 3, objectGroup));
        pool.Add(initEF(diamondObj, max: 3, objectGroup));

        pool.Add(initEF(stunEF, max: 1, effectGroup));
        pool.Add(initEF(basketCatchEF, max: 4, effectGroup));
        pool.Add(initEF(explosionBombEF, max: 2, effectGroup));
        pool.Add(initEF(shineSpoutGoldEF, max: 2, effectGroup));
    }

/// -----------------------------------------------------------------------------------------------------------------
#region OBJECT POOL
/// -----------------------------------------------------------------------------------------------------------------
    private ObjectPool<GameObject> initEF(GameObject obj, int max, Transform parentTf){
        return new ObjectPool<GameObject>(
            () => instantiate(obj, parentTf), //* 生成
            onGet,//(obj) => onGetEF(obj), //* 呼出
            onRelease, //(obj) => onReleaseEF(obj), //* 戻し
            Destroy, //(obj) => Destroy(obj),
            maxSize : max //* 最大生成回数
        );
    }
    private GameObject instantiate(GameObject obj, Transform parentTf) => Instantiate(obj, parentTf);
    private void onGet(GameObject obj) {
        obj.SetActive(true);
        //! もし、Objクラスだったら、Coroutine初期化
        Obj objComponent = obj.GetComponent<Obj>();
        if (objComponent != null && objComponent.CoroutineID != null) 
            StopCoroutine(objComponent.CoroutineID);
    }
    private void onRelease(GameObject obj) => obj.SetActive(false);
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (POOL TYPE) ★Coroutine！
/// -----------------------------------------------------------------------------------------------------------------
    public GameObject createObj(int idx, Vector3 pos, WaitForSeconds delay) {
        GameObject obj = pool[idx].Get();
        //! オブジェクト生成は必ずObjクラスにすること
        //* 名前
        obj.name = (idx == 0)? IDX.AppleObj.ToString()
            : (idx == 1)? IDX.GoldAppleObj.ToString()
            : (idx == 2)? IDX.BombObj.ToString()
            : (idx == 3)? IDX.DiamondObj.ToString() : null;
        if(obj.name == null) {
            Debug.LogError("ERROR: MGEM:: createObjで、オブジェクト名がNULLです！");
            return null;
        }
        //* 生成(コールティン 開始) 
        obj.GetComponent<Obj>().CoroutineID = StartCoroutine(coCreateObj(obj, idx, pos, delay));
        return obj;
    }
    private IEnumerator coCreateObj(GameObject obj, int idx, Vector3 position, WaitForSeconds delay){
        Debug.Log($"coCreateObj(idx={idx}, pos={position}):: -> {obj.name}");
        obj.transform.position = position;

        yield return delay;
        Debug.Log($"coCreateObj():: delay -> release:: obj= {obj}");
        if (obj != null && obj.activeSelf) { //* もし、既に(Release)破壊されたら以下処理しない
            pool[idx].Release(obj);
        }
    }

    public void showEF(int idx, Vector3 pos, WaitForSeconds delay)
        => StartCoroutine(coShowEF(idx, pos, delay));

    private IEnumerator coShowEF(int idx, Vector3 position, WaitForSeconds delay){
        GameObject obj = pool[idx].Get();
        Debug.Log($"coShowEF(idx={idx}, pos={position}):: -> {obj.name}");
        obj.transform.position = position;

        yield return delay;
        pool[idx].Release(obj);
    }

    /// <summary>
    /// Destroy Object to Pool
    /// </summary>
    public void releaseObj(GameObject obj, int idx) {
        pool[idx].Release(obj);
    }

    public void releaseAllObj() {
        for(int i = 0; i < objectGroup.childCount; i++) {
            var obj = objectGroup.GetChild(i).gameObject;
            string name = obj.name;
            int idx = (name == IDX.AppleObj.ToString())? 0
                : (name == IDX.GoldAppleObj.ToString())? 1
                : (name == IDX.BombObj.ToString())? 2
                : (name == IDX.DiamondObj.ToString())? 3 : -1;

            //* 戻す
            if (obj != null && obj.activeSelf) {
                pool[idx].Release(obj);
                showEF((int)IDX.BasketCatchEF, obj.transform.position, Util.time2);
            }
        }
    }
#endregion
}