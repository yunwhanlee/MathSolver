using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] bool isSelect; public bool IsSelect {get => isSelect; set => isSelect = value;}
    [SerializeField] RectTransform funitureModeCanvasRectTf; public RectTransform FunitureModeCanvasRectTf {get => funitureModeCanvasRectTf; set => funitureModeCanvasRectTf = value;}
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
