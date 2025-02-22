using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class QuestManager : MonoBehaviour {
    public enum MQ_ID { //* MainQuest ID
        Tutorial,
        //* Map1
        UnlockMap1Windmill, 
        UnlockMap1Orchard,
        //* Map2
        OpenJungleMap2,
        UnlockMap2Bush,
        UnlockMap2MoneyWat,
        //* Map3
        OpenTundraMap3,
        UnlockMap3SnowMountain,
        UnlockMap3IceDragon,

        ComingSoon
    }

    //* Quests
    [SerializeField] bool isFinishMainQuest;    public bool IsFinishMainQuest {get => isFinishMainQuest; set => isFinishMainQuest = value;}
    [SerializeField] Quest[] mainQuests;    public Quest[] MainQuests {get => mainQuests;}

    void Start() {
        //* メインクエスト 達成 表示
        InvokeRepeating("updateMainQuestBox", 0.1f, 0.1f);
    }

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    private void updateMainQuestBox() { //* InvokeRepeating
        if(DB.Dt.MainQuestID >= mainQuests.Length) {
            Debug.Log("NO MORE MAIN QUEST");
            return;
        }
        var mq = mainQuests[DB.Dt.MainQuestID];
        mq.updateStatusGauge();
        //* MainQuestBox At Home
        HM._.ui.MainQuestBoxIconImg.sprite = mq.IconImg.sprite;
        HM._.ui.MainQuestBoxTitleTxt.text = LM._.localize(mq.ContentStr);
        HM._.ui.MainQuestBoxSlider.value = (float)mq.ClearCurVal / mq.ClearMaxVal;
        
        //* Hand Focus
        bool isFinish = mq.ClearCurVal >= mq.ClearMaxVal;
        bool isActiveAchivePanel = HM._.ui.AchiveRankPanel.activeSelf;
        bool isActiveHandFocues = HM._.ui.HandFocusTf.gameObject.activeSelf;
        bool isMainQuestEnd = DB.Dt.MainQuestID == (int)MQ_ID.ComingSoon;
        if(!isMainQuestEnd) {
            if(!isActiveAchivePanel && !isActiveHandFocues && isFinish) {
                var homeMainQuestBoxPos = new Vector2(-300, 650);
                HM._.ui.activeHandFocus(homeMainQuestBoxPos);
            }
        }
        //* メインクエストの達成：MainQuestBoxクリックのみできるように制限をかける
        isFinishMainQuest = isActiveHandFocues;
    }
    public void updateMainQuestList() {
        Debug.Log($"updateMainQuestList():: MainQuestID= {DB.Dt.MainQuestID}");
        int i = 0;
        Array.ForEach(mainQuests, mq => {
            if(i == DB.Dt.MainQuestID) {
                mq.gameObject.SetActive(true);

                //* Buttons
                switch(DB.Dt.MainQuestID) {
                    case (int)MQ_ID.Tutorial: //* チュートリアルは最初からAccept
                        mq.setBtns(true);
                        break;
                    case (int)MQ_ID.UnlockMap1Windmill:
                        mq.setBtns(DB.Dt.IsUnlockMap1BG2Arr[0]);
                        break;
                    case (int)MQ_ID.UnlockMap1Orchard:
                        mq.setBtns(DB.Dt.IsUnlockMap1BG3Arr[0]);
                        break;
                    case (int)MQ_ID.OpenJungleMap2:
                        mq.setBtns(DB.Dt.IsOpenMap2UnlockBG1Arr[0]);
                        break;
                    case (int)MQ_ID.UnlockMap2Bush:
                        mq.setBtns(DB.Dt.IsUnlockMap2BG2Arr[0]);
                        break;
                    case (int)MQ_ID.UnlockMap2MoneyWat:
                        mq.setBtns(DB.Dt.IsUnlockMap2BG3Arr[0]);
                        break;
                    case (int)MQ_ID.OpenTundraMap3:
                        mq.setBtns(DB.Dt.IsOpenMap3UnlockBG1Arr[0]);
                        break;
                    case (int)MQ_ID.UnlockMap3SnowMountain:
                        mq.setBtns(DB.Dt.IsUnlockMap3BG2Arr[0]);
                        break;
                    case (int)MQ_ID.UnlockMap3IceDragon:
                        mq.setBtns(DB.Dt.IsUnlockMap3BG3Arr[0]);
                        break;
                }
            }
            else {
                mq.gameObject.SetActive(false);
            }
            i++;
        });
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region REWARD
/// -----------------------------------------------------------------------------------------------------------------
    public void getReward(int id) {
        const int REWARD = 1;
        Debug.Log($"getReward({id}):: DB.Dt.MainQuestID= {DB.Dt.MainQuestID}");
        switch(id) {
            case (int)MQ_ID.Tutorial:
                setMainQuestReward(new Dictionary<RewardItemSO, int> {{HM._.ui.RwdSOList[(int)Enum.RWD_IDX.WoodChair], 1}});
                break;
            //* FOREST
            case (int)MQ_ID.UnlockMap1Windmill:                
                setMainQuestReward();
                if(!DB.Dt.IsUnlockMap1BG2Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_BG2_REWARD);
                break;
            case (int)MQ_ID.UnlockMap1Orchard:
                setMainQuestReward();
                if(!DB.Dt.IsUnlockMap1BG3Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_BG3_REWARD);
                break;
            //* JUNGLE
            case (int)MQ_ID.OpenJungleMap2:
                setMainQuestReward(getExtraReward(Enum.RWD_IDX.WoodenWolfStatue));
                if(!DB.Dt.IsOpenMap2UnlockBG1Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.OPEN_MAP2_UNLOCK_BG1_REWARD);
                break;
            case (int)MQ_ID.UnlockMap2Bush:
                setMainQuestReward(getExtraReward(Enum.RWD_IDX.FrogChair));
                if(!DB.Dt.IsUnlockMap2BG2Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_BG2_REWARD);
                break;
            case (int)MQ_ID.UnlockMap2MoneyWat:
                setMainQuestReward();
                if(!DB.Dt.IsUnlockMap2BG3Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_BG3_REWARD);
                break;
            //* TUNDRA
            case (int)MQ_ID.OpenTundraMap3:
                setMainQuestReward(getExtraReward(Enum.RWD_IDX.GoldenMonkeyStatue));
                if(!DB.Dt.IsOpenMap3UnlockBG1Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.OPEN_MAP3_UNLOCK_BG1_REWARD);
                break;
            case (int)MQ_ID.UnlockMap3SnowMountain:
                setMainQuestReward();
                if(!DB.Dt.IsUnlockMap3BG2Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_BG2_REWARD);
                break;
            case (int)MQ_ID.UnlockMap3IceDragon:
                setMainQuestReward(getExtraReward(Enum.RWD_IDX.IceDragonStatue));
                if(!DB.Dt.IsUnlockMap3BG3Arr[REWARD])
                    HM._.ui.OnAcceptRewardPopUp += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_BG3_REWARD);
                break;
            case (int)MQ_ID.ComingSoon:
                Debug.Log("Coming Soon");
                break;
        }
        
        DB.Dt.MainQuestID++;
        updateMainQuestList();

        if(HM._.ui.HandFocusTf.gameObject.activeSelf)
            HM._.ui.HandFocusTf.gameObject.SetActive(false);
    }
    public Dictionary<RewardItemSO, int> getExtraReward(Enum.RWD_IDX enumRewardIdx) {
        return new Dictionary<RewardItemSO, int> {
            {HM._.ui.RwdSOList[(int)enumRewardIdx], 1}
        };
    }
    public void setMainQuestReward(Dictionary<RewardItemSO, int> extraItem = null) {
        //* Value    (Def)                      (Unit)
        int fameVal = 10  + (DB.Dt.MainQuestID * 5);
        int coinVal = 300 + (DB.Dt.MainQuestID * 150);
        int expVal = 100 + (DB.Dt.MainQuestID * 50);

        //* Add Reward List
        var rwdList = new Dictionary<RewardItemSO, int> {
            { HM._.ui.RwdSOList[(int)Enum.RWD_IDX.Coin], coinVal },
            { HM._.ui.RwdSOList[(int)Enum.RWD_IDX.Exp], expVal }
        };

        //* Extraアイテムが有ったら、加える
        if (extraItem != null) 
            foreach (var item in extraItem)
                rwdList.Add(item.Key, item.Value);

        StartCoroutine(HM._.ui.coActiveRewardPopUp(fame: fameVal, rwdList));
    }
#endregion
}
