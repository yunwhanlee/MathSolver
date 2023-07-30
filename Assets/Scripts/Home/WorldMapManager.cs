using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class WorldMapManager : MonoBehaviour {
    const int FIRST = 0, SECOND = 1, THIRD = 2;

    [SerializeField] Canvas worldMapCanvas;
    [SerializeField] Map[] maps;
    [SerializeField] List<UnityAction> onMapPopUpActionList;

    private void Start() {
        var m1 = maps[0];
        var m2 = maps[1];
        var m3 = maps[2];
        onMapPopUpActionList = new List<UnityAction>();

        //* マップ 初期化
        m1.init(1, 4, 7);
        m2.init(10, 14, 17);
        m3.init(20, 24, 27);

        //* Check UnLockTrigger
        //* Map 1
        if(m1.IsBgUnlocks[SECOND] && !DB.Dt.IsMap1BG2Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgNames[SECOND]));
        }
        if(m1.IsBgUnlocks[THIRD] && !DB.Dt.IsMap1BG3Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m1.BgNames[THIRD]));
        }
        //* Map 2
        if(m2.IsBgUnlocks[FIRST] && !DB.Dt.IsMap2BG1Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgNames[FIRST]));
        }
        if(m2.IsBgUnlocks[SECOND] && !DB.Dt.IsMap2BG2Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgNames[SECOND]));
        }
        if(m2.IsBgUnlocks[THIRD] && !DB.Dt.IsMap2BG3Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgNames[THIRD]));
        }
        //* Map 3
        if(m3.IsBgUnlocks[FIRST] && !DB.Dt.IsMap3BG1Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgNames[FIRST]));
        }
        if(m3.IsBgUnlocks[SECOND] && !DB.Dt.IsMap3BG2Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m2.BgNames[SECOND]));
        }
        if(m3.IsBgUnlocks[THIRD] && !DB.Dt.IsMap3BG3Trigger) {
            onMapPopUpActionList.Add(() => displayUnlockPopUp(m3.BgNames[THIRD]));
        }

        if(onMapPopUpActionList.Count > 0) {
            onMapPopUpActionList[0].Invoke();
            onMapPopUpActionList.RemoveAt(0);
        }
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickClosePopUpBtn() {
        HM._.ui.MapUnlockPopUp.SetActive(false);

        if(onMapPopUpActionList.Count > 0) {
            HM._.ui.MapUnlockPopUp.SetActive(true);
            onMapPopUpActionList[0].Invoke();
            onMapPopUpActionList.RemoveAt(0);
        }
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private void displayUnlockPopUp(string name) {
        Debug.Log($"displayUnlockPopUp({name})::");
        HM._.ui.MapUnlockPopUp.SetActive(true);
        HM._.ui.MapUnlockPopUpNameTxt.text = name;
    }

#endregion
}
