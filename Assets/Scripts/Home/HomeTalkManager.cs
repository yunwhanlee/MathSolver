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
        , TUTORIAL_FINISH
    };
    enum SPEAKER_IDX {FRONTOOTH_BOY, PLAYER};

    [Header("BUTTON TYPE")]
    [SerializeField] GameObject TutorialRoomPanelBtn;

    
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

        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    }

    void generateData() {
        //* TEST用
        talkDt.Add((int)TALK_ID_IDX.FRONTOUTH_BOY, new string[] {
            "아닛! 나를 찾아내다니.. \n옛다 10000코인!:0"
            , "더 필요하면 언제든 찾오세요!\n담백하다!:0"
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
            , "자 그럼 공간을 이동해볼까?!:1"
            , "위쪽에 나무판 화살표를\n누르면 이동할 수 있다네!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_FUNITURESHOP, new string[] {
            "여기는 장인 도톨씨가 운영하는\n가구점이라네.:1"
            , "성격은 괴팍하지만 꽤나 실력이 좋지.:1"
            , "의자와 탁자, 장식품, 벽지, 매트 등:1"
            , "다양한 가구를 \n구매할 수 있다네!:1"
            , "다음으로 넘어가볼까?:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_CLOTHSHOP, new string[] {
            "여기는 뭉이어멈이 하는 의류점이라네:1"
            , "그때그때 들어오는 옷이 달라서:1"
            , "구매할때마다 두근거림이 멈추지않지.:1"
            , "그리고 이건 비밀인데..:1"
            , "가끔식 귀여운 펫도 나온다더군!:1"
            , "자, 다음으로 넘어가지.:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_INV, new string[] {
            "마지막으로 이곳은\n인벤토리 공간이라네:1"
            , "의류점에서 구매한\n캐릭터와 펫 변경이 가능하지!:1"
            , "펫은 지금은 없지만, \n얻게되면 같이 다닐수 있다네:1"
            , "클릭하면 춤도 추고, \n귀여운게 보는 맛이 일품이지!:1"
            , "홈 화면에 대한 설명은\n이것으로 끝이라네.:1"
            , "처음으로 돌아가볼까.:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_GOGAME, new string[] {
            "자.. 어디보자..:1"
            , "동물친구들을\n도와주러 갈겸..:1"
            , "자네의 수학실력을\n한번 파악해볼까?:1"
            , "!!!:0"
            , "겁먹지 말게나!\n내가 있지 않나?!:1"
            , "...:0"
            , "찬찬히,\n설명해주겠다네.:1"
            , "준비가되면, 화면아래\n발판으로 가보시게나!:1"
        });
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_FINISH, new string[] {
            "후... 드디어\n집에 돌아왔구만:1"
            , "첫 해결사 일이었을텐데\n고생많았네!:1"
            , "받은 보수로\n가구점이나 의류점에서:1"
            , "아이템을\n구매할 수 있다네:1"
            , "다양한 스킨과 가구를 구매해서:1"
            , "이 텅빈 집을\n아름답게 꾸며주게나!:1"
            , "자 그럼.. 이제...:1"
            , "저기요.. 잠시만요!\n멈춰!!:0"
            , "으잉?:1"
            , "중간평가용 체험판은\n여기까지입니다!:0"
            , "더 다양한 업데이트로\n다시 찾아뵙겠습니다!:0"
            , "튜토리얼을 다시 하고 싶으시다면:0"
            , "홈 화면 우측상단에\n설정아이콘을 누른 뒤:0"
            , "튜토리얼 버튼을\n눌러주세요!:0"
            , "언어설정도\n가능하지만,:0"
            , "현재는 홈 씬만\n대응 가능합니다.:0"
            , "플레이해주셔서 진심으로\n감사합니다!:0"
            , "가..감사합니다!:1"
            , "아 맞다! 저를 찾아서 누르시면:0"
            , "코인을 드립니다!\n과연 저는 어딧을까요?:0"
            , "의류점과 가구점에서\n아이템을 구매해서:0"
            , "자유롭게\n테스트 해주세요!:0"
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

        //* 対話 終了
        if(rawMsg == null) {
            Time.timeScale = 1;
            isAction = false;
            talkIdx = 0;

            //* 追加処理
            switch(id) {
                case (int)TALK_ID_IDX.FRONTOUTH_BOY: 
                    HM._.ui.test_GetCoinFromFrontouthBoy(); 
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_ROOM: 
                    TutorialRoomPanelBtn.SetActive(false); 
                    DB.Dt.IsTutoRoomTrigger = false;
                    HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                    action((int)TALK_ID_IDX.TUTORIAL_FUNITURESHOP);
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_FUNITURESHOP:
                    DB.Dt.IsTutoFunitureShopTrigger = false;
                    HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                    action((int)TALK_ID_IDX.TUTORIAL_CLOTHSHOP);
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_CLOTHSHOP: 
                    DB.Dt.IsTutoClothShopTrigger = false;
                    HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                    action((int)TALK_ID_IDX.TUTORIAL_INV);
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_INV:
                    DB.Dt.IsTutoInventoryTrigger = false;
                    HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_GOGAME:
                    DB.Dt.IsTutoGoGameTrigger = false; 
                    break;
                case (int)TALK_ID_IDX.TUTORIAL_FINISH:
                    DB.Dt.IsTutoFinishTrigger = false; 
                    break;
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
                case (int)SPEAKER_IDX.FRONTOOTH_BOY: 
                    speakerImg.sprite = speakerSprDtList[(int)SPEAKER_IDX.FRONTOOTH_BOY]; 
                    break;
                case (int)SPEAKER_IDX.PLAYER: 
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
