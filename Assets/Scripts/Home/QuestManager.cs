using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        int i = 0;
        Array.ForEach(mainQuests, mq => {
            if(i == DB.Dt.MainQuestID) {
                mq.gameObject.SetActive(true);
                // mq.acceptQuest(); //* メインクエストは自動承知
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
        switch(id) {
            case (int)MQ_ID.Tutorial:
                StartCoroutine(hui.coActiveRewardPopUp(fame: 10, new Dictionary<RewardItemSO, int>() {
                    {rwdList[(int)Enum.RWD_IDX.Coin], 300},
                    {rwdList[(int)Enum.RWD_IDX.Exp], 100},
                    {rwdList[(int)Enum.RWD_IDX.WoodChair], 1}
                }));
                break;
            case (int)MQ_ID.UnlockMap1Windmill:
            case (int)MQ_ID.UnlockMap1Orchard:
            case (int)MQ_ID.OpenJungleMap2:
            case (int)MQ_ID.UnlockMap2Bush:
            case (int)MQ_ID.UnlockMap2MoneyWat:
            case (int)MQ_ID.OpenTundraMap3:
            case (int)MQ_ID.UnlockMap3SnowMountain:
            case (int)MQ_ID.UnlockMap3IceDragon:
                //* Value     Def                        Unit
                int fameVal = 10  + (DB.Dt.MainQuestID * 5);
                int coinVal = 300 + (DB.Dt.MainQuestID * 150);
                int expVal =  100 + (DB.Dt.MainQuestID * 50);
                StartCoroutine(hui.coActiveRewardPopUp(fame: fameVal, new Dictionary<RewardItemSO, int>() {
                    {rwdList[(int)Enum.RWD_IDX.Coin], coinVal},
                    {rwdList[(int)Enum.RWD_IDX.Exp], expVal},
                }));
                break;
        }
        DB.Dt.MainQuestID++;
        updateMainQuestList();

        if(HM._.htm.TutoHandFocusTf.gameObject.activeSelf)
            HM._.htm.TutoHandFocusTf.gameObject.SetActive(false);
    }
#endregion
}
