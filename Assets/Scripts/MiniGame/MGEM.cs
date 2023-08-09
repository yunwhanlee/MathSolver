using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MGEM : MonoBehaviour {
    public enum IDX {
        //* Obj
        AppleObj, GoldAppleObj, BombObj
        //* EF
    }

    //* MiniGame1
    [SerializeField] GameObject appleObj;
    [SerializeField] GameObject goldAppleObj;
    [SerializeField] GameObject bombObj;

    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();
    [SerializeField] Transform objectGroup;
    [SerializeField] Transform effectGroup;

    void Awake() {
        //! enum IDX 順番に合わせること!
        pool.Add(initEF(appleObj, max: 10, objectGroup));
        pool.Add(initEF(goldAppleObj, max: 3, objectGroup));
        pool.Add(initEF(bombObj, max: 3, objectGroup));
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
        obj.GetComponent<Obj>().CoroutineID = StartCoroutine(coCreateObj(obj, idx, pos, delay));
        return obj;
    }
    private IEnumerator coCreateObj(GameObject obj, int idx, Vector3 position, WaitForSeconds delay){
        Debug.Log($"coCreateObj(idx={idx}, pos={position}):: -> {obj.name}");
        obj.transform.position = position;

        yield return delay;
        if(!obj.activeSelf){
            yield break; //* もし、既に(Release)破壊されたら以下処理しない
        } 
        pool[idx].Release(obj);
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

#endregion
}