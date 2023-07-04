using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using TexDrawLib.Samples; //* TEXDraw
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class GUI : MonoBehaviour
{
    IEnumerator coTxtTeleTypeID;
    TextTeleType txtTeleType;

    // public Button[] answerBtns; public Button[] AnswerBtns {get => answerBtns; set => answerBtns = value;}
    // [SerializeField] GameObject questionFrame;  public GameObject QuestionFrame {get => questionFrame; set => questionFrame = value;}
    // [SerializeField] GameObject hintFrame;  public GameObject HintFrame {get => hintFrame; set => hintFrame = value;}
    [SerializeField] GameObject successResultFrame;  public GameObject SuccessResultFrame {get => successResultFrame; set => successResultFrame = value;}
    [SerializeField] GameObject successEffectFrame;  public GameObject SuccessEffectFrame {get => successEffectFrame; set => successEffectFrame = value;}
    // [SerializeField] TextMeshProUGUI quizTxt; public TextMeshProUGUI QuizTxt {get => quizTxt; set => quizTxt = value;}
    // [SerializeField] TextMeshProUGUI stageTxt;  public TextMeshProUGUI StageTxt {get => stageTxt; set => StageTxt = value;}

    void Start() {
        txtTeleType = GetComponent<TextTeleType>();

        // var pb = GM._.Problems[0];

        // stageTxt.gameObject.SetActive(false);
        // questionFrame.SetActive(false);
        // hintFrame.SetActive(false);
        successEffectFrame.SetActive(false);

        //* 質問文章
        // quizTxt.text = pb.sentence;

        //* Answer Buttons 初期化
        // for(int i = 0; i < answerBtns.Length; i++) {
        //     answerBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = pb.answers[i].ToString();
        //     answerBtns[i].gameObject.SetActive(false);
        // }
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void onclickBtnBackToHome() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    // public IEnumerator coShowStageTxt(int curQuestionIndex) {
    //     int stageNum = curQuestionIndex + 1;
    //     GM._.qm.StageTxt.text = $"STAGE {stageNum} / 8";
    //     GM._.qm.StageTxt.gameObject.SetActive(true);
        
    //     yield return new WaitForSeconds(1.3f);
    //     GM._.qm.StageTxt.gameObject.SetActive(false);
    // }
    public IEnumerator coShowQuestion(string qstTEXTDraw) {
        // GM._.CustomerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID);
        
        //* TEXTDraw 分析
        List<string> analList = AnalyzeTEXTDraw(qstTEXTDraw);

        //* ストーリーテリング・オブジェクト生成
        GM._.qm.QuizTxt.text = GM._.qstSO.makeQuizSentence(analList);

        //* テレタイプ
        coTxtTeleTypeID = txtTeleType.coTextVisible(GM._.qm.QuizTxt);
        StartCoroutine(coTxtTeleTypeID);
        
        yield return new WaitForSeconds(1.5f);
        GM._.PlThinkingEFObj.SetActive(true);
    }

    private List<string> AnalyzeTEXTDraw(string qstEquation) {
        const string X_EQUATION_DELETE_PART = ", x, =, ?";
        List<string> resList = new List<string>();
        //* & -> ""
        string filterTxt = qstEquation.Replace("&", "");
        //* 正規表現で 必要な部分だけ リスト
        MatchCollection matches = Regex.Matches(filterTxt, Config.TEXTDRAW_REGEX_PATTERN);
        resList.AddRange(matches.Cast<Match>().Select(match => match.Value));
        //* リスト ➝ 文字列に変換
        string listStr = string.Join(", ", resList.ToArray());
        //* X方程式なら、後ろに要らない部分を消す
        if(listStr.Contains(X_EQUATION_DELETE_PART)) listStr = listStr.Replace(X_EQUATION_DELETE_PART, "");
        Debug.Log($"AnalyzeTEXTDraw:: listStr= <color=white>{listStr}</color>");
        //* 文字列 ➝ リストに戻す
        resList = listStr.Split(", ").ToList();
        return resList;
    }
#endregion
}
