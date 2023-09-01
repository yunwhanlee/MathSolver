using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HEM : MonoBehaviour { //* Home Effect Manager
    public enum IDX {
        FunitureSetupEF, CoinBurstTopEF, UISparkleAreaWhiteEF
    };
    [SerializeField] Transform effectGroup;
    List<IObjectPool<GameObject>> pool = new List<IObjectPool<GameObject>>();

    [Header("CREATE TYPE")]
    [SerializeField] GameObject funitureSetupEF;      public GameObject FunitureSetupEF {get => funitureSetupEF; set => funitureSetupEF = value;}
    [SerializeField] GameObject coinBurstTopEF;      public GameObject CoinBurstTopEF {get => coinBurstTopEF; set => coinBurstTopEF = value;}
    [Header("PLAYER SKIN AURA EF(CREATE FOREVER TYPE)")]
    [SerializeField] GameObject angelAuraEF;

    [SerializeField] GameObject devilBlueAuraEF;
    [SerializeField] GameObject devilGreenAuraEF;
    [SerializeField] GameObject devilPinkAuraEF;
    [SerializeField] GameObject devilRedAuraEF;

    [SerializeField] GameObject golbinAuraEF;
    [SerializeField] GameObject greenGolbinAuraEF;
    [SerializeField] GameObject orangeNeonGolbinAuraEF;
    [SerializeField] GameObject pinkNeonGolbinAuraEF;
    [SerializeField] GameObject purpleNeonGolbinAuraEF;
    [SerializeField] GameObject redGolbinAuraEF;

    [SerializeField] GameObject joseonWolfAuraEF;

    [SerializeField] GameObject spiritBlueAuraEF;
    [SerializeField] GameObject spiritGreenAuraEF;
    [SerializeField] GameObject spiritPurpleAuraEF;
    [SerializeField] GameObject spiritRedAuraEF;
    [SerializeField] GameObject spiritYellowAuraEF;

    [SerializeField] GameObject divideWizardAuraEF;
    [SerializeField] GameObject overpowerWolfAuraEF;

    void Awake() {
        pool.Add(initEF(funitureSetupEF, max: 2));
        pool.Add(initEF(coinBurstTopEF, max: 2));
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
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION : PLAYER SKIN AURA EF(CREATE FOREVER TYPE)
/// -----------------------------------------------------------------------------------------------------------------
    public void createPlayerSkinAuraEF() {
        Player pl = HM._.pl;
        //* 初期化 (削除) : 切り替えたりするとき、重なるバグ対応
        for(int i = 0; i < pl.AuraEFGroup.childCount; i++)
            Destroy(pl.AuraEFGroup.GetChild(i).gameObject);

        //* Sprite Library Assetを基準で判断
        string skinName = pl.SprLib.spriteLibraryAsset.name;
        Debug.Log($"setPlayerSkinAuraEF():: {skinName}");

        if(skinName.Contains("Angel")) Instantiate(angelAuraEF, pl.AuraEFGroup);
        
        else if(skinName == "BlueDevil") Instantiate(devilBlueAuraEF, pl.AuraEFGroup);
        else if(skinName == "CandyDevil") Instantiate(devilPinkAuraEF, pl.AuraEFGroup);
        else if(skinName == "Devil") Instantiate(devilRedAuraEF, pl.AuraEFGroup);
        else if(skinName == "GreenDevil") Instantiate(devilGreenAuraEF, pl.AuraEFGroup);
        else if(skinName == "RedDevil") {Instantiate(devilPinkAuraEF, pl.AuraEFGroup); Instantiate(devilRedAuraEF, pl.AuraEFGroup);}

        else if(skinName == "Goblin") Instantiate(golbinAuraEF, pl.AuraEFGroup);
        else if(skinName == "GreenGoblin") Instantiate(greenGolbinAuraEF, pl.AuraEFGroup);
        else if(skinName == "OrangeNeonGoblin") Instantiate(orangeNeonGolbinAuraEF, pl.AuraEFGroup);
        else if(skinName == "PinkNeonGoblin") Instantiate(pinkNeonGolbinAuraEF, pl.AuraEFGroup);
        else if(skinName == "PurpleNeonGoblin") Instantiate(purpleNeonGolbinAuraEF, pl.AuraEFGroup);
        else if(skinName == "RedGoblin") Instantiate(redGolbinAuraEF, pl.AuraEFGroup);

        else if(skinName.Contains("JoseonWolf")) Instantiate(joseonWolfAuraEF, pl.AuraEFGroup);

        else if(skinName == "GreenSpirit") Instantiate(spiritGreenAuraEF, pl.AuraEFGroup);
        else if(skinName == "PinkNeonSpirit") {Instantiate(spiritPurpleAuraEF, pl.AuraEFGroup); Instantiate(spiritRedAuraEF, pl.AuraEFGroup);}
        else if(skinName == "PinkSpirit") Instantiate(spiritRedAuraEF, pl.AuraEFGroup);
        else if(skinName == "RedSpirit") Instantiate(spiritRedAuraEF, pl.AuraEFGroup);
        else if(skinName == "SandSpirit") Instantiate(spiritYellowAuraEF, pl.AuraEFGroup);
        else if(skinName == "Spirit") Instantiate(spiritBlueAuraEF, pl.AuraEFGroup);
        else if(skinName == "WhiteSpirit") {Instantiate(spiritGreenAuraEF, pl.AuraEFGroup); Instantiate(spiritYellowAuraEF, pl.AuraEFGroup);}
        else if(skinName == "YellowNeonSpirit") {Instantiate(spiritPurpleAuraEF, pl.AuraEFGroup); Instantiate(spiritYellowAuraEF, pl.AuraEFGroup);}

        else if(skinName == "DivideWolf") Instantiate(divideWizardAuraEF, pl.AuraEFGroup);
        else if(skinName == "OverPowerWolf") Instantiate(overpowerWolfAuraEF, pl.AuraEFGroup);
    }
#endregion
}
