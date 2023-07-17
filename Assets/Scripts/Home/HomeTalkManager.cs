using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : TalkManager {
    public enum TALK_ID_IDX {
        TUTORIAL_ROOM
        , TUTORIAL_FUNITURESHOP
        , TUTORIAL_CLOTHSHOP
        , TUTORIAL_INV
        , TUTORIAL_GOGAME
        , TUTORIAL_FINISH
    };

    [Header("EXTRA")]
    [SerializeField] GameObject TutorialRoomPanelBtn;

    new void Start() {
        base.Start();
        TutorialRoomPanelBtn.SetActive(DB.Dt.IsTutoRoomTrigger);
    }

    public override void generateData() {
        //* チュートリアル用
        talkDt.Add((int)TALK_ID_IDX.TUTORIAL_ROOM, new string[] {
            "오 반갑네! 이번에 새로온 \n수학 조수구만.:1"
            , "자네 이름이 무엇인가?:1"
            , "...:1"
            , $"NICKNAME..!\n정말 멋진 이름이구만!:1"
            , "나는 이 별의마을의 수학해결사\n 늑선생이라네. 반갑네!:1"
            , "수백년동안 우리 늑대가문은 \n 수학을 통해서:1"
            , "마을의 고민을\n해결해주었다네.:1"
            , "하지만... 사실..:1"
            , "난 수학을 잘 못한다네..:1"
            , "그래서 자네의 힘이 절실히 필요해!:1"
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
            , "겁먹지 말게나!\n내가 있지 않나?!:1"
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
        });
    }
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    protected override string setEvent(int id) {
        Debug.Log($"GameTalkManager:: setEvent(id={id}):: talkIdx= {talkIdx}");
        string rawMsg = "";
        switch(id) {
            case (int)TALK_ID_IDX.TUTORIAL_ROOM:
                if(talkIdx == 2) {
                    Time.timeScale = 1;
                    HM._.ui.displayNickNamePopUp(isActive: true);
                }
                if(talkIdx == 3) {
                    Debug.Log("setEvent:: DB.Dt.NickName => " + DB.Dt.NickName);
                    string msg = talkDt[(int)TALK_ID_IDX.TUTORIAL_ROOM][3];
                    rawMsg = msg.Replace("NICKNAME", $"<color=blue>{DB.Dt.NickName}</color>");
                    Time.timeScale = 0;
                }
                break;
        }
        return (rawMsg == "")? getMsg(id, talkIdx) : rawMsg;
    }
/// 
    protected override void endSwitchProccess(int id) {
        switch(id) {
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
    }
    #endregion
}
