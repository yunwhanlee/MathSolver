using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimEvent : MonoBehaviour {   
    //! Runtimeのみ反応

    //* GCDFrame
    [SerializeField] TextMeshProUGUI GCD_ContentTxt;
    [SerializeField] TextMeshProUGUI GCD_Val1CommonDivisorTxt;
    [SerializeField] TextMeshProUGUI GCD_Val2CommonDivisorTxt;

    public void setGCD_ContentTxt(string str) {
        GCD_ContentTxt.text = str;
    }
    public void setGCD_Val1CommonDivisorTxt(string str) {
        GCD_Val1CommonDivisorTxt.text = str;
    }
    public void setGCD_Val2CommonDivisorTxt(string str) {
        GCD_Val2CommonDivisorTxt.text = str;
    }
}