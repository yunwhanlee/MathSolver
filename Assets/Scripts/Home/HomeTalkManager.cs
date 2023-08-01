using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : TalkManager {
    public enum ID {
        TUTO_ROOM
        , TUTO_FUNITURESHOP
        , TUTO_CLOTHSHOP
        , TUTO_INV
        , TUTO_GOGAME
        , TUTO_FINISH
    };

    [Header("EXTRA")]
    [SerializeField] GameObject TutorialRoomPanelBtn;

    void Start() {
        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    }

    public override void generateData() {
        //* チュートリアル用
        talkDt.Add((int)ID.TUTO_ROOM, new string[] {
            "만나서 반가워!\n새로온 수학조수구나!:0"
            , "이름이 어떻게되니?:0"
            , "...:0"
            , $"NICKNAME..!\n정말 멋진 이름이야!:1"
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
            , "물건 들어오는게 랜덤이라:7"
            , "뭐가 나올지 저도 모른답니다. 호호:7"
            , "자주 놀러와요~!\n호홍:7"
            , "자!\n다시 화살표를 눌러줘!:0"
        });
        talkDt.Add((int)ID.TUTO_INV, new string[] {
            "여기는 인벤토리 공간이야.:0"
            , "스킨과 펫 변경이 가능해!:0"
            , "다시 화살표를 눌러\n처음으로 돌아가자!:0"
        });
        talkDt.Add((int)ID.TUTO_GOGAME, new string[] {
            "흠.. 어디보자..:0"
            , "동물친구들을\n도와주러 갈겸..:0"
            , "자네의 수학실력을\n한번 파악해볼까?:1"
            , "겁먹지 말게나!\n내가 있지 않나?!:1"
            , "찬찬히,\n설명해주겠다네.:0"
            , "준비가되면, 화면아래\n발판으로 가보시게나!:0"
        });
        talkDt.Add((int)ID.TUTO_FINISH, new string[] {
            "후... 드디어\n집에 돌아왔구만:0"
            , "첫 해결사 일이었을텐데\n고생많았네!:1"
            , "받은 보수로\n가구점이나 의류점에서:0"
            , "아이템을\n구매할 수 있지:0"
            , "다양한 스킨과 가구를 구매해서:0"
            , "이 텅빈 집을\n아름답게 꾸며주게나!:0"
            , "이거는 나의 작은 선물이네!:0"
        });
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
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
                if(talkIdx == 3) {
                    string msg = talkDt[(int)ID.TUTO_ROOM][3];
                    rawMsg = msg.Replace("NICKNAME", $"<color=blue>{DB.Dt.NickName}</color>");
                    Time.timeScale = 0;
                }
                if(talkIdx == 12) {
                    tutoHandFocusTf.gameObject.SetActive(true);
                    tutoHandFocusTf.anchoredPosition = new Vector2(155, 375);
                }
                break;
            case (int)ID.TUTO_FUNITURESHOP:
                if(talkIdx == 4) {
                    tutoHandFocusTf.gameObject.SetActive(true);
                    tutoHandFocusTf.anchoredPosition = new Vector2(155, 375);
                }
                break;
            case (int)ID.TUTO_CLOTHSHOP:
                if(talkIdx == 6) {
                    tutoHandFocusTf.gameObject.SetActive(true);
                    tutoHandFocusTf.anchoredPosition = new Vector2(155, 375);
                }
                break;
            case (int)ID.TUTO_INV:
                if(talkIdx == 2) {
                    tutoHandFocusTf.gameObject.SetActive(true);
                    tutoHandFocusTf.anchoredPosition = new Vector2(155, 375);
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
                break;
            case (int)ID.TUTO_FINISH:
                DB.Dt.IsTutoFinishTrigger = false;
                StartCoroutine(HM._.ui.coActiveRewardPopUp(fame: 10, new Dictionary<RewardItemSO, int>() {
                    {HM._.ui.RwdSOList[(int)Enum.RWD_IDX.Coin], 300},
                    {HM._.ui.RwdSOList[(int)Enum.RWD_IDX.Exp], 100},
                }));
                break;
        }
    }
    #endregion
}
