using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation; //* SpriteLibrary

public class Animal : MonoBehaviour {
    Animator anim; public Animator Anim {get => anim;}
    SpriteLibrary sprLib; public SpriteLibrary SprLib {get => sprLib;}
    [SerializeField] List<SpriteLibraryAsset> animalSprLibAssetList; public List<SpriteLibraryAsset> AnimalSprLibAssetList {get => animalSprLibAssetList; set => animalSprLibAssetList = value;}

    void Start() {
        anim = GetComponent<Animator>();
        sprLib = GetComponent<SpriteLibrary>();

        //* Random Animal
        int randIdx = Random.Range(0, animalSprLibAssetList.Count);
        sprLib.spriteLibraryAsset = animalSprLibAssetList[randIdx];

        animalSprLibAssetList.RemoveAt(randIdx);
    }


}
