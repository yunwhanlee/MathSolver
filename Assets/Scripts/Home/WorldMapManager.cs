using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class WorldMapManager : MonoBehaviour {
    const int FIRST = 0, SECOND = 1, THIRD = 2;
    const int OUTLINE_FRAME_SIZE = 850;
    const int INNER_FRAME_SIZE = 700;

    [SerializeField] Sprite normalEdgeSpr, goldenEdgeSpr;
    [SerializeField] Color normalFontClr, goldenFontClr;
    [SerializeField] Canvas worldMapCanvas;
    [SerializeField] Map[] maps;    public Map[] Maps {get => maps;}
    [SerializeField] List<UnityAction> onMapPopUpActionList;

    private void Start() {
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
            Debug.Log($"lockBtnList : {btn.name}");
            var btnClr = btn.colors;
            float subVal = i * SUB_UNIT;
            float val = DEF_CLR - subVal;
            btnClr.disabledColor = new Color(val, val, val);
        });

        //* Check UnLockTrigger
        //* Map 1
        if(m1.IsBgUnlocks[SECOND] && !DB.Dt.IsMap1BG2Trigger) {
            DB.Dt.IsMap1BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgBtns[SECOND], m1.BgNames[SECOND]));
        }
        if(m1.IsBgUnlocks[THIRD] && !DB.Dt.IsMap1BG3Trigger) {
            DB.Dt.IsMap1BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgBtns[THIRD], m1.BgNames[THIRD]));
        }
        //* Map 2
        if(m2.IsBgUnlocks[FIRST] && !DB.Dt.IsMap2BG1Trigger) {
            DB.Dt.IsMap2BG1Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[FIRST], m2.MapName, true));
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[FIRST], m2.BgNames[FIRST]));
        }
        if(m2.IsBgUnlocks[SECOND] && !DB.Dt.IsMap2BG2Trigger) {
            DB.Dt.IsMap2BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[SECOND], m2.BgNames[SECOND]));
        }
        if(m2.IsBgUnlocks[THIRD] && !DB.Dt.IsMap2BG3Trigger) {
            DB.Dt.IsMap2BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgBtns[THIRD], m2.BgNames[THIRD]));
        }
        //* Map 3
        if(m3.IsBgUnlocks[FIRST] && !DB.Dt.IsMap3BG1Trigger) {
            DB.Dt.IsMap3BG1Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[FIRST], m3.MapName, true));
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[FIRST], m3.BgNames[FIRST]));
        }
        if(m3.IsBgUnlocks[SECOND] && !DB.Dt.IsMap3BG2Trigger) {
            DB.Dt.IsMap3BG2Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[SECOND], m3.BgNames[SECOND]));
        }
        if(m3.IsBgUnlocks[THIRD] && !DB.Dt.IsMap3BG3Trigger) {
            DB.Dt.IsMap3BG3Trigger = true;
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgBtns[THIRD], m3.BgNames[THIRD]));
        }

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
    private void displayUnlockPopUp(Button btn, string name, bool isMapUnlock = false) {
        Debug.Log($"displayUnlockPopUp({name}):: btn= {btn.name}");
        const int HIGHLIGHTEF = 1;
        Time.timeScale = 0;
        btn.GetComponent<Animator>().SetTrigger(Enum.ANIM.DoFirstActive.ToString());
        btn.transform.GetChild(HIGHLIGHTEF).gameObject.SetActive(true);
        HM._.ui.MapUnlockPopUp.SetActive(true);

        HM._.ui.MapUnlockPopUpNameTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpCttTxt.color = isMapUnlock? goldenFontClr : normalFontClr;
        HM._.ui.MapUnlockPopUpNameTxt.text = name;
        HM._.ui.MapUnlockPopUpCttTxt.text = isMapUnlock? "Found a new map!" : "New place open!";

        HM._.ui.MapImageOutlineFrame.GetComponent<Image>().sprite = isMapUnlock? goldenEdgeSpr : normalEdgeSpr;
        HM._.ui.MapImageOutlineFrame.sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE, OUTLINE_FRAME_SIZE) : new Vector2(INNER_FRAME_SIZE, INNER_FRAME_SIZE);
        HM._.ui.MapImageOutlineFrame.GetChild(0).GetComponent<RectTransform>().sizeDelta = isMapUnlock? new Vector2(OUTLINE_FRAME_SIZE - 50, OUTLINE_FRAME_SIZE - 50) : new Vector2(INNER_FRAME_SIZE - 50, INNER_FRAME_SIZE - 50);
    }

#endregion
}
