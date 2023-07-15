using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDecoArea : MonoBehaviour {
//---------------------------------------------------------------------------------------------------------------
#region COLLIDE
//---------------------------------------------------------------------------------------------------------------
    void OnTriggerStay2D(Collider2D col) {
        //* プレイヤーがテーブルに置いている飾り物より前に表示
        if(col.CompareTag(Enum.TAG.Player.ToString())) {
            var pl = col.GetComponentInParent<Player>();
            var table = this.GetComponentInParent<RoomObject>();
            //* 実際のSortingLayerは下なので、Sorting Layer名で調整
            if(pl.Sr.sortingOrder == table.Sr.sortingOrder)
                pl.Sr.sortingLayerName = Enum.SORTING_LAYER.FrontDecoObj.ToString(); //
            else
                pl.Sr.sortingLayerName = Enum.SORTING_LAYER.Default.ToString(); // 
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.TableDecoArea.ToString())) {
            var decoObj = col.GetComponentInParent<RoomObject>();
            // decoObj.Sr.color = Color.red;
            decoObj.TbDecoAreaSortingAddVal = 2;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.TableDecoArea.ToString())) {
            var decoObj = col.GetComponentInParent<RoomObject>();
            // decoObj.Sr.color = Color.white;
            decoObj.TbDecoAreaSortingAddVal = 0;
        }
        //* Sorting Layer名 戻す
        if(col.CompareTag(Enum.TAG.Player.ToString())) {
            var pl = col?.GetComponentInParent<Player>();
            //* テーブルと衝突され、DECORATIONモードに入って、テーブルが消えたら、以下処理しない
            if(pl == null) return;
            pl.Sr.sortingLayerName = Enum.SORTING_LAYER.Default.ToString();
        }
    }
#endregion
}
