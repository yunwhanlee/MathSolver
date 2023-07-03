using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxObj : MonoBehaviour {
    [SerializeField] int val;     public int Val {get => val; set => val = value;}
    [SerializeField] bool isBlockMerge;  public bool IsBlockMerge {get => isBlockMerge; set => isBlockMerge = value;}
    [SerializeField] TextMeshProUGUI valueTxt;  public TextMeshProUGUI ValueTxt {get => valueTxt; set => valueTxt = value;}
    [SerializeField] Image objImg;  public Image ObjImg {get => objImg; set => objImg = value;}

    void Start() {
        val = 0;
    }
    void Update() {
        if(valueTxt.text == "?") return;
        if(valueTxt.text.Contains("+")) return;
        if(valueTxt.text.Contains("minus") || valueTxt.text.Contains("-")) return;

        valueTxt.text = val.ToString();
    }
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------
#region COLLIDER
//-------------------------------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col) {
        if(!isBlockMerge && col.gameObject.CompareTag(Enum.TAG.Obj.ToString())) {
            val++;
            Destroy(col.gameObject);
        }
    }
#endregion
}
