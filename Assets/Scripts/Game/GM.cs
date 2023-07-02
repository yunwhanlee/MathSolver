using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib.Samples;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

public class GM : MonoBehaviour
{
    public static GM _;
    public GUI gui;
    public QuizManager qm;
    public QuestionSO qstSO;

    [SerializeField] UnityAction onAnswerObjAction; public UnityAction OnAnswerObjAction {get => onAnswerObjAction; set => onAnswerObjAction = value;}

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

    [Header("OBJ & BOX")]
    [SerializeField] Sprite[] objSprs;  public Sprite[] ObjSprs {get => objSprs; set => objSprs = value;}
    [SerializeField] Transform objGroupTf;  public Transform ObjGroupTf {get => objGroupTf; set => objGroupTf = value;}
    [SerializeField] GameObject objPf; public GameObject ObjPf {get => objPf; set => objPf = value;}
    [SerializeField] GameObject boxPf; public GameObject BoxPf {get => boxPf; set => boxPf = value;}

    void Awake() {
        _ = this;
        gui = FindObjectOfType<GUI>();
        qm = FindObjectOfType<QuizManager>();

        //* Anim
        successEFAnim.gameObject.SetActive(false);

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
    public void initObjSprite() {
        playerSprRdr.sprite = playerSprs[(int)Enum.EXPRESSION.Idle];
        customerSprRdr.sprite = customerSprs[(int)Enum.EXPRESSION.Idle];
    }
    // IEnumerator myCo() {
    //     yield return gui.coShowStageTxt(0);
    //     yield return gui.coShowQuestion("");
    //     yield return coCreateStuffObj(2, 1);
    // }
    // public IEnumerator coCreateStuffObj(int n1, int n2) {
    //     int cnt = 50 / 10;
    //     for(int i = 0; i < cnt; i++){
    //         yield return new WaitForSeconds(0.1f);
    //         var stuff = Instantiate(stuffObjPf, stuffGroupTf);
    //         stuff.transform.position = new Vector2(0, 5);
    //     }
    // }
    public Sprite getObjSprite(string name) {
        Sprite res = null;
        var enumObjIdx = System.Enum.GetValues(typeof(Enum.OBJ_SPR_IDX));
        foreach (var enumVal in enumObjIdx) {
            string enumValStr = enumVal.ToString().ToLower();
            if (enumValStr == name) {
                Debug.Log($"getObjSpriteIndex({name}):: {enumValStr} == {name} -> {enumVal.ToString() == name}");
                int idx = (int)enumVal;
                res =  GM._.ObjSprs[idx];
            }
        }
        return res;
    }

    public void createObj(string objName, int num, float posX = 0) 
        => StartCoroutine(coCreateObj(objName, num, posX));

    private IEnumerator coCreateObj(string objName, int num, float posX) {
        for(int i = 0; i < num; i++) {
            if(i % 10 == 0) {
                BoxObj box = Instantiate(GM._.BoxPf, GM._.ObjGroupTf).GetComponent<BoxObj>();
                box.transform.position = new Vector2(posX, 4);
                box.ObjImg.sprite = getObjSprite(objName);
                yield return new WaitForSeconds(0.8f);
            }
            yield return new WaitForSeconds(0.05f);
            var obj = Instantiate(GM._.ObjPf, GM._.ObjGroupTf);
            float randX = Random.Range(-0.2f + posX, 0.2f + posX);
            obj.transform.position = new Vector2(randX, 2);
            obj.GetComponent<SpriteRenderer>().sprite = getObjSprite(objName);
        }
    }

    public void addObj(string objName, int befNum, int num)
        => StartCoroutine(coAddObj(objName, befNum, num));

    private IEnumerator coAddObj(string objName, int befNum, int num) {
        for(int i = befNum; i < num + befNum; i++) {
            if(i % 10 == 0) {
                BoxObj box = Instantiate(GM._.BoxPf, GM._.ObjGroupTf).GetComponent<BoxObj>();
                box.transform.position = new Vector2(0, 4);
                box.ObjImg.sprite = getObjSprite(objName);
                yield return new WaitForSeconds(0.8f);
            }
            yield return new WaitForSeconds(0.05f);
            var obj = Instantiate(GM._.ObjPf, GM._.ObjGroupTf);
            float randX = Random.Range(-0.2f, 0.2f);
            obj.transform.position = new Vector2(randX, 2);
            obj.GetComponent<SpriteRenderer>().sprite = getObjSprite(objName);
        }
    }

    public void substractObj(int num)
        => StartCoroutine(coSubstractObj(num));

    private IEnumerator coSubstractObj(int num) {
        for(int i = 0; i < num; i++) {
            int lastIdx = GM._.ObjGroupTf.childCount - 1;
            var lastBox = GM._.ObjGroupTf.GetChild(lastIdx).GetComponent<BoxObj>();
            lastBox.Val--;
            yield return new WaitForSeconds(0.05f);
            if(lastBox.Val <= 0) {
                DestroyImmediate(lastBox.gameObject);
            }
        }
    }

    public void multiplyObj(string objName, int befNum, int num) {
        int val = (befNum * num) - befNum;
        StartCoroutine(coAddObj(objName, befNum, val));
    }


    public void rigidPopStuffObjs() {
        for(int i = 0; i < objGroupTf.childCount; i++) {
            var rigid = objGroupTf.GetChild(i).GetComponent<Rigidbody2D>();
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
    public void playObjAnimByAnswer(bool isCorret) {
        Debug.Log($"playObjAnimByAnswer(isCorret= {isCorret})");
        const int SUCCESS = (int)Enum.EXPRESSION.Success;
        const int FAIL = (int)Enum.EXPRESSION.Fail;

        //* Effect
        plThinkingEFObj.SetActive(false);

        //* Anim
        playerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        customerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());

        //* Chara Sprite
        playerSprRdr.sprite = isCorret? playerSprs[SUCCESS] : playerSprs[FAIL];
        customerSprRdr.sprite = isCorret? customerSprs[SUCCESS] : customerSprs[FAIL];

        //* BG  Sprite
        cloud1ExpressSprRdr.sprite = isCorret? cloud1Sprs[SUCCESS] : cloud1Sprs[FAIL];
        cloud2ExpressSprRdr.sprite = isCorret? cloud2Sprs[SUCCESS] : cloud2Sprs[FAIL];
        sunExpressSprRdr.sprite = isCorret? sunSprs[SUCCESS] : sunSprs[FAIL];
    }
#endregion
}
