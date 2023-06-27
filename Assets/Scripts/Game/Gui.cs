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

    public Button[] answerBtns; public Button[] AnswerBtns {get => answerBtns; set => answerBtns = value;}
    [SerializeField] GameObject questionFrame;  public GameObject QuestionFrame {get => questionFrame; set => questionFrame = value;}
    [SerializeField] GameObject hintFrame;  public GameObject HintFrame {get => hintFrame; set => hintFrame = value;}
    [SerializeField] GameObject successResultFrame;  public GameObject SuccessResultFrame {get => successResultFrame; set => successResultFrame = value;}
    [SerializeField] GameObject successEffectFrame;  public GameObject SuccessEffectFrame {get => successEffectFrame; set => successEffectFrame = value;}
    [SerializeField] TextMeshProUGUI quizTxt; public TextMeshProUGUI QuizTxt {get => quizTxt; set => quizTxt = value;}
    [SerializeField] TextMeshProUGUI stageTxt;  public TextMeshProUGUI StageTxt {get => stageTxt; set => StageTxt = value;}


    void Start() {
        txtTeleType = GetComponent<TextTeleType>();

        var pb = GM._.Problems[0];

        // stageTxt.gameObject.SetActive(false);
        questionFrame.SetActive(false);
        hintFrame.SetActive(false);
        successEffectFrame.SetActive(false);

        //* 質問文章
        // quizTxt.text = pb.sentence;

        //* Answer Buttons 初期化
        for(int i = 0; i < answerBtns.Length; i++) {
            answerBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = pb.answers[i].ToString();
            answerBtns[i].gameObject.SetActive(false);
        }
    }

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void onclickBtnBackToHome() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public IEnumerator coShowStageTxt(int curQuestionIndex) {
        int stageNum = curQuestionIndex + 1;
        stageTxt.text = $"STAGE {stageNum} / 8";
        stageTxt.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1.3f);
        stageTxt.gameObject.SetActive(false);
    }
    public IEnumerator coShowQuestion(string qstTEXTDraw) {
        GM._.CustomerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID);
        // questionFrame.gameObject.SetActive(true);
        //* TEXTDraw 分析
        List<string> analList = AnalyzeTEXTDraw(qstTEXTDraw);
        
        //* 演算子
        string sign = analList.Find(s => s=="+"||s=="-"||s=="times"||s=="frac");
        analList.Remove(sign);

        //* ストーリーテリング
        quizTxt.text = GM._.qstSO.makeQuizSentence(sign, analList);

        //* テレタイプ
        coTxtTeleTypeID = txtTeleType.coTextVisible(quizTxt);
        StartCoroutine(coTxtTeleTypeID);

        //* オブジェクト生成

        // yield return new WaitForSeconds(1);
        // for(int i = 0; i < answerBtns.Length; i++) 

        //     answerBtns[i].gameObject.SetActive(true);
        
        yield return new WaitForSeconds(1.5f);
        GM._.PlThinkingEFObj.SetActive(true);
    }

    private List<string> AnalyzeTEXTDraw(string qstEquation) {
        List<string> resList = new List<string>();
        //* Analyze TEXTDraw To Simple Math Equation
        string filterTxt = qstEquation.Replace("&", "");
        
        //* +, -, x(times), ÷(frac), 整数
        string pattern = @"[-+?]|times|frac|\d+"; 
        MatchCollection matches = Regex.Matches(filterTxt, pattern);
        resList.AddRange(matches.Cast<Match>().Select(match => match.Value));
        resList.ForEach(li => Debug.Log("AnalyzeTEXTDraw():: li= " + li));
        return resList;
    }
    public IEnumerator coFailAnswer() {
        GM._.PlayerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        GM._.CustomerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        questionFrame.SetActive(false);
        yield return new WaitForSeconds(1.2f);
        //* Retry With Hint
        questionFrame.SetActive(true);
    }
    public IEnumerator coSuccessAnswer() {
        GM._.PlayerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        GM._.CustomerAnim.SetTrigger(Enum.ANIM.DoBounce.ToString());
        questionFrame.SetActive(false);
        hintFrame.SetActive(false);

        yield return new WaitForSeconds(1.2f);
        //* Success Effect
        GM._.SuccessEFAnim.gameObject.SetActive(true);

        yield return new WaitForSeconds(4);
        GM._.rigidPopStuffObjs();

        yield return new WaitForSeconds(1.5f);
        //* success Result
        successResultFrame.SetActive(true);
    }
    public void onClickAnswerBtn(int idx) {
        GM._.PlThinkingEFObj.SetActive(false);
        int val = int.Parse(answerBtns[idx].GetComponentInChildren<TextMeshProUGUI>().text);
        if(val == GM._.Problems[0].res) {
            Debug.Log("正解！");
            answerBtns[idx].GetComponent<Image>().color = Color.yellow;

            //* Chara Expression
            GM._.PlayerSprRdr.sprite = GM._.PlayerSprs[(int)Enum.EXPRESSION.Success];
            GM._.CustomerSprRdr.sprite = GM._.CustomerSprs[(int)Enum.EXPRESSION.Success];

            //* BG Expression
            GM._.Cloud1ExpressSprRdr.sprite = GM._.Cloud1Sprs[(int)Enum.EXPRESSION.Success];
            GM._.Cloud2ExpressSprRdr.sprite = GM._.Cloud2Sprs[(int)Enum.EXPRESSION.Success];
            GM._.SunExpressSprRdr.sprite = GM._.SunSprs[(int)Enum.EXPRESSION.Success];

            StartCoroutine(coSuccessAnswer());
        }
        else {
            Debug.Log("バツ！");
            answerBtns[idx].GetComponent<Image>().color = Color.red;

            //* Hint
            hintFrame.SetActive(true);

            //* Chara Expression
            GM._.PlayerSprRdr.sprite = GM._.PlayerSprs[(int)Enum.EXPRESSION.Fail];
            GM._.CustomerSprRdr.sprite = GM._.CustomerSprs[(int)Enum.EXPRESSION.Fail];

            //* BG Expression
            GM._.Cloud1ExpressSprRdr.sprite = GM._.Cloud1Sprs[(int)Enum.EXPRESSION.Fail];
            GM._.Cloud2ExpressSprRdr.sprite = GM._.Cloud2Sprs[(int)Enum.EXPRESSION.Fail];
            GM._.SunExpressSprRdr.sprite = GM._.SunSprs[(int)Enum.EXPRESSION.Fail];

            StartCoroutine(coFailAnswer());
        }
    }
#endregion
}
