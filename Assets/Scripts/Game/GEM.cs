using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro; 

public class GEM : MonoBehaviour { //* Game Effect Manager
    public enum IDX {
        DropItemTxtEF, //! 順番合わせること
    };
    [SerializeField] Transform effectGroup;
    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();

    [Header("CREATE TYPE")]
    [SerializeField] GameObject dropItemTxtEF;      public GameObject DropItemTxtEF {get => dropItemTxtEF; set => dropItemTxtEF = value;}

    // [Header("ACTIVE TYPE")]
    //

    void Awake() {
        pool.Add(initEF(dropItemTxtEF, max: 5)); //! 順番合わせること
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
    private GameObject instantiateEF(GameObject obj) => Instantiate(obj, effectGroup);
    private void onGetEF(GameObject obj) => obj.SetActive(true);
    private void onReleaseEF(GameObject obj) => obj.SetActive(false);
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (POOL TYPE) ★Coroutine！
/// -----------------------------------------------------------------------------------------------------------------
    public void showEF(int idx, Vector3 pos, WaitForSeconds delay)
        => StartCoroutine(coShowEF(idx, pos, delay));
    private IEnumerator coShowEF(int idx, Vector3 position, WaitForSeconds delay){
        GameObject effect = pool[idx].Get();
        Debug.Log($"coShowEF(idx={idx}, pos={position}):: -> {effect.name}");
        effect.transform.position = position;

        yield return delay;
        pool[idx].Release(effect);
    }

    public void showDropItemTxtEF(int val, Vector3 pos, Color clr)
        => StartCoroutine(coShowDropItemTxtEF(val, pos, clr));
    private IEnumerator coShowDropItemTxtEF(int val, Vector3 position, Color clr){
        GameObject effect = pool[(int)IDX.DropItemTxtEF].Get();
        effect.transform.position = position;

        TextMeshPro txtObj = effect.GetComponentInChildren<TextMeshPro>();
        txtObj.text = $"+{val}";
        txtObj.color = clr;

        yield return Util.time1;
        pool[(int)IDX.DropItemTxtEF].Release(effect);
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
