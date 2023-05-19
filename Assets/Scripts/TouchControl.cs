using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    const float MAX_DISTANCE = 15f;

    void Update(){
        if(Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, transform.forward, MAX_DISTANCE);
            if(hit) {
                Debug.Log($"hit.name= {hit.transform.name}, mouseWorldPos= {mouseWorldPos}");
                if(hit.transform.name == "IconArea") return;

                GM._.pl.TargetPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            }
            Debug.DrawRay(mouseWorldPos, transform.forward * 50, Color.red, 0.3f);
        }
    }
}
