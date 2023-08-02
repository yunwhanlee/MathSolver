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

    //* value
    [SerializeField] Quest[] mainQuests;    public Quest[] MainQuests {get => mainQuests;}

/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void updateMainQuestList() {
        int i = 0;
        Array.ForEach(mainQuests, mq => {
            mq.gameObject.SetActive(DB.Dt.MainQuestID == i++);
            mq.acceptQuest();
        });
    }


#endregion
}
