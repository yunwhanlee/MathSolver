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
    const int BOX_S_MAX = 10; // Small
    const int BOX_M_MAX = 50; // Medium
    const int MOX_L_MAX = 100; // Large
    const float OBJ_RAND_RANGE_X = 0.2f;
    const float BOX_SPAWN_Y = 6.0f;
    const float OBJ_SPAWN_Y = 4.0f;


    public static GM _;
    public GUI gui;
    public QuizManager qm;
    public QuestionSO qstSO;

    [SerializeField] UnityAction onAnswerObjAction; public UnityAction OnAnswerObjAction {get => onAnswerObjAction; set => onAnswerObjAction = value;}
    [SerializeField] UnityAction<int> onAnswerBoxAction;  public UnityAction<int> OnAnswerBoxAction {get => onAnswerBoxAction; set => onAnswerBoxAction = value;}

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
    [SerializeField] GameObject twoArmsBalanceObj;  public GameObject WwoArmsBalanceObj {get => twoArmsBalanceObj; set => twoArmsBalanceObj = value;}
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
    public void createQuestionMarkBox(string objName, int num, float posX) {
        StartCoroutine(coCreateQuestionMarkBox(objName, num, posX));
    }
    public void createExtraOprBox(string opr, string objName, int num, float posX) {
        StartCoroutine(coCreateExtraOprBox(opr, objName, num, posX));
    }
    public void showQuestionMarkAnswerBox(int answer) {
        Debug.Log($"showQuestionMarkAnswerBox(answer= {answer})::");
        for(int i = 0; i < GM._.ObjGroupTf.childCount; i++) {
            var questionMarkBox = GM._.ObjGroupTf.GetChild(i).GetComponent<BoxObj>();
            if(questionMarkBox.ValueTxt.text == "?") {
                //* ? ➝ 正解
                questionMarkBox.ValueTxt.text = answer.ToString();
                questionMarkBox.Val = answer;
                break;
            }
        }
    }
    public void addObj(string objName, int befNum, int num) {
        StartCoroutine(coAddObj(objName, befNum, num));
    }
    public void substractObj(int num) {
        StartCoroutine(coSubstractObj(num));
    }
    public void multiplyObj(string objName, int befNum, int num) {
        int val = (befNum * num) - befNum;
        StartCoroutine(coAddObj(objName, befNum, val));
    }
    public void divideObj(string objName, int befNum, int num) {
        StartCoroutine(coDivideObj(objName, befNum, num));
    }
    public void greatestCommonDivisorObj(string objName, int befNum, int gcd, float posX) {
        StartCoroutine(coGreatestCommonDivisorObj(objName, befNum, gcd, posX));
    }

    private IEnumerator coCreateObj(string objName, int num, float posX) {
        int boxCnt = num / BOX_S_MAX;
        for(int i = 0; i < num; i++) {
            Debug.Log($"coCreateObj:: i= {i}, num = {num}");
            if(i < boxCnt * BOX_S_MAX) {
                i += BOX_S_MAX - 1;
                BoxObj box = instBox(objName, posX);
                yield return Util.time0_3;
                box.Val = BOX_S_MAX;
            }
            else {
                if(i % BOX_S_MAX == 0) {
                    instBox(objName, posX);
                    yield return Util.time0_8;
                }
                instObj(objName, posX);
                yield return Util.time0_05;
            }
        }
    }

    private IEnumerator coCreateQuestionMarkBox(string objName, int num, float posX) {
        yield return coCreateObj(objName, num, posX);
        yield return Util.time0_3;

        //* 「?」Box
        BoxObj box = instBox(objName, posX);
        yield return Util.time0_3;
        box.ValueTxt.text = "?";
        box.ValueTxt.color = Color.magenta;
        box.ValueTxt.fontStyle = FontStyles.Bold;
    }


    private IEnumerator coCreateExtraOprBox(string opr, string objName, int num, float posX) {
        Debug.Log($"coCreateExtraOprBox(opr= {opr}, objName= {objName}, num= {num}, posX= {posX})::");
        yield return Util.time0_8;
        BoxObj box = instBox(objName, posX);
        box.IsBlockMerge = true;
        box.ValueTxt.text = $"{opr}{num}";
        box.ValueTxt.color = (opr == "+")? Color.blue : Color.red;
    }

    private IEnumerator coAddObj(string objName, int befNum, int num) {
        int boxCnt = (num + befNum) / BOX_S_MAX;
        int lastBoxIdx = GM._.ObjGroupTf.childCount - 1;
        int lastBoxVal = GM._.ObjGroupTf.GetChild(lastBoxIdx).GetComponent<BoxObj>().Val;
        bool isNotEnoughTen = (lastBoxVal % BOX_S_MAX != 0);
        int remainVal = BOX_S_MAX - lastBoxVal;
        Debug.Log($"lastBoxVal= {lastBoxVal}, lastBoxVal % 10 = {lastBoxVal % BOX_S_MAX}, remainVal= {remainVal}");
        
        for(int i = befNum; i < num + befNum; i++) {
            if(isNotEnoughTen && i < befNum + remainVal) {
                instObj(objName);
                yield return Util.time0_05;
            }
            else if(i < boxCnt * BOX_S_MAX) {
                i += BOX_S_MAX - 1;
                BoxObj box = instBox(objName);
                yield return Util.time0_3;
                box.Val = BOX_S_MAX;
            }
            else {
                if(i % BOX_S_MAX == 0) {
                    instBox(objName);
                    yield return Util.time0_8;
                }
                instObj(objName);
                yield return Util.time0_05;
            }
        }
    }

    private IEnumerator coSubstractObj(int num) {
        for(int i = 0; i < num; i++) {
            int lastIdx = GM._.ObjGroupTf.childCount - 1;
            var lastBox = GM._.ObjGroupTf.GetChild(lastIdx).GetComponent<BoxObj>();
            lastBox.Val--;
            yield return Util.time0_025;
            if(lastBox.Val <= 0) {
                DestroyImmediate(lastBox.gameObject);
            }
        }
    }

    private IEnumerator coDivideObj(string objName, int befNum, int num) {
        int val = befNum / num;
        int rest = befNum % num;

        //* 以前の物 削除
        foreach (Transform child in GM._.ObjGroupTf) {
            Destroy(child.gameObject);
        }
        yield return null; //* 破壊するまで、1フレーム 待機
        
        //TODO EFFECT

        //* ボックス
        for(int i = 0; i < val; i++) {
            BoxObj box = instBox(objName);
            box.IsBlockMerge = true;
            yield return Util.time0_3;
            box.Val = num;
        }

        //* 残り
        yield return Util.time0_5;
        for(int i = 0; i < rest; i++) {
            instObj(objName);
            yield return Util.time0_05;
        }
    }

    private IEnumerator coGreatestCommonDivisorObj(string objName, int befNum, int gcd, float posX) {
        int val = befNum / gcd;
        int rest = befNum % gcd;
        // Debug.Log($"coGreatestCommonDivisorObj({objName}, {befNum}, {gcd}, {posX}):: val= {befNum}, rest= {rest}");

        //* 以前の物 削除
        foreach (Transform child in GM._.ObjGroupTf) {
            Destroy(child.gameObject);
        }
        yield return null; //* 破壊するまで、1フレーム 待機
        
        //TODO EFFECT

        //* ボックス
        for(int i = 0; i < gcd; i++) {
            BoxObj box = instBox(objName, posX);
            box.IsBlockMerge = true;
            yield return Util.time0_3;
            box.Val = val;
            Debug.Log($"coGreatestCommonDivisorObj:: i({i}) < gcd({gcd}), val= {box.Val}");
        }

        //* 残り
        yield return Util.time0_5;
        for(int i = 0; i < rest; i++) {
            instObj(objName, posX);
            yield return Util.time0_05;
        }
    }

    private BoxObj instBox(string objName, float posX = 0) {
            BoxObj box = Instantiate(GM._.BoxPf, GM._.ObjGroupTf).GetComponent<BoxObj>();
            
            box.transform.position = new Vector2(posX, BOX_SPAWN_Y);
            box.ObjImg.sprite = getObjSprite(objName);
            return box;
    }

    private void instObj(string objName, float posX = 0) {
        var obj = Instantiate(GM._.ObjPf, GM._.ObjGroupTf);
        float randX = Random.Range(-OBJ_RAND_RANGE_X + posX, OBJ_RAND_RANGE_X + posX);
        obj.transform.position = new Vector2(randX, OBJ_SPAWN_Y);
        obj.GetComponent<SpriteRenderer>().sprite = getObjSprite(objName);
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
