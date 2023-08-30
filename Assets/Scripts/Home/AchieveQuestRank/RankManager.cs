using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankManager : MonoBehaviour {
    [SerializeField] GameObject myRankInfoObj;  public GameObject MyRankInfoObj {get => myRankInfoObj;}
    [SerializeField] GameObject needToLoginTxtObj; public GameObject NeedToLoginTxtObj {get => needToLoginTxtObj;}

    // void Start() {
    //     HM._.actm.reqGetAllUsers();
    // }
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------    
    public void setMyRankInfo(string infoDtStr) {
        const int ID = 0, LV = 1, FAME = 2, SKIN = 3;
        string[] userInfoArr = infoDtStr.Split("_");

        var tf = myRankInfoObj.transform;
        tf.GetChild(ID).GetComponent<TextMeshProUGUI>().text = userInfoArr[ID];
        tf.GetChild(LV).GetComponent<TextMeshProUGUI>().text = userInfoArr[LV];
        tf.GetChild(FAME).GetComponent<TextMeshProUGUI>().text = userInfoArr[FAME];
        // tf.GetChild(SKIN).GetComponentInChildren<Image>().sprite = 
    }
#endregion
}
