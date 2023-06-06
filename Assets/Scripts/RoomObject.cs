using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour {
    const int DECARATION_FRONT = -1, 
        REVERSE_Y = -1, 
        PIVOT_OFFSET_Y = -1, 
        OFFSET_Z = -1;
    Transform tf;
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr;}
    [SerializeField] bool isSelect; public bool IsSelect {get => isSelect; set => isSelect = value;}
    [SerializeField] bool isDecoration; public bool IsDecoration  {get => isDecoration; set => isDecoration = value;}
    [SerializeField] RectTransform funitureModeCanvasRectTf; public RectTransform FunitureModeCanvasRectTf {get => funitureModeCanvasRectTf; set => funitureModeCanvasRectTf = value;}

    void Start() {
        tf = this.transform;
        sr = GetComponent<SpriteRenderer>();

        //* レイヤー
        if(HM._.state == HM.STATE.DECORATION_MODE) return;

        if(isDecoration) {
            tf.position = new Vector3(tf.position.x, tf.position.y, DECARATION_FRONT); //* Z値 -1にして目の前に
            var prSr = tf.parent.GetComponentInParent<SpriteRenderer>();
            sr.sortingOrder = prSr.sortingOrder;
            Debug.Log($"SORTING BB RoomObject:: isDecoration:: {sr.gameObject.name}.sortingOrder({sr.sortingOrder}) = {prSr.gameObject.name}.sortingOrder({prSr.sortingOrder})");
        }
        else {
            tf.position = new Vector3(tf.position.x, tf.position.y, 0);
            sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;
            Debug.Log($"SORTING BB RoomObject:: {sr.gameObject.name}.sortingOrder= {sr.sortingOrder})");
        }
    }
//---------------------------------------------------------------------------------------------------------------
#region MOUSE EVENT
//---------------------------------------------------------------------------------------------------------------
    private void OnMouseDown() {
        if(!isSelect) return;
        Debug.Log("OnMouseDown");
    }
    private void OnMouseDrag() {
        if(!isSelect) return;
        Debug.Log("OnMouseDrag");
        funitureModeCanvasRectTf.gameObject.SetActive(false);
        tf.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tf.position = new Vector3(tf.position.x, tf.position.y + PIVOT_OFFSET_Y, OFFSET_Z);
    }
    private void OnMouseUp() {
        if(!isSelect) return;
        Debug.Log("OnMouseUp");

        funitureModeCanvasRectTf.gameObject.SetActive(true);
    }
#endregion

//---------------------------------------------------------------------------------------------------------------
#region CLICK EVENT
//---------------------------------------------------------------------------------------------------------------
    public void onClickFunitureModeItemCloseBtn() {
        HM._.ui.onClickDecorateModeCloseBtn();
        Destroy(this.gameObject);
    }
    public void onClickFunitureModeItemFlatBtn() {
        float sx = tf.localScale.x * -1;
        tf.localScale = new Vector2(sx, 1);
        funitureModeCanvasRectTf.localScale = new Vector2(sx, 1);
    }
    public void onClickFunitureModeItemSetUpBtn() {
        HM._.ui.onClickDecorateModeCloseBtn();
        this.GetComponent<SpriteRenderer>().sortingOrder = 2;
        funitureModeCanvasRectTf.gameObject.SetActive(false);
        isSelect = false;

        //* Z値 ０に戻す
        tf.position = new Vector3(tf.position.x, tf.position.y, 0);

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;
    }
#endregion
}
