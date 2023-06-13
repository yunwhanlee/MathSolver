using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDecorateArea : MonoBehaviour {
    RoomObject tableObj;

    void Start() {
        tableObj = GetComponentInParent<RoomObject>();
    }

    void OnTriggerStay2D(Collider2D col) {
        if(col.CompareTag(Enum.FUNITURE_CATE.Decoration.ToString())) {
            Debug.Log($"TableDecorateArea:: OnTriggerEnter2D():: col.name= {col.name}, tag= {col.tag}");
            RoomObject decorationObj = col.GetComponent<RoomObject>();
            //* 飾りがターブル上にあったら、レイヤーをターブルと同じく修正
            if(decorationObj.Sr.sortingOrder != tableObj.Sr.sortingOrder)
                decorationObj.Sr.sortingOrder = tableObj.Sr.sortingOrder;
        }
    }
}
