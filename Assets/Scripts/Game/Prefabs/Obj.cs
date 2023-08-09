using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{

    [SerializeField] Collider2D col;
    [SerializeField] Rigidbody2D rigid; public Rigidbody2D Rigid {get => rigid;}
    [SerializeField] SpriteRenderer sprRdr; public SpriteRenderer SprRdr {get => sprRdr;}
    [SerializeField] bool isDisappear;   public bool IsDisappear {get => isDisappear; set => isDisappear = value;}

    [SerializeField] Coroutine coroutineID = null;  public Coroutine CoroutineID {get => coroutineID; set => coroutineID = value;}

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void addForce(Vector2 dir) {
        // Debug.Log($"Obj:: addForce(dir= {dir})");
        int burstPower = Random.Range(250, 400);
        float power = burstPower * Time.fixedDeltaTime;
        rigid.AddForce(dir * power, ForceMode2D.Impulse);
    }
    public IEnumerator coDisappear() {
        float spd = 5 * Time.deltaTime;
        col.enabled = false;
        SpriteRenderer sprRdr = GetComponent<SpriteRenderer>();
        while(sprRdr.color.a > 0) {
            sprRdr.color = new Color(1,1,1,sprRdr.color.a - spd);
            yield return null;
        }
        Destroy(this);
    }
#endregion
}
