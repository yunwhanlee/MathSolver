using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;
using TexDrawLib.Samples;
using TMPro;
using System;
using Random = UnityEngine.Random;

public enum Status { WAITING, DIAGNOSIS, LEARNING }
public class QuizManager : MonoBehaviour {
    const int QUIZ_CNT = 8;
    const int BTN_CNT = 3;
    const int EXP_RWD_UNIT = 10;
    const int COIN_RWD_UNIT = 100;
    const float RETRY_PANELTY_PER = 0.1f;

    [SerializeField] WJ_Connector wj_connector;
    [SerializeField] Status status;  public Status Status => status;

    [Header("SPRITE")]
    [SerializeField] Sprite correctHeartSpr;
    [SerializeField] Sprite wrongHeartSpr;

    [Header("PANEL")]
    [SerializeField] GameObject diagChooseDiffPanel;            // 난이도 선택 패널
    [SerializeField] GameObject questionPanel;                  // 문제 패널(진단,학습)
    [SerializeField] GameObject quizGroup;
    [SerializeField] GameObject answerBtnGroup;
    [SerializeField] Transform answerProgressFrameTf;            // 答えた結果を💛で表示

    [Header("HINT")]
    [SerializeField] GameObject hintFrame;
    [SerializeField] TEXDraw questionEquationTxtDraw;           // 문제 텍스트(※TextDraw로 변경 필요)

    [Header("HELP")]
    [SerializeField] string helpAnimType;   public string HelpAnimType {get => helpAnimType; set => helpAnimType = value;}
    [SerializeField] int helpAnimPlayIdx;    public int HelpAnimPlayIdx {get => helpAnimPlayIdx; set => helpAnimPlayIdx = value;}
    [SerializeField] GameObject helpSpeachBtn;  public GameObject HelpSpeachBtn {get => helpSpeachBtn;}

    [Header("QUIZ")]
    [SerializeField] TextMeshProUGUI questionDescriptionTxt;    // 문제 설명 텍스트
    [SerializeField] TextMeshProUGUI quizTxt; public TextMeshProUGUI QuizTxt {get => quizTxt; set => quizTxt = value;}
    [SerializeField] TextMeshProUGUI stageTxt;  public TextMeshProUGUI StageTxt {get => stageTxt; set => StageTxt = value;}

    [SerializeField] Button[] diagSelectDiffBtn;
    [SerializeField] Button[] answerBtn = new Button[BTN_CNT]; public Button[] AnswerBtn {get => answerBtn;}  // 정답 버튼들
    TEXDraw[] answerBtnTxtDraw;                                 // 정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("STATUS")]
    [SerializeField] int curQuestionIndex;  public int CurQuestionIndex {get => curQuestionIndex;}

    //* 最初選択の答え 保存 (오답시 다시 기회제공으로 인한, 결과오류에 대응)
    [SerializeField] string firstChoiceAnswer;  public string FirstChoiceAnswer {get => firstChoiceAnswer;}
    //* 経過時間 カウントトリガー
    [SerializeField] bool isSolvingQuestion; public bool IsSolvingQuestion {get => isSolvingQuestion; set => isSolvingQuestion = value;}
    //* 経過時間
    [SerializeField] float questionSolveTime;
    [SerializeField] string[] quizAnswerResultArr;  public string[] QuizAnswerResultArr {get => quizAnswerResultArr;}

    [Header("DEBUG")]
    [SerializeField] WJ_DisplayText wj_displayText; // 텍스트 표시용(필수X)
    [SerializeField] Button getLearningButton;      // 문제 받아오기 버튼

    private void Awake() {
        //* Init
        diagChooseDiffPanel.SetActive(false);
        questionPanel.SetActive(false);
        answerBtnTxtDraw = new TEXDraw[answerBtn.Length];
        // quizAnswerResultArr = new string[8] {"N", "N", "N", "N", "N", "N", "N", "N"};

        for (int i = 0; i < answerBtn.Length; ++i)
            answerBtnTxtDraw[i] = answerBtn[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("대기중", "", "", "");
    }

    void OnEnable() => Setup();

    void Update() {
        //* 問題 経過時間 カウント
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }
//-------------------------------------------------------------------------------------------------------------
#region EVENT BUTTON
//-------------------------------------------------------------------------------------------------------------
    public void onClickDiagChooseDifficultyBtn(int diffLevel) { //* #2
        //* (BUG) 重なって実行すること対応
        Array.ForEach(diagSelectDiffBtn, diffBtn => diffBtn.gameObject.SetActive(false));
        //* 選択レベルの診断評価 スタート
        Debug.Log($"WJ_Sample:: onClickDiagChooseDifficultyBtn({diffLevel})");
        status = Status.DIAGNOSIS;
        quizAnswerResultArr = new string[8];
        wj_connector.FirstRun_Diagnosis(diffLevel); //* サーバから通信し、GetDiagnosis()呼び出す
    }
    public void onClickGetLearningBtn() {
        Debug.Log($"WJ_Sample:: onClickGetLearningBtn()");
        // quizAnswerResultArr = new string[8];
        wj_connector.Learning_GetQuestion();
        wj_displayText.SetState("문제풀이 중", "-", "-", "-");
    }
    public void onClickSelectAnswerBtn(int idx) => StartCoroutine(SelectAnswer(idx));
    public void onClickHelpSpeachBtn() {
        Time.timeScale = 0;
        helpAnimPlayIdx = 0; //* 初期化
        GM._.gui.HelpPanelAnim.gameObject.SetActive(true);
        if(helpAnimType == "frac") {
            GM._.gui.HelpPanelAnim.SetInteger(Enum.ANIM.HelpFraction.ToString(), helpAnimPlayIdx++);
        }
        
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region MAIN FUNC
//-------------------------------------------------------------------------------------------------------------
    private void Setup() { //* #1
        switch (status) {
            case Status.WAITING:
                //* 診断評価パンネル 表示
                diagChooseDiffPanel.SetActive(true);
                break;
        }

        //* 診断評価 と 学習メソッド コールバック 登録
        if (wj_connector) {
            wj_connector.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_connector.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
    }

    /// <summary>
    //* 진단평가:: 문제 받아오기 (초기 한번만 실행, 종료 후 수준평가 반환 값이 없음)
    /// </summary>
    private void GetDiagnosis() {
        Debug.Log("WJ_Sample:: GetDiagnosis():: 診断評価");
        switch (wj_connector.cDiagnotics.data.prgsCd) {
            case "W":
                StartCoroutine(coDisplayQuestion(wj_connector.cDiagnotics.data.textCn,
                            wj_connector.cDiagnotics.data.qstCn,
                            wj_connector.cDiagnotics.data.qstCransr,
                            wj_connector.cDiagnotics.data.qstWransr)
                );
                wj_displayText.SetState("진단평가 중", "", "", "");
                break;
            case "E":
                Debug.Log("진단평가 끝! 학습 단계로 넘어갑니다.");
                wj_displayText.SetState("진단평가 완료", "", "", "");
                status = Status.LEARNING;
                getLearningButton.interactable = true;

                //* 結果パンネル 表示
                StartCoroutine(GM._.rm.coDisplayResultPanel());
                break;
        }
    }

    /// <summary>
    //* 학습평가:: n번째 문제 받아오기 (진단평가 이후 반복 실행, 종료 후 수준평가 반환 값 있음!)
    /// </summary>
    private void GetLearning(int idx) {
        Debug.Log($"WJ_Sample:: GetLearning(${idx}) 問題読込");
        if (idx == 0) curQuestionIndex = 0;

        StartCoroutine(coDisplayQuestion(wj_connector.cLearnSet.data.qsts[idx].textCn,
                    wj_connector.cLearnSet.data.qsts[idx].qstCn,
                    wj_connector.cLearnSet.data.qsts[idx].qstCransr,
                    wj_connector.cLearnSet.data.qsts[idx].qstWransr)
        );
    }

    /// <summary>
    //* 받아온 데이터를 가지고 문제를 표시
    /// </summary>
    /// param : タイトル, 数学式, 正解 一つ, 誤答 二つ
    IEnumerator coDisplayQuestion(string title, string qstEquation, string qstCorrectAnswer, string qstWrongAnswers) {
        Debug.Log($"WJ_Sample:: coDisplayQuestion(title ={title}, \nqstEquation =<b>{qstEquation}</b>, \nqstCorrectAnswer= {qstCorrectAnswer}, \nqstWrongAnswers= {qstWrongAnswers})::");

        //* Init
        firstChoiceAnswer = null;
        diagChooseDiffPanel.SetActive(false);
        interactableAnswerBtns(false);
        helpSpeachBtn.SetActive(false);

        //* 動物 切り替え
        if(curQuestionIndex != 0) GM._.Anm.setRandomSprLibAsset();

        //* 処理
        yield return coShowStageTxt();
        yield return coMakeQuestion(title, qstEquation, qstCorrectAnswer, qstWrongAnswers);
        yield return GM._.gui.coShowQuestion(qstEquation);
    }

    IEnumerator coShowStageTxt() {
        int stageNum = curQuestionIndex + 1;
        stageTxt.text = $"STAGE {stageNum} / 8";
        stageTxt.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.3f);
        stageTxt.gameObject.SetActive(false);
    }

    IEnumerator coMakeQuestion(string title, string qstEquation, string qstCorrectAnswer, string qstWrongAnswers) {
        string correctAnswer;
        string[] wrongAnswers;

        questionPanel.SetActive(true);
        StartCoroutine(Util.coPlayBounceAnim(GM._.Anm.transform));

        questionDescriptionTxt.text = title;
        questionEquationTxtDraw.text = qstEquation;

        //* Set Answerボタン
        correctAnswer = qstCorrectAnswer;
        wrongAnswers = qstWrongAnswers.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, BTN_CNT - 1) + 1;

        for(int i=0; i<answerBtn.Length; i++) {
            if (i < ansrCount)
                answerBtn[i].gameObject.SetActive(true);
            else
                answerBtn[i].gameObject.SetActive(false);
        }

        int ansrIndex = Random.Range(0, ansrCount);

        for(int i = 0, q = 0; i < ansrCount; ++i, ++q) {
            if (i == ansrIndex) {
                answerBtnTxtDraw[i].text = correctAnswer;
                --q;
            }
            else
                answerBtnTxtDraw[i].text = wrongAnswers[q];
        }
        // isSolvingQuestion = true; //* 経過時間 カウント ON
        
        yield return null;
    }

    /// <summary>
    //* 답을 고르고 맞았는 지 체크
    /// </summary>
    IEnumerator SelectAnswer(int idx) {
        if(GM._.IsSelectCorrectAnswer) {
            Debug.Log($"SelectAnswer(idx= {idx}):: 正解なので他のボタン選択できないように。 ➝ GM._.IsSelectCorrectAnswer= {GM._.IsSelectCorrectAnswer}");
            yield break;
        }

        bool isCorrect = false;
        string ansrCwYn = "N";

        switch (status) {
            case Status.DIAGNOSIS:
                isCorrect   = answerBtnTxtDraw[idx].text.CompareTo(wj_connector.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                //* 最初選択の答え 保存
                setFirstChoiceAnswer(ref ansrCwYn);

                quizAnswerResultArr[curQuestionIndex] = ansrCwYn;

                //* チュートリアル Quiz Answer
                if(GM._.qm.CurQuestionIndex == 0 && DB.Dt.IsTutoDiagFirstAnswerTrigger) {
                    GM._.gtm.IsTutoQuizAnswerCorret = isCorrect;
                    GM._.gtm.action((int)GameTalkManager.TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER);
                }   

                //* 答えした状況💛Frameで表示
                Image heartImg = answerProgressFrameTf.GetChild(curQuestionIndex).GetComponent<Image>();
                heartImg.sprite = (ansrCwYn == "Y")? correctHeartSpr : wrongHeartSpr;

                //* 経過時間　カウント STOP
                isSolvingQuestion = false;

                //* Answer結果 アニメー
                if(isCorrect) { // 正解
                    yield return coSuccessAnswer(idx);
                }
                else { // 誤答
                    yield return coFailAnswer(idx);
                    break; //TODO もう一回 チャレンジ　システム構築
                } 

                curQuestionIndex++;

                wj_connector.Diagnosis_SelectAnswer(answerBtnTxtDraw[idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("진단평가 중", answerBtnTxtDraw[idx].text, ansrCwYn, questionSolveTime + " 초");

                questionPanel.SetActive(false);
                questionSolveTime = 0;
                break;

            case Status.LEARNING:
                isCorrect   = answerBtnTxtDraw[idx].text.CompareTo(wj_connector.cLearnSet.data.qsts[curQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                //* 最初選択の答え 保存
                setFirstChoiceAnswer(ref ansrCwYn);

                //* 経過時間　カウント STOP
                isSolvingQuestion = false; 

                //* Answer結果 アニメー
                if(isCorrect) { // 正解
                    yield return coSuccessAnswer(idx);
                }
                else { // 誤答
                    yield return coFailAnswer(idx);
                    break; //TODO もう一回 チャレンジ　システム構築
                }  

                curQuestionIndex++;

                wj_connector.Learning_SelectAnswer(curQuestionIndex, answerBtnTxtDraw[idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("문제풀이 중", answerBtnTxtDraw[idx].text, ansrCwYn, questionSolveTime + " 초");

                if (curQuestionIndex >= 8) 
                {
                    questionPanel.SetActive(false);
                    wj_displayText.SetState("문제풀이 완료", "", "", "");
                }
                else GetLearning(curQuestionIndex);

                questionSolveTime = 0;
                break;
        }
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime) {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private void setFirstChoiceAnswer(ref string answerResult) {
        if(firstChoiceAnswer == null) firstChoiceAnswer = answerResult;
        else answerResult = firstChoiceAnswer;
    }
    private void initBtnColor() {
        Array.ForEach(answerBtn, btn => btn.GetComponent<Image>().color = Color.white);
    }
    public void interactableAnswerBtns(bool isActive) {
        Array.ForEach(answerBtn, btn => btn.interactable = isActive);
    }
    public IEnumerator coSuccessAnswer(int idx) {
        StartCoroutine(GM._.Pl.coRoarEF());
        StartCoroutine(GM._.Anm.coActiveAnimalHeartPoofEF());
        GM._.IsSelectCorrectAnswer = true;

        //* 演算子によって登録した関数 コールバック
        if(GM._.OnAnswerObjAction != null) {
            GM._.OnAnswerObjAction();
            GM._.OnAnswerObjAction = null;
        }
        //* X方程式：未知数表示 コールバック
        if(GM._.OnAnswerBoxAction != null) {
            var textDraw = answerBtn[idx].GetComponentInChildren<TEXDraw>();
            int answer = int.Parse(textDraw.text);
            GM._.OnAnswerBoxAction(answer);
            GM._.OnAnswerBoxAction = null;
        }

        //* リワード 
        Debug.Log($"coSuccessAnswer:: firstChoiceAnswer= {firstChoiceAnswer}");
        bool isY = firstChoiceAnswer == "Y"; //* 最初選択の答え
        
        const int OFFSET_Y = 2;
        var plPos = GM._.Pl.transform.position;

        //TODO Calculatiate Exp & Coin with LV & Item...
        int exp = (isY)? EXP_RWD_UNIT : (int)(EXP_RWD_UNIT * RETRY_PANELTY_PER);
        int coin = (isY)? COIN_RWD_UNIT : (int)(COIN_RWD_UNIT * RETRY_PANELTY_PER);
        Debug.Log("exp= " + exp + ", coin= " + coin);

        GM._.rm.setReward(exp, coin);
        Vector2 pos1 = new Vector2(plPos.x, plPos.y + OFFSET_Y);
        Vector2 pos2 = new Vector2(plPos.x, plPos.y + OFFSET_Y + 0.325f);
        GM._.gem.showDropItemTxtEF(exp, pos1, Color.green);
        GM._.gem.showDropItemTxtEF(coin, pos2, Color.yellow);

        //* 答え
        answerBtn[idx].GetComponent<Image>().color = Color.yellow;
        GM._.charaAnimByAnswer(isCorret: true);

        quizGroup.SetActive(false);
        hintFrame.SetActive(false);
        yield return new WaitForSeconds(3f);
        quizGroup.SetActive(true);

        initBtnColor();

        //TODO EFFECT
        /*
        //* Success Effect
        // GM._.SuccessEFAnim.gameObject.SetActive(true);

        // yield return new WaitForSeconds(4);
        // GM._.rigidPopStuffObjs();

        // yield return new WaitForSeconds(1.5f);
        //* success Result
        // successResultFrame.SetActive(true);
        */
    }
    public IEnumerator coFailAnswer(int idx) {
        GM._.cam.Anim.SetTrigger(Enum.ANIM.DoCamShake.ToString());
        StartCoroutine(GM._.Anm.coActiveAnimalHeartBreakEF());
        answerBtn[idx].GetComponent<Image>().color = Color.red;
        GM._.charaAnimByAnswer(isCorret: false);
        hintFrame.SetActive(true);
        interactableAnswerBtns(false);

        yield return Util.time0_5;
        interactableAnswerBtns(true);
    }

#endregion
}
