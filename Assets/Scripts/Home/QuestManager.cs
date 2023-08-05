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
    }



    //* Quests
    [SerializeField] Quest[] mainQuests;    public Quest[] MainQuests {get => mainQuests;}

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void updateMainQuestList() {
        Debug.Log($"updateMainQuestList():: MainQuestID= {DB.Dt.MainQuestID}");
        int i = 0;
        Array.ForEach(mainQuests, mq => {
            if(i == DB.Dt.MainQuestID) {
                mq.gameObject.SetActive(true);
                //* チュートリアルは最初からAccept
                if(DB.Dt.MainQuestID == (int)MQ_ID.Tutorial)
                    mq.acceptQuest();
                // else
                //     HM._.ui.OnRewardPopUpAccept += () => mq.acceptQuest();
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
        Debug.Log($"getReward({id}):: DB.Dt.MainQuestID= {DB.Dt.MainQuestID}");
        switch(id) {
            case (int)MQ_ID.Tutorial:
                var extraItem = new Dictionary<RewardItemSO, int>() {{HM._.ui.RwdSOList[(int)Enum.RWD_IDX.WoodChair], 1}};
                setRewardList(extraItem);
                break;
            case (int)MQ_ID.UnlockMap1Windmill:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_WINDMILL_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest(); //* 他のMainQuestは以前のクエストリワードを受けたら、自動受け取り
                break;
            case (int)MQ_ID.UnlockMap1Orchard:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP1_ORCHARD_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.OpenJungleMap2:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.OPEN_JUNGLE_MAP2_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.UnlockMap2Bush:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_BUSH_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.UnlockMap2MoneyWat:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP2_MONKEYWAT_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.OpenTundraMap3:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.OPEN_TUNDRA_MAP3_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.UnlockMap3SnowMountain:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_SNOWMOUNTAION_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
            case (int)MQ_ID.UnlockMap3IceDragon:
                setRewardList();
                HM._.ui.OnRewardPopUpAccept += () => HM._.htm.action((int)HomeTalkManager.ID.UNLOCK_MAP3_ICEDRAGON_REWARD);
                // HM._.ui.OnRewardPopUpAccept += () => mainQuests[id].acceptQuest();
                break;
        }
        
        DB.Dt.MainQuestID++;
        updateMainQuestList();

        if(HM._.htm.TutoHandFocusTf.gameObject.activeSelf)
            HM._.htm.TutoHandFocusTf.gameObject.SetActive(false);
    }

    private void setRewardList(Dictionary<RewardItemSO, int> extraItem = null) {
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
