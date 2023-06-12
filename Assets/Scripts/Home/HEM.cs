using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HEM : MonoBehaviour //* Home Effect Manager
{
    public enum IDX {
        FunitureSetupEF,
    };

    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();

    [Header("POOL TYPE")]
    [SerializeField] GameObject funitureSetupEF;      public GameObject FunitureSetupEF {get => funitureSetupEF; set => funitureSetupEF = value;}

    void Awake() {
        pool.Add(initEF(funitureSetupEF, max: 2));
    }
/// -----------------------------------------------------------------------------------------------------------------
#region OBJECT POOL
/// -----------------------------------------------------------------------------------------------------------------
    private ObjectPool<GameObject> initEF(GameObject obj, int max){
        return new ObjectPool<GameObject>(
            () => instantiateEF(obj), //* 生成
            onGetEF,//(obj) => onGetEF(obj), //* 呼出
            onReleaseEF, //(obj) => onReleaseEF(obj), //* 戻し
            Destroy, //(obj) => Destroy(obj),
            maxSize : max //* 最大生成回数
        );
    }
    private GameObject instantiateEF(GameObject obj) => Instantiate(obj, HM._.EffectGroup);
    private void onGetEF(GameObject obj) => obj.SetActive(true);
    private void onReleaseEF(GameObject obj) => obj.SetActive(false);
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (POOL TYPE) ★Coroutine！
/// -----------------------------------------------------------------------------------------------------------------
    public IEnumerator coShowEF(int idx, Vector3 position, WaitForSeconds delay){
        GameObject effect = pool[idx].Get();
        Debug.Log($"coShowEF(idx={idx}, pos={position}):: -> {effect.name}");
        effect.transform.position = position;
        yield return delay;
        pool[idx].Release(effect);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (ACTIVE TYPE)
/// -----------------------------------------------------------------------------------------------------------------
    // public IEnumerator coActiveMagicBuffEF(){
    //     camMagicBuffEF.SetActive(true);
    //     yield return Util.delay1;
    //     camMagicBuffEF.SetActive(false);
    // }
#endregion
}
