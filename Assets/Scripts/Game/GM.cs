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
    public enum GAME_STT {PLAY, RESULT};
    [SerializeField] GAME_STT gameStatus;   public GAME_STT GameStatus {get => gameStatus; set => gameStatus = value;}

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

    [Header("MAP")]
    [SerializeField] Transform[] maps;
    [Header("SPECIAL CONTROL BG")]
    [SerializeField] GameObject windMillBG;
    [SerializeField] GameObject jungleFlowerBG;
    [SerializeField] GameObject jungleFlowerBG1;
    [SerializeField] GameObject jungleFlowerBG2;

    [Header("BG SPRITE")]
    [SerializeField] GameObject cloud1; public GameObject Cloud1 {get => cloud1; set => cloud1 = value;}
    [SerializeField] GameObject cloud2; public GameObject Cloud2 {get => cloud2; set => cloud2 = value;}
    [SerializeField] GameObject windMillWing; public GameObject WindMillWing {get => windMillWing; set => windMillWing = value;}

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

        gameStatus = GAME_STT.PLAY;

        //* Anim
        successEFAnim.gameObject.SetActive(false);

        //* マップ初期化 (全て非表示)
        Array.ForEach(maps, map => map.gameObject.SetActive(false));
    }

    void Start() {

        //* 曇り移動
        StartCoroutine(coUpdateCloudMoving());

        //* Forest BG2: WindMill
        StartCoroutine(coRotateWindMillWing());

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
            pet.Col.enabled = false;

            //* マップによって、転換イメージ適用
            foreach(Transform chd in GM._.gui.BgDirectorAnim.transform)
                chd.GetComponent<Image>().sprite = GM._.gui.MapTransitionSprs[DB._.SelectMapIdx];

            suffleMapBG();
            StartCoroutine(coSetMapBG(0));
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
    public IEnumerator coMakeQuiz(List<string> analList) {
        List<string> leftSideList = new List<string>(); //* 左辺
        List<string> rightSideList = new List<string>(); //* 右辺

        //TODO 現在は 単一演算のみ対応
        string lOpr = null; //* 左演算子 
        string rOpr = null; //* 右演算子

        List<string> lNums = new List<string>(); //* 左定数
        List<string> rNums = new List<string>(); //* 右定数
        
        //* X方程式か？
        bool isXEquation = analList.Exists(li => li == "x");
        if(isXEquation) analList.RemoveAll(str => str == "x");

        Debug.Log($"coMakeQuiz(analList.Cnt= {analList.Count}, isXEquation= {isXEquation})::");
        Debug.Log($"coMakeQuiz:: analList: <color=white>{string.Join(", ", analList.ToArray())}</color>");
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

        Debug.Log($"coMakeQuiz:: (横): leftOpr= <color=yellow>{lOpr}</color>, rightOpr= <color=yellow>{rOpr}</color>");
        Debug.Log($"coMakeQuiz:: (横): lNums= <color=green>{string.Join(", ", lNums.ToArray())}</color>, rNums= <color=green>{string.Join(", ", rNums.ToArray())}</color>");

        //* キーワード 切り替え
        var quiz = qm.QuizTxt;
        quiz.text = "미 지원..";

        // initObjList();
        qSO.Obj1Name = Util.getRandomList(qSO.ObjNameList);
        qSO.Obj2Name = Util.getRandomList(qSO.ObjNameList);
        int lN1 = int.Parse(lNums[0]); 
        int lN2 = lNums.Count > 1? int.Parse(lNums[1]) : 0;
        int rN1 = rNums.Count > 0? int.Parse(rNums[0]) : 0;
        int rN2 = rNums.Count > 1? int.Parse(rNums[1]) : 0;
        Debug.Log($"coMakeQuiz:: lN1= {lN1}, lN2= {lN2}, rN1= {rN1}, rN2= {rN2}");
        switch(lOpr) {
            case "+": {
                //* (定数式) N1 + N2 = ?
                if(!isXEquation) {
                    quiz.text = replaceTxtKeyword(qSO.QstPlus, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                    yield return coCreateObj(qSO.Obj1Name, lN1);
                    yield return coCreateOprBlinkBox(lOpr, qSO.Obj1Name, lN2);
                    //* Callback
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
                        yield return coCreateOprBlinkBox(rOpr, qSO.Obj1Name, rN2, POS_X);
                        //* Callback Right BlinkBox Operation
                        Debug.Log($"± N3 rOpr-> {rOpr}");
                        switch(rOpr) {
                            case "+":
                                OnAnswerObjAction += () => addObj(qSO.Obj1Name, befNum: 0, rN2, POS_X);
                                break;
                            case "-":
                                OnAnswerObjAction += () => substractObj(qSO.Obj1Name, rN2, tgBoxOpt: "RightBox");
                                break;
                        }
                    }
                    //* Callback: Left ?MarkBox Operation 
                    OnAnswerBoxAction += showQuestionMarkAnswerBox;
                    OnAnswerBoxAction += (answer) => addObj(qSO.Obj1Name + "?", befNum: 0, answer, -POS_X);
                }
                break;
            }
            case "-": { //* 38 - 13 = ?
                quiz.text = replaceTxtKeyword(qSO.QstMinus, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                yield return coCreateObj(qSO.Obj1Name, lN1);// createObj(qSO.Obj1Name, lN1);
                yield return coCreateOprBlinkBox(lOpr, qSO.Obj1Name, lN2);
                //* Callback
                OnAnswerObjAction += () => substractObj(qSO.Obj1Name, lN2, tgBoxOpt: "LastBox");
                break;
            }
            case "times": { //* 31 times 2
                quiz.text = replaceTxtKeyword(qSO.QstMultiply, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                yield return coCreateObj(qSO.Obj1Name, lN1);
                //* Callback
                OnAnswerObjAction += () => multiplyObj(qSO.Obj1Name, lN1, lN2);
                break;
            }
            case "frac": {
                GM._.qm.HelpAnimType = "frac";
                GM._.qm.HelpSpeachBtn.SetActive(true);

                int value = lN1 / lN2;
                int rest = lN1 % lN2;
                Debug.Log($"value= {value}, rest= {rest}");

                quiz.text = replaceTxtKeyword(qSO.QstDivide, new string[]{qSO.Obj1Name, lNums[0], lNums[1]});
                //* 残りが有ったら、分数で表記
                if(rest != 0)
                    quiz.text += "\n나머지는요?\n(분수로 알려주세요.)";

                yield return coCreateObj(qSO.Obj1Name, lN1);
                //* Callback
                OnAnswerObjAction += () => divideObj(qSO.Obj1Name, befNum: lN1, lN2);
                break;
            }
            case "underline":
            case "left": { //* 最大公約数
                GM._.qm.HelpAnimType = "underline";
                GM._.qm.HelpSpeachBtn.SetActive(true);

                const float POS_X = 0.65f;
                int gcd = Util.getGreatestCommonDivisor(lN1, lN2);
                quiz.text = replaceTxtKeyword(qSO.QstGreatestCommonDivisor, new string[]{qSO.Obj1Name, lNums[0], lNums[1], qSO.Obj2Name});
                yield return coCreateObj(qSO.Obj1Name, lN1, posX: -POS_X);
                yield return coCreateObj(qSO.Obj2Name, lN2, posX: POS_X);
                //* Callback
                OnAnswerObjAction += () => greatestCommonDivisorObj(qSO.Obj1Name, lN1, gcd, -POS_X);
                OnAnswerObjAction += () => greatestCommonDivisorObj(qSO.Obj2Name, lN2, gcd, POS_X);
                break;
            }
        }
        //* 準備完了：選択ボタン 活性化
        yield return coActiveSelectAnswer();
    }

    public void initObjList() {
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

    public void addObj(string objName, int befNum, int num, float posX = 0) {
        Vector2 startPos = Vector2.zero;
        if(objName.Contains("?")) 
            startPos = getQuestionMarkBoxPos();
        else 
            startPos = deleteBlinkBox(opr: "+");
        
        setOprResult("+", objName, num, startPos);
    }
    public void substractObj(string objName, int num, string tgBoxOpt) {
        deleteBlinkBox(opr: "-");
        StartCoroutine(coSubstractObj(objName, num, tgBoxOpt));
    }
    public void multiplyObj(string objName, int befNum, int num) {
        StartCoroutine(coMultiplyBox(objName, befNum, num));
    }
    public void divideObj(string objName, int befNum, int num) {
        StartCoroutine(coDivideObj(objName, befNum, num));
    }
    public void greatestCommonDivisorObj(string objName, int befNum, int gcd, float posX) {
        StartCoroutine(coGreatestCommonDivisorObj(objName, befNum, gcd, posX));
    }
    public void showQuestionMarkAnswerBox(int answer) {
        Debug.Log($"showQuestionMarkAnswerBox(answer= {answer})::");
        for(int i = 0; i < objGroupTf.childCount; i++) {
            var questionMarkBox = objGroupTf.GetChild(i).GetComponent<BoxObj>();
            if(questionMarkBox.ValueTxt.text == "?") {
                //* ? ➝ 正解
                questionMarkBox.ValueTxt.text = 0.ToString(); //answer.ToString();
                questionMarkBox.Val = 0; //answer;
                break;
            }
        }
    }

    ///* オブジェクト 生成
    private IEnumerator coCreateObj(string objName, int num, float posX = 0) {
        for(int i = 0; i < num; i++) {
            // Debug.Log($"coCreateObj:: i= {i}, num = {num}");
            if(i == 0) {
                instBox(objName, posX);
                yield return Util.time0_5;
            }
            //* そこに入れる、オブジェクト生成
            instObj(objName, posX);
            yield return Util.time0_05;
        }
    }

    private Vector2 deleteBlinkBox(string opr) {
        Vector2 pos = Vector2.zero;
        foreach(Transform chd in objGroupTf) {
            if(chd.name.Contains(Enum.BOX_NAME._Blink.ToString())) {
                pos = chd.position;
                if(opr == "+")
                    GM._.gem.showEF((int)GEM.IDX.PlusBlinkBoxBurstEF, chd.position, Util.time2);
                else if(opr == "-")
                    GM._.gem.showEF((int)GEM.IDX.MinusBlinkBoxBurstEF, chd.position, Util.time2);
                DestroyImmediate(chd.gameObject);
            }
        }
        return pos;
    }
    private Vector2 getQuestionMarkBoxPos() {
        Vector2 pos = Vector2.zero;
        foreach(Transform chd in objGroupTf) {
            if(chd.name.Contains(Enum.BOX_NAME._QuestionMark.ToString())) {
                pos = chd.position;
                GM._.gem.showEF((int)GEM.IDX.QuestionMarkBoxBurstEF, chd.position, Util.time2);
            }
        }
        return pos;
    }

    private void setOprResult(string opr, string objName, int num, Vector2 startPos) {
        const float DIR_X_RANGE = 0.06875f;
        for(int i = 0; i < num; i++) {
            //* オブジェクト生成し、上に上げる
            var obj = Instantiate(objPf, objGroupTf).GetComponent<Obj>();
            obj.transform.position = startPos;
            obj.SprRdr.sprite = getObjSprite(objName);
            Vector2 dir = new Vector2(Random.Range(-DIR_X_RANGE, DIR_X_RANGE), 1.0f).normalized;
            obj.addForce(dir);

            //* マイナス結果なら、追加処理
            if(opr == "+")
                Debug.Log("なし");
            else if(opr == "-")
                StartCoroutine(obj.coDisappear());
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
        yield return coCreateObj(objName, num, posX);
        yield return Util.time0_2;

        //* 「?」Box
        BoxObj box = instBox(objName, posX);
        box.name += Enum.BOX_NAME._QuestionMark.ToString();
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

    private IEnumerator coCreateOprBlinkBox(string opr, string objName, int num, float posX = 0) {
        yield return Util.time0_2;
        Debug.Log($"coCreateExtraOprBox(opr= {opr}, objName= {objName}, num= {num}, posX= {posX})::");
        BoxObj box = instBox(objName, posX);
        box.name += Enum.BOX_NAME._Blink.ToString();
        box.IsBlockMerge = true;
        box.ValueTxt.text = $"{opr}{num}";
        box.ValueTxt.color = (opr == "+")? Color.blue : Color.red;
        box.Anim.SetTrigger((opr == "+")? Enum.ANIM.DoBlinkAdd.ToString() : Enum.ANIM.DoBlinkMinus.ToString());
    }

    private IEnumerator coAddObj(string objName, int befNum, int num, float posX) {
        Debug.Log($"coAddObj(objName= {objName}, befNum= {befNum}, num= {num})");
        for(int i = befNum; i < num + befNum; i++) {
            instObj(objName, posX);
            yield return Util.time0_05;
        }
    }

    private IEnumerator coSubstractObj(string objName, int num, string tgBoxOpt) {
        BoxObj tgBox = null;
        //* ターゲットBOX 探す
        switch(tgBoxOpt) {
            case "LastBox":
                int lastIdx = objGroupTf.childCount - 1;
                var lastBox = objGroupTf.GetChild(lastIdx).GetComponent<BoxObj>();
                tgBox = lastBox;
                break;
            case "RightBox":
                Transform[] chds = objGroupTf.GetComponentsInChildren<Transform>();
                var tf = Array.Find(chds, chd => chd.name == Enum.BOX_NAME.RightBox.ToString());
                tgBox = tf.GetComponent<BoxObj>();
                break;
        }
        Debug.Log($"coSubstractObj(num= {num}, tgBoxOpt= {tgBoxOpt}):: {tgBox.name}");

        //* 減る 処理
        for(int i = 0; i < num; i++) {
            tgBox.Val--;
            setOprResult("-", objName, num, tgBox.transform.position);
            yield return Util.time0_05;
            if(tgBox.Val <= 0) {
                DestroyImmediate(tgBox.gameObject);
            }
        }
    }

    private IEnumerator coMultiplyBox(string objName, int n1, int multiNum) {
        //* 既にBOXは一つあるので、iは０ではなく１
        for(int i = 1; i < multiNum; i++) { 
            BoxObj box = instBox(objName);
            box.Val = n1;
            yield return Util.time0_3;
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
            box.name = (posX == 0)? Enum.BOX_NAME.Box.ToString()
                : (posX < 0)? Enum.BOX_NAME.LeftBox.ToString()
                : Enum.BOX_NAME.RightBox.ToString();
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

    public IEnumerator coRotateWindMillWing() {
        float moveSpeed = -8 * Time.deltaTime;
        while(windMillWing.activeSelf) {
            windMillWing.transform.Rotate(0, 0, moveSpeed);
            yield return null;
        }
    }

    private void suffleMapBG() {
        var map = maps[DB._.SelectMapIdx];
        map.gameObject.SetActive(true);

        //* Suffle BGs
        if(GM._.qm.CurQuestionIndex == 0) {
            foreach(Transform bg in map) {
                int rand = Random.Range(0, 100);
                Debug.Log($"suffleMapBG():: {bg.name}: (rand > 50?)= {(rand > 50)}");
                if(rand > 50) bg.SetAsFirstSibling();
            }
        }
    }

    public IEnumerator coSetMapBG(int bgIdx) {
        var map = maps[DB._.SelectMapIdx];
        map.gameObject.SetActive(true);        

        //* 背景 Switch変更 前処理
        if(bgIdx > 0) {
            //* Switch Anim
            GM._.gui.BgDirectorAnim.SetTrigger(Enum.ANIM.DoSwitchBG.ToString());
            yield return Util.time0_8;
            //* 動物 切り替え
            GM._.Anm.setRandomSprLibAsset();
        }

        const int PETPOS = 0;
        const int PALYERPOS = 1;
        const int ANIMALPOS = 2;
        for(int i = 0; i < map.childCount; i++) {
            var bg = map.GetChild(i);
            bg.gameObject.SetActive(bgIdx == i);
            if(bgIdx == i) {
                pet.TgPos = bg.GetChild(PETPOS).transform.localPosition;
                pet.Sr.flipX = true;
            }
        }

        //* 特別背景 処理
        var curBg = map.GetChild(bgIdx);
        if(windMillBG.activeSelf) {
            cam.Anim.SetTrigger(Enum.ANIM.DoWindMillScrollDown.ToString());
            cloud1.transform.position = new Vector2(cloud1.transform.position.x, 12);
            cloud2.transform.position = new Vector2(cloud2.transform.position.x, 10);
            yield return Util.time0_5;
        }
        else if(jungleFlowerBG.activeSelf) {
            Debug.Log("jungleFlowerBG");
            anm.Sr.sortingOrder = 12;
            plSpot.localScale = Vector2.one * 3;
            pl.Sr.flipX = true;
            qm.HelpSpeachBtn.transform.position = new Vector2(-415, 200);
            pet.Sr.sortingLayerName = Enum.SORTING_LAYER.FrontDecoObj.ToString();
            //* Position
            pet.TgPos = curBg.GetChild(PETPOS).transform.localPosition;
            pet.transform.position = new Vector2(pet.TgPos.x - 5, pl.TgPos.y);
            pl.TgPos = curBg.GetChild(PALYERPOS).transform.localPosition;
            pl.transform.position = new Vector2(pl.TgPos.x - 5, pl.TgPos.y);
            anm.transform.position = curBg.GetChild(ANIMALPOS).transform.localPosition;
        }
        else {
            cloud1.transform.position = new Vector2(cloud1.transform.position.x, 3.5f);
            cloud2.transform.position = new Vector2(cloud2.transform.position.x, 3);
            pet.Sr.sortingLayerName = Enum.SORTING_LAYER.Default.ToString();
            pet.TgPos = curBg.GetChild(PETPOS).transform.localPosition;
            pet.transform.position = new Vector2(pet.TgPos.x - 5, pl.TgPos.y);
            pl.TgPos = new Vector2(-2, -3);
            pl.transform.position = new Vector2(pl.TgPos.x - 5, pl.TgPos.y);
            plSpot.localScale = Vector2.one;
            pl.Sr.flipX = true;
            anm.transform.position = new Vector2(2, -3);
        }

        //* 背景 Switch変更 後処理
        if(bgIdx > 0) {
            yield return Util.time0_8;
        }
        
    }
#endregion
}
