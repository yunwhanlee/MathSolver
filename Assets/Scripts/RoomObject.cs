using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomObject : MonoBehaviour {
    const int REVERSE_Y = -1, 
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
            tf.position = new Vector3(tf.position.x, tf.position.y, 0);
            var prSr = tf.parent.GetComponentInParent<SpriteRenderer>();
            sr.sortingOrder = prSr.sortingOrder;
            Debug.Log($"SORTING BB RoomObject:: isDecoration:: {sr.gameObject.name}.sortingOrder({sr.sortingOrder}) = {prSr.gameObject.name}.sortingOrder({prSr.sortingOrder})");
        }
        else {
            setSortingOrderByPosY();
            Debug.Log($"SORTING BB RoomObject:: {sr.gameObject.name}.sortingOrder= {sr.sortingOrder})");
        }
    }
//---------------------------------------------------------------------------------------------------------------
#region MOUSE EVENT
//---------------------------------------------------------------------------------------------------------------
    private void OnMouseDown() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;

        //* 既に選択されたオブジェクトが有ったら、他が選択できないように
        RoomObject[] roomObjs = HM._.roomObjectGroup.GetComponentsInChildren<RoomObject>();
        bool isExistSelectedObj = Array.Exists(roomObjs, obj => obj.IsSelect);

        if(!isSelect && !isExistSelectedObj) {
            StartCoroutine(coPlayItemBounceAnim());
        }
        Debug.Log("OnMouseDown");
    }
    private void OnMouseDrag() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;
        if(!isSelect) return;
        Debug.Log("OnMouseDrag");
        funitureModeCanvasRectTf.gameObject.SetActive(false);
        tf.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tf.position = new Vector3(tf.position.x, tf.position.y + PIVOT_OFFSET_Y, OFFSET_Z);
    }
    private void OnMouseUp() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;
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
        setSortingOrderByPosY();
        funitureModeCanvasRectTf.gameObject.SetActive(false);
        isSelect = false;

        //* Z値 ０に戻す
        tf.position = new Vector3(tf.position.x, tf.position.y, 0);
        //* アウトライン 消す
        sr.material = HM._.sprUnlitMt;

        //* タッチの動き
        HM._.touchCtr.enabled = true;
        HM._.pl.enabled = true;
    }
#endregion
//---------------------------------------------------------------------------------------------------------------
#region FUNC
//---------------------------------------------------------------------------------------------------------------
    private void setSortingOrderByPosY() {
        tf.position = new Vector3(tf.position.x, tf.position.y, 0);
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;
    }
#endregion
//---------------------------------------------------------------------------------------------------------------
#region ANIM
//---------------------------------------------------------------------------------------------------------------
    IEnumerator coPlayItemBounceAnim() {
        float ORG_SC_X = tf.localScale.x;
        float ORG_SC_Y = tf.localScale.y;
        const float MAX_SC = 1.3f;
        const float DURATION = 0.1f; // アニメー再生時間

        //* 他のオブジェクトは 初期化
        RoomObject[] roomObjs = HM._.roomObjectGroup.GetComponentsInChildren<RoomObject>();
        Array.ForEach(roomObjs, obj => {
            if(obj.Sr.sortingLayerName == Enum.SORTINGLAYER.Mat.ToString()
            || obj.Sr.sortingLayerName == Enum.SORTINGLAYER.Default.ToString()){
                obj.FunitureModeCanvasRectTf.gameObject.SetActive(false);
                obj.IsSelect = false;
                obj.Sr.material = HM._.sprUnlitMt;
            }
        });

        //* スケール増加 アニメー
        float elapsedTime = 0.0f;
        while (elapsedTime < DURATION) {
            float time = elapsedTime / DURATION; // 経過時間の比率
            float scaleFactor = Mathf.Lerp(1.0f, MAX_SC, time);

            tf.localScale = new Vector2(ORG_SC_X * scaleFactor, ORG_SC_Y * scaleFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //* スケール減衰 アニメー
        elapsedTime = 0.0f;
        while (elapsedTime < DURATION) {
            float time = elapsedTime / DURATION; // 経過時間の比率
            float scaleFactor = Mathf.Lerp(MAX_SC, 1.0f, time);

            tf.localScale = new Vector2(ORG_SC_X * scaleFactor, ORG_SC_Y * scaleFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //* 最後のフレームで、元のサイズに戻す
        tf.localScale = new Vector2(ORG_SC_X, ORG_SC_Y);
        
        //* ドラッグ操作 ON
        this.FunitureModeCanvasRectTf.gameObject.SetActive(true);
        this.IsSelect = true;
        this.sr.material = HM._.outlineAnimMt;
    }
#endregion
}

