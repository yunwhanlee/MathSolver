using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib.Samples;

public class GM : MonoBehaviour
{
    public static GM _;
    public GUI gui;
    public QuestionSO qstSO;

    [Header("CHARA")]
    [SerializeField] GameObject plThinkingEFObj; public GameObject PlThinkingEFObj {get => plThinkingEFObj; set => plThinkingEFObj = value;}

    [Header("ANIM")]
    [SerializeField] Animator playerAnim;   public Animator PlayerAnim {get => playerAnim; set => playerAnim = value;}
    [SerializeField] Animator customerAnim; public Animator CustomerAnim {get => customerAnim; set => customerAnim = value;}
    [SerializeField] Animator successEFAnim; public Animator SuccessEFAnim {get => successEFAnim; set => successEFAnim = value;}

    [Header("CHARA SPRITE")]
    [SerializeField] Sprite[] playerSprs; public Sprite[] PlayerSprs {get => playerSprs; set => playerSprs = value;}
    [SerializeField] Sprite[] customerSprs; public Sprite[] CustomerSprs {get => customerSprs; set => customerSprs = value;}
    [SerializeField] SpriteRenderer playerSprRdr;   public SpriteRenderer PlayerSprRdr {get => playerSprRdr; set => playerSprRdr = value;}
    [SerializeField] SpriteRenderer customerSprRdr;   public SpriteRenderer CustomerSprRdr {get => customerSprRdr; set => customerSprRdr = value;}

    [Header("BG SPRITE")]
    [SerializeField] Sprite[] cloud1Sprs; public Sprite[] Cloud1Sprs {get => cloud1Sprs; set => cloud1Sprs = value;}
    [SerializeField] Sprite[] cloud2Sprs; public Sprite[] Cloud2Sprs {get => cloud2Sprs; set => cloud2Sprs = value;}
    [SerializeField] Sprite[] sunSprs; public Sprite[] SunSprs {get => sunSprs; set => sunSprs = value;}
    [SerializeField] SpriteRenderer cloud1ExpressSprRdr; public SpriteRenderer Cloud1ExpressSprRdr {get => cloud1ExpressSprRdr; set => cloud1ExpressSprRdr = value;}
    [SerializeField] SpriteRenderer cloud2ExpressSprRdr; public SpriteRenderer Cloud2ExpressSprRdr {get => cloud2ExpressSprRdr; set => cloud2ExpressSprRdr = value;}
    [SerializeField] SpriteRenderer sunExpressSprRdr; public SpriteRenderer SunExpressSprRdr {get => sunExpressSprRdr; set => sunExpressSprRdr = value;}

    [SerializeField] Transform stuffGroupTf;  public Transform StuffGroupTf {get => stuffGroupTf; set => stuffGroupTf = value;}

    [SerializeField] Problem[]  problems;   public Problem[] Problems {get => problems;}
    [SerializeField] GameObject stuffObjPf; public GameObject StuffObjPf {get => stuffObjPf; set => stuffObjPf = value;}


    void Awake() {
        _ = this;
        problems[0].sentence = $"<sprite name=apple>{problems[0].n1}개를 친구 {problems[0].n2}마리에게 똑같이 나눠주고 싶어요, 몇 개씩 줘야하죠?";
        //* Anim
        successEFAnim.gameObject.SetActive(false);

        //* Chara Expression
        playerSprRdr.sprite = playerSprs[(int)Enum.EXPRESSION.Idle];
        customerSprRdr.sprite = customerSprs[(int)Enum.EXPRESSION.Idle];

        //* BG Expression
        cloud1ExpressSprRdr.sprite = null;
        cloud2ExpressSprRdr.sprite = null;
        sunExpressSprRdr.sprite = null;

        //* 問題出し
        // StartCoroutine(myCo());
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    IEnumerator myCo() {
        yield return gui.coShowStageTxt(0);
        yield return gui.coShowQuestion("");
        yield return coCreateStuffObj(2, 1);
    }
    public IEnumerator coCreateStuffObj(int n1, int n2) {
        int cnt = 50 / 10;
        for(int i = 0; i < cnt; i++){
            yield return new WaitForSeconds(0.1f);
            var stuff = Instantiate(stuffObjPf, stuffGroupTf);
            stuff.transform.position = new Vector2(0, 5);
        }
    }
    public void rigidPopStuffObjs() {
        for(int i = 0; i < stuffGroupTf.childCount; i++) {
            var rigid = stuffGroupTf.GetChild(i).GetComponent<Rigidbody2D>();
            rigid.constraints = RigidbodyConstraints2D.None;

            const int POWER = 25;
            float rX = Random.Range(-1.0f, 1.0f);
            Debug.Log("rX=>" + rX);
            var dir = new Vector2(rX, 1);
            rigid.AddForce(dir * POWER, ForceMode2D.Impulse);
            float rot = Random.Range(-30.0f, 30.0f);
            Debug.Log("rot=>" + rot);
            rigid.AddTorque(rot, ForceMode2D.Impulse);
        }
    }
#endregion
}
