using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    const float CHASE_DELAY = 0.5f;
    Vector2 targetPos;
    [SerializeField] float moveSpeed;

    void Update() {
        //* プレイヤー隣
        targetPos = new Vector2(GM._.pl.transform.position.x + 1, GM._.pl.transform.position.y - 0.5f);

        if(targetPos.x != transform.position.x
        || targetPos.y != transform.position.y) {
            transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }
}
