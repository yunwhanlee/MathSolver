using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;
using TexDrawLib.Samples;
using TMPro;


public enum Status { WAITING, DIAGNOSIS, LEARNING }
public class WJ_Sample : MonoBehaviour
{
    const int BTN_CNT = 3;

    [SerializeField] WJ_Connector wj_connector;
    [SerializeField] Status status;  public Status Status => status;

    [Header("PANEL")]
    [SerializeField] GameObject diagChooseDiffPanel;  //난이도 선택 패널
    [SerializeField] GameObject questionPanel;         //문제 패널(진단,학습)
    [SerializeField] TextMeshProUGUI questionDescriptionTxt;        //문제 설명 텍스트
    [SerializeField] TEXDraw questionEquationTxtDraw;           //문제 텍스트(※TextDraw로 변경 필요)
    [SerializeField] Button[] btAnsr = new Button[BTN_CNT]; //정답 버튼들
    TEXDraw[] textAnsr;                  //정답 버튼들 텍스트(※TextDraw로 변경 필요)

    [Header("STATUS")]
    [SerializeField] int curQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("DEBUG")]
    [SerializeField] WJ_DisplayText wj_displayText; //* 텍스트 표시용(필수X)
    [SerializeField] Button getLearningButton; //* 문제 받아오기 버튼

    private void Awake() {
        //* Init
        diagChooseDiffPanel.SetActive(false);
        questionPanel.SetActive(false);

        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("대기중", "", "", "");
    }

    void OnEnable() => Setup();

    void Update() {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private void Setup() {
        switch (status) {
            case Status.WAITING:
                diagChooseDiffPanel.SetActive(true);
                break;
        }

        if (wj_connector) {
            wj_connector.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_connector.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
    }

    /// <summary>
    //* 실력 진단평가 문제 받아오기 (초기 한번만 실행)
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
                break;
        }
    }

    /// <summary>
    //*  n 번째 학습 문제 받아오기
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
    IEnumerator coDisplayQuestion(string title, string qstEquation, string qstCorrectAnswer, string qstWrongAnswers) {
        Debug.Log("WJ_Sample:: coDisplayQuestion(title, qstEquation, qstCorrectAnswer, qstWrongAnswers)::"
            + "\n title= " + title //* タイトル
            + "\n question Equation= " + qstEquation //* 数学式
            + "\n question Correct Answer= " + qstCorrectAnswer //* 正解 一つ
            + "\n question Wrong Answers= " + qstWrongAnswers //* 誤答 二つ
        );
        //* 処理
        diagChooseDiffPanel.SetActive(false);
        yield return GM._.gui.coShowStageTxt(curQuestionIndex);
        yield return coMakeQuestion(title, qstEquation, qstCorrectAnswer, qstWrongAnswers);
    }

    IEnumerator coMakeQuestion(string title, string qstEquation, string qstCorrectAnswer, string qstWrongAnswers) {
        questionPanel.SetActive(true);

        string correctAnswer;
        string[] wrongAnswers;

        questionDescriptionTxt.text = title;
        questionEquationTxtDraw.text = qstEquation;

        correctAnswer = qstCorrectAnswer;
        wrongAnswers = qstWrongAnswers.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, BTN_CNT-1) + 1;

        for(int i=0; i<btAnsr.Length; i++) {
            if (i < ansrCount)
                btAnsr[i].gameObject.SetActive(true);
            else
                btAnsr[i].gameObject.SetActive(false);
        }

        int ansrIndex = Random.Range(0, ansrCount);

        for(int i = 0, q = 0; i < ansrCount; ++i, ++q) {
            if (i == ansrIndex) {
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = wrongAnswers[q];
        }
        isSolvingQuestion = true;
        
        yield return null;
    }

    /// <summary>
    //* 답을 고르고 맞았는 지 체크
    /// </summary>
    public void SelectAnswer(int _idx) {
        Debug.Log($"WJ_Sample:: SelectAnswer({_idx}):: ");
        bool isCorrect;
        string ansrCwYn = "N";

        switch (status) {
            case Status.DIAGNOSIS:
                isCorrect   = textAnsr[_idx].text.CompareTo(wj_connector.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                curQuestionIndex++;

                wj_connector.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("진단평가 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");

                questionPanel.SetActive(false);
                questionSolveTime = 0;
                break;

            case Status.LEARNING:
                isCorrect   = textAnsr[_idx].text.CompareTo(wj_connector.cLearnSet.data.qsts[curQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn    = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                curQuestionIndex++;

                wj_connector.Learning_SelectAnswer(curQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("문제풀이 중", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " 초");

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
#region EVENT BUTTON
//-------------------------------------------------------------------------------------------------------------
    public void ButtonEvent_ChooseDifficulty(int diffLevel) {
        Debug.Log($"WJ_Sample:: ButtonEvent_ChooseDifficulty({diffLevel})");
        status = Status.DIAGNOSIS;
        wj_connector.FirstRun_Diagnosis(diffLevel);
    }
    public void ButtonEvent_GetLearning() {
        Debug.Log($"WJ_Sample:: ButtonEvent_GetLearning()");
        wj_connector.Learning_GetQuestion();
        wj_displayText.SetState("문제풀이 중", "-", "-", "-");
    }
#endregion
}
