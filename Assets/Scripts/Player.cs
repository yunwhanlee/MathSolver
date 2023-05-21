using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Collider2D col;
    public Animator anim;

    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 targetPos; public Vector2 TargetPos {get => targetPos; set => targetPos = value;}
    [SerializeField] bool doIdle;
    [SerializeField] bool doWalk;
    void Start() {
        col = GetComponent<Collider2D>();
        targetPos = transform.position;
    }

    void Update() {
        if(targetPos.x != transform.position.x
        || targetPos.y != transform.position.y) {
            float distX = Mathf.Abs(targetPos.x - transform.position.x);
            float distY = Mathf.Abs(targetPos.y - transform.position.y);
            
            const float WALK_STOP_VAL = 0.05f;
            if(distX < WALK_STOP_VAL && distY < WALK_STOP_VAL) {
                transform.position = targetPos;
                if(!doIdle) { doIdle = true; anim.SetTrigger(Enum.ANIM.DoIdle.ToString());}
                if(doWalk) doWalk = false;
            }
            else {
                transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
                if(doIdle) doIdle = false;
                if(!doWalk) { doWalk = true; anim.SetTrigger(Enum.ANIM.DoWalk.ToString());}
            }

            Debug.Log($"pl.transform.position -> distance x= {distX}, y= {distY}");
        }
    }

///------------------------------------------------------------------------------------------
#region Collider
///------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag(Enum.TAG.GoGame.ToString())) {
            HM._.ui.GoGameDialog.SetActive(true);
        }
    }
#endregion
}
