using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour {
    const int FRONT_Z = -1;
    const float CHASE_DELAY = 0.5f;
    Transform tf;
    Transform plTf;
    Vector2 targetPos;
    [SerializeField] SpriteRenderer sr; public SpriteRenderer Sr {get => sr;}
    [SerializeField] float moveSpeed;

    void Start() {
        tf = transform;
        plTf = HM._.pl.transform;
    }
    void Update() {
        //* レイヤー
        const int REVERSE_Y = -1;
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(tf.position.y) * REVERSE_Y;

        //* プレイヤー隣
        targetPos = new Vector2(plTf.position.x + 1, plTf.position.y - 0.5f);

        if(targetPos.x != tf.position.x || targetPos.y != tf.position.y) {
            tf.position = Vector2.Lerp(tf.position, targetPos, moveSpeed * Time.deltaTime);
            tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);
        }
    }
}
