using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RankManager : MonoBehaviour {
    const int ID = 0, LV = 1, FAME = 2, SKIN = 3, RANK = 5;
    [SerializeField] Color meColor;
    [SerializeField] GameObject myRankInfoObj;  public GameObject MyRankInfoObj {get => myRankInfoObj;}
    [SerializeField] GameObject needToLoginTxtObj; public GameObject NeedToLoginTxtObj {get => needToLoginTxtObj;}
    [SerializeField] Transform RankScrollContentTf;
    [SerializeField] GameObject userInfoListPf;
    [SerializeField] Sprite[] rankerFrameSprs;
    [SerializeField] Image[] rankerSkinImgs;
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------    
    public void createRankUserList() {
        //* Rank リスト 生成
        Debug.Log("RankManager:: HM._.actm.UserInfoList.Count= " + HM._.actm.UserInfoList.Count);
        //* オブジェクト 初期化
        for (int i = RankScrollContentTf.childCount - 1; i >= 0; i--)
            Destroy(RankScrollContentTf.GetChild(i).gameObject);

        int rankNum = 1;
        HM._.actm.UserInfoList.ForEach(user => {
            Debug.Log($"RankManager:: Instantiate UserInfoList id={user.Id}, lv={user.Lv}, fame={user.Fame}, skin={user.SkinName}");
            var ins = Instantiate(userInfoListPf, RankScrollContentTf);
            ins.transform.GetChild(ID).GetComponent<TextMeshProUGUI>().text = user.Id + " " + (DB.Dt.AccountID != "" && user.Id == DB.Dt.AccountID? "<color=red>(ME)</color>" : "");
            ins.transform.GetChild(LV).GetComponent<TextMeshProUGUI>().text = "LV" + user.Lv;
            ins.transform.GetChild(FAME).GetComponent<TextMeshProUGUI>().text = user.Fame;
            //* Skin
            PlayerSkin plSkin = Array.Find(DB.Dt.PlSkins, sk => sk.Name == user.SkinName);
            ins.transform.GetChild(SKIN).GetComponentsInChildren<Image>()[1].sprite = plSkin.Spr;
            //* Ranker Styling (1st, 2nd, 3rd)
            if(rankNum == 1) {
                rankerSkinImgs[0].sprite = plSkin.Spr;
                ins.GetComponent<Image>().sprite = rankerFrameSprs[0];
            }
            else if(rankNum == 2) {
                rankerSkinImgs[1].sprite = plSkin.Spr;
                ins.GetComponent<Image>().sprite = rankerFrameSprs[1];
            }
            else if(rankNum == 3) {
                rankerSkinImgs[2].sprite = plSkin.Spr;
                ins.GetComponent<Image>().sprite = rankerFrameSprs[2];
            }
            //* RankNum
            ins.transform.GetChild(RANK).GetComponent<TextMeshProUGUI>().text = $"{rankNum}";

            //* My RankNum
            if(DB.Dt.AccountID != "" && user.Id == DB.Dt.AccountID) {
                HM._.ui.MyRankTxt.text = $"{rankNum}";
                myRankInfoObj.transform.GetChild(RANK).GetComponent<TextMeshProUGUI>().text = $"{rankNum}";
            }
            rankNum++;
        });

        
    }

    public void setMyRankInfo(string infoDtStr) {
        Debug.Log("setMyRankInfo():: infoDtStr= " + infoDtStr);
        string[] userInfoArr = infoDtStr.Split("_");

        var myTf = myRankInfoObj.transform;
        myTf.GetChild(ID).GetComponent<TextMeshProUGUI>().text = DB.Dt.AccountID;
        myTf.GetChild(LV).GetComponent<TextMeshProUGUI>().text = "LV" + userInfoArr[LV];
        myTf.GetChild(FAME).GetComponent<TextMeshProUGUI>().text = userInfoArr[FAME];
        PlayerSkin plSkin = Array.Find(DB.Dt.PlSkins, sk => sk.Name == userInfoArr[SKIN]);
        myTf.GetChild(SKIN).GetComponentsInChildren<Image>()[1].sprite = plSkin.Spr;
        // tf.GetChild(SKIN).GetComponentInChildren<Image>().sprite = 
        // int myRankNum = HM._.actm.UserInfoList.FindIndex(user => user.Id == DB.Dt.AccountID);
        // tf.GetChild(RANK).GetComponent<TextMeshProUGUI>().text = (myRankNum).ToString();
    }
#endregion
}
