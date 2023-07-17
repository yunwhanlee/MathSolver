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

    [Header("PANEL")] 
    [SerializeField] GameObject quizPanel;  public GameObject QuizPanel {get => quizPanel; set => quizPanel = value;}
    [SerializeField] GameObject resultPanel;  public GameObject ResultPanel {get => resultPanel; set => resultPanel = value;}
    [SerializeField] GameObject successResultFrame;  public GameObject SuccessResultFrame {get => successResultFrame; set => successResultFrame = value;}
    [SerializeField] GameObject successEffectFrame;  public GameObject SuccessEffectFrame {get => successEffectFrame; set => successEffectFrame = value;}

    [Header("CANVAS ANIM")] 
    [SerializeField] GameObject blackPanel;
    [SerializeField] Canvas canvasAnim;
    [SerializeField] Animator switchScreenAnim; public Animator SwitchScreenAnim {get => switchScreenAnim;}
    [SerializeField] GameObject sceneSpawnBGAnim;
    [SerializeField] Animator helpPanelAnim;    public Animator HelpPanelAnim {get => helpPanelAnim;}

    [Header("DEBUG")] 
    [SerializeField] TextMeshProUGUI isCreatingQuizObjTxt;

    IEnumerator Start() {
        //TEST
        helpPanelAnim.SetInteger(Enum.ANIM.HelpGCD.ToString(), ++GM._.qm.HelpAnimPlayIdx);

        txtTeleType = GetComponent<TextTeleType>();

        //* Black Out Spawn Anim
        blackPanel.SetActive(true);
        yield return Util.time0_2; //! なぜか BlackOutがすぐ始まると、Imageが見えなくなる。
        blackPanel.SetActive(false);
        switchScreenAnim.gameObject.SetActive(true);
        switchScreenAnim.SetTrigger(Enum.ANIM.BlackOut.ToString());
        //* BG Spawn Anim
        yield return Util.time0_2;
        sceneSpawnBGAnim.SetActive(true);

        //* チュートリアル
        if(DB.Dt.IsTutoDiagChoiceDiffTrigger) {
            yield return Util.time1;
            yield return Util.time0_3;
            GM._.gtm.action((int)GameTalkManager.TALK_ID_IDX.TUTORIAL_DIAG_CHOICE_DIFF);
        }
    }
//-------------------------------------------------------------------------------------------------------------
#region EVENT
//-------------------------------------------------------------------------------------------------------------
    public void onclickBtnBackToHome() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public void onClickHelpPanelAnimBtn() {
        Debug.Log("onClickHelpPanelAnimBtn");
        if(GM._.qm.HelpAnimType == "frac") {
            const int MAX_IDX = 2;
            GM._.gui.HelpPanelAnim.SetInteger(Enum.ANIM.HelpFraction.ToString(), GM._.qm.HelpAnimPlayIdx++);
            if(GM._.qm.HelpAnimPlayIdx > MAX_IDX) StartCoroutine(coFinishHelpAnim());
        }
        else if(GM._.qm.HelpAnimType == "underline") {
            const int MAX_IDX = 3;
            GM._.gui.HelpPanelAnim.SetInteger(Enum.ANIM.HelpGCD.ToString(), GM._.qm.HelpAnimPlayIdx++);
            if(GM._.qm.HelpAnimPlayIdx > MAX_IDX) StartCoroutine(coFinishHelpAnim());
        }
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public IEnumerator coShowQuestion(string qstTEXTDraw) {
        if(coTxtTeleTypeID != null) StopCoroutine(coTxtTeleTypeID);

        //* Init Trigger
        GM._.IsSelectCorrectAnswer = false;
        
        //* TEXTDraw 分析
        List<string> analList = AnalyzeTEXTDraw(qstTEXTDraw);

        //* ストーリーテリング・オブジェクト生成
        // GM._.qm.QuizTxt.text = GM._.makeQuiz(analList);
        StartCoroutine(GM._.coMakeQuiz(analList));
        

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
    IEnumerator coFinishHelpAnim() {
        yield return Util.realTime1;
        Time.timeScale = 1;
        helpPanelAnim.gameObject.SetActive(false);
    }
#endregion
}
