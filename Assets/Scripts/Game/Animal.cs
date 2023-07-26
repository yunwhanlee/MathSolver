using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation; //* SpriteLibrary

public class Animal : MonoBehaviour {
    [Header("OUTSIDE")]
    Animator anim; public Animator Anim {get => anim;}
    SpriteRenderer sr;  public SpriteRenderer Sr {get => sr;}

    [Header("ACTIVE TYPE")]
    [SerializeField] GameObject animalHeartPoofEF;
    [SerializeField] GameObject animalHeartBreakEF;

    [Header("VALUE")]
    SpriteLibrary sprLib; public SpriteLibrary SprLib {get => sprLib;}
    [SerializeField] List<SpriteLibraryAsset> animalSprLibAssetList; public List<SpriteLibraryAsset> AnimalSprLibAssetList {get => animalSprLibAssetList; set => animalSprLibAssetList = value;}
    [SerializeField] List<SpriteLibraryAsset> jungleSmallAnimalSprLibAstList;   public List<SpriteLibraryAsset> JungleSmallAnimalSprLibAstList {get => jungleSmallAnimalSprLibAstList; set => jungleSmallAnimalSprLibAstList = value;}

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sprLib = GetComponent<SpriteLibrary>();

        //* Set Random SpriteLibraryAsset
        // setRandomSprLibAsset();
    }
///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setRandomSprLibAsset() {
        List<SpriteLibraryAsset> animalList;
        if(GM._.JungleFlowerBG.activeSelf) //* 一般動物
            animalList = jungleSmallAnimalSprLibAstList;
        else //* 小さい動物
            animalList = animalSprLibAssetList;

        int randIdx = Random.Range(0, animalList.Count);
        sprLib.spriteLibraryAsset = animalList[randIdx];
        //* このリスト 削除
        animalList.RemoveAt(randIdx);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region EFFECT
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
