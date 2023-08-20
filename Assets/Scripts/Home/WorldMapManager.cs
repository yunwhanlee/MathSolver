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
    [SerializeField] List<UnityAction> onActionList;    public List<UnityAction> OnActionList {get => onActionList; set => onActionList = value;}

    void OnEnable() => init();

    public void init() {
        Debug.Log("OnEnable() WorldMapManager:: ");
        var m1 = maps[0];
        var m2 = maps[1];
        var m3 = maps[2];
        onActionList = new List<UnityAction>();

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
        //* StarMountain Finish ➝ Windmill
        if(m1.IsBgUnlocks[SECOND] && !DB.Dt.IsMap1BG2Trigger) {
            DB.Dt.IsMap1BG2Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m1.BgBtns[SECOND], m1.BgNames[SECOND]));
            onActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Windmill ➝ Orchard + Minigame1
        if(m1.IsBgUnlocks[THIRD] && !DB.Dt.IsMap1BG3Trigger) {
            DB.Dt.IsMap1BG3Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m1.BgBtns[THIRD], m1.BgNames[THIRD]));
            onActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_MINIGAME));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion
        #region UNLOCK MAP2
        //* Orchard Finish ➝ Jungle Open & Swamp
        if(m2.IsBgUnlocks[FIRST] && !DB.Dt.IsMap2BG1Trigger) {
            DB.Dt.IsMap2BG1Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m2.BgBtns[FIRST], m2.MapName, true));
            onActionList.Add(() => displayMapUnlockPopUp(m2.BgBtns[FIRST], m2.BgNames[FIRST]));
            onActionList.Add(() => HM._.ui.OnDisplayNewItemPopUp()); //* Forest Finish : WoodenWolf Statue
            onActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Swamp Finish ➝ Bush
        if(m2.IsBgUnlocks[SECOND] && !DB.Dt.IsMap2BG2Trigger) {
            DB.Dt.IsMap2BG2Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m2.BgBtns[SECOND], m2.BgNames[SECOND]));
            onActionList.Add(() => HM._.ui.OnDisplayNewItemPopUp()); //* Swamp Finish : Frog Chair
            onActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Bush Finish ➝ MokeyWat + Minigame2
        if(m2.IsBgUnlocks[THIRD] && !DB.Dt.IsMap2BG3Trigger) {
            DB.Dt.IsMap2BG3Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m2.BgBtns[THIRD], m2.BgNames[THIRD]));
            // onMapPopUpActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_MINIGAME));
            onActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion
        #region UNLOCK MAP3
        //* MokeyWat Finish ➝ Tundra Open & Entrance of tundra
        if(m3.IsBgUnlocks[FIRST] && !DB.Dt.IsMap3BG1Trigger) {
            DB.Dt.IsMap3BG1Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m3.BgBtns[FIRST], m3.MapName, true));
            onActionList.Add(() => displayMapUnlockPopUp(m3.BgBtns[FIRST], m3.BgNames[FIRST]));
            onActionList.Add(() => HM._.ui.OnDisplayNewItemPopUp()); //* Jungle Finish : GoldenMonkey Statue
            onActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Entrance of tundra Finish ➝ Snow mountain
        if(m3.IsBgUnlocks[SECOND] && !DB.Dt.IsMap3BG2Trigger) {
            DB.Dt.IsMap3BG2Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m3.BgBtns[SECOND], m3.BgNames[SECOND]));
            onActionList.Add(() => HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn());
        }
        //* Snow mountain Finish ➝ Ice Dragon + Minigame3
        if(m3.IsBgUnlocks[THIRD] && !DB.Dt.IsMap3BG3Trigger) {
            DB.Dt.IsMap3BG3Trigger = true;
            onActionList.Add(() => displayMapUnlockPopUp(m3.BgBtns[THIRD], m3.BgNames[THIRD]));
            onActionList.Add(() => HM._.ui.OnDisplayNewItemPopUp()); //* Snow mountain Finish : IceDragon Statue
            onActionList.Add(() => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_MINIGAME));
            //? endSwitchProccess():: (int)ID.UNLOCK_MAP1_MINIGAMEで　MainQuests[MainQuestID].onClickAcceptBtn()実行
        }
        #endregion

        //* アクションリスト 開始
        callbackOnActionList();
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickMapUnlockPopUpCloseBtn() {
        Debug.Log("<color=red>onClickMapUnlockPopUpCloseBtn()::</color>");
        Time.timeScale = 1;
        HM._.ui.MapUnlockPopUp.SetActive(false);

        //* 次のアクション 読込
        callbackOnActionList();
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// (内容) UnityAction onActionListを通じて、順番通りメソッドを処理
    /// (登録) onActionList.Add(() => メソッド());
    /// (次番) 必ず前のメソッドが終わった最後に、この関数を呼び出すこと！
    /// </summary>
    public void callbackOnActionList() {
        if(onActionList == null) return;
        if(onActionList.Count <= 0) return;
        Debug.Log($"callbackOnActionList():: onActionList.Count= {onActionList.Count}");
        onActionList[0]?.Invoke();
        onActionList.RemoveAt(0);
    }
    public void displayMapUnlockPopUp(Button btn, string name, bool isMapUnlock = false) {
        Debug.Log($"displayMapUnlockPopUp(btn == null? {btn == null}, {name}):: ");
        Time.timeScale = 0;
        if(btn) btn.GetComponent<Animator>().SetTrigger(Enum.ANIM.DoFirstActive.ToString());
        if(btn) {
            Transform highLightEFTf = Array.Find(btn.GetComponentsInChildren<Transform>(true), tf => tf.name == "HighlightEF");
            highLightEFTf.gameObject.SetActive(true);
        }        
        HM._.ui.MapUnlockPopUp.SetActive(true);

        HM._.ui.MapUnlockPopUpNameTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpCttTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpNameTxt.text = LM._.localize(name);

        //* Content Text
        HM._.ui.MapUnlockPopUpCttTxt.text = 
            (name == Enum.MG.Minigame1.ToString())? LM._.localize(Config.MINIGAME1_TITLE)
            : (name == Enum.MG.Minigame2.ToString())? LM._.localize(Config.MINIGAME2_TITLE)
            : (name == Enum.MG.Minigame3.ToString())? LM._.localize(Config.MINIGAME3_TITLE)
            : isMapUnlock? LM._.localize("Found a new map!")
            : !isMapUnlock? LM._.localize("New place open!")
            : null;

        //* Sprite
        HM._.ui.MapUnlockImg.sprite = 
            (name == Enum.MAP.Jungle.ToString())? HM._.wmm.Maps[1].MapSpr
            : (name == Enum.MAP.Tundra.ToString())? HM._.wmm.Maps[2].MapSpr
            : (name == Enum.MG.Minigame1.ToString())? HM._.wmm.Maps[0].MiniGameSpr
            : (name == Enum.MG.Minigame2.ToString())? HM._.wmm.Maps[1].MiniGameSpr
            : (name == Enum.MG.Minigame3.ToString())? HM._.wmm.Maps[2].MiniGameSpr
            : btn.GetComponent<Image>().sprite;

        HM._.ui.MapImageOutlineFrame.GetComponent<Image>().sprite = isMapUnlock? goldenEdgeSpr : normalEdgeSpr;
        HM._.ui.MapImageOutlineFrame.sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE, OUTLINE_FRAME_SIZE) : new Vector2(INNER_FRAME_SIZE, INNER_FRAME_SIZE);
        HM._.ui.MapImageOutlineFrame.GetChild(0).GetComponent<RectTransform>().sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE - 50, OUTLINE_FRAME_SIZE - 50) : new Vector2(INNER_FRAME_SIZE - 50, INNER_FRAME_SIZE - 50);
    }

    public void showUnlockBg(int mapIdx, int bgIdx) {
        //* ワールドマップ 表示 → 自動で、Enabled():: init():: callbackMapPopUpAction() 実行
        gameObject.SetActive(true);

        HM._.ui.TopGroup.SetActive(true);
        HM._.ui.AchiveRankPanel.SetActive(false);

        //* Set Scroll PosY
        var map = maps[mapIdx]; 
        var bg = map.BgBtns[bgIdx];
        var limitLvTxt = map.BgLimitLvTxts[bgIdx];
        float parentAnchoredPosY = map.BgBtnGroup.GetComponent<RectTransform>().anchoredPosition.y;
        float childPosY = bg.GetComponent<RectTransform>().localPosition.y;

        limitLvTxt.text = "<color=yellow>NEW!</color>";
        //* スクロールを上がるには、⊖値代入
        Debug.Log($"<color=yellow>mapIdx({mapIdx}) scrollViewCtt.anchoredPosition= parentAnchoredPosY({parentAnchoredPosY}) + childPosY({childPosY})</color>");
        scrollViewCtt.anchoredPosition = new Vector2(0, (-parentAnchoredPosY) + (-childPosY));
        HM._.em.showEF((int)HEM.IDX.FunitureSetupEF, Vector3.zero, Util.time8);
    }

    public Map getMap(int idx) {
        int res = (idx == 0)? (int)Enum.MAP.Forest
        : (idx == 1)? (int)Enum.MAP.Jungle
        : (idx == 2)? (int)Enum.MAP.Tundra
        : idx;
        return maps[res];
    }

#endregion
}
