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
        //* Map 4
        COMING_SOON,
    };

    [Header("EXTRA")]
    [SerializeField] GameObject TutorialRoomPanelBtn;
    [SerializeField] Sprite noneBgSpr;
    [SerializeField] Sprite orchardBgSpr;
    [SerializeField] Sprite swampBgSpr;
    [SerializeField] Sprite monkeywatBgSpr;
    [SerializeField] Sprite tundraEntraceSpr;
    [SerializeField] Sprite snowMountainSpr;
    [SerializeField] Sprite iceDragonBgSpr;
    [SerializeField] GameObject thankYouForPlayingGroup;

    // void Start() {
    //     TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    // }

    void Update() {
        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    }

    public override void generateData() {
        #region TUTORIAL
        //* チュートリアル用
        talkDt.Add((int)ID.TUTO_ROOM, new string[] {
             $"만나서 반가워! 새로운 수학조수구나!:{(int)SPK.Pl_Idle}" // -> talkIdx[1]
            , $"이름이 어떻게되니?:{(int)SPK.Pl_Idle}"
            , $"...:{(int)SPK.Pl_Idle}"
            , $"NICKNAME!:{(int)SPK.Pl_Idle}"
            , $"정말 멋진 이름이야.:{(int)SPK.Pl_Happy}"
            , $"나는 이 마을 수학해결사 늑선생이야. 잘 부탁해!:{(int)SPK.Pl_Idle}"
            , $"사실... 비밀인데...:{(int)SPK.Pl_Idle}"
            , $"돌부리에 걸려 넘어진 뒤로 머리를 다쳐서:{(int)SPK.Pl_Idle}"
            , $"수학 기억이 안나..:{(int)SPK.Pl_Sad}"
            , $"나와함께 여러지역의 수학문제를 해결하고:{(int)SPK.Pl_Idle}"
            , $"잊어버린 수학의기억조각 찾는 것을 도와줘!:{(int)SPK.Pl_Idle}"
            , $"이곳은 우리 사무실!:{(int)SPK.Pl_Idle}"
            , $"여러가구를 구입하고<br>꾸밀 수 있어!:{(int)SPK.Pl_Idle}"
            , $"위쪽 나무판 화살표를 눌러보겠니?:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.TUTO_FUNITURESHOP, new string[] {
            $"이곳은 장인 도톨씨의 가구점이야.:{(int)SPK.Pl_Idle}" // -> talkIdx[1]
            , $"어서오시게!:{(int)SPK.DotalMan}"
            , $"의자와 탁자, 장식품, 벽지, 매트 등:{(int)SPK.DotalMan}"
            , $"다양한 가구를 판매하고있으니<br>자주 들러주게나!:{(int)SPK.DotalMan}"
            , $"알겠지?<br>다시 화살표를 눌러줘!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.TUTO_CLOTHSHOP, new string[] {
            $"이곳은 뭉이어멈의 의류점이야.:{(int)SPK.Pl_Idle}"
            , $"어머어머 반가워요~!:{(int)SPK.MoongMom}"
            , $"저희가게는 스킨과 펫을 팔고있어요. 호호:{(int)SPK.MoongMom}"
            , $"그때그때 물건이<br>들어오는게 랜덤이라:{(int)SPK.MoongMom}"
            , $"뭐가 나올지 저도 모른답니다. 호호:{(int)SPK.MoongMom}"
            , $"자주 놀러와요~!<br>호홍:{(int)SPK.MoongMom}"
            , $"자!<br>다시 화살표를 눌러줘!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.TUTO_INV, new string[] {
            $"여기는 인벤토리야.:{(int)SPK.Pl_Idle}"
            , $"스킨과 펫의<br>변경이 가능해!:{(int)SPK.Pl_Idle}"
            , $"화살표를 눌러<br>처음으로 돌아가자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.TUTO_GOGAME, new string[] {
            $"흠.. 어디보자..:{(int)SPK.Pl_Idle}"
            , $"동물친구들을<br>도와주러 갈겸,:{(int)SPK.Pl_Idle}"
            , $"수학실력을<br>한 번 파악해볼까?:{(int)SPK.Pl_Happy}"
            , $"대화가 종료되면<br>손가락이 발판을 가리킬거야.:{(int)SPK.Pl_Idle}"
            , $"자 그럼.. 가볼까?!:{(int)SPK.Pl_Happy}"
        });
        talkDt.Add((int)ID.TUTO_WORLDMAP, new string[] {
            $"이곳은 월드맵이야.:{(int)SPK.Pl_Idle}"
            , $"지금은 한 지역만 갈 수 있지만:{(int)SPK.Pl_Sad}"
            , $"레벨이 오를수록 더 많은 지역이 열릴거야!:{(int)SPK.Pl_Idle}"
            , $"돌아가고 싶다면, 우측 아래의 집을 클릭하면되!:{(int)SPK.Pl_Idle}"
            , $"자 그럼, 손가락이<br>가리킨 곳을 클릭해!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.TUTO_FINISH, new string[] {
            $"수고 많았어!:{(int)SPK.Pl_Idle}"
            , $"첫 해결사 일인데도<br>잘 하던데?!:{(int)SPK.Pl_Happy}"
            , $"보상을 받으러 가볼까?<br>퀘스트창을 열어보자.:{(int)SPK.Pl_Idle}"
            , $"손가락이 가리키는<br>아이콘을 누른 뒤,:{(int)SPK.Pl_Idle}"
            , $"퀘스트 아이콘 선택.:{(int)SPK.Pl_Idle}"
            , $"보상버튼을 클릭해!:{(int)SPK.Pl_Idle}"
        });
        #endregion

        #region MAP1 FOREST
        talkDt.Add((int)ID.UNLOCK_MAP1_BG2_ACCEPT, new string[] {
            $"자 이제 본격적으로<br>해결사 일을 진행해볼까?:{(int)SPK.Pl_Idle}",
            $"문제를 풀면서, 레벨3를 달성해보자!:{(int)SPK.Pl_Idle}",
            $"아! 보상으로 얻은 가구를 배치하고, 랜덤뽑기도 해보자!:{(int)SPK.Pl_Happy}",
            $"화면 오른쪽 위에 빛나는 아이콘이 있어!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_BG2_REWARD, new string[] {
            $"안녕하세요.<br>해결사 선생님!:{(int)SPK.Mole_Idle}"
            , $"괜찮으시면,풍차농장도<br>들러주세요!:{(int)SPK.Mole_Happy}"
            , $"오오.. 다른곳에서도 소문이 났나봐:{(int)SPK.Pl_Idle}"
            , $"이제 풍차농장에도 갈 수 있어!:{(int)SPK.Pl_Happy}"
        });

        talkDt.Add((int)ID.UNLOCK_MAP1_BG3_ACCEPT, new string[] {
            $"풍차지역도 도와주면서<br>레벨을 5까지 올려보자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_BG3_REWARD, new string[] {
            $"해결사 선생님!!:{(int)SPK.Bear_Idle}"
            , $"얼마나 찾고 있었는지 몰라요:{(int)SPK.Bear_Sad}"
            , $"저희 과수원에도 꼭 놀러와주세요!:{(int)SPK.Bear_Happy}"
            , $"월드맵으로 가보자<br>과수원에도 갈수 있어!:{(int)SPK.Pl_Happy}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP1_MINIGAME, new string[] {
            $"과수원에서 사과따기 체험을 진행하고 있는데요.:{(int)SPK.Bear_Idle}"
            , $"떨어지는 사과를 받는 게임이에요!:{(int)SPK.Bear_Idle}"
            , $"모두 클리어하면 엄청난 보상도 있답니다!:{(int)SPK.Bear_Happy}"
            , $"과수원지역 <size=100><sprite name=exclamation></size>를 찾아주세요.<br>첫회는 무료에요!:{(int)SPK.Bear_Happy}"
            , $"사과따기 미니게임이 오픈했어!:{(int)SPK.Pl_Happy}"
        });
        #endregion
        #region MAP2 JUNGLE
        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT, new string[] {
            $"과수원도 도와주면서<br>레벨을 7을 달성하자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD, new string[] { //* Orchard BG ON
            $"벌써 이만큼 성장하다니<br>정말 대단해!:{(int)SPK.Pl_Happy}"
            , $"선생님, 감사합니다!:{(int)SPK.Bear_Happy}:{(int)SPK.Mole_Happy}:{(int)SPK.Duck_Happy}"
            , $"주민들이 선물을 준비했어요. 받아주세요!:{(int)SPK.Bear_Happy}"
            , $"정말 감사합니다!:{(int)SPK.Pl_Happy}"
            //* 초원 퀘스트 끝.

            //* 정글 퀘스트 시작.
            , $"(어디선가 머리 콩!):-{(int)SPK.Pl_Happy}" // 4
            , $"아얏!:{(int)SPK.Pl_Sad}"
            , $"우끼끼!!:{(int)SPK.Monkey_Idle}"
            , $"뭐야?! 처음보는<br>원숭이인데?:{(int)SPK.Pl_Idle}"
            , $"(머리 콩!):{(int)SPK.Monkey_Happy}" // 8
            , $"아악! 또 때렷어!:{(int)SPK.Pl_Sad}"
            , $"우끼! 나 잡아봐라!<br>(후다닥):{(int)SPK.Monkey_Idle}"
            , $"잡아! 당장 잡아!!:{(int)SPK.Pl_Happy}"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_ACCEPT, new string[] {
            $"으...<br>정글로 도망가버렸어..:{(int)SPK.Pl_Sad}"
            , $"(톡톡..):{(int)SPK.Empty}"
            , $"저기.. 안녕하세유?<br>혹시 수학해결사 맞개굴?:{(int)SPK.Frog_Idle}"
            , $"맞는데..<br>누..누구시죠?:{(int)SPK.Pl_Idle}"
            , $"혹시, 머리를 두번때리고 간 원숭이를 찾으시개굴?:{(int)SPK.Frog_Idle}"
            , $"아앗..맞아요!:{(int)SPK.Pl_Sad}"
            , $"정글 늪지대주민인데,<br>저희 고민을 도와주면:{(int)SPK.Frog_Idle}"
            , $"원숭이가 어디로<br>갔는지 알려드릴개굴!:{(int)SPK.Frog_Happy}"
            , $"정글맵이 열렸어!:{(int)SPK.Pl_Happy}"
            , $"흠.. 우선 늪지대를 도와주며 레벨9를 달성하자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG2_REWARD, new string[] { //* Swamp BG ON
            $"도와줘서 너무 감사개굴.:{(int)SPK.Frog_Happy}"
            , $"원숭이는 어딧나!:{(int)SPK.Pl_Happy}"
            , $"원숭이들은 몽키와트라는 신전에서 생활해개굴:{(int)SPK.Frog_Idle}"
            , $"아마 거기로 가면<br>찾을 수 있개굴.:{(int)SPK.Frog_Idle}"
            , $"덤불지대 근처라는 소문은 들었지만,:{(int)SPK.Frog_Idle}"
            , $"워낙 비밀에 쌓인 곳이라<br> 그 이상은 몰라개굴.:{(int)SPK.Frog_Sad}"
            , $"덤불지대? 어디지?!:{(int)SPK.Pl_Idle}"
            , $"걱정마개굴! 착한 개미친구에게 부탁해놨개굴.:{(int)SPK.Frog_Idle}"
            , $"조만간 선생님을<br>찾아갈거야 개굴.:{(int)SPK.Frog_Idle}"
            , $"좋았어!!:{(int)SPK.Pl_Happy}"
            , $"이건 늪지대 주민의 선물이다 개굴!:{(int)SPK.Frog_Happy}"
        });

        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_ACCEPT, new string[] {
            $"(그로부터 며칠 뒤..):{(int)SPK.Empty}"
            , $"안녕하세개미!:{(int)SPK.Ant_Idle}"
            , $"여기가 수학천재 해결사님의 집인가개미?:{(int)SPK.Ant_Happy}"
            , $"(두리번 두리번..):{(int)SPK.Pl_Idle}"
            , $"음..누가 날 불렀는데 아무도 없..:{(int)SPK.Pl_Idle}"
            , $"발 아래개미!:{(int)SPK.Ant_Sad}"
            , $"앗! 왠 개미가?!:{(int)SPK.Pl_Sad}"
            , $"개구리한테 들었개미<br>원숭이를 찾고있다개미?:{(int)SPK.Ant_Idle}"
            , $"맞아요! 어딧나요<br>원숭이는!!:{(int)SPK.Pl_Happy}"
            , $"잠시개미, 지금 덤불마을<br>수학문제로 힘들개미:{(int)SPK.Ant_Sad}"
            , $"수학해결사님 우리를 좀 도와줘개미:{(int)SPK.Ant_Sad}"
            , $"그럼 알려주개미!!:{(int)SPK.Ant_Idle}"
            , $"크아..:{(int)SPK.Pl_Sad}"
            , $"어쩔수없다! 수학이 필요한 곳을 외면할 수는 없지:{(int)SPK.Pl_Happy}"
            , $"고맙개미!:{(int)SPK.Ant_Happy}"
            , $"덤불마을도 도와주며 레벨 11을 달성하자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_BG3_REWARD, new string[] {
            $"정말 고맙개미!!:{(int)SPK.Ant_Happy}"
            , $"덕분에 덤불마을이 수학으로 평화로워졌개미!:{(int)SPK.Ant_Happy}"
            , $"자 그럼 이제 원숭이가 있는 신전을 알려주세요!:{(int)SPK.Pl_Idle}"
            , $"바로 몽키와트라는 곳이다개미.:{(int)SPK.Ant_Idle}"
            , $"미리 길을 열어두었개미. 그리로 가면된다개미!:{(int)SPK.Ant_Idle}"
            , $"고마워! 드디어 찾았다! 기다려라 몽키와트 신전!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP2_MINIGAME, new string[] {
            $"잠깐!!:{(int)SPK.WarriorMonkey_Idle}"
            , $"네..넹?:{(int)SPK.Pl_Sad}"
            , $"우리 신전에서는 하늘로 올라가는 행사가 있지:{(int)SPK.WarriorMonkey_Idle}"
            , $"경건한 마음으로<br>바나나를 획득한다!:{(int)SPK.WarriorMonkey_Idle}"
            , $"맞다개굴! 신전의행사<br>하늘로 점프! 미니게임이 열렸개굴.:{(int)SPK.Frog_Happy}"
            , $"전부클리어하면,<br>귀여운 펫 보상도 있굴!:{(int)SPK.Frog_Happy}"
            , $"신전지역 <size=100><sprite name=exclamation></size>를 클릭해봐개굴!:{(int)SPK.Frog_Idle}"
        });
        #endregion

        #region MAP3 TUNDRA
        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT, new string[] { //* #1 MonkeyWat BG ON
            $"흠..여기가 몽키와트 신전인가..:{(int)SPK.Pl_Idle}"
            , $"룰루랄라~ 우끼~:{(int)SPK.Monkey_Idle}"
            , $"아닛! 드디어 찾았다 이 녀석!!:{(int)SPK.Pl_Happy}"
            , $"으끼끼익?!:{(int)SPK.Monkey_Sad}"
            , $"누구냐?! 침입자다!!:{(int)SPK.WarriorMonkey_Idle}"
            , $"히익!:{(int)SPK.Pl_Sad}"
            , $"저..저는 절 때린<br>아기원숭이를 잡으러..:{(int)SPK.Pl_Idle}"
            , $"뭐라고?! 우리 아기원숭이를 때리러왔다고?!:{(int)SPK.WarriorMonkey_Idle}"
            , $"그게 아니..:{(int)SPK.Pl_Sad}"
            , $"몽키킬러가 확실해! 체포하라!!:{(int)SPK.WarriorMonkey_Idle}"
            , $"안돼! 멈춰!!:{(int)SPK.Ant_Idle}_FLIP:{(int)SPK.Frog_Idle}_FLIP"
            , $"너희들은 마음씨 착한<br>개구리와개미?!:{(int)SPK.WarriorMonkey_Idle}"
            , $"이 분은! 몽키킬러가 아니개굴!:{(int)SPK.Frog_Idle}_FLIP"
            , $"저희 정글을 도와주신 수학해결사다 개미!:{(int)SPK.Ant_Idle}_FLIP"
            , $"음.. 하지만 증거가 없다! 마침잘됫군.:{(int)SPK.WarriorMonkey_Idle}"
            , $"우리신전의 수학고민이 날이가는대로 쌓여서:{(int)SPK.WarriorMonkey_Idle}"
            , $"몽키신님이 화나셨는지 날씨가 좋지않다.:{(int)SPK.WarriorMonkey_Idle}"
            , $"우리신전을 도와주면 믿도록하지!:{(int)SPK.WarriorMonkey_Idle}"
            , $"제가 도와드려야지요!<br>(사..살았다..):{(int)SPK.Pl_Idle}"
            , $"신전도 도와주면서, 레벨 13을 달성하자.:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD, new string[] { //* #1 MonkeyWat BG ON
            $"고맙다! 수학선생!!:{(int)SPK.WarriorMonkey_Idle}"
            , $"하하! 별말씀을요!:{(int)SPK.Pl_Idle}"
            , $"신전의 수학고민을 해결해준 덕분에:{(int)SPK.WarriorMonkey_Happy}"
            , $"신님의 노여움이 사그라들어 날씨가 좋아졌다!:{(int)SPK.WarriorMonkey_Happy}"
            , $"처음엔 정말 미안했다. 아기원숭이를 때린다는 줄 알고.:{(int)SPK.WarriorMonkey_Sad}"
            , $"아 맞다! 그 원숭이는 어딧나요?:{(int)SPK.Pl_Happy}"
            , $"흠.. 그러고보니 나도 그 이후 본적이 없다..:{(int)SPK.WarriorMonkey_Idle}"
            , $"스으윽..(정글 신전 높은 곳에서):{(int)SPK.Empty}"
            , $"허.허.허.허<br>나를 찾고 있는가.끼?..:{(int)SPK.Monkey_Idle}"
            , $"아앗?! 너는!!:{(int)SPK.Pl_Happy}"
            , $"(퍼엉!):{(int)SPK.Empty}"
            , $"우끼끼....:{(int)SPK.Monkey_God}"
            , $"다.. 당신은?!<br>몽키신님??!!:{(int)SPK.WarriorMonkey_Sad}_FLIP"
            , $"!!!:{(int)SPK.Pl_Sad}"
            , $"황금바나나 몽키신님을 위하여.. 우끼!:{(int)SPK.WarriorMonkey_Happy}_FLIP:{(int)SPK.Frog_Idle}_FLIP:{(int)SPK.Ant_Idle}_FLIP"
            , $"허.허.. 미안하게 됫끼.<br>정글에 수학고민이 쌓여:{(int)SPK.Monkey_God}"
            , $"스트레스로 인해,<br>날씨가 안좋아졌끼..:{(int)SPK.Monkey_God}"
            , $"하지만, 원숭이들은 정글밖에 나가본적이 없끼.:{(int)SPK.Monkey_God}"
            , $"그래서 이 내가 직접 수학해결사를 찾아나섯다.끼.:{(int)SPK.Monkey_God}"
            , $"그런거구나..(감동):{(int)SPK.Pl_Idle}"
            , $"과격하게 행동한건 미안하끼.. 나에겐 시간이<br>얼마 없었다네..:{(int)SPK.Monkey_God}"
            , $"아닙니다.. 정글을<br>위해 하신건데요.:{(int)SPK.Pl_Idle}"
            , $"(주륵):{(int)SPK.WarriorMonkey_Sad}_FLIP"
            , $"이제.. 시간이<br>다 됐구만..끼:{(int)SPK.Monkey_God}"
            , $"나는 다시 하늘로 올라가겠다끼..:{(int)SPK.Monkey_God}"
            , $"수학영웅이여, 부디<br>이 선물을 받아주게끼!:{(int)SPK.Monkey_God}"
            , $"우리 정글의 수호자며 친구라는 증표다끼!:{(int)SPK.Monkey_God}"
            , $"그럼...<br>(스르르륵..):{(int)SPK.Monkey_God}"
            , $"(정글과 이 세계의<br>평화를 부탁하네....):{(int)SPK.Monkey_God}"
            , $"몽키신님을 위하여.. 우끼!:{(int)SPK.WarriorMonkey_Happy}_FLIP"
            , $"우끼!!!:{(int)SPK.Pl_Happy}:{(int)SPK.Frog_Idle}_FLIP:{(int)SPK.Ant_Idle}_FLIP"
            , $"--정글 퀘스트 끝--:{(int)SPK.Empty}"

            , $"으어어 선생님..:{(int)SPK.Mole_Sad}" 
            , $"곰돌이가 어디서<br>고대문서를 구하더니,:{(int)SPK.Mole_Idle}"
            , $"전설의 용을 보겠다며<br>툰드라지역에 갔어요..:{(int)SPK.Mole_Idle}"
            , $"그런데, 지금까지 아무런 소식이 없어요. 무슨일이 생긴건 아닐까요?:{(int)SPK.Mole_Idle}"
            , $"저는 추운데가면 하루도 못버텨서.. 선생님께서 한번 가주시면 안될까요?:{(int)SPK.Mole_Sad}"
            , $"용이요? 그저 전설 아닌가요?:{(int)SPK.Pl_Idle}"
            , $"일단 가보겠습니다.:{(int)SPK.Pl_Idle}"
            , $"감사합니다 선생님..:{(int)SPK.Mole_Sad}"
            , $"(툰드라 지역):{(int)SPK.Empty}" //* Tundra Entrance BG ON
            , $"(휘이이잉...):{(int)SPK.Empty}"
            , $"으.. 춥다..여기가... 툰드라지역 입구인가?:{(int)SPK.Pl_Sad}"
            , $"엉엉.. 엉엉!:{(int)SPK.Seal_Idle}"
            , $"아닛! 물개다! 저기 혹시 곰돌을 보신적이 있나요?:{(int)SPK.Pl_Idle}"
            , $"엉엉! 엉!:{(int)SPK.Seal_Idle}"
            , $"음...설마 말을<br>못하는건가?..:{(int)SPK.Pl_Idle}"
            , $"엉! 봤다고 엉엉!:{(int)SPK.Seal_Sad}"
            , $"”엉엉”이 ”어어”라는 말이었구나..:{(int)SPK.Pl_Sad}"
            , $"착한 곰돌친구였엉!:{(int)SPK.Seal_Idle}"
            , $"아아.. 그렇군요!<br>혹시 어디로 갔나요?:{(int)SPK.Pl_Idle}"
            , $"근데 혹시, 유명한<br>수학영웅 맞엉?!:{(int)SPK.Seal_Idle}"
            , $"하하.. 네! 맞습니다 제가 바로 그..:{(int)SPK.Pl_Happy}"
            , $"너무 반갑덩!!:{(int)SPK.Seal_Happy}"
            , $"우리 툰드라지역에도 수학고민이 많아서:{(int)SPK.Seal_Idle}"
            , $"모두 만나고 싶었덩! 우리를 도와주면:{(int)SPK.Seal_Idle}"
            , $"곰돌이 친구가 있는 곳을 말해주겠덩!:{(int)SPK.Seal_Idle}"
            , $"크흣.. 알겠습니다!:{(int)SPK.Pl_Idle}"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_ACCEPT, new string[] {
            $"입구지역을 도와주면서 레벨 15를 달성하자!:{(int)SPK.Pl_Happy}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG2_REWARD, new string[] {
            $"너무 고맙덩! 마을이 평화를 되찾았엉!:{(int)SPK.Seal_Idle}"
            , $"하하 다행이군요!:{(int)SPK.Pl_Happy}"
            , $"곰돌이는 전설의 용을<br>찾으러 설산으로 갔엉! 엉!:{(int)SPK.Seal_Idle}"
            , $"텐트랑 여러가지 채비를 하고갔으니<br>분명 아직 있을거덩!:{(int)SPK.Seal_Idle}"
            , $"감사합니다!!:{(int)SPK.Pl_Idle}"
            , $"바로 설산으로 가자!:{(int)SPK.Pl_Idle}"
        });

        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_ACCEPT, new string[] { //* SnowMountain BG ON
            $"허억..허억..<br>정말 높은산이다..:{(int)SPK.Pl_Sad}"
            , $"곰돌이는 어디있지.. 설마...:{(int)SPK.Pl_Idle}"
            , $"곰돌아! 곰돌아~!!:{(int)SPK.Pl_Happy}"
            , $"넹? 저 부르셨나요?:{(int)SPK.TundraBear_Idle}"
            , $"으아닛!:{(int)SPK.Pl_Sad}"
            , $"잘 지내셨나요? 그런데 여기까지 무슨일로?:{(int)SPK.TundraBear_Idle}"
            , $"아니! 왜 그동안 아무 연락이 없었니?!:{(int)SPK.Pl_Happy}"
            , $"아아.. 죄송해요..:{(int)SPK.TundraBear_Idle}"
            , $"무사하니 다행이다! 자 같이 돌아가자!:{(int)SPK.Pl_Happy}"
            , $"안되요! 저는 전설의 용을 꼭 봐야되요!:{(int)SPK.TundraBear_Sad}"
            , $"이곳 설산주민들은 분명히 알고있는데,:{(int)SPK.TundraBear_Idle}"
            , $"계속 모른척하고<br>알려주지 않아요..:{(int)SPK.TundraBear_Sad}"
            , $"가만히보니 주민들이<br>수학고민이 많은거같아:{(int)SPK.TundraBear_Idle}"
            , $"여기에 텐트를 치고<br>수학공부 중이었어요..:{(int)SPK.TundraBear_Idle}"
            , $"여어! 계시묘?:{(int)SPK.SnowRabbit_Idle}_FLIP"
            , $"앗! 드디어 손님이! 잠시만요.:{(int)SPK.TundraBear_Happy}"
            , $"(후다닥):{(int)SPK.Empty}"
            , $"이곳이 수학고민을 들어주는 곳이묘?:{(int)SPK.SnowRabbit_Idle}_FLIP"
            , $"아..예! 맞습니다!:{(int)SPK.TundraBear_Idle}"
            , $"흠 그럼 1+1이 뭐묘?:{(int)SPK.SnowRabbit_Idle}_FLIP"
            , $"하하! 1 + 1요?<br>그건 바로..:{(int)SPK.TundraBear_Happy}"
            , $"바로..:{(int)SPK.TundraBear_Idle}"
            , $"...:{(int)SPK.TundraBear_Idle}"
            , $"끄어어 모르겠어어!:{(int)SPK.TundraBear_Sad}"
            , $"에라이 순엉터네묘! 난 간다묘.:{(int)SPK.SnowRabbit_Sad}_FLIP"
            , $"1 + 1은 2이지요.:{(int)SPK.Pl_Idle}_FLIP"
            , $"아닛? 그걸 어..어떻.. 잠깐.. 다. 당신은묘?!:{(int)SPK.SnowRabbit_Idle}_FLIP"
            , $"수학영웅 늑선생?!!:{(int)SPK.SnowRabbit_Idle}_FLIP"
            , $"우리 툰드라지역에<br>수학고민을 해결해주러 왔묘?!:{(int)SPK.SnowRabbit_Happy}_FLIP"
            , $"아하하.. 그게..:{(int)SPK.Pl_Idle}_FLIP"
            , $"맞습니다! 그리고<br>저의 절친이죠!:{(int)SPK.TundraBear_Happy}"
            , $"이럴수묘! 안그래도 우리지역에 수학고민이 많아 힘들었묘.:{(int)SPK.SnowRabbit_Happy}_FLIP"
            , $"당연 도와드려야죠! 그리고.. 혹시..:{(int)SPK.TundraBear_Idle}"
            , $"전설의 용에 대해서 알 수 있나요?..:{(int)SPK.TundraBear_Idle}"
            , $"물론이묘! 설산지역을<br>도와주면 알려주겠다묘!:{(int)SPK.SnowRabbit_Happy}_FLIP"
            , $"크! 들으셨죠 선생님!:{(int)SPK.TundraBear_Happy}"
            , $"하..하하! 네! 제가<br>도와드리겠습니다!:{(int)SPK.Pl_Idle}_FLIP"
            , $"휴..설산도 도와주면서 레벨 17을 달성하자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_BG3_REWARD, new string[] { //* SnowMountain BG ON
            $"정말 고맙다묘!:{(int)SPK.SnowRabbit_Happy}"
            , $"당연한 걸 한겁니다!:{(int)SPK.Pl_Happy}"
            , $"크흠.. 그럼 전설의 용에 대해서는..:{(int)SPK.TundraBear_Idle}_FLIP"
            , $"..그래그래 알려주겠묘. 너희에겐 괜찮을것 같묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"따라오라묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"(알려주지 않으면 알수 없는 길로 올라간다.):{(int)SPK.Empty}"
            , $"(빙산 꼭대기):{(int)SPK.Empty}" //* Ice Dragon BG ON
            , $"오오!! 저건.. 용?!:{(int)SPK.TundraBear_Happy}_FLIP"
            , $"다 왔다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"이 툰드라 정상에는 용이 산다는 전설이 있다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"저 용은 우리 툰드라를 지켜주던 빙룡님이다묘:{(int)SPK.SnowRabbit_Idle}"
            , $"하지만, 지금은 보다시피 얼음속에 잠들었다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"... 크흠..:{(int)SPK.SnowRabbit_Idle}"
            , $"자! 나와도 된다묘!<br>우릴 도운 착한분이다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"(스윽..):{(int)SPK.Empty}"
            , $"꾸아앙..아..안녕..:{(int)SPK.BabyDragon}"
            , $"이럴수가.. 아기 용!:{(int)SPK.Pl_Idle}"
            , $"갑자기 무슨일에서인지 빙룡님은 잠이 드셨고,:{(int)SPK.SnowRabbit_Idle}"
            , $"처음에는 작은 알만 덩그러니 놓여있었다묘..:{(int)SPK.SnowRabbit_Idle}"
            , $"우린 그 동안, 아기용을 지키기위해서, 이 모든걸 비밀로 간직했다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"거기까진 좋았지만 아기용이 부화하고, 수학을 알고싶어하는데:{(int)SPK.SnowRabbit_Idle}"
            , $"우리들은<br>수학을 못한다묘..:{(int)SPK.SnowRabbit_Sad}"
            , $"그래서 수학고민이 많았던 거군요?:{(int)SPK.TundraBear_Idle}_FLIP"
            , $"맞다묘..:{(int)SPK.SnowRabbit_Idle}"
            , $"수학선생께 부탁이 있묘. 우리 아기용에게 수학을 알려줄 수 있겠냐묘?:{(int)SPK.SnowRabbit_Idle}"
            , $"그렇게 아기용이 수학을 만족하게되면 큰 사례를 하겠다묘!:{(int)SPK.SnowRabbit_Idle}"
            , $"당연히 해드려야죠! 걱정하지마세요:{(int)SPK.Pl_Idle}"
            , $"크흑.. 정말 고맙묘.:{(int)SPK.SnowRabbit_Sad}"
            , $"고마워..:{(int)SPK.BabyDragon}"
            , $"빙산지역도 도와주면서 레벨 19을 달성하자!:{(int)SPK.Pl_Idle}"
        });
        talkDt.Add((int)ID.UNLOCK_MAP3_MINIGAME, new string[] { //* NoneMapBG ON
            $"우리 툰드라지역 주민은 썰매타기를 좋아한다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"썰매를 타고 블루배리줍는 것이 낙이다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"특히 아기용이 썰매를<br>잘타는 동물 좋아한다묘.:{(int)SPK.SnowRabbit_Idle}"
            , $"썰매타기를 전부 클리어하면, 많이 친해 질 수가 있을거다묘!:{(int)SPK.SnowRabbit_Happy}"
            , $"우와! 이제 미니게임<br>썰매타기를 할 수 있어요:{(int)SPK.TundraBear_Happy}"
        });
        talkDt.Add((int)ID.COMING_SOON, new string[] {
            $"현재 개발된 게임 스토리는 여기까지입니다.:{(int)SPK.Pl_Idle}"
            ,$"방금 개방 된<br>썰매타기 미니게임과:{(int)SPK.Pl_Idle}"
            ,$"가구 및 스킨 등 나머지 컨텐츠를 즐겨주세요!:{(int)SPK.Pl_Idle}"
            ,$"추후 업데이트하여 더욱<br>좋은 모습으로<br>찾아뵙겠습니다!:{(int)SPK.Pl_Idle}"
            ,$"플레이 해주셔서<br>진심으로 감사합니다!!:{(int)SPK.Empty}"
        });
        #endregion
    }

    ///---------------------------------------------------------------------------------------------------------------------------------------------------
    #region SET EVENT
    ///---------------------------------------------------------------------------------------------------------------------------------------------------

    protected override void setBG(){
        if(curId == (int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD)
            bgImg.sprite = orchardBgSpr;
        else if(curId == (int)ID.UNLOCK_MAP2_BG2_REWARD)
            bgImg.sprite = swampBgSpr;
        else if(curId == (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT || curId == (int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD)
            bgImg.sprite = monkeywatBgSpr;
        else if(curId == (int)ID.UNLOCK_MAP3_BG2_REWARD)
            bgImg.sprite = tundraEntraceSpr;
        else if(curId == (int)ID.UNLOCK_MAP3_BG3_ACCEPT || curId == (int)ID.UNLOCK_MAP3_BG3_REWARD)
            bgImg.sprite = snowMountainSpr;
        else 
            bgImg.sprite = noneBgSpr;
    }

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
                    rawMsg = msg.Replace("NICKNAME", $"NICKNAME<color=blue>{DB.Dt.NickName}</color>");
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
                    HM._.ui.displayAchiveRankPanel();
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
            case (int)ID.UNLOCK_MAP1_MINIGAME:     break;

            case (int)ID.OPEN_MAP2_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP2_UNLOCK_BG1_REWARD:
                if(talkIdx == 5 || talkIdx == 9)
                    SM._.sfxPlay(SM.SFX.Explosion.ToString());
                break;
            case (int)ID.UNLOCK_MAP2_BG2_ACCEPT:        break;
            case (int)ID.UNLOCK_MAP2_BG2_REWARD:        break;
            case (int)ID.UNLOCK_MAP2_BG3_ACCEPT:        break;
            case (int)ID.UNLOCK_MAP2_BG3_REWARD:        break;
            case (int)ID.UNLOCK_MAP2_MINIGAME:          break;

            case (int)ID.OPEN_MAP3_UNLOCK_BG1_ACCEPT:   break;
            case (int)ID.OPEN_MAP3_UNLOCK_BG1_REWARD:
                if(talkIdx == 32) bgImg.sprite = noneBgSpr;
                else if(talkIdx == 40) bgImg.sprite = tundraEntraceSpr;
                break;
            case (int)ID.UNLOCK_MAP3_BG2_ACCEPT:        break;
            case (int)ID.UNLOCK_MAP3_BG2_REWARD:        break;
            case (int)ID.UNLOCK_MAP3_BG3_ACCEPT:        break;
            case (int)ID.UNLOCK_MAP3_BG3_REWARD:
                if(talkIdx == 6) bgImg.sprite = iceDragonBgSpr;
                break;
            case (int)ID.UNLOCK_MAP3_MINIGAME:          break;
            case (int)ID.COMING_SOON:
                if(talkIdx == 4) {
                    thankYouForPlayingGroup.SetActive(true);
                }
                break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }
/// 
    protected override void endSwitchProccess(int id) {
        const int ACCEPT = 0, REWARD = 1; //* Unlock Map:BG
        var enumValArr = (ID[])System.Enum.GetValues(typeof(ID));
        ID idVal = Array.Find(enumValArr, value => (int)value == id);
        Debug.Log($"<color=yellow>endSwitchProccess( id=> {id}. {idVal}):: </color>");
        SM._.disableTalk();
        
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
                // HM._.ui.OnDisplayNewItemPopUp();
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
            case (int)ID.COMING_SOON:
                thankYouForPlayingGroup.SetActive(false);
                break;
            #endregion
        }
    }
    #endregion
}
