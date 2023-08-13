using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class WorldMapManager : MonoBehaviour {
    //* Scroll Pos
    const int START_POS = -1920;

    const int FIRST = 0, SECOND = 1, THIRD = 2;
    const int OUTLINE_FRAME_SIZE = 850;
    const int INNER_FRAME_SIZE = 700;
    [SerializeField] RectTransform scrollViewCtt;   public RectTransform ScrollViewCtt {get => scrollViewCtt;}
    [SerializeField] Sprite normalEdgeSpr, goldenEdgeSpr;
    [SerializeField] Color normalFontClr, goldenFontClr;
    [SerializeField] Map[] maps;    public Map[] Maps {get => maps;}
    [SerializeField] List<UnityAction> onMapPopUpActionList;

    void OnEnable() => init();

    public void init() {
        Debug.Log("OnEnable() WorldMapManager:: ");
        var m1 = maps[0];
        var m2 = maps[1];
        var m3 = maps[2];
        onMapPopUpActionList = new List<UnityAction>();

        List<Button> lockBtnList = new List<Button>();

        //* マップ 初期化
        lockBtnList.AddRange(m1.init(1, 4, 7));
        lockBtnList.AddRange(m2.init(10, 14, 17));
        lockBtnList.AddRange(m3.init(20, 24, 27));

        //* LockBtnList グレーグラデーション処理
        const float DEF_CLR = 0.5f;
        const float SUB_UNIT = 0.05f;
        int i = 0;
        lockBtnList.ForEach(btn => {
            var btnClr = btn.colors;
            float subVal = i++ * SUB_UNIT;
            float val = DEF_CLR - subVal;
            btnClr.disabledColor = new Color(val, val, val);
            Debug.Log($"lockBtnList : {btn.name}.disabledColor= {val}");
            btn.colors = btnClr; //* 適用
        });

        #region UNLOCK MAP1
        //* Windmill
        if(m1.IsBgUnlocks[SECOND] && !DB.Dt.IsMap1BG2Trigger) {
            DB.Dt.IsMap1BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgBtns[SECOND], m1.BgNames[SECOND]));
            onMapPopUpActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Orchard + Minigame1
        if(m1.IsBgUnlocks[THIRD] && !DB.Dt.IsMap1BG3Trigger) {
            DB.Dt.IsMap1BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgBtns[THIRD], m1.BgNames[THIRD]));
            onMapPopUpActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_MINIGAME));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion
        #region UNLOCK MAP2
        //* Jungle Open & Swamp
        if(m2.IsBgUnlocks[FIRST] && !DB.Dt.IsMap2BG1Trigger) {
            DB.Dt.IsMap2BG1Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[FIRST], m2.MapName, true));
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[FIRST], m2.BgNames[FIRST]));
            onMapPopUpActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Bush
        if(m2.IsBgUnlocks[SECOND] && !DB.Dt.IsMap2BG2Trigger) {
            DB.Dt.IsMap2BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[SECOND], m2.BgNames[SECOND]));
            onMapPopUpActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* MokeyWat + Minigame2
        if(m2.IsBgUnlocks[THIRD] && !DB.Dt.IsMap2BG3Trigger) {
            DB.Dt.IsMap2BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[THIRD], m2.BgNames[THIRD]));
            onMapPopUpActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_MINIGAME));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion
        #region UNLOCK MAP3
        //* Tundra Open & Entrance of tundra
        if(m3.IsBgUnlocks[FIRST] && !DB.Dt.IsMap3BG1Trigger) {
            DB.Dt.IsMap3BG1Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[FIRST], m3.MapName, true));
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[FIRST], m3.BgNames[FIRST]));
            onMapPopUpActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Snow mountain
        if(m3.IsBgUnlocks[SECOND] && !DB.Dt.IsMap3BG2Trigger) {
            DB.Dt.IsMap3BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[SECOND], m3.BgNames[SECOND]));
            onMapPopUpActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Ice Dragon + Minigame3
        if(m3.IsBgUnlocks[THIRD] && !DB.Dt.IsMap3BG3Trigger) {
            DB.Dt.IsMap3BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[THIRD], m3.BgNames[THIRD]));
            onMapPopUpActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_MINIGAME));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion

        //* アクション 読込
        callbackMapPopUpAction();
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickClosePopUpBtn() {
        Time.timeScale = 1;
        HM._.ui.MapUnlockPopUp.SetActive(false);

        //* 次のアクション 読込
        callbackMapPopUpAction();
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private void callbackMapPopUpAction() {
        if(onMapPopUpActionList.Count > 0) {
            onMapPopUpActionList[0].Invoke();
            onMapPopUpActionList.RemoveAt(0);
        }
    }
    public void displayUnlockPopUp(Button btn, string name, bool isMapUnlock = false) {
        Debug.Log($"displayUnlockPopUp(btn == null? {btn == null}, {name}):: ");
        const int HIGHLIGHTEF = 1;
        Time.timeScale = 0;
        if(btn) btn.GetComponent<Animator>().SetTrigger(Enum.ANIM.DoFirstActive.ToString());
        if(btn) {
            Transform highLightEFTf = Array.Find(btn.GetComponentsInChildren<Transform>(true), tf => tf.name == "HighlightEF");
            highLightEFTf.gameObject.SetActive(true);
        }
        // }btn.transform.GetChild(HIGHLIGHTEF).gameObject.SetActive(true);
        HM._.ui.MapUnlockPopUp.SetActive(true);

        HM._.ui.MapUnlockPopUpNameTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpCttTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpNameTxt.text = name;
        HM._.ui.MapUnlockPopUpCttTxt.text = 
            (name == Enum.MAP.Minigame1.ToString())? "Take the Falling Apples!"
            : (name == Enum.MAP.Minigame2.ToString())? "TODO"
            : (name == Enum.MAP.Minigame3.ToString())? "TODO"
            : isMapUnlock? "Found a new map!"
            : !isMapUnlock? "New place open!"
            : null;

        //* Unlock Map or Bg or Minigame
        HM._.ui.MapUnlockImg.sprite = 
            (name == Enum.MAP.Jungle.ToString())? HM._.wmm.Maps[1].MapSpr
            : (name == Enum.MAP.Tundra.ToString())? HM._.wmm.Maps[2].MapSpr
            : (name == Enum.MAP.Minigame1.ToString())? HM._.wmm.Maps[0].MiniGameSpr
            : (name == Enum.MAP.Minigame2.ToString())? HM._.wmm.Maps[1].MiniGameSpr
            : (name == Enum.MAP.Minigame3.ToString())? HM._.wmm.Maps[2].MiniGameSpr
            : btn.GetComponent<Image>().sprite;

        HM._.ui.MapImageOutlineFrame.GetComponent<Image>().sprite = isMapUnlock? goldenEdgeSpr : normalEdgeSpr;
        HM._.ui.MapImageOutlineFrame.sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE, OUTLINE_FRAME_SIZE) : new Vector2(INNER_FRAME_SIZE, INNER_FRAME_SIZE);
        HM._.ui.MapImageOutlineFrame.GetChild(0).GetComponent<RectTransform>().sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE - 50, OUTLINE_FRAME_SIZE - 50) : new Vector2(INNER_FRAME_SIZE - 50, INNER_FRAME_SIZE - 50);
    }

    public void showUnlockBg(int mapIdx, int bgIdx) {
        //* ワールドマップ 表示 → 自動で、Enabled():: init():: callbackMapPopUpAction() 実行
        gameObject.SetActive(true);

        // HM._.ui.TopGroup.SetActive(true);
        // HM._.ui.AchiveRankPanel.SetActive(false);

        //* SetScrollPos
        var bg = maps[mapIdx].BgBtns[bgIdx];
        var limitLvTxt = maps[mapIdx].BgLimitLvTxts[bgIdx];
        Debug.Log($"setScrollPos({mapIdx}, {bgIdx}):: {bg.name}");

        limitLvTxt.text = "<color=yellow>NEW!</color>";
        //* ↑方向：⊖値
        float posY = START_POS - bg.GetComponent<RectTransform>().localPosition.y;
        scrollViewCtt.anchoredPosition = new Vector2(0, posY);
    }

    public Map getMap(int idx) {
        int res = (idx == (int)Enum.MAP.Minigame1)? (int)Enum.MAP.Forest
        : (idx == (int)Enum.MAP.Minigame2)? (int)Enum.MAP.Jungle
        : (idx == (int)Enum.MAP.Minigame3)? (int)Enum.MAP.Tundra
        : idx;
        return maps[res];
    }

#endregion
}
