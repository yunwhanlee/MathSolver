using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map : MonoBehaviour {
    [SerializeField] string mapName;       public string MapName {get => mapName; set => mapName = value;}
    [SerializeField] Sprite mapSpr;       public Sprite MapSpr {get => mapSpr; set => mapSpr = value;}
    [SerializeField] bool[] isBgUnlocks;    public bool[] IsBgUnlocks {get => isBgUnlocks; set => isBgUnlocks = value;}
    [SerializeField] string[] bgNames;   public string[] BgNames {get => bgNames; set => bgNames = value;}
    [SerializeField] Button[] bgBtns;        public Button[] BgBtns {get => bgBtns;}
    [SerializeField] Image[] bgImgs;    public Image[] BgImgs {get => bgImgs;}
    [SerializeField] TextMeshProUGUI[] bgLimitLvTxts;     public TextMeshProUGUI[] BgLimitLvTxts {get => bgLimitLvTxts;}

    // void Awake() {
        // isBgUnlocks = new bool[3];
    // }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public List<Button> init(int limitLvIdx1, int limitLvIdx2, int limitLvIdx3) {
        List<Button> lockBtnList = new List<Button>();

        const int FIRST = 0, SECOND = 1, THIRD = 2;
        int LV = DB.Dt.Lv;

        isBgUnlocks[FIRST] = LV >= limitLvIdx1;
        isBgUnlocks[SECOND] = LV >= limitLvIdx2;
        isBgUnlocks[THIRD] = LV >= limitLvIdx3;

        //* 処理
        bgBtns[FIRST].interactable = isBgUnlocks[FIRST];
        bgLimitLvTxts[FIRST].gameObject.SetActive(!isBgUnlocks[FIRST]);

        bgBtns[SECOND].interactable = isBgUnlocks[SECOND];
        bgLimitLvTxts[SECOND].gameObject.SetActive(!isBgUnlocks[SECOND]);

        bgBtns[THIRD].interactable = isBgUnlocks[THIRD];
        bgLimitLvTxts[THIRD].gameObject.SetActive(!isBgUnlocks[THIRD]);

        //* 返す(ロックBGリスト集める ➡ グレーグラデーションするため)
        if(!isBgUnlocks[FIRST]) lockBtnList.Add(bgBtns[FIRST]);
        if(!isBgUnlocks[SECOND]) lockBtnList.Add(bgBtns[SECOND]);
        if(!isBgUnlocks[THIRD]) lockBtnList.Add(bgBtns[THIRD]);

        Debug.Log($"WorldMapManager:: init({limitLvIdx1}, {limitLvIdx2}, {limitLvIdx3}):: " + 
            "bgBtns[FIRST].interactable =" + bgBtns[FIRST].interactable + 
            ", bgBtns[SECOND].interactable =" + bgBtns[SECOND].interactable + 
            ", bgBtns[THIRD].interactable =" + bgBtns[THIRD].interactable
        );

        return lockBtnList;
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region ANIM
///---------------------------------------------------------------------------------------------------------------------------------------------------
#endregion
    public IEnumerator coPlayBounce() {
        //* Value
        bool isDecreasing = false;
        const float originSc = 1.0f;
        float spd = 1.5f * Time.deltaTime;
        const float maxSc = 1.2f;

        //* Scale Bounce
        while(true) {
            if(!isDecreasing) {
                transform.localScale = new Vector2(transform.localScale.x + spd, transform.localScale.y + spd);
                if(transform.localScale.x > maxSc) isDecreasing = true;
            }
            else {
                transform.localScale = new Vector2(transform.localScale.x - spd, transform.localScale.y - spd);
                if(transform.localScale.x <= originSc) break;
            }
            yield return null;
        }
    }
#endregion
}
