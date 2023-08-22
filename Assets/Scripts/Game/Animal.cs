
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation; //* SpriteLibrary

public class Animal : MonoBehaviour {
    string[] TALK_CORRECT_STRS = {LM._.localize("Great!") , LM._.localize("Wow!"), LM._.localize("Yeah!"), LM._.localize("Thanks!")};
    string[] TALK_WRONG_STRS = {LM._.localize("Oops!"), LM._.localize("No.."), LM._.localize("Omg"), LM._.localize("Sir?!")};

    [Header("OUTSIDE")]
    Animator anim; public Animator Anim {get => anim;}
    SpriteRenderer sr;  public SpriteRenderer Sr {get => sr;} //* Sorting Order

    [Header("ACTIVE TYPE")]
    [SerializeField] GameObject animalHeartPoofEF;
    [SerializeField] GameObject animalHeartBreakEF;
    [SerializeField] GameObject animalTalkEF;

    [Header("VALUE")]
    SpriteLibrary sprLib;
    
    [SerializeField] List<SpriteLibraryAsset> animalSprLibAssetList;
    [SerializeField] List<SpriteLibraryAsset> jungleSmallAnimalSprLibAstList;

    [Header("FOREST")]
    [SerializeField] List<SpriteLibraryAsset> forestAnimalList; // 最大8
    [Header("JUNGLE")]
    [SerializeField] List<SpriteLibraryAsset> swampAnimalList; // 最大8
    [SerializeField] List<SpriteLibraryAsset> bushAnimalList; // 最大4
    [SerializeField] List<SpriteLibraryAsset> monkeyWatAnimalList; // 最大3
    [Header("TUNDRA : デザインもっと必要")]
    [SerializeField] List<SpriteLibraryAsset> tundraAnimalList; // 最大8

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sprLib = GetComponent<SpriteLibrary>();

        animalTalkEF.SetActive(false);

        //* Set Random SpriteLibraryAsset
        // setRandomAnimal();
    }
///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setRandomAnimal() {
        List<SpriteLibraryAsset> animalList = null;

        switch(GM._.BgStatus) {
            case GM.BG_STT.StarMountain :
            case GM.BG_STT.Windmill :
            case GM.BG_STT.Orchard : {
                animalList = forestAnimalList;
                break;
            }
            case GM.BG_STT.Swamp : {
                animalList = swampAnimalList;
                break;
            }
            case GM.BG_STT.Bush : {
                animalList = bushAnimalList;
                break;
            }
            case GM.BG_STT.MonkeyWat : {
                animalList = monkeyWatAnimalList;
                break;
            }
            case GM.BG_STT.EntranceOfTundra :
            case GM.BG_STT.SnowMountain :
            case GM.BG_STT.IceDragon : {
                animalList = tundraAnimalList;
                break;
            }
        }

        // if(GM._.BgStatus == GM.BG_STT.Bush) //* 一般動物
        //     animalList = jungleSmallAnimalSprLibAstList;
        // else //* 小さい動物
        //     animalList = animalSprLibAssetList;

        //! Error対応
        if(animalList == null) {
            Debug.LogError("Animal:: animalListを初期化することができません。");
            return;
        }

        int randIdx = Random.Range(0, animalList.Count);
        sprLib.spriteLibraryAsset = animalList[randIdx];

        Debug.Log($"setRandomAnimal():: BgStatus= {GM._.BgStatus}, animalList[{randIdx}]= {animalList[randIdx].name}");

        //* このリスト 削除
        animalList.RemoveAt(randIdx);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region EFFECT
/// -----------------------------------------------------------------------------------------------------------------
    public IEnumerator coCorrectEF() {
        animalHeartPoofEF.SetActive(true);
        animalTalkEF.SetActive(true);
        animalTalkEF.GetComponentInChildren<TextMeshPro>().text = TALK_CORRECT_STRS[Random.Range(0, TALK_CORRECT_STRS.Length)];
        yield return Util.time2;
        animalHeartPoofEF.SetActive(false);
        animalTalkEF.SetActive(false);
    }
    public IEnumerator coWrongEF() {
        animalHeartBreakEF.SetActive(true);
        animalTalkEF.SetActive(true);
        animalTalkEF.GetComponentInChildren<TextMeshPro>().text = TALK_WRONG_STRS[Random.Range(0, TALK_WRONG_STRS.Length)];
        yield return Util.time2;
        animalHeartBreakEF.SetActive(false);
        animalTalkEF.SetActive(false);
    }
#endregion
}
