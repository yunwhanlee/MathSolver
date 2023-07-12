using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib.Samples;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;
using System.Text.RegularExpressions;

public class GM : MonoBehaviour {
    const int BOX_S_MAX = 10; // Small
    const int BOX_M_MAX = 50; // Medium
    const int MOX_L_MAX = 100; // Large
    const float OBJ_RAND_RANGE_X = 0.2f;
    const float BOX_SPAWN_Y = 6.0f;
    const float OBJ_SPAWN_Y = 4.0f;

    public static GM _;
    public Cam cam;
    public GUI gui; // Game UI Manager
    public GEM gem; // Game Effect Manager
    public QuizManager qm;
    public ResultManager rm;
    public GameTalkManager gtm;
    public QuestionSO qSO;

    [Header("ACTION")]
    [SerializeField] UnityAction onAnswerObjAction; public UnityAction OnAnswerObjAction {get => onAnswerObjAction; set => onAnswerObjAction = value;}
    [SerializeField] UnityAction<int> onAnswerBoxAction;  public UnityAction<int> OnAnswerBoxAction {get => onAnswerBoxAction; set => onAnswerBoxAction = value;}

    [Header("VALUE")]
    [SerializeField] bool isSelectCorrectAnswer;    public bool IsSelectCorrectAnswer {get => isSelectCorrectAnswer; set => isSelectCorrectAnswer = value;}
    
    [Header("WORLD SPACE")]
    [SerializeField] GameObject worldSpaceQuizGroup;    public GameObject WorldSpaceQuizGroup {get => worldSpaceQuizGroup;}

    [Header("Spot")]
    [SerializeField] Transform plSpot;
    [SerializeField] Transform petSpot;
    
    [Header("ANIMAL")]
    [SerializeField] Animal anm; public Animal Anm {get => anm;}
    [Header("LOAD OBJECT FROM HOME")]
    [SerializeField] Player pl; public Player Pl {get => pl; set => pl = value;}
    [SerializeField] Pet pet; public Pet Pet {get => pet; set => pet = value;}

    [SerializeField] GameObject plThinkingEFObj; public GameObject PlThinkingEFObj {get => plThinkingEFObj; set => plThinkingEFObj = value;}

    [Header("ANIM")]
    [SerializeField] Animator successEFAnim; public Animator SuccessEFAnim {get => successEFAnim; set => successEFAnim = value;}

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
        rm = FindObjectOfType<ResultManager>();
        gtm = FindObjectOfType<GameTalkManager>();

        //* Anim
        successEFAnim.gameObject.SetActive(false);
    }

    void Start() {
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
            StartCoroutine(GM._.rm.coDisplayResultPanel());
        }
    }

//-------------------------------------------------------------------------------------------------------------
#region QUESTION SO FUNC
//-------------------------------------------------------------------------------------------------------------
    public IEnumerator makeQuiz(List<string> analList) {
        List<string> leftSideList = new List<string>(); //* 左辺
        List<string> rightSideList = new List<string>(); //* 右辺
        string lOpr = null; //* 左演算子
        string rOpr = null; //* 右演算子
        List<string> lNums = new List<string>(); //* 左定数
        List<string> rNums = new List<string>(); //* 右定数
        
        //* X方程式か？
        bool isXEquation = analList.Exists(li => li == "x");
        if(isXEquation) analList.RemoveAll(str => str == "x");

        Debug.Log($"makeQuizSentence(analList.Cnt= {analList.Count}, isXEquation= {isXEquation})::");
        Debug.Log($"makeQuizSentence:: analList: <color=white>{string.Join(", ", analList.ToArray())}</color>");
        /*【 TYPE 分析 】
            ※ "="が有る：(横)、ない：(縦)。
            ※ (縦)は「X方程式」がない。
            ※ 「x, =, ?」：三つは分離してから削除。

            ② (縦)：38, -, 13, ? (引く)
            ③ (縦)：12, times, 4, ? (掛け)
            ⑤ (縦)：underline, 3, 9, ? || left, 8, 10, ? (最大公約数)
            ① (横)：4, +, 3, =, ? (足す)
            ④ (横)：frac, 12, 4, =, ? (分数) 
            ⑥ (横)：2, +, x, =, 8, 「x, =, ?」 (X方程式)
            ⑦ (横)：1, +, x, =, 8, minus, 4, 「x, =, ?」 (X方程式＋右式⊖定数)
            ⑧ (横)：4, +, x, =, 7, +, 1, 「x, =, ?」 (X方程式＋右式⊕定数)
        */

        //* (横)：左・右辺、(縦)：左辺のみ
        bool isHrzEqu = analList.Exists(li => li == "=");

        //* 左辺・右辺 分離
        int splitIdx = (isHrzEqu)? analList.FindIndex(c=>c=="=") : analList.FindIndex(c=>c=="?");
        for(int i = 0; i < analList.Count; i++) {
            if(i < splitIdx) leftSideList.Add(analList[i]);
            else             rightSideList.Add(analList[i]);
        }

        //* 要らない部分 削除
        leftSideList.Remove("?");
        rightSideList.Remove("?");
        rightSideList.Remove("=");

        //* (横)：演算子と定数に分ける
        if(isHrzEqu) {
            lNums = separateOperatorAndNumbers(out lOpr, leftSideList);
            rNums = separateOperatorAndNumbers(out rOpr, rightSideList);
        }
        //* (縦)：演算子と定数に分ける
        else {
            lNums = separateOperatorAndNumbers(out lOpr, leftSideList);
        }

        Debug.Log($"makeQuizSentence:: (横): leftOpr= <color=yellow>{lOpr}</color>, rightOpr= <color=yellow>{rOpr}</color>");
        Debug.Log($"makeQuizSentence:: (横): lNums= <color=green>{string.Join(", ", lNums.ToArray())}</color>, rNums= <color=green>{string.Join(", ", rNums.ToArray())}</color>");

        //* キーワード 切り替え
        var quiz = qm.QuizTxt;
        quiz.text = "미 지원..";

        initObjList();
        qSO.Obj1Name = Util.getRandomList(qSO.ObjNameList);
        qSO.Obj2Name = Util.getRandomList(qSO.ObjNameList);
        int lN1 = int.Parse(lNums[0]); 
        int lN2 = lNums.Count > 1? int.Parse(lNums[1]) : 0;
        int rN1 = rNums.Count > 0? int.Parse(rNums[0]) : 0;
        int rN2 = rNums.Count > 1? int.Parse(rNums[1]) : 0;
        Debug.Log($"makeQuizSentence:: lN1= {lN1}, lN2= {lN2}, rN1= {rN1}, rN2= {rN2}");
        switch(lOpr) {
            case "+": {
                //* (定数式) N1 + N2 = ?
                if(!isXEquation) {
                    quiz.text = replaceTxtKeyword(qSO.QstPlus, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                    yield return coCreateObj(qSO.Obj1Name, lN1);
                    yield return coCreateExtraOprBox(lOpr, qSO.Obj1Name, lN2);
                    OnAnswerObjAction += () => addObj(qSO.Obj1Name, befNum: lN1, lN2);
                }
                //* (X方程式) N1 + X = N2
                else {
                    //* 文章
                    quiz.text = replaceTxtKeyword(qSO.QstPlus_XEqu, new string[]{qSO.Obj1Name, lNums[0], rNums[0]});
                    //* ± N3
                    if(rNums.Count > 1) {
                        rOpr = (rOpr == "minus")? "-" : rOpr; //* 言語➝記号に変更
                        quiz.text += replaceExtraOprKeyword(rOpr, rNums[1]);
                    }
                    else {
                        quiz.text += "가 됫어요.";
                    }
                    quiz.text += "\n친구는 몇 개를 주었나요?";

                    //* オブジェクト
                    const float POS_X = 0.65f;
                    yield return coCreateObj(qSO.Obj1Name, rN1, POS_X);
                    yield return coCreateQuestionMarkBox(qSO.Obj1Name, lN1, -POS_X);
                    //* ± N3
                    if(rNums.Count > 1) {
                        yield return coCreateExtraOprBox(rOpr, qSO.Obj1Name, rN2, POS_X);
                    }
                    OnAnswerBoxAction = showQuestionMarkAnswerBox; //* (BUG) "?"があるboxオブジェクトを探すので、ちゃんと"?"が入ってから、コールバック関数を登録
                }
                break;
            }
            case "-": { //* 38 - 13 = ?
                quiz.text = replaceTxtKeyword(qSO.QstMinus, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                yield return coCreateObj(qSO.Obj1Name, lN1);// createObj(qSO.Obj1Name, lN1);
                yield return coCreateExtraOprBox(lOpr, qSO.Obj1Name, lN2);
                OnAnswerObjAction += () => substractObj(lN2);
                break;
            }
            case "times": { //* 31 times 2
                quiz.text = replaceTxtKeyword(qSO.QstMultiply, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                yield return coCreateObj(qSO.Obj1Name, lN1);
                OnAnswerObjAction += () => multiplyObj(qSO.Obj1Name, befNum: lN1, lN2);
                break;
            }
            case "frac": {
                int value = lN1 / lN2;
                int rest = lN1 % lN2;
                Debug.Log($"value= {value}, rest= {rest}");

                quiz.text = replaceTxtKeyword(qSO.QstDivide, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                yield return coCreateObj(qSO.Obj1Name, lN1);
                OnAnswerObjAction += () => divideObj(qSO.Obj1Name, befNum: lN1, lN2);

                //* 残りが有ったら、分数で表記
                if(rest != 0)
                    quiz.text += " 나머지는요?\n(분수로 알려주세요!)";
                break;
            }
            case "underline":
            case "left": { //* 最大公約数
                const float POS_X = 0.65f;
                int gcd = Util.getGreatestCommonDivisor(lN1, lN2);
                quiz.text = replaceTxtKeyword(qSO.QstGreatestCommonDivisor, new string[]{qSO.Obj1Name, lNums[0], lNums[1], qSO.Obj2Name});
                yield return coCreateObj(qSO.Obj1Name, lN1, posX: -POS_X);
                yield return coCreateObj(qSO.Obj2Name, lN2, posX: POS_X);
                OnAnswerObjAction += () => greatestCommonDivisorObj(qSO.Obj1Name, lN1, gcd, -POS_X);
                OnAnswerObjAction += () => greatestCommonDivisorObj(qSO.Obj2Name, lN2, gcd, POS_X);
                break;
            }
        }
        yield return coActiveSelectAnswer();
    }

    private void initObjList() {
        qSO.ObjNameList = new List<string>(qSO.DefObjNames);
        foreach(Transform obj in objGroupTf)
            Destroy(obj.gameObject);
    }
    private List<string> separateOperatorAndNumbers(out string oprerator, List<string> equationList) {
        //* 演算子
        oprerator = equationList.Find(str => Regex.IsMatch(str, Config.OPERATION_REGEX_PATTERN));
        if(oprerator != null) equationList.Remove(oprerator);
        //* 残った定数を返す
        return equationList;
    }
    private string replaceTxtKeyword(string sentence, string[] keys) {
        Debug.Log($"replaceTxtKeyword:: keys.Length= {keys.Length} : {string.Join(", ", keys)}");
        const int OBJ1 = 0, N1 = 1, N2 = 2, OBJ2 = 3;
        //* Keyword 変換
        string res = sentence.Replace("OBJ1", $"<sprite name={keys[OBJ1]}>");
        res = res.Replace("N1", keys[N1]);
        res = res.Replace("N2", keys[N2]);
        if(OBJ2 < keys.Length) 
            res = res.Replace("OBJ2", $"<sprite name={keys[OBJ2]}>");

        return res;
    }
    private string replaceExtraOprKeyword(string rOpr, string key) {
        string res = "";
        switch(rOpr) {
            case "+":
                res = $"...\n<color=blue>앗! {key}개 더 있네요.</color>";
                break;
            case "-": // case "minus":
                res = $"...\n<color=red>앗! 죄송.. {key}개 빼야되요.</color>";
                break;
        }
        Debug.Log($"replaceExtraOprKeyword(rOpr= {rOpr}, key= {key}):: res= {res}");
        return res;
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public Sprite getObjSprite(string name) {
        Sprite res = null;
        var enumObjIdx = System.Enum.GetValues(typeof(Enum.OBJ_SPR_IDX));
        foreach (var enumVal in enumObjIdx) {
            string enumValStr = enumVal.ToString().ToLower();
            if (enumValStr == name) {
                Debug.Log($"getObjSpriteIndex({name}):: {enumValStr} == {name} -> {enumVal.ToString() == name}");
                int idx = (int)enumVal;
                res =  objSprs[idx];
            }
        }
        return res;
    }

    public void showQuestionMarkAnswerBox(int answer) {
        Debug.Log($"showQuestionMarkAnswerBox(answer= {answer})::");
        for(int i = 0; i < objGroupTf.childCount; i++) {
            var questionMarkBox = objGroupTf.GetChild(i).GetComponent<BoxObj>();
            if(questionMarkBox.ValueTxt.text == "?") {
                //* ? ➝ 正解
                questionMarkBox.ValueTxt.text = answer.ToString();
                questionMarkBox.Val = answer;
                break;
            }
        }
    }
    public void addObj(string objName, int befNum, int num) {
        foreach(Transform chd in objGroupTf) {
            if(chd.name == Enum.OBJ_NAME.BlinkBox.ToString())
                DestroyImmediate(chd.gameObject);
        }
        StartCoroutine(coAddObj(objName, befNum, num));
    }
    public void substractObj(int num) {
        foreach(Transform chd in objGroupTf) {
            if(chd.name == Enum.OBJ_NAME.BlinkBox.ToString())
                DestroyImmediate(chd.gameObject);
        }
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
    private IEnumerator coCreateObj(string objName, int num, float posX = 0, bool isFinish = true) {
        int boxCnt = num / BOX_S_MAX;
        for(int i = 0; i < num; i++) {
            Debug.Log($"coCreateObj:: i= {i}, num = {num}");
            //* お先に、10個入りBOX生成
            if(i < boxCnt * BOX_S_MAX) {
                i += BOX_S_MAX - 1;
                BoxObj box = instBox(objName, posX);
                box.Val = BOX_S_MAX;
                yield return Util.time0_3;
            }
            else {
                //* 残り 0個 BOX生成
                if(i % BOX_S_MAX == 0) {
                    instBox(objName, posX);
                    yield return Util.time0_5;
                }
                //* そこに入れる、オブジェクト生成
                instObj(objName, posX);
                yield return Util.time0_05;
            }
        }
    }

    private IEnumerator coActiveSelectAnswer() {
        yield return Util.time0_5; //* オブジェクト落ちる時間
        qm.interactableAnswerBtns(true);
        qm.IsSolvingQuestion = true; //* 経過時間 カウント START

        //* チュートリアル：診断評価の最初問題
        showTutoDiagFirstQuiz();
    }

    private IEnumerator coCreateQuestionMarkBox(string objName, int num, float posX) {
        yield return coCreateObj(objName, num, posX, isFinish: false);
        yield return Util.time0_3;

        //* 「?」Box
        BoxObj box = instBox(objName, posX);
        // yield return Util.time0_3;
        box.ValueTxt.text = "?";
        box.ValueTxt.color = Color.magenta;
        box.ValueTxt.fontStyle = FontStyles.Bold;

        //* チュートリアル：診断評価の最初問題
        showTutoDiagFirstQuiz();
    }

    private void showTutoDiagFirstQuiz() {
        if(qm.CurQuestionIndex == 0 && qm.Status == Status.DIAGNOSIS && DB.Dt.IsTutoDiagFirstQuizTrigger) {
            gtm.action((int)GameTalkManager.TALK_ID_IDX.TUTORIAL_DIAG_FIRST_QUIZ);
        }
    }

    private IEnumerator coCreateExtraOprBox(string opr, string objName, int num, float posX = 0) {
        yield return Util.time0_2;
        Debug.Log($"coCreateExtraOprBox(opr= {opr}, objName= {objName}, num= {num}, posX= {posX})::");
        BoxObj box = instBox(objName, posX);
        box.name = Enum.OBJ_NAME.BlinkBox.ToString();
        box.IsBlockMerge = true;
        box.ValueTxt.text = $"{opr}{num}";
        box.ValueTxt.color = (opr == "+")? Color.blue : Color.red;
        box.Anim.SetTrigger((opr == "+")? Enum.ANIM.DoBlinkAdd.ToString() : Enum.ANIM.DoBlinkMinus.ToString());
    }

    private IEnumerator coAddObj(string objName, int befNum, int num) {
        int boxCnt = (num + befNum) / BOX_S_MAX;
        int lastBoxIdx = objGroupTf.childCount - 1;
        int lastBoxVal = objGroupTf.GetChild(lastBoxIdx).GetComponent<BoxObj>().Val;
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
                box.Val = BOX_S_MAX;
                yield return Util.time0_3;
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
            int lastIdx = objGroupTf.childCount - 1;
            var lastBox = objGroupTf.GetChild(lastIdx).GetComponent<BoxObj>();
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
        foreach (Transform child in objGroupTf) {
            Destroy(child.gameObject);
        }
        yield return null; //* 破壊するまで、1フレーム 待機
        
        //TODO EFFECT

        //* ボックス
        for(int i = 0; i < val; i++) {
            BoxObj box = instBox(objName);
            box.IsBlockMerge = true;
            box.Val = num;
            yield return Util.time0_3;
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
        foreach (Transform child in objGroupTf) {
            Destroy(child.gameObject);
        }
        yield return null; //* 破壊するまで、1フレーム 待機
        
        //TODO EFFECT

        //* ボックス
        for(int i = 0; i < gcd; i++) {
            BoxObj box = instBox(objName, posX);
            box.IsBlockMerge = true;
            box.Val = val;
            Debug.Log($"coGreatestCommonDivisorObj:: i({i}) < gcd({gcd}), val= {box.Val}");
            yield return Util.time0_3;
        }

        //* 残り
        yield return Util.time0_5;
        for(int i = 0; i < rest; i++) {
            instObj(objName, posX);
            yield return Util.time0_05;
        }
    }

    private BoxObj instBox(string objName, float posX = 0) {
            var box = Instantiate(boxPf, objGroupTf).GetComponent<BoxObj>();
            box.transform.position = new Vector2(posX, BOX_SPAWN_Y);
            box.ObjImg.sprite = getObjSprite(objName);
            return box;
    }

    private void instObj(string objName, float posX = 0) {
        var obj = Instantiate(objPf, objGroupTf);
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
