using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Collider2D col;

    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveTargetPos; public Vector2 MoveTargetPos {get => moveTargetPos; set => moveTargetPos = value;}
    void Start() {
        col = GetComponent<Collider2D>();

        moveTargetPos = transform.position;
    }

    void Update() {
        if(moveTargetPos.x != transform.position.x
        || moveTargetPos.y != transform.position.y) {
            transform.position = Vector2.Lerp(transform.position, moveTargetPos, moveSpeed * Time.deltaTime);
        }
    }
}
