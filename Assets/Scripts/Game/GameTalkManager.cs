using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTalkManager : MonoBehaviour {
    public enum TALK_ID_IDX {
        TUTORIAL_DIAG_CHOICE_DIFF
        , TUTORIAL_DIAG_FIRST_QUIZ
        , TUTORIAL_DIAG_FIRST_ANSWER
        , TUTORIAL_DIAG_RESULT
    }
    enum SPEAKER_IDX {FRONTOUTH_BOY, PLAYER};

    [Header("BUTTON TYPE")]
    [Header("OBJECT TYPE")]

    //* DATA
    Dictionary<int, string[]> talkDt;
    [SerializeField] List<Sprite> speakerSprDtList;

    //* Speaker Spr
    [SerializeField] Sprite frontoothBoySpr;

    //* Value
    [SerializeField] bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] int curId;
    [SerializeField] int talkIdx;
    [SerializeField] GameObject talkDialog;
    [SerializeField] TextMeshProUGUI talkTxt;
    [SerializeField] Image speakerImg;

    [SerializeField] bool isTutoQuizAnswerCorret;  public bool IsTutoQuizAnswerCorret {get => isTutoQuizAnswerCorret; set => isTutoQuizAnswerCorret = value;}

    void Awake() {
        //* 対話データ
        talkDt = new Dictionary<int, string[]>();
        generateData();
    }

    void Start() {
        //* キャラの画像データ
        speakerSprDtList = new List<Sprite>();
        speakerSprDtList.Add(frontoothBoySpr);
        speakerSprDtList.Add(HM._.pl.IdleSpr);
    }

    void generateData() {
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_CHOICE_DIFF, new string[] {
            "자네! 와주었구만!:1"
            , "우선 자네의 수학능력을\n분석해야 된다네.:1"
            , "부담 가질것 없이,\n위의 네가지 난이도중에서:1"
            , "자신에게 맞는 것을\n선택해주시게!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_QUIZ, new string[] {
            "드디어 동물친구들의\n질문이 시작되었네!:1"
            , "먼저, 화면 위쪽을보면,\n친구들의 질문이 있다네:1"
            , "그리고 화면 가운데에,\n고민거리 물건이 나오지.:1"
            , "우리가 할일은..:1"
            , "이 정보들을 가지고\n아래의 세가지 버튼중에서:1"
            , "정답을 선택하는거네!:1"
            , "한번 해보겠나?!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER, new string[] {
            "답을 선택하였구만!\n 결과는...?!:1"
            , "ANSWER:1"
            , "정답을 맞추면 가운데\n물건이 자동으로 정돈된다네:1"
            , "틀렸어도 괜찮다네!\n수학은 포기하지 않는 힘!:1"
            , "우리가 수학힌트를 줄테니\n정답을 다시 찾아보면 된다네.:1"
            , "수학고민는 총 8문제가\n출제된다네.:1"
            , "자! 그럼 이제..!:1"
            , "나머지 문제를\n풀어볼까?!:1"
            , "화이팅!!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_DIAG_RESULT, new string[] {
            "마을의 수학고민을\n 다 해결해 줬구만!:1"
            , "처음인데 잘해주었어!\n정말 고생 많았네!:1"
            , "자, 다음에 나오는\n화면에서는.:1"
            , "수학해결사 결과를\n확인 할 수 있다네.:1"
            , "경험치, 보수와 함께,:1"
            , "틀리지 않고 맞춘 정답 수 만큼:1"
            , "하늘의 별을 획득 할 수 있다네!:1"
            , "그럼, 결과표를\n확인하러 가볼까?!:1"
        });
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    //* TalkDialogのPlayActionBtnへ張り付ける
    public void onClickPlayActionBtn() => play(); 
    //* 対話開始をボタンイベントでする時、使います
    public void onClickRegistActionBtn(int id) => action(id); 
#endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void action(int id) {
        curId = id;
        play(); //* 最初スタート
    }
    public void play() {
        talk(curId);
        talkDialog.SetActive(isAction);
    }

    private void talk(int id) {
        string rawMsg = getMsg(id, talkIdx);

            //* データ必要な対話 処理
            switch(id) {
                case 2:
                    if(talkIdx == 1) {
                        string msg = talkDt[(int)TALK_ID_IDX.TUTORIAL_DIAG_FIRST_ANSWER][1];
                        rawMsg = msg.Replace("ANSWER", isTutoQuizAnswerCorret? "<color=blue>와우 정답이라네!</color>" : "<color=red>아쉽게 틀렸구먼!</color>");
                    }
                    break;
            }

        //* 対話終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 終了後、追加処理
            switch(id) {
                case 0: DB.Dt.IsTutoDiagChoiceDiffTrigger = false; break;
                case 1: DB.Dt.IsTutoDiagFirstQuizTrigger = false; break;
                case 2: DB.Dt.IsTutoDiagFirstAnswerTrigger = false; break;
                case 3: DB.Dt.IsTutoDiagResultTrigger = false; break;
            }
            return;
        }
        //* 対話表示
        else {
            Time.timeScale = 0;
            //* 分析 「メッセージ」と「スピーカー画像」
            string msg = rawMsg.Split(":")[0];
            string spkKey = rawMsg.Split(":")[1];
            //* メッセージ
            talkTxt.text = msg;

            //* スピーカー画像
            // switch(int.Parse(spkKey)) {
            //     //TODO
            // }
        }
        
        //* 次の対話準備
        isAction = true;
        talkIdx++;
    }

    private string getMsg(int id, int talkIdx) {
        string[] msgs = talkDt[id];
        if(talkIdx == msgs.Length)
            return null;
        else 
            return msgs[talkIdx];
    }
#endregion
}
