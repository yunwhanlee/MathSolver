using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map : MonoBehaviour {
    [SerializeField] Button btn;        public Button Btn {get => btn;}
    [SerializeField] string mapName;       public string MapName {get => mapName; set => mapName = value;}
    [SerializeField] bool[] isBgUnlocks;    public bool[] IsBgUnlocks {get => isBgUnlocks; set => isBgUnlocks = value;}
    [SerializeField] string[] bgNames;   public string[] BgNames {get => bgNames; set => bgNames = value;}
    [SerializeField] Image[] bgImgs;    public Image[] BgImgs {get => bgImgs;}
    [SerializeField] TextMeshProUGUI[] bgLimitLvTxts;     public TextMeshProUGUI[] BgLimitLvTxts {get => bgLimitLvTxts;}

    void Awake() {
        isBgUnlocks = new bool[3];
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void init(int limitLvIdx1, int limitLvIdx2, int limitLvIdx3) {
        const int FIRST = 0, SECOND = 1, THIRD = 2;
        int LV = DB.Dt.Lv;

        isBgUnlocks[FIRST] = LV >= limitLvIdx1;
        isBgUnlocks[SECOND] = LV >= limitLvIdx2;
        isBgUnlocks[THIRD] = LV >= limitLvIdx3;

        //* 処理
        btn.interactable = (LV >= limitLvIdx1);
        bgImgs[FIRST].color = (isBgUnlocks[FIRST])? Color.white : Color.gray;
        bgLimitLvTxts[FIRST].gameObject.SetActive(!isBgUnlocks[FIRST]);
        bgImgs[SECOND].color = (isBgUnlocks[SECOND])? Color.white : Color.gray;
        bgLimitLvTxts[SECOND].gameObject.SetActive(!isBgUnlocks[SECOND]);
        bgImgs[THIRD].color = (isBgUnlocks[THIRD])? Color.white : Color.gray;
        bgLimitLvTxts[THIRD].gameObject.SetActive(!isBgUnlocks[THIRD]);
    }


#endregion
}
