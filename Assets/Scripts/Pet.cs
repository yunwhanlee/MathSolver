using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour {
    const int FRONT_Z = -1;
    const float CHASE_DELAY = 0.5f;
    const float MIN_X = -2.5f, MAX_X = 2.5f, MIN_Y = -3.5f, MAX_Y = 2;
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

        //* プレイヤー 追いかける
        float x = Mathf.Clamp((plTf.position.x + 1), MIN_X, MAX_X);
        float y = Mathf.Clamp((plTf.position.y - 0.5f), MIN_Y, MAX_Y);
        targetPos = new Vector2(x, y);

        if(targetPos.x != tf.position.x || targetPos.y != tf.position.y) {
            tf.position = Vector2.Lerp(tf.position, targetPos, moveSpeed * Time.deltaTime);
            tf.position = new Vector3(tf.position.x, tf.position.y, FRONT_Z);
        }
    }
}
