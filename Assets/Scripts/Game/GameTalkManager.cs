using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTalkManager : TalkManager {
    public enum ID {
        TUTO_DIAG_CHOICE_DIFF
        , TUTO_DIAG_FIRST_QUIZ
        , TUTO_DIAG_FIRST_ANSWER
        , TUTO_DIAG_RESULT
    }

    [Header("EXTRA")]
    [SerializeField] bool isTutoQuizAnswerCorret;  public bool IsTutoQuizAnswerCorret {get => isTutoQuizAnswerCorret; set => isTutoQuizAnswerCorret = value;}

    public override void generateData() {
        talkDt.Add((int)ID.TUTO_DIAG_CHOICE_DIFF, new string[] {
            "왔구나!\n첫 시간인 만큼..:0"
            , "수학실력 파악과 함께\n플레이 방법을 설명해줄게!:0"
            , "위의 4가지 난이도 중:0"
            , "자신에게 맞는 것을\n골라줘!:1"
        });
        talkDt.Add((int)ID.TUTO_DIAG_FIRST_QUIZ, new string[] {
            "드디어 수학해결사 일이 시작되었어!:0"
            , "화면 위쪽을보면,\n문제 질문이 나오구,:0"
            , "중앙에는 관련된 물체가 나올거야.:0"
            , "우리가 할 일은..\n이 정보들로:0"
            , "아래 3가지 버튼중에서\n정답을 고르는거야!:1"
            , "정답을 선택해 줘!:0"
        });
        talkDt.Add((int)ID.TUTO_DIAG_FIRST_ANSWER, new string[] {
            "결과는...?!:0"
            , "ANSWER:0"
            , "정답을 맞추면, 화면 맨아래 별을 획득!:0"
            , "동시에 물체도 정답에 맞게 정돈 되.:0"
            , "틀렸어도 괜찮아!:0"
            , "수학식 힌트와 함께\n다시 정답을 선택 할 수 있어.:1"
            , "그럼 이제\n나머지 문제를 해결해볼까?:0"
            , "화이팅!! 끼에에엑:1"
        });
        talkDt.Add((int)ID.TUTO_DIAG_RESULT, new string[] {
            "와우, 8문제를\n전부 해결했어!:0"
            , "처음인데 잘해주었어!:1"
            , "이제 결과를 확인하러 가즈아!!:0"
        });
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected override string setEvent(int id) {
        Debug.Log($"GameTalkManager:: setEvent(id={id}):: talkIdx= {talkIdx}");
        string rawMsg = "";
        switch(id) {
            case (int)ID.TUTO_DIAG_FIRST_ANSWER:
                if(talkIdx == 1) {
                    string msg = talkDt[(int)ID.TUTO_DIAG_FIRST_ANSWER][1];
                    rawMsg = msg.Replace("ANSWER", isTutoQuizAnswerCorret? "<color=blue>와우 정답!</color>" : "<color=red>아쉽게 틀렸어..</color>");
                }
                break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }

    protected override void endSwitchProccess(int id) {
        switch(id) {
            case 0:
                DB.Dt.IsTutoDiagChoiceDiffTrigger = false;
                break;
            case 1:
                DB.Dt.IsTutoDiagFirstQuizTrigger = false;
                break;
            case 2:
                DB.Dt.IsTutoDiagFirstAnswerTrigger = false;
                break;
            case 3:
                DB.Dt.IsTutoDiagResultTrigger = false;
                break;
        }
    }
#endregion
}
