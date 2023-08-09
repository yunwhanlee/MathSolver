using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro; 

public class GEM : MonoBehaviour { //* Game Effect Manager
    public enum IDX {
        //* 順番合わせること
        DropItemTxtEF, 
        PlusBlinkBoxBurstEF,
        MinusBlinkBoxBurstEF,
        QuestionMarkBoxBurstEF,
    };
    [SerializeField] Transform effectGroup;
    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();

    [Header("CREATE TYPE")]
    [SerializeField] GameObject dropItemTxtEF;      public GameObject DropItemTxtEF {get => dropItemTxtEF; set => dropItemTxtEF = value;}
    [SerializeField] GameObject plusBlinkBoxBurstEF;    public GameObject PlusBlinkBoxBurstEF {get => plusBlinkBoxBurstEF; set => plusBlinkBoxBurstEF = value;}
    [SerializeField] GameObject minusBlinkBoxBurstEF;    public GameObject MinusBlinkBoxBurstEF {get => minusBlinkBoxBurstEF; set => minusBlinkBoxBurstEF = value;}
    [SerializeField] GameObject questionMarkBoxBurstEF; public GameObject QuestionMarkBoxBurstEF {get => questionMarkBoxBurstEF; set => questionMarkBoxBurstEF = value;}

    void Awake() {
        //* 順番合わせること
        pool.Add(initEF(dropItemTxtEF, max: 5));
        pool.Add(initEF(plusBlinkBoxBurstEF, max: 2));
        pool.Add(initEF(minusBlinkBoxBurstEF, max: 2));
        pool.Add(initEF(questionMarkBoxBurstEF, max: 2));
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
}
