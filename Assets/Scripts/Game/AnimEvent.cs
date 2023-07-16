using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimEvent : MonoBehaviour
{
    //* GCDFrame
    [SerializeField] TextMeshProUGUI GCD_TitleTxt;
    [SerializeField] TextMeshProUGUI GCD_ContentTxt;

    public void setTextMeshPro(string str) {
        Debug.Log("SIBAL");
        GCD_TitleTxt.text = str;
    }
}
