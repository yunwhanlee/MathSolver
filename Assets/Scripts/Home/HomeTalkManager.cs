using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
        UNLOCK_MAP1_MINIGAME,
        //* Jungle
        OPEN_MAP2_UNLOCK_BG1_ACCEPT, OPEN_MAP2_UNLOCK_BG1_REWARD,
        UNLOCK_MAP2_BG2_ACCEPT, UNLOCK_MAP2_BG2_REWARD,
        UNLOCK_MAP2_BG3_ACCEPT, UNLOCK_MAP2_BG3_REWARD,
        UNLOCK_MAP2_MINIGAME,
        //* Tundra
        OPEN_MAP3_UNLOCK_BG1_ACCEPT, OPEN_MAP3_UNLOCK_BG1_REWARD,
        UNLOCK_MAP3_BG2_ACCEPT, UNLOCK_MAP3_BG2_REWARD,
        UNLOCK_MAP3_BG3_ACCEPT, UNLOCK_MAP3_BG3_REWARD,
        UNLOCK_MAP3_MINIGAME,
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

        #region MAP1 FOREST
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
            "풍차지역도 도와주면서\n레벨을 7까지 올려보자!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_BG3_REWARD, new string[] {
            "해결사 선생님!!:4"
            , "얼마나 찾고 있었는지 몰라요:5"
            , "저희 과수원에도 꼭 놀러와주세요!:3"
            , "월드맵으로 가보자\n이제 과수원에도 갈수 있어!:1"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_MINIGAME, new string[] {
            "저희 과수원에서 사과따기 체험을 진행하고 있는데요.:3"
            , "떨어지는 사과를 받는 게임이에요!:3"
            , "모두 클리어하면 엄청난 보상도 있답니다!:4"
            , "과수원지역 <size=100><sprite name=exclamation></size>를 찾아주세요.\n첫회는 무료에요!:4"
            , "사과따기 미니게임이 오픈했어!:1"
        });
        #endregion
        #region MAP2 JUNGLE
        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT, new string[] {
            "이제 과수원 지역도 도와주면서\n레벨을 10을 달성해보자!:0"
        });
        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD, new string[] {
            "이야 벌써 레벨10을 달성했구나!\n대단해!:1"
            , "이미 별숲마을에서는 수학 잘하기로 소문이 자자하다구!:1"
            , "(어디선가 머리 콩!):9"
            , "아얏!:2"
            , "우끼끼!!:8"
            , "뭐야?! 처음보는 원숭이인데?:0"
            , "(머리 콩!):9"
            , "아악! 또 때렷어!:2"
            , "우끼! 나 잡아봐라!\n(후다닥):8"
            , "잡아! 당장 잡아!!:1"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_ACCEPT, new string[] {
            "으...\n정글로 도망가버렸어..:2"
            , "(톡톡..):11"
            , "저기.. 안녕하세유?\n혹시 수학해결사 맞개굴?:11"
            , "맞는데..\n누..누구시죠?:0"
            , "혹시, 머리를 두번때리고 간 원숭이를 찾으시개굴?:11"
            , "아앗..맞아요!:2"
            , "저는 정글 늪지대주민인디,\n저희 수학문제를 도와주면:11"
            , "원숭이가 어디로 갔는지 알려드릴개굴.:12"
            , "새로운 정글맵이 오픈했어!:1"
            , "흠.. 우선 늪지대 지역을 도와주며 레벨 14를 달성하자!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_REWARD, new string[] {
            "도와주셔서 너무 감사개굴.:12"
            , "이제 어디로 갔는지 당장 말해!:1"
            , "원숭이들은 몽키와트라는 신전에서 생활해개굴:11"
            , "아마 거기가시면 찾으실 수 있개굴.:11"
            , "덤불지대 근처라는 소문은 들었지만,:11"
            , "워낙 비밀에 쌓인 곳이라서 그 이상은 몰라개굴.:13"
            , "덤불지대? 거기가 어디지?!:0"
            , "걱정마개굴! 개미친구에게 부탁해놨개굴.:11"
            , "조만간 선생님을 찾아갈거라개굴.:11"
            , "좋았어!!:1"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_ACCEPT, new string[] {
            "(그로부터 며칠 뒤..):0"
            , "안녕하세개미!:14"
            , "여기가 수학천재 해결사님의 집인가개미?:15"
            , "(두리번 두리번..):0"
            , "음..뭐지 누가 날 불렀는데 아무도 없..:0"
            , "발 아래개미! :16"
            , "앗! 왠 개미가?!:2"
            , "개구리 친구한테 부탁받았개미\n원숭이를 찾고있다개미?:14"
            , "맞아요! 어딧나요\n원숭이는!!:1"
            , "잠시만개미, 우리 덤불마을 지금 수학문제 때문에 힘들개미:16"
            , "수학해결사님 우리들도 좀 도와줘개미:15"
            , "그럼 알려주개미!! :14"
            , "크아..:2"
            , "어쩔수없다! 수학이 필요한 곳을 외면할 수는 없는법:1"
            , "고맙개미!:15"
            , "덤불마을도 도와주며 레벨 17을 달성하자!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_REWARD, new string[] {
            "정말 고맙개미!!:15"
            , "덕분에 덤불마을이 수학의힘으로 평화로워졌개미!:15"
            , "자 그럼 이제 원숭이가 있는 신전을 알려줘야지?:0"
            , "알고있개미. 그 신전은 몽키와트라는 곳이다개미.:14"
            , "미리 길을 열어두었개미. 그리로 가면된다개미!:14"
            , "고마워! 드디어 찾았다! 기다려라 몽키와트 신전!:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_MINIGAME, new string[] { //* #2
            "잠깐!!:17"
            , "네..넹?:2"
            , "우리 신전에서는 하늘로 올라가는 전통행사가 있지:17"
            , "경건한 마음으로 바나나와 황금바나나를 획득한다!!:17"
            , "맞다개굴! 정글 전통행사 하늘로 점프! 미니게임이 열렸개굴.:12"
            , "전부클리어하면, 귀여운 펫 보상도 있다개굴!:12"
            , "신전지역 <size=100><sprite name=exclamation></size>를 클릭해봐개굴!:11"
        });
        #endregion

        #region MAP3 TUNDRA
        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT, new string[] { //* #1
            "흠..여기가 몽키와트 신전인가..:0"
            , "룰루랄라~ 우끼~:8"
            , "아닛! 드디어 찾았다 이 녀석!!:1"
            , "으끼끼익?!:10"
            , "누구냐?! 침입자다!!:17"
            , "히익!:2"
            , "저..저는 절 때린 아기원숭이를 잡으러..:2"
            , "뭐라고?! 우리 아기원숭이를 때리러왔다고?!:17"
            , "그게 아니..:2"
            , "몽키킬러가 확실하다! 체포하라!!:17"
            , "잠시개굴!:11_FLIP"
            , "멈춰개미!!:14_FLIP"
            , "너희들은 마음씨 착한\n개구리와개미?!:17"
            , "이 분은! 몽키킬러가 아니개굴! :11_FLIP"
            , "저희 정글을 도와주시는 수학해결사님이다개미! :14_FLIP"
            , "음.. 하지만 증거가 없다! 마침잘됫군.:17"
            , "우리신전의 수학고민이 날이가는대로 쌓여서:17"
            , "몽키신님이 화나셧는지 날씨가 좋지않아.:17"
            , "우리신전을 도와주면 믿도록하지!:17"
            , "히익 제가 도와드려야지요!\n(사..살았다..):0"
            , "신전도 도와주면서 친밀도를 쌓고, 레벨 20을 달성하자.:0"
        });
        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD, new string[] {
            "TODO OPEN_MAP3_UNLOCK_BG1_REWARD:0"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_ACCEPT, new string[] {
            "TODO UNLOCK_MAP3_BG2_ACCEPT:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_REWARD, new string[] {
            "TODO UNLOCK_MAP3_BG2_REWARD:0"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_ACCEPT, new string[] {
            "TODO UNLOCK_MAP3_BG3_ACCEPT:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_REWARD, new string[] {
            "TODO UNLOCK_MAP3_BG3_REWARD:0"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_MINIGAME, new string[] {
            "TODO UNLOCK_MAP3_MINIGAME"
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
                if(talkIdx == 12) HM._.ui.activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_FUNITURESHOP:
                if(talkIdx == 4) HM._.ui.activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_CLOTHSHOP:
                if(talkIdx == 6) HM._.ui.activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_INV:
                if(talkIdx == 2) HM._.ui.activeHandFocus(WOOD_ARROW_POS);
                break;
            case (int)ID.TUTO_GOGAME:
                // なし
                break;
            case (int)ID.TUTO_WORLDMAP:
                if(talkIdx == 0) HM._.ui.HandFocusTf.gameObject.SetActive(false);
                else if(talkIdx == 4) HM._.ui.activeHandFocus(STAR_MOUNTAION_POS);
                break;
            case (int)ID.TUTO_FINISH:
                if(talkIdx == 3) {
                    talkFrameTf.anchoredPosition = new Vector2(0, 500);
                    HM._.ui.MenuTapFrame.anchoredPosition = new Vector2(0, HM._.ui.MenuTapFrame.anchoredPosition.y);
                    HM._.ui.activeHandFocus(MENUTAP_QUEST_ICON_POS);
                }
                else if(talkIdx == 4) {
                    talkFrameTf.anchoredPosition = new Vector2(0, 0);
                    HM._.ui.onClickAchiveRankIconBtn();
                    HM._.ui.onClickAchiveRankTypeBtn(1);
                    HM._.ui.activeHandFocus(QUEST_CATE_POS);
                }
                else if(talkIdx == 5) {
                    HM._.ui.activeHandFocus(QUEST_REWARD_BTN_POS);
                }
                break;
            #endregion
            case (int)ID.UNLOCK_MAP1_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP1_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP1_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP1_BG3_REWARD:   break;
            case (int)ID.UNLOCK_MAP1_MINIGAME:   break;

            case (int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD:   break;
            case (int)ID.UNLOCK_MAP2_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP2_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP2_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP2_BG3_REWARD:   break;
            case (int)ID.UNLOCK_MAP2_MINIGAME:   break;

            case (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD:   break;
            case (int)ID.UNLOCK_MAP3_BG2_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP3_BG2_REWARD:   break;
            case (int)ID.UNLOCK_MAP3_BG3_ACCEPT:   break;
            case (int)ID.UNLOCK_MAP3_BG3_REWARD:   break;
            case (int)ID.UNLOCK_MAP3_MINIGAME:   break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }
/// 
    protected override void endSwitchProccess(int id) {
        const int ACCEPT = 0, REWARD = 1; //* Unlock Map:BG
        var enumValArr = (ID[])System.Enum.GetValues(typeof(ID));
        ID idVal = Array.Find(enumValArr, value => (int)value == id);
        Debug.Log($"<color=yellow>endSwitchProccess( id=> {id}. {idVal}):: </color>");
        
        switch(id) {
            #region TUTORIAL
            case (int)ID.TUTO_ROOM:
                DB.Dt.IsTutoRoomTrigger = false;
                TutorialRoomPanelBtn.SetActive(false); 
                HM._.ui.HandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_FUNITURESHOP);
                break;
            case (int)ID.TUTO_FUNITURESHOP:
                DB.Dt.IsTutoFunitureShopTrigger = false;
                HM._.ui.HandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_CLOTHSHOP);
                break;
            case (int)ID.TUTO_CLOTHSHOP: 
                DB.Dt.IsTutoClothShopTrigger = false;
                HM._.ui.HandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                action((int)ID.TUTO_INV);
                break;
            case (int)ID.TUTO_INV:
                DB.Dt.IsTutoInventoryTrigger = false;
                HM._.ui.HandFocusTf.gameObject.SetActive(false);
                HM._.ui.onClickWoodSignArrowBtn(dirVal: 1);
                break;
            case (int)ID.TUTO_GOGAME:
                DB.Dt.IsTutoGoGameTrigger = false;
                HM._.ui.activeHandFocus(FOOT_HOLD_POS);
                break;
            case (int)ID.TUTO_WORLDMAP:
                DB.Dt.IsTutoWorldMapTrigger = false;
                HM._.ui.HandFocusTf.gameObject.SetActive(false);
                break;
            case (int)ID.TUTO_FINISH:
                DB.Dt.IsTutoFinishTrigger = false;
                break;
            #endregion
            #region MAP1 FOREST
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
            case (int)ID.UNLOCK_MAP1_MINIGAME:
                DB.Dt.IsUnlockMinigame1 = true;
                HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn();
                break;
            #endregion
            #region MAP2 JUNGLE
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
            case (int)ID.UNLOCK_MAP2_MINIGAME:
                DB.Dt.IsUnlockMinigame2 = true;
                // HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn();
                break;
            #endregion
            #region MAP3 TUNDRA
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT:
                DB.Dt.IsOpenMap3UnlockBG1Arr[ACCEPT] = true;
                HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_MINIGAME);
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
            case (int)ID.UNLOCK_MAP3_MINIGAME:
                DB.Dt.IsUnlockMinigame3 = true;
                HM._.qm.MainQuests[DB.Dt.MainQuestID].onClickAcceptBtn();
                break;
            #endregion
        }
    }
    #endregion
}
