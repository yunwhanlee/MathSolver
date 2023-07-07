using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : MonoBehaviour {
    enum TALK_ID_IDX {FRONTOUTH_BOY, TUTORIAL_FIRST};
    enum SPEAKER_IDX {FRONTOUTH_BOY, PLAYER};

    [Header("BUTTON TYPE")]
    [SerializeField] GameObject TutorialBeginPanelBtn;
    // [SerializeField] Button tutorialBeginPanelBtn;

    [Header("OBJECT TYPE")]
    //TODO
    
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
        //* TEST用
        talkDt.Add((int)TALK_ID_IDX.FRONTOUTH_BOY, new string[] {
            "아닛! 나를 찾아내다니.. \n옛다 10000코인!:0"
            , "더 필요하면 언제든 오시게나. \n담백하다!:0"
        });
        //* チュートリアル用
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_FIRST, new string[] {
            "반갑네! 자네가 이번에 새로 부임한 \n수학조수구만!:1"
            , "나는 동물마을의 수학해결사\n 늑선생이라네.:1"
        });
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickPlayActionBtn() => playAction();
    public void onClickRegistActionBtn(int id) => registAction(id);
#endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void registAction(int id) {
        curId = id;
        playAction(); //* 最初スタート
    }
    public void playAction() {
        talk(curId);
        talkDialog.SetActive(isAction);
    }

    private void talk(int id) {
        string rawMsg = getMsg(id, talkIdx);

        //* 対話終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 追加処理
            switch(id) {
                case 0: HM._.ui.test_GetCoinFromFrontouthBoy(); break;
                case 1: TutorialBeginPanelBtn.SetActive(false); break;
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
            switch(int.Parse(spkKey)) {
                case 0: speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.FRONTOUTH_BOY]; break;
                case 1: speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.PLAYER]; break;
            }
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
