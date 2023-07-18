using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTalkManager : TalkManager {
    public enum TALK_ID_IDX {
        TUTORIAL_DIAG_CHOICE_DIFF
        , TUTORIAL_DIAG_FIRST_QUIZ
        , TUTORIAL_DIAG_FIRST_ANSWER
        , TUTORIAL_DIAG_RESULT
    }

    [Header("EXTRA")]
    [SerializeField] bool isTutoQuizAnswerCorret;  public bool IsTutoQuizAnswerCorret {get => isTutoQuizAnswerCorret; set => isTutoQuizAnswerCorret = value;}

    public override void generateData() {
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_CHOICE_DIFF, new string[] {
            "오! 와주었구만!:0"
            , "우선 자네의 수학능력을\n분석해야되네:0"
            , "부담 가질것 없이,\n위의 네가지 난이도중에서:0"
            , "자신에게 맞는 것을\n선택해주시게!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_QUIZ, new string[] {
            "드디어 동물친구들의\n질문이 시작되었어.:0"
            , "먼저, 화면 위쪽을보면,\n질문이 표시 된다네.:0"
            , "그리고 화면 중앙에,\n고민거리 물건이 나오지.:0"
            , "우리가 할일은..:0"
            , "이 정보들을 통해서\n아래의 세 가지 버튼중에서:0"
            , "정답을 선택하는거네!:1"
            , "한번 해보겠나?!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER, new string[] {
            "답을 선택하였구만!\n 결과는...?!:0"
            , "ANSWER:0"
            , "정답을 맞추면 가운데\n물건이 자동으로 정돈된다네:0"
            , "틀렸어도 괜찮다네!\n수학은 포기하지 않는 힘!:1"
            , "우리가 수학힌트를 줄테니\n정답을 다시 찾아보면 된다네.:1"
            , "수학고민는 총 8문제가\n출제된다네.:0"
            , "자! 그럼 이제..!:0"
            , "나머지 문제를\n풀어볼까?!:0"
            , "화이팅!!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_RESULT, new string[] {
            "마을의 수학고민을\n 다 해결해 줬구만!:0"
            , "처음인데 잘해주었어!\n정말 고생 많았네!:1"
            , "자, 다음으로\n이동 할 화면에서는:0"
            , "수학해결사 결과를\n확인 할 수 있다네.:0"
            , "경험치, 보수와 함께:0"
            , "틀리지 않고 맞춘 정답 수 만큼:0"
            , "하늘의 별을 획득 할 수 있다네!:0"
            , "그럼, 결과를\n확인하러 가볼까?!:1"
        });
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected override string setEvent(int id) {
        Debug.Log($"GameTalkManager:: setEvent(id={id}):: talkIdx= {talkIdx}");
        string rawMsg = "";
        switch(id) {
            case (int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER:
                if(talkIdx == 1) {
                    string msg = talkDt[(int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER][1];
                    rawMsg = msg.Replace("ANSWER", isTutoQuizAnswerCorret? "<color=blue>와우 정답이라네!</color>" : "<color=red>아쉽게 틀렸구먼..</color>");
                }
                break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }

    protected override void endSwitchProccess(int id) {
        switch(id) {
            case 0: DB.Dt.IsTutoDiagChoiceDiffTrigger = false;
                break;
            case 1: DB.Dt.IsTutoDiagFirstQuizTrigger = false; 
                break;
            case 2: DB.Dt.IsTutoDiagFirstAnswerTrigger = false; 
                break;
            case 3: DB.Dt.IsTutoDiagResultTrigger = false; 
                break;
        }
    }
#endregion
}
