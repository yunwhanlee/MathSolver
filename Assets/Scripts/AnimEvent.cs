using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimEvent : MonoBehaviour {   
    //? PARAM 可能：int, float, string

    //* GCDFrame
    [SerializeField] TextMeshProUGUI GCD_ContentTxt;
    [SerializeField] TextMeshProUGUI GCD_Val1CommonDivisorTxt;
    [SerializeField] TextMeshProUGUI GCD_Val2CommonDivisorTxt;
    [SerializeField] Button btn;



    //* GAME
    public void setGCD_ContentTxt(string str) {
        GCD_ContentTxt.text = LM._.localize(str);
    }
    public void setGCD_Val1CommonDivisorTxt(string str) {
        GCD_Val1CommonDivisorTxt.text = str;
    }
    public void setGCD_Val2CommonDivisorTxt(string str) {
        GCD_Val2CommonDivisorTxt.text = str;
    }

    //* HOME
    public void setBtnInteractable(int active) {
        btn.interactable = (active == 0)? false : true;
    }

}