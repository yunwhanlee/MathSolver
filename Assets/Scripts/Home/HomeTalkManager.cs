using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : TalkManager {
    readonly Vector2 WOOD_ARROW_POS = new (300, 770);
    
    readonly Vector2 FOOT_HOLD_POS = new (100, -950);
    readonly Vector2 STAR_MOUNTAION_POS = new (-120, -250);
    readonly Vector2 MENUTAP_QUEST_ICON_POS = new (500, -650);
    readonly Vector2 QUEST_CATE_POS = new (100, 550);
    readonly Vector2 QUEST_REWARD_BTN_POS = new (450, 250);

    public enum ID {
        TUTO_ROOM, TUTO_FUNITURESHOP, TUTO_CLOTHSHOP, TUTO_INV, TUTO_GOGAME, TUTO_WORLDMAP, TUTO_FINISH,
        UNLOCK_MAP1_BG2_ACCEPT, UNLOCK_MAP1_BG2_REWARD,
        UNLOCK_MAP1_BG3_ACCEPT, UNLOCK_MAP1_BG3_REWARD,
        //* Jungle
        OPEN_MAP2_UNLOCK_BG1_ACCEPT, OPEN_MAP2_UNLOCK_BG1_REWARD,
        UNLOCK_MAP2_BG2_ACCEPT, UNLOCK_MAP2_BG2_REWARD,
        UNLOCK_MAP2_BG3_ACCEPT, UNLOCK_MAP2_BG3_REWARD,
        //* Tundra
        OPEN_MAP3_UNLOCK_BG1_ACCEPT, OPEN_MAP3_UNLOCK_BG1_REWARD,
        UNLOCK_MAP3_BG2_ACCEPT, UNLOCK_MAP3_BG2_REWARD,
        UNLOCK_MAP3_BG3_ACCEPT, UNLOCK_MAP3_BG3_REWARD,
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

        #region UNLOCK QUEST
        talkDt.Add((int)ID.UNLOCK_MAP1_BG2_ACCEPT, new string[] {
            "자 이제 본격적으로\n해결사 일을 진행해볼까?:0",
            "문제를 풀면서, 레벨4를 달성해보자!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_BG2_REWARD, new string[] {
            "안녕하세요.\n해결사 선생님!:3"
            , "괜찮으시면,풍차농장에도\n꼭 들러주세요!:3"
            , "오오.. 다른곳에서도 소문이 났나봐:0"
            , "월드맵으로 가보자\n이제 풍차농장에도 갈수 있어!:1"
        });

        talkDt.Add((int)ID.UNLOCK_MAP1_BG3_ACCEPT, new string[] {
            "새로운 지역도 도와주면서\n레벨을 7까지 올려보자!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_BG3_REWARD, new string[] {
            "해결사 선생님!!:4"
            , "얼마나 찾고 있었는지 몰라요:5"
            , "저희 과수원에도 꼭 놀러와주세요!:3"
            , "월드맵으로 가보자\n이제 과수원에도 갈수 있어!:1"
        });

        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT, new string[] {
            "새로운 지역도 도와주면서\n레벨을 10을 달성해줘!:0"
        });
        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD, new string[] {
            "이야 벌써 레벨10을 달성했구나!\n대단해!:1"
            , "이미 별숲마을에서는 수학 잘하기로 소문이 자자하다구!:1"
            , "(어디선가 머리 콩!):8"
            , "아얏!:2"
            , "우끼끼!!:8"
            , "뭐야?! 처음보는 원숭이인데?:0"
            , "(머리 콩!):8"
            , "아악! 또 때렷어!:2"
            , "우끼! 나 잡아봐라!\n(후다닥):8"
            , "잡아! 당장 잡아!!:1"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_ACCEPT, new string[] {
            "으...\n정글로 도망가버렸어..:2"
            , "(톡톡..):9"
            , "저기.. 안녕하세유?\n혹시 수학해결사 맞나유?:9"
            , "맞는데..\n누..누구시죠?:0"
            , "혹시, 머리를 두번때리고 간 원숭이를 찾으시나유?:9"
            , "아앗..맞아요!:2"
            , "저는 정글 늪지대주민인디.\n저희 수학문제를 도와주면:9"
            , "원숭이가 어디로 갔는지 알려드릴게유.:9"
            , "수학연장 챙겨라\n바로 갑니다.:1"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_REWARD, new string[] {
            "도와주셔서 너무 감사합니다유.:9"
            , "이제 어디로 갔는지 당장 말해요!:0"
            , "원숭이들은 몽키와트라는 신전에서 생활해유:9"
            , "아마 거기가시면 찾으실 수 있을거에유.:9"
            , "덤불지대 근처라는 소문은 들었지만:9"
            , "워낙 베일에 쌓인 곳이라서 그 이상은 몰라유..:9"
            , "덤불지대? 거기가 어디지?!:0"
            , "걱정마세유. 제 친구 개미에게 부탁해놨어유.:9"
            , "조만간 선생님을 찾아뵐거에유.:9"
            , "좋았어!!:1"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_ACCEPT, new string[] {
            "안녕하세개미!:10"
            , "여기가 수학천재 해결사님의 집인가개미?:10"
            , "(두리번 두리번..):0"
            , "음..뭐지 누가 날 불렀는데 아무도 없..:0"
            , "발 아래개미! :10"
            , "앗! 왠 개미가?!:2"
            , "개구리 친구한테 부탁받았개미\n원숭이를 찾고있다개미?:10"
            , "맞아요! 어딧나요 원숭이는!!:1"
            , "잠시만개미, 우리 덤불마을 지금 수학문제 때문에 힘들개미:10"
            , "수학해결사님 우리들을 좀 도와줘개미:10"
            , "그럼 알려주개미!! :10"
            , "크아.. 이놈의 인기..\n또 시작이군..:2"
            , "어쩔수없다. 수학이 필요한 곳을 외면할 수는 없지:1"
            , "수학연장 챙겨라!:1"
            , "고맙개미!:10"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_REWARD, new string[] {
            "정말 고맙개미!!:10"
            , "덕분에 덤불마을이 수학의힘으로 평화로워졌개미!:10"
            , "자 그럼 이제 원숭이가 있는 신전을 알려줘야지?:0"
            , "알고있개미. 그 신전은 몽키와트라는 곳이다개미.:10"
            , "미리 길을 열어두었개미. 그리고 가면된다개미.:10"
            , "고마워! 드디어 찾았다 몽키와트 신전!:0"
        });

        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT, new string[] {
            "TODO:0"
        });
        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD, new string[] {
            "TODO:0"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_ACCEPT, new string[] {
            "TODO:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_REWARD, new string[] {
            "TODO:0"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_ACCEPT, new string[] {
            "TODO:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_REWARD, new string[] {
            "TODO:0"
        });
        #endregion
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region SET EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected override string setEvent(int id) {
        Debug.Log($"GameTalkManager:: setEvent(id={id}):: talkIdx= {talkIdx}");

        string rawMsg = "";
        //* 一旦ここ使わなくても、全てのIDは登録すること！
        switch(id) {
            #region TUTORIAL
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
            case (int)ID.UNLOCK_MAP1_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP1_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP1_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP1_BG3_REWARD:   break;
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD:   break;
            case (int)ID.UNLOCK_MAP2_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP2_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP2_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP2_BG3_REWARD:   break;
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD:   break;
            case (int)ID.UNLOCK_MAP3_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP3_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP3_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP3_BG3_REWARD:   break;
            #endregion
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }
/// 
    protected override void endSwitchProccess(int id) {
        const int ACCEPT = 0, REWARD = 1; //* Unlock Map:BG
        Debug.Log($"<b>endSwitchProccess(id= {id}):: </b>");
        switch(id) {
            case (int)ID.TUTO_ROOM:
                DB.Dt.IsTutoRoomTrigger = false;
                TutorialRoomPanelBtn.SetActive(false); 
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
        //* Forest
            case (int)ID.UNLOCK_MAP1_BG2_ACCEPT:
                DB.Dt.IsUnlockMap1BG2Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP1_BG2_REWARD:
                DB.Dt.IsUnlockMap1BG2Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 0, bgIdx: 1);
                break;
            case (int)ID.UNLOCK_MAP1_BG3_ACCEPT:
                DB.Dt.IsUnlockMap1BG3Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP1_BG3_REWARD:
                DB.Dt.IsUnlockMap1BG3Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 0, bgIdx: 2);
                break;
        //* Jungle
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT:
                DB.Dt.IsOpenMap2UnlockBG1Arr[ACCEPT] = true;
                break;
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD:
                DB.Dt.IsOpenMap2UnlockBG1Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 1, bgIdx: 0);
                break;
            case (int)ID.UNLOCK_MAP2_BG2_ACCEPT:
                DB.Dt.IsUnlockMap2BG2Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP2_BG2_REWARD:
                DB.Dt.IsUnlockMap2BG2Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 1, bgIdx: 1);
                break;
            case (int)ID.UNLOCK_MAP2_BG3_ACCEPT:
                DB.Dt.IsUnlockMap2BG3Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP2_BG3_REWARD:
                DB.Dt.IsUnlockMap2BG3Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 1, bgIdx: 2);
                break;
        //* Tundra
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT:
                DB.Dt.IsOpenMap3UnlockBG1Arr[ACCEPT] = true;
                break;
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD:
                DB.Dt.IsOpenMap3UnlockBG1Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 2, bgIdx: 0);
                break;
            case (int)ID.UNLOCK_MAP3_BG2_ACCEPT:
                DB.Dt.IsUnlockMap3BG2Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP3_BG2_REWARD:
                DB.Dt.IsUnlockMap3BG2Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 2, bgIdx: 1);
                break;
            case (int)ID.UNLOCK_MAP3_BG3_ACCEPT:
                DB.Dt.IsUnlockMap3BG3Arr[ACCEPT] = true;
                break;
            case (int)ID.UNLOCK_MAP3_BG3_REWARD:
                DB.Dt.IsUnlockMap3BG3Arr[REWARD] = true;
                HM._.wmm.showUnlockBg(mapIdx: 2, bgIdx: 2);
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
