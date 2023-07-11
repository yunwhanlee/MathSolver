using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation; //* SpriteLibrary

public class Animal : MonoBehaviour {
    [Header("OUTSIDE")]
    Animator anim; public Animator Anim {get => anim;}

    [Header("ACTIVE TYPE")]
    [SerializeField] GameObject animalHeartPoofEF;
    [SerializeField] GameObject animalHeartBreakEF;

    [Header("VALUE")]
    SpriteLibrary sprLib; public SpriteLibrary SprLib {get => sprLib;}
    [SerializeField] List<SpriteLibraryAsset> animalSprLibAssetList; public List<SpriteLibraryAsset> AnimalSprLibAssetList {get => animalSprLibAssetList; set => animalSprLibAssetList = value;}

    void Start() {
        anim = GetComponent<Animator>();
        sprLib = GetComponent<SpriteLibrary>();

        //* Set Random SpriteLibraryAsset
        setRandomSprLibAsset();
    }
///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setRandomSprLibAsset() {
        int randIdx = Random.Range(0, animalSprLibAssetList.Count);
        sprLib.spriteLibraryAsset = animalSprLibAssetList[randIdx];
        //* このリスト 削除
        animalSprLibAssetList.RemoveAt(randIdx);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION (ACTIVE TYPE)
/// -----------------------------------------------------------------------------------------------------------------
    public IEnumerator coActiveAnimalHeartPoofEF() {
        animalHeartPoofEF.SetActive(true);
        yield return Util.time2;
        animalHeartPoofEF.SetActive(false);
    }
    public IEnumerator coActiveAnimalHeartBreakEF() {
        animalHeartBreakEF.SetActive(true);
        yield return Util.time2;
        animalHeartBreakEF.SetActive(false);
    }
#endregion
}
