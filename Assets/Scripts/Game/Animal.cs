using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.Animation; //* SpriteLibrary

public class Animal : MonoBehaviour {
    string[] TALK_CORRECT_STRS = {"대단해!", "우와!", "역시!", "만세!", "고마워요!"};
    string[] TALK_WRONG_STRS = {"헉!", "안돼..", "끄아아", "앗?!", "선생님?!"};

    [Header("OUTSIDE")]
    Animator anim; public Animator Anim {get => anim;}
    SpriteRenderer sr;  public SpriteRenderer Sr {get => sr;}

    [Header("ACTIVE TYPE")]
    [SerializeField] GameObject animalHeartPoofEF;
    [SerializeField] GameObject animalHeartBreakEF;
    [SerializeField] GameObject animalTalkEF;

    [Header("VALUE")]
    SpriteLibrary sprLib; public SpriteLibrary SprLib {get => sprLib;}
    [SerializeField] List<SpriteLibraryAsset> animalSprLibAssetList;
    [SerializeField] List<SpriteLibraryAsset> jungleSmallAnimalSprLibAstList;

    void Start() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sprLib = GetComponent<SpriteLibrary>();

        animalTalkEF.SetActive(false);

        //* Set Random SpriteLibraryAsset
        // setRandomSprLibAsset();
    }
///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    public void setRandomSprLibAsset() {
        List<SpriteLibraryAsset> animalList;
        if(GM._.BgStatus == GM.BG_STT.JungleFlower) //* 一般動物
            animalList = jungleSmallAnimalSprLibAstList;
        else //* 小さい動物
            animalList = animalSprLibAssetList;

        int randIdx = Random.Range(0, animalList.Count);
        sprLib.spriteLibraryAsset = animalList[randIdx];

        Debug.Log($"setRandomSprLibAsset():: BgStatus= {GM._.BgStatus}, animalList[{randIdx}]= {animalList[randIdx].name}");

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
