using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Collider2D col;

    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 targetPos; public Vector2 TargetPos {get => targetPos; set => targetPos = value;}
    void Start() {
        col = GetComponent<Collider2D>();
        targetPos = transform.position;
    }

    void Update() {
        if(targetPos.x != transform.position.x
        || targetPos.y != transform.position.y) {
            transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
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
