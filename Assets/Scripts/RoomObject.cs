using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    const int DECARATION_FRONT = -1, REVERSE_Y = -1;
    Transform tf;
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr;}
    [SerializeField] bool isSelect; public bool IsSelect {get => isSelect; set => isSelect = value;}
    [SerializeField] bool isDecoration; public bool IsDecoration  {get => isDecoration; set => isDecoration = value;}
    [SerializeField] RectTransform funitureModeCanvasRectTf; public RectTransform FunitureModeCanvasRectTf {get => funitureModeCanvasRectTf; set => funitureModeCanvasRectTf = value;}

    void Start() {
        tf = this.transform;

        //* レイヤー
        if(isDecoration) {
            tf.position = new Vector3(tf.position.x, tf.position.y, DECARATION_FRONT); //* Z値 -1にして目の前に
            var prSr = GetComponentInParent<SpriteRenderer>();
            sr.sortingOrder = prSr.sortingOrder;
        }
        else {
            tf.position = new Vector3(tf.position.x, tf.position.y, 0);
            sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * REVERSE_Y;
        }
    }
//---------------------------------------------------------------------------------------------------------------
#region MOUSE EVENT
//---------------------------------------------------------------------------------------------------------------
    private void OnMouseDown() {
        Debug.Log("OnMouseDown");
        if(!isSelect) return;
    }
    private void OnMouseDrag() {
        Debug.Log("OnMouseDrag");
        if(!isSelect) return;

        funitureModeCanvasRectTf.gameObject.SetActive(false);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
        
    }
    private void OnMouseUp() {
        Debug.Log("OnMouseUp");
        if(!isSelect) return;

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
        float sx = transform.localScale.x * -1;
        transform.localScale = new Vector2(sx, 1);
        funitureModeCanvasRectTf.localScale = new Vector2(sx, 1);
    }
    public void onClickFunitureModeItemSetUpBtn() {
        HM._.ui.onClickDecorateModeCloseBtn();
        this.GetComponent<SpriteRenderer>().sortingOrder = 2;
        funitureModeCanvasRectTf.gameObject.SetActive(false);
        isSelect = false;

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;
    }
#endregion
}
