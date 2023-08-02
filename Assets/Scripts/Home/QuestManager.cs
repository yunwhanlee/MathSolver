using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {
    public enum MQ_ID { //* MainQuest ID
        TUTORIAL, 
        UNLOCK_M1_WINDMILL, 
        UNLOCK_M1_ORCHARD,
        OPEN_JUNGLE_UNLOCK_M2_SWAMP, 
        UNLOCK_M2_BUSH, 
        UNLOCK_M2_MONKEYWAT,
        OPEN_TUNDRA_UNLOCK_M3_GLACIER, 
        UNLOCK_M3_SNOWMOUNTAIN, 
        UNLOCK_M3_ICEDRAGON,
    }

    [SerializeField] Quest[] mainQuests;

    void Start() {
        
    }
}
