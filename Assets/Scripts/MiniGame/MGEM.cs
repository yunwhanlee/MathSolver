using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MGEM : MonoBehaviour {
    public enum IDX {
        //* Obj
        AppleObj,
        //* EF
    }

    
    [SerializeField] GameObject appleObj;      public GameObject AppleObj {get => appleObj; set => appleObj = value;}

    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();
    [SerializeField] Transform objectGroup;
    [SerializeField] Transform effectGroup;

    void Awake() {
        //* 順番合わせること
        pool.Add(initEF(appleObj, max: 15, objectGroup));
    }

/// -----------------------------------------------------------------------------------------------------------------
#region OBJECT POOL
/// -----------------------------------------------------------------------------------------------------------------
    private ObjectPool<GameObject> initEF(GameObject obj, int max, Transform parentTf){
        return new ObjectPool<GameObject>(
            () => instantiateEF(obj, parentTf), //* 生成
            onGetEF,//(obj) => onGetEF(obj), //* 呼出
            onReleaseEF, //(obj) => onReleaseEF(obj), //* 戻し
            Destroy, //(obj) => Destroy(obj),
            maxSize : max //* 最大生成回数
        );
    }
    private GameObject instantiateEF(GameObject obj, Transform parentTf) => Instantiate(obj, parentTf);
    private void onGetEF(GameObject obj) => obj.SetActive(true);
    private void onReleaseEF(GameObject obj) => obj.SetActive(false);
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (POOL TYPE) ★Coroutine！
/// -----------------------------------------------------------------------------------------------------------------
    public GameObject createObj(int idx, Vector3 pos, WaitForSeconds delay) {
        GameObject obj = pool[idx].Get();
        StartCoroutine(coCreateObj(obj, idx, pos, delay));
        return obj;
    }
    private IEnumerator coCreateObj(GameObject obj, int idx, Vector3 position, WaitForSeconds delay){
        Debug.Log($"coCreateObj(idx={idx}, pos={position}):: -> {obj.name}");
        obj.transform.position = position;
        yield return delay;
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

#endregion
}