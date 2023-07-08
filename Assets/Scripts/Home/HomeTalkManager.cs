using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : MonoBehaviour {
    public enum TALK_ID_IDX {
        FRONTOUTH_BOY
        , TUTORIAL_ROOM
        , TUTORIAL_FUNITURESHOP
        , TUTORIAL_CLOTHSHOP
        , TUTORIAL_INV
        , TUTORIAL_GOGAME
    };
    enum SPEAKER_IDX {FRONTOUTH_BOY, PLAYER};

    [Header("BUTTON TYPE")]
    [SerializeField] GameObject TutorialRoomPanelBtn;
    [SerializeField] GameObject TutorialFunitureShopPanelBtn;
    [SerializeField] GameObject TutorialClothShopPanelBtn;
    [SerializeField] GameObject TutorialInventoryPanelBtn;
    [SerializeField] GameObject TutorialGoGamePanelBtn;

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
        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
        TutorialFunitureShopPanelBtn.SetActive(DB.Dt.IsTutoFunitureShopTrigger);
        TutorialClothShopPanelBtn.SetActive(DB.Dt.IsTutoClothShopTrigger);
        TutorialInventoryPanelBtn.SetActive(DB.Dt.IsTutoInventoryTrigger);

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
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_ROOM, new string[] {
            "오오! 자네가 이번에 새로온 \n수학 조수구만!:1"
            , "나는 이 별의마을의 수학해결사\n 송백 늑선생이라네.:1"
            , "수백년동안 우리 늑대가문은 \n 수학을 통해서:1"
            , "마을의 고민을\n해결해주었다네.:1"
            , "하지만... 사실..:1"
            , "난 수학을 잘 못한다네..:1"
            , "히익?!...:0"
            , "그래서 자네의 힘이 절실히 필요하네!:1"
            , "크흠..(헛 기침) :1"
            , "자 여기는 내 방안이라네.\n마을의 고민을 도와주고\n받은 코인으로:1"
            , "여러가구를 구입하고\n꾸밀 수 있지!:1"
            , "위쪽에 나무표지판\n오른쪽 화살표를 눌러보겠나?:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_FUNITURESHOP, new string[] {
            "여기는 장인 도톨씨가 운영하는\n가구점이라네.:1"
            , "성격은 괘팍하지만 꽤나 실력이 좋지.:1"
            , "의자와 탁자, 장식품, 벽지, 매트 등:1"
            , "다양한 가구를 \n구매할 수 있다네!:1"
            , "다음으로 넘어가볼까?:1"
            , "다시 나무표지판\n오른쪽 화살표를 눌러보겠나?:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_CLOTHSHOP, new string[] {
            "여기는 뭉이어멈이 하는 의류점이라네:1"
            , "그때그때 들어오는 옷이 달라서:1"
            , "구매할때마다 두근거림이 멈추지않지.:1"
            , "아 그리고 이건 비밀인데..\n가끔식 귀여운 펫도 나온다더군!:1"
            , "자, 다음으로 넘어가지.:1"
            , "나무표지판\n오른쪽 화살표를 눌러주게!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_INV, new string[] {
            "마지막으로 이곳은\n인벤토리 공간이라네:1"
            , "의류점에서 구매한\n캐릭터와 펫 변경이 가능하지!:1"
            , "펫은 지금은 없지만, \n얻게되면 같이 다닐수 있다네:1"
            , "클릭하면 춤도 추고, \n귀여운게 보는 맛이 일품이지!:1"
            , "크흠..(헛 기침) :1"
            , "홈 화면에 대한 설명은\n이것으로 끝이라네:1"
            , "다시 오른쪽 화살표를 눌러\n처음으로 돌아가보게!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_GOGAME, new string[] {
            "자.. 어디보자..:1"
            , "수학문제로 동물친구들을\n도와주러 갈겸..:1"
            , "자네의 실력을\n한번 파악해볼까?:1"
            , "!!!:0"
            , "겁먹지 말게나!\n내가 있지 않나?!:1"
            , "...:0"
            , "찬찬히,\n설명해주겠다네.:1"
            , "준비가되면, 화면아래\n발판으로 가보시게나!:1"
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

        //* 対話終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 追加処理
            switch(id) {
                case 0: 
                    HM._.ui.test_GetCoinFromFrontouthBoy(); break;
                case 1: 
                    DB.Dt.IsTutoRoomTrigger = false;
                    TutorialRoomPanelBtn.SetActive(false); break;
                case 2:
                    DB.Dt.IsTutoFunitureShopTrigger = false;
                    TutorialFunitureShopPanelBtn.SetActive(false); break;
                case 3: 
                    DB.Dt.IsTutoClothShopTrigger = false;
                    TutorialClothShopPanelBtn.SetActive(false); break;
                case 4:
                    DB.Dt.IsTutoInventoryTrigger = false;
                    TutorialInventoryPanelBtn.SetActive(false); break;
                case 5:
                    DB.Dt.IsTutoGoGameTrigger = false;
                    TutorialInventoryPanelBtn.SetActive(false); break;
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
                case 0: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.FRONTOUTH_BOY]; 
                    break;
                case 1: 
                case 2: 
                case 3: 
                case 4: 
                case 5: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.PLAYER]; 
                    break;
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
