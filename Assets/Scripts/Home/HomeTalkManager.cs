using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : TalkManager {
    readonly Vector2 WOOD_ARROW_POS = new Vector2(300, 770);
    readonly Vector2 FOOT_HOLD_POS = new Vector2(100, -950);
    readonly Vector2 STAR_MOUNTAION_POS = new Vector2(-120, -250);
    readonly Vector2 MENUTAP_QUEST_ICON_POS = new Vector2(500, -650);
    readonly Vector2 QUEST_CATE_POS = new Vector2(100, 550);
    readonly Vector2 QUEST_REWARD_BTN_POS = new Vector2(450, 250);


    public enum ID {
        TUTO_ROOM, TUTO_FUNITURESHOP, TUTO_CLOTHSHOP, TUTO_INV, TUTO_GOGAME, TUTO_WORLDMAP, TUTO_FINISH,
        UNLOCK_MAP1_WINDMILL, UNLOCK_MAP1_ORCHARD, OPEN_JUNGLE_MAP2,
    };

    [Header("EXTRA")]
    [SerializeField] GameObject TutorialRoomPanelBtn;

    void Start() {
        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    }

    public override void generateData() {
        #region TUTORIAL
        //* チュートリアル用
        talkDt.Add((int)ID.TUTO_ROOM, new string[] {
            "만나서 반가워!\n새로온 수학조수구나!:0"
            , "이름이 어떻게되니?:0"
            , "...:0"
            , $"NICKNAME!\n정말 멋진 이름이야!:1"
            , "나는 이 마을 수학해결사 늑선생이야. 잘 부탁해!:0"
            , "사실... 비밀인데...:0"
            , "돌부리에 걸려 넘어진 뒤로 머리를 다쳐서:0"
            , "수학 기억이 잘 안나..:2"
            , "나와함께 여러지역의 수학문제를 해결하고:0"
            , "잊어버린 수학의기억조각 찾는 것을 도와줘!:0"
            , "이곳은 우리 사무실!:0"
            , "여러가구를 구입하고\n꾸밀 수 있어!:0"
            , "위쪽 나무판 화살표를 눌러보겠니?:0"
        });
        talkDt.Add((int)ID.TUTO_FUNITURESHOP, new string[] {
            "이곳은 장인 도톨씨의 가구점이야.:0"
            , "어서오시게!:6"
            , "의자와 탁자, 장식품, 벽지, 매트 등:6"
            , "다양한 가구를 판매하고있으니\n자주 들러주게나!:6"
            , "알겠지?\n다시 화살표를 눌러줘!:0"
        });
        talkDt.Add((int)ID.TUTO_CLOTHSHOP, new string[] {
            "이곳은 뭉이어멈의 의류점이야.:0"
            , "어머어머 반가워요~!:7"
            , "저희가게는 스킨과 펫을 팔고있어요. 호호:7"
            , "그때그때 물건이\n들어오는게 랜덤이라:7"
            , "뭐가 나올지 저도 모른답니다. 호호:7"
            , "자주 놀러와요~!\n호홍:7"
            , "자!\n다시 화살표를 눌러줘!:0"
        });
        talkDt.Add((int)ID.TUTO_INV, new string[] {
            "여기는 인벤토리야.:0"
            , "스킨과 펫의\n변경이 가능해!:0"
            , "화살표를 눌러\n처음으로 돌아가자!:0"
        });
        talkDt.Add((int)ID.TUTO_GOGAME, new string[] {
            "흠.. 어디보자..:0"
            , "동물친구들을\n도와주러 갈겸,:0"
            , "수학실력을\n한 번 파악해볼까?:1"
            , "대화가 종료되면\n손가락이 발판을 가리킬거야.:0"
            , "자 그럼.. 가볼까?!:1"
        });
        talkDt.Add((int)ID.TUTO_WORLDMAP, new string[] {
            "이곳은 월드맵이야.:0"
            , "지금은 한 지역만 갈 수 있지만:2"
            , "레벨이 오를수록 더 많은 지역이 열릴거야!:0"
            , "돌아가고 싶다면, 우측 아래의 집을 클릭하면되!:0"
            , "자 그럼, 손가락이\n가리킨 지역을 클릭해 줘!:0"
        });
        talkDt.Add((int)ID.TUTO_FINISH, new string[] {
            "수고 많았어!:0"
            , "첫 해결사 일인데도\n잘 하던데?!:1"
            , "보상을 받으러 가볼까?\n퀘스트창을 열어보자.:0"
            , "손가락이 가리키는\n아이콘을 누른 뒤,:0"
            , "퀘스트 카테고리 선택.:0"
            , "보상버튼을 클릭해 줘!:0"
        });
        #endregion

        #region FOREST
        talkDt.Add((int)ID.TUTO_FINISH, new string[] {
            ":0"
        });
        #endregion
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region SET EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected override string setEvent(int id) {
        Debug.Log($"GameTalkManager:: setEvent(id={id}):: talkIdx= {talkIdx}");

        string rawMsg = "";
        switch(id) {
            case (int)ID.TUTO_ROOM:
                if(talkIdx == 2) {
                    Time.timeScale = 1;
                    HM._.ui.showNickNamePopUp(isActive: true);
                }
                else if(talkIdx == 3) {
                    string msg = talkDt[(int)ID.TUTO_ROOM][3];
                    rawMsg = msg.Replace("NICKNAME", $"<color=blue>{DB.Dt.NickName}</color>");
                    Time.timeScale = 0;
                }
                if(talkIdx == 12) activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_FUNITURESHOP:
                if(talkIdx == 4) activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_CLOTHSHOP:
                if(talkIdx == 6) activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_INV:
                if(talkIdx == 2) activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_GOGAME:
                // なし
                break;
            case (int)ID.TUTO_WORLDMAP:
                if(talkIdx == 0) tutoHandFocusTf.gameObject.SetActive(false);
                else if(talkIdx == 4) activeHandFocus(STAR_MOUNTAION_POS);
                break;
            case (int)ID.TUTO_FINISH:
                if(talkIdx == 3) {
                    talkFrameTf.anchoredPosition = new Vector2(0, 500);
                    HM._.ui.MenuTapFrame.anchoredPosition = new Vector2(0, HM._.ui.MenuTapFrame.anchoredPosition.y);
                    activeHandFocus(MENUTAP_QUEST_ICON_POS);
                }
                else if(talkIdx == 4) {
                    talkFrameTf.anchoredPosition = new Vector2(0, 0);
                    HM._.ui.onClickAchiveRankIconBtn();
                    HM._.ui.onClickAchiveRankTypeBtn(1);
                    activeHandFocus(QUEST_CATE_POS);
                }
                else if(talkIdx == 5) {
                    activeHandFocus(QUEST_REWARD_BTN_POS);
                }
                break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }
/// 
    protected override void endSwitchProccess(int id) {
        switch(id) {
            case (int)ID.TUTO_ROOM:
                TutorialRoomPanelBtn.SetActive(false); 
                DB.Dt.IsTutoRoomTrigger = false;
                tutoHandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_FUNITURESHOP);
                break;
            case (int)ID.TUTO_FUNITURESHOP:
                DB.Dt.IsTutoFunitureShopTrigger = false;
                tutoHandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_CLOTHSHOP);
                break;
            case (int)ID.TUTO_CLOTHSHOP: 
                DB.Dt.IsTutoClothShopTrigger = false;
                tutoHandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_INV);
                break;
            case (int)ID.TUTO_INV:
                DB.Dt.IsTutoInventoryTrigger = false;
                tutoHandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                break;
            case (int)ID.TUTO_GOGAME:
                DB.Dt.IsTutoGoGameTrigger = false;
                activeHandFocus(FOOT_HOLD_POS);
                break;
            case (int)ID.TUTO_WORLDMAP:
                DB.Dt.IsTutoWorldMapTrigger = false;
                tutoHandFocusTf.gameObject.SetActive(false);
                break;
            case (int)ID.TUTO_FINISH:
                DB.Dt.IsTutoFinishTrigger = false;
                break;
        }
    }
    #endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void activeHandFocus(Vector2 pos) {
        tutoHandFocusTf.gameObject.SetActive(true);
        tutoHandFocusTf.anchoredPosition = pos;
    }
#endregion
}
