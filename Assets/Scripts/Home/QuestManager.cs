using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestManager : MonoBehaviour {
    public enum MQ_ID { //* MainQuest ID
        Tutorial, 
        UnlockMap1Windmill, 
        UnlockMap1Orchard,
        OpenJungleMap2,
        UnlockMap2Bush,
        UnlockMap2MoneyWat,
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
        int i = 0;
        Array.ForEach(mainQuests, mq => {
            if(i == DB.Dt.MainQuestID) {
                mq.gameObject.SetActive(true);
                mq.acceptQuest();
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
        var hui = HM._.ui;
        var rwdList = hui.RwdSOList;
        if(id == (int)MQ_ID.Tutorial) {
            StartCoroutine(hui.coActiveRewardPopUp(fame: 10, new Dictionary<RewardItemSO, int>() {
                {rwdList[(int)Enum.RWD_IDX.Coin], 300},
                {rwdList[(int)Enum.RWD_IDX.Exp], 100},
                {rwdList[(int)Enum.RWD_IDX.WoodChair], 1}
            }));
        }
        DB.Dt.MainQuestID++;
        updateMainQuestList();

        if(HM._.htm.TutoHandFocusTf.gameObject.activeSelf)
            HM._.htm.TutoHandFocusTf.gameObject.SetActive(false);
    }
#endregion
}
