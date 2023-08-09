using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGM : MonoBehaviour {

    //* OUTSIDE
    public static MGM _;
    public Cam cam;
    public MGUI ui;
    public GEM gem;
    //TODO MiniGameTalkManager

    [Header("MINI GAME MAPS")]
    [SerializeField] GameObject[] maps;

    void Start() {
        int idx = 
            (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame1_Orchard)? 0
            : (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame2_Monkeywat)? 1
            : (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame3_IceDragon)? 2 : -1;
        if(idx == -1) {
            Debug.LogError("存在しないマップINDEXです。");
            return;
        } 
        maps[idx].SetActive(true);
    }
}
