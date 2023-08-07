using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomObject : MonoBehaviour {
    const int REVERSE_Y = -1, OFFSET_Z = -1;
    const float DRAG_MIN_X = -2;
    const float DRAG_MAX_X = 2;
    const float DRAG_MIN_Y = -4;
    const float DRAG_MAX_Y = 1;
    [SerializeField] float pivotOffsetHalfY;
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr;}
    [SerializeField] bool isSelect; public bool IsSelect {get => isSelect; set => isSelect = value;}
    [SerializeField] Transform sitSpot;  public Transform SitSpot {get => sitSpot;}
    [SerializeField] int tbDecoAreaSortingAddVal = 0;   public int TbDecoAreaSortingAddVal {get => tbDecoAreaSortingAddVal; set => tbDecoAreaSortingAddVal = value;}

    public void Start() {
        // if(HM._.state == HM.STATE.DECORATION_MODE) return;
        sr = GetComponent<SpriteRenderer>();
        pivotOffsetHalfY = sr.bounds.size.y * 0.5f;

        if(sr.sortingOrder == 100) {
            Debug.Log($"RoomObject.cs:: Start():: Create Deco Mode: {this.name} sortingOrder = 100");
            return;
        }
        

        //* レイヤー
        setSortingOrderByPosY();
        Debug.Log($"BBB Spr割り当て:: {sr.gameObject.name}.sortingOrder= {sr.sortingOrder})");
    }
//---------------------------------------------------------------------------------------------------------------
#region MOUSE EVENT
//---------------------------------------------------------------------------------------------------------------
    private void OnMouseDown() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;

        Debug.Log($"OnMouseDown::");
        //* 既に選択されたオブジェクトが有ったら、他が選択できないように
        RoomObject[] roomObjs = HM._.roomObjectGroup.GetComponentsInChildren<RoomObject>();
        bool isExistSelectedObj = Array.Exists(roomObjs, obj => obj.IsSelect);

        if(isExistSelectedObj) {
            //* もし選択して他の物選択したら、調整ターゲット変更
            if(HM._.fUI.CurSelectedObj != Array.Find(roomObjs, obj => obj.IsSelect)) {
                selectObj();
            }
        }
        else if(!isSelect && !isExistSelectedObj) {
            selectObj();
        }
    }
    private void OnMouseDrag() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;
        if(!isSelect) return;

        Debug.Log($"OnMouseDrag::");
        HM._.ui.DecorateModePanel.SetActive(false);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float centerY = transform.position.y - pivotOffsetHalfY;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, DRAG_MIN_X, DRAG_MAX_X),
            Mathf.Clamp(centerY, DRAG_MIN_Y, DRAG_MAX_Y),
            OFFSET_Z
        );
        setSortingOrderByPosY();
    }
    private void OnMouseUp() {
        if(HM._.state != HM.STATE.DECORATION_MODE) return;
        if(!isSelect) return;

        HM._.fUI.saveFunitureItemPos();

        Debug.Log("OnMouseUp");
        HM._.ui.DecorateModePanel.SetActive(true);
    }
#endregion
//---------------------------------------------------------------------------------------------------------------
#region FUNC
//---------------------------------------------------------------------------------------------------------------
    private void selectObj() {
        HM._.fUI.CurSelectedObj = this.gameObject;
        HM._.fUI.BefPos = this.gameObject.transform.position;
        StartCoroutine(coPlayItemBounceAnim());
    }
    public void setSortingOrderByPosY(bool backBefPos = false) {
        Debug.Log($"setSortingOrderByPosY():: this.name= {this.name}");
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if(backBefPos) transform.position = new Vector3(HM._.fUI.BefPos.x, HM._.fUI.BefPos.y, 0);
        sr = GetComponent<SpriteRenderer>();
        if(this.CompareTag(Enum.FUNITURE_CATE.Decoration.ToString())) {
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * REVERSE_Y + tbDecoAreaSortingAddVal;
        }
        else {
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y) * REVERSE_Y;
        }
    }
#endregion
//---------------------------------------------------------------------------------------------------------------
#region ANIM
//---------------------------------------------------------------------------------------------------------------
    IEnumerator coPlayItemBounceAnim() {
        // float ORG_SC_X = transform.localScale.x;
        // float ORG_SC_Y = transform.localScale.y;
        // const float MAX_SC = 1.5f;
        // const float DURATION = 0.1f; // アニメー再生時間

        //* 他のオブジェクトは 初期化
        RoomObject[] roomObjs = HM._.roomObjectGroup.GetComponentsInChildren<RoomObject>();
        Array.ForEach(roomObjs, obj => {
            if(obj.Sr.sortingLayerName == Enum.SORTING_LAYER.Mat.ToString()
            || obj.Sr.sortingLayerName == Enum.SORTING_LAYER.Default.ToString()){
                HM._.ui.DecorateModePanel.SetActive(false);
                obj.IsSelect = false;
                obj.Sr.material = HM._.sprUnlitMt;
            }
        });

        //* ドラッグ 操作 ON
        HM._.ui.DecorateModePanel.SetActive(true);
        this.IsSelect = true;
        this.sr.material = HM._.outlineAnimMt;

        //* スケール増加 アニメー
        // float elapsedTime = 0.0f;
        // while (elapsedTime < DURATION) {
        //     float time = elapsedTime / DURATION; // 経過時間の比率
        //     float scaleFactor = Mathf.Lerp(1.0f, MAX_SC, time);

        //     transform.localScale = new Vector2(ORG_SC_X * scaleFactor, ORG_SC_Y * scaleFactor);

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        // //* スケール減衰 アニメー
        // elapsedTime = 0.0f;
        // while (elapsedTime < DURATION) {
        //     float time = elapsedTime / DURATION; // 経過時間の比率
        //     float scaleFactor = Mathf.Lerp(MAX_SC, 1.0f, time);

        //     transform.localScale = new Vector2(ORG_SC_X * scaleFactor, ORG_SC_Y * scaleFactor);

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        yield return null;

        //* 最後のフレームで、元のサイズに戻す
        // transform.localScale = new Vector2(ORG_SC_X, ORG_SC_Y);
    }
#endregion
}

