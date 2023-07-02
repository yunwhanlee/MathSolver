using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxObj : MonoBehaviour {
    int value;
    [SerializeField] TextMeshProUGUI valueTxt;
    [SerializeField] Image objImg;  public Image ObjImg {get => objImg; set => objImg = value;}

    void Start() {
        value = 0;

    }
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
#endregion
//-------------------------------------------------------------------------------------------------------------
#region COLLIDER
//-------------------------------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag(Enum.TAG.Obj.ToString())) {
            value++;
            valueTxt.text = value.ToString();
            Destroy(col.gameObject);
        }
    }
#endregion
}
