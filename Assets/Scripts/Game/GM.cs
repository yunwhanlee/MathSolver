using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib.Samples;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

public class GM : MonoBehaviour {
    const int BOX_S_MAX = 10; // Small
    const int BOX_M_MAX = 50; // Medium
    const int MOX_L_MAX = 100; // Large
    const float OBJ_RAND_RANGE_X = 0.2f;
    const float BOX_SPAWN_Y = 6.0f;
    const float OBJ_SPAWN_Y = 4.0f;
    const float RESULT_PANEL_START_DIST = 3.0f;
    const float RESULT_PANEL_PET_DANCE_POS_X = 1.75f;


    public static GM _;
    public GUI gui; // Game UI Manager
    public GEM gem; // Game Effect Manager
    public QuizManager qm;
    public QuestionSO qstSO;

    [Header("ACTION")]
    [SerializeField] UnityAction onAnswerObjAction; public UnityAction OnAnswerObjAction {get => onAnswerObjAction; set => onAnswerObjAction = value;}
    [SerializeField] UnityAction<int> onAnswerBoxAction;  public UnityAction<int> OnAnswerBoxAction {get => onAnswerBoxAction; set => onAnswerBoxAction = value;}

    [Header("VALUE")]
    [SerializeField] bool isSelectCorrectAnswer;    public bool IsSelectCorrectAnswer {get => isSelectCorrectAnswer; set => isSelectCorrectAnswer = value;}
    [SerializeField] int rewardExp;     public int RewardExp {get => rewardExp; set => rewardExp = value;}
    [SerializeField] int rewardCoin;    public int RewardCoin {get => rewardCoin; set => rewardCoin = value;}

    [Header("WORLD SPACE")]
    [SerializeField] GameObject worldSpaceQuizGroup;
    [SerializeField] GameObject worldSpaceResultGroup;

    [Header("Spot")]
    [SerializeField] Transform plSpot;
    [SerializeField] Transform petSpot;
    [SerializeField] Transform resPlSpot;
    [SerializeField] Transform resPetSpot;
    
    [Header("ANIMAL")]
    [SerializeField] Animal anm; public Animal Anm {get => anm;}
    [Header("LOAD OBJECT FROM HOME")]
    [SerializeField] Player pl; public Player Pl {get => pl; set => pl = value;}
    [SerializeField] Pet pet; public Pet Pet {get => pet; set => pet = value;}

    [SerializeField] GameObject plThinkingEFObj; public GameObject PlThinkingEFObj {get => plThinkingEFObj; set => plThinkingEFObj = value;}

    [Header("ANIM")]
    [SerializeField] Animator customerAnim; public Animator CustomerAnim {get => customerAnim; set => customerAnim = value;}
    [SerializeField] Animator successEFAnim; public Animator SuccessEFAnim {get => successEFAnim; set => successEFAnim = value;}

    [Header("CHARA SPRITE")]
    [SerializeField] SpriteRenderer customerSprRdr;   public SpriteRenderer CustomerSprRdr {get => customerSprRdr; set => customerSprRdr = value;}

    [Header("BG SPRITE")]
    [SerializeField] GameObject cloud1; public GameObject Cloud1 {get => cloud1; set => cloud1 = value;}
    [SerializeField] GameObject cloud2; public GameObject Cloud2 {get => cloud2; set => cloud2 = value;}

    [Header("OBJ & BOX")]
    [SerializeField] GameObject twoArmsBalanceObj;  public GameObject WwoArmsBalanceObj {get => twoArmsBalanceObj; set => twoArmsBalanceObj = value;}
    [SerializeField] Sprite[] objSprs;  public Sprite[] ObjSprs {get => objSprs; set => objSprs = value;}
    [SerializeField] Transform objGroupTf;  public Transform ObjGroupTf {get => objGroupTf; set => objGroupTf = value;}
    [SerializeField] GameObject objPf; public GameObject ObjPf {get => objPf; set => objPf = value;}
    [SerializeField] GameObject boxPf; public GameObject BoxPf {get => boxPf; set => boxPf = value;}

    void Awake() {
        _ = this;
        gui = FindObjectOfType<GUI>();
        gem = FindObjectOfType<GEM>();
        qm = FindObjectOfType<QuizManager>();

        //* Anim
        successEFAnim.gameObject.SetActive(false);
    }

    void Start() {
        rewardExp = 0;
        rewardCoin = 0;
        
        //* 曇り移動
        StartCoroutine(coUpdateCloudMoving());

        if(DB._) {
            Debug.Log($"GM:: Start():: pl= {pl}, pet= {pet}");
            //* Load Player From HOME
            const int START_LEFT_POS_X = -3;
            pl = DB._.transform.GetChild(0).GetComponent<Player>();
            pl.transform.gameObject.SetActive(true);

            //* Set Parent DB → plSpot 
            pl.transform.SetParent(plSpot);
            //* Move To plSpot Pos
            pl.transform.position = new Vector2(plSpot.position.x + START_LEFT_POS_X, plSpot.position.y);
            pl.TgPos = plSpot.position;

            //* Load Pet From HOME
            pet = DB._.transform.GetChild(0).GetComponent<Pet>();
            pet.transform.gameObject.SetActive(true);

            //* Set Parent DB → petSpot 
            pet.transform.SetParent(petSpot);
            //* Move To plSpot Pos
            pet.transform.position = new Vector2(petSpot.position.x - 5, petSpot.position.y);
            pet.IsChasePlayer = false;
            pet.TgPos = petSpot.position;
        }
    }

    void Update() {
        //* TEST : QuizPanel -> Result Panel
        if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(coSetResultPanelReward());
            StartCoroutine(coSetResultPanelObj());
        }
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    IEnumerator coSetResultPanelReward() {
        bool isExpUP = true;
        bool isCoinUP = true;
        int expVal = 0;
        int coinVal = 0;

        while(isExpUP || isCoinUP) {
            if(expVal < rewardExp) GM._.gui.ExpTxt.text = $"+{++expVal}";
            else    isExpUP = false;

            if(coinVal < rewardCoin) GM._.gui.CoinTxt.text = $"+{++coinVal}";
            else    isCoinUP = false;

            yield return Util.time0_01;
        }
    }
    IEnumerator coSetResultPanelObj() {
        bool isIncreasing = false;

        //* Off
        worldSpaceQuizGroup.SetActive(false);
        gui.QuizPanel.SetActive(false);

        //* On
        worldSpaceResultGroup.SetActive(true);
        gui.ResultPanel.SetActive(true);

        pl.transform.SetParent(resPlSpot);
        pl.transform.position = new Vector2(resPlSpot.position.x - RESULT_PANEL_START_DIST, resPlSpot.position.y);
        pl.TgPos = resPlSpot.position;
        pet.transform.SetParent(resPetSpot);
        pet.transform.position = new Vector2(resPetSpot.position.x + RESULT_PANEL_START_DIST, resPetSpot.position.y);
        pet.TgPos = resPetSpot.position;

        yield return Util.time1;
        pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pl.transform));
        pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pet.transform));

        while(true) {
            if(!isIncreasing) {
                yield return Util.time2; yield return Util.time1;
                pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
                StartCoroutine(Util.coPlayBounceAnim(pl.transform));
                
                pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
                pet.TgPos = new Vector2(-RESULT_PANEL_PET_DANCE_POS_X, resPetSpot.position.y);
                isIncreasing = true;
            }
            else {
                yield return Util.time2; yield return Util.time1;
                pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
                StartCoroutine(Util.coPlayBounceAnim(pl.transform));

                pet.Anim.SetTrigger(Enum.ANIM.DoDance.ToString());
                pet.TgPos = new Vector2(RESULT_PANEL_PET_DANCE_POS_X, resPetSpot.position.y);
                isIncreasing = false;
            }
        }

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

    /// <summary>
    ///* オブジェクト 生成
    /// </summary>
    /// <param name="isFinish">オブジェクト生成終了</param>
    private IEnumerator coCreateObj(string objName, int num, float posX, bool isFinish = true) {
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
        //* 次にオブジェクト生成がなかったら、選択ボタン 表示
        if(isFinish) {
            yield return Util.time1;
            GM._.qm.interactableAnswerBtns(true);
        }
    }

    private IEnumerator coCreateQuestionMarkBox(string objName, int num, float posX) {
        //! (BUG) coCreateObj()で「？BOX 」が出る前に「Answerボタン」が活性化になる
        yield return coCreateObj(objName, num, posX, isFinish: false);
        yield return Util.time0_3;

        //* 「?」Box
        BoxObj box = instBox(objName, posX);
        // yield return Util.time0_3;
        box.ValueTxt.text = "?";
        box.ValueTxt.color = Color.magenta;
        box.ValueTxt.fontStyle = FontStyles.Bold;

        yield return Util.time1;
        GM._.qm.interactableAnswerBtns(true);
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
    public void charaAnimByAnswer(bool isCorret) {
        Debug.Log($"playObjAnimByAnswer(isCorret= {isCorret})");

        //* Effect
        plThinkingEFObj.SetActive(false);

        //* Player Anim
        pl.Anim.SetTrigger(isCorret? Enum.ANIM.DoSuccess.ToString() : Enum.ANIM.DoFail.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pl.transform));

        //* Pet Anim
        int rand = Random.Range(0, 2);
        string petSuccessAnim = (rand == 0)? Enum.ANIM.DoSuccess.ToString() : Enum.ANIM.DoDance.ToString();
        pet.Anim.SetTrigger(isCorret? petSuccessAnim : Enum.ANIM.DoFail.ToString());
        StartCoroutine(Util.coPlayBounceAnim(pet.transform));

        //* Animal Anim
        anm.Anim.SetTrigger(isCorret? Enum.ANIM.DoSuccess.ToString() : Enum.ANIM.DoFail.ToString());
        StartCoroutine(Util.coPlayBounceAnim(anm.transform));
    }

    private IEnumerator coUpdateCloudMoving() {
        float moveSpeed = 0.25f * Time.deltaTime;
        while(true) {
            //* Cloud1
            if(cloud1.transform.position.x <= 7)
                cloud1.transform.Translate(Vector2.right * moveSpeed);
            else 
                cloud1.transform.position = new Vector2(-7, cloud1.transform.position.y);
            //* Cloud2
            if(cloud2.transform.position.x >= -5)
                cloud2.transform.Translate(Vector2.left * moveSpeed * 0.5f);
            else 
                cloud2.transform.position = new Vector2(5, cloud2.transform.position.y);
            yield return Util.time0_025;
        }
    }
#endregion
}
