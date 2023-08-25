using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchieveManager : MonoBehaviour {
    public enum ID {
        CorrectAnswerCnt,
        SkinCnt,
        PetCnt,
        CoinAmount,
    }

    //* Achieve
    [SerializeField] Achieve[] achieves;    public Achieve[] Achieves {get => achieves;}
    [SerializeField] GameObject[] notifyIcons;

    void Start() => StartCoroutine(coUpdateData());
    IEnumerator coUpdateData() {
        //* データアップデート 及び お知らせアイコン付く
        while(true) {
            bool isActiveNotify = Array.Exists(achieves, acv => acv.updateLvAndStatusGauge());
            Array.ForEach(notifyIcons, icon => icon.SetActive(isActiveNotify));
            yield return Util.time1;
        }
    }
}
