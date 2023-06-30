using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxObj : MonoBehaviour {
    int value;
    [SerializeField] TextMeshProUGUI valueTxt;
    [SerializeField] Image objImg;

    void Start() {
        value = 0;
        string objName = GM._.qstSO.Obj1Name;
        objImg.sprite = GM._.getObjSprite(objName);
    }
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
