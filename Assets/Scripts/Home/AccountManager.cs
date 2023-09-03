using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;

[Serializable]
public class UserInfo {
	//* Value
    [SerializeField] string id; public string Id {get => id;}
    [HideInInspector] string password; public string Password {get => password;}
    [SerializeField] string info; public string Info {get => info;}

	[SerializeField] string rankNum;	public string RankNum {get => rankNum; set => rankNum = value;}
	[SerializeField] string lv;	public string Lv {get => lv;}
	[SerializeField] string fame;	public string Fame {get => fame;}
	[SerializeField] string skinName;	public string SkinName {get => skinName;}

	//* Constructor
	public UserInfo(string id, string info) {
		this.id = id;
		this.info = info;
		lv = info.Split("_")[1];
		fame = info.Split("_")[2];
		skinName = info.Split("_")[3];
	}
}

[Serializable]
public class UserData {
    public UserInfo[] data;
}

public class AccountManager : MonoBehaviour {
	const int LOGIN = 0, REGISTER = 1;
	public enum Type {login, register, save};
	[SerializeField] TMP_InputField[] idInputs;
	[SerializeField] TMP_InputField[] passwordInputs;
	[Header("ユーザデータ：ID_LEVEL_FAME_SKIN")]
	[SerializeField] TextMeshProUGUI autoLoginLogTxt;
	[Header("サーバから、Rankへ表示するuserInfoListを受け取る")]
	[SerializeField] List<UserInfo> userInfoList;	public List<UserInfo> UserInfoList {get => userInfoList;}
	private string serverURL = "https://4ruh0zv0zf.execute-api.ap-northeast-1.amazonaws.com/default/Lambda";

	void Start() {
		autoLoginLogTxt.gameObject.SetActive(false);
		reqGetAllUsers();
	}
/// -----------------------------------------------------------------------------------------------------------------
#region EVENT
/// -----------------------------------------------------------------------------------------------------------------
	public void onClickSignInLoginBtn() {
		if(Util.preventInputTxtBug(idInputs[LOGIN].text, isOnlyEng: true)) return;
		if(Util.preventInputTxtBug(passwordInputs[LOGIN].text, true)) return;

		SM._.sfxPlay(SM.SFX.BtnClick.ToString());
		reqLogin();
	}
	public void onClickSignUpRegisterBtn() {
		if(Util.preventInputTxtBug(idInputs[REGISTER].text, true)) return;
		if(Util.preventInputTxtBug(passwordInputs[REGISTER].text, true)) return;
		SM._.sfxPlay(SM.SFX.BtnClick.ToString());
		reqRegister();
	} 
	public void onClickSettingLogoutBtn() {
		SM._.sfxPlay(SM.SFX.BtnClick.ToString());
		HM._.ui.LoginBtn.gameObject.SetActive(true);
		HM._.ui.LogoutBtn.gameObject.SetActive(false);
		DB.Dt.IsLogin = false;
		DB.Dt.AccountID = "";
		DB.Dt.AccountPassword = "";
		HM._.rm.MyRankInfoObj.SetActive(false);
		HM._.rm.NeedToLoginTxtObj.SetActive(true);
	}
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
	public void reqLogin() => StartCoroutine(coAccount(Type.login));
	public void reqRegister() => StartCoroutine(coAccount(Type.register));
	public void reqSaveMyInfo() {
		if(!DB.Dt.IsLogin) return; //* ログインしたかったら、処理しない
		Item curSkin = Array.Find(DB.Dt.PlSkins, skin => skin.IsArranged);
		string updatedinfoDt = $"{DB.Dt.NickName}_{DB.Dt.Lv}_{DB.Dt.Fame}_{curSkin.Name}";
		StartCoroutine(coAccount(Type.save, updatedinfoDt));
		HM._.rm.setMyRankInfo(updatedinfoDt);
	} 
	public void reqAutoLogin() => StartCoroutine(coAutoLogin());
	public void reqGetAllUsers() => StartCoroutine(coGetAllUsers());

	IEnumerator coGetAllUsers() { //* サーバから、登録したユーザリスト習得

		//! 何か GETができないから、POSTに一旦した。
		WWWForm form = new WWWForm();
		form.AddField("command", "get_all_users");
		form.AddField("id", "");
		form.AddField("password", "");
		form.AddField("info", "");

		UnityWebRequest www = UnityWebRequest.Post(serverURL, form);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success) {
			Debug.LogError("Error fetching user data: " + www.error);
		} 
		else {
			string res = www.downloadHandler.text;
			Debug.Log("coGetAllUsers():: <color=yellow>res= " + res + "</color>");
			//* 初期化
			userInfoList = new List<UserInfo>(); 


			//* ユーザリストをクラス化 (リスト)
			UserData userDt = JsonUtility.FromJson<UserData>("{\"data\":" + res + "}");
			foreach (UserInfo userInfo in userDt.data) {
				Debug.Log("id: " + userInfo.Id);
				Debug.Log("info: " + userInfo.Info);
				userInfoList.Add(new UserInfo(userInfo.Id, userInfo.Info));
			}

			userInfoList.ForEach(info => {
				Debug.Log($"userInfoList:: id={info.Id} lv={info.Lv} fame={info.Fame}");
			});

			//* Sort
			Debug.Log("userInfoList レベルで SORT");
			userInfoList.Sort((a, b) => {
				int levelComparison = int.Parse(a.Lv).CompareTo(int.Parse(b.Lv));
				if (levelComparison != 0) return levelComparison; // 레벨로 우선 정렬
				else return int.Parse(a.Fame).CompareTo(int.Parse(b.Fame)); // 레벨이 같을 경우 Fame으로 정렬
			});
			userInfoList.Reverse();

			for(int i=0; i<userInfoList.Count; i++)
				userInfoList[i].RankNum = $"{i + 1}";

			//* クラスリストをRankPanelのContentとして、生成
			HM._.rm.createRankUserList();
		}
	}

	/// <summary>
	/// アカウントに関した処理関数をサーバへ送信
	/// </summary>
	/// <param name="cmd">LOGINとREGISTERとSAVEタイプを指定。</param>
	/// <param name="infoDtStr">SAVEの場合のみ、infoDtStrへ値が入ってくる。</param>
	/// <returns></returns>
	IEnumerator coAccount(Type cmd, string infoDtStr = "") {
		WWWForm form = new WWWForm();
		//* ID
		string id = (cmd == Type.login)? idInputs[LOGIN].text
			: (cmd == Type.register)? idInputs[REGISTER].text
			: DB.Dt.AccountID;
		//* PW
		string pw = (cmd == Type.login)? passwordInputs[LOGIN].text
			: (cmd == Type.register)? passwordInputs[REGISTER].text
			: DB.Dt.AccountPassword;
		
		Debug.Log($"coAccount():: REQUEST TO SERVER FROM -> <color=yellow>cmd={cmd}, id= {id}, pw= {pw}, infoDt= {infoDtStr}</color>");
		//* サーバに投げる フォーム作成
		form.AddField("command", cmd.ToString());
		form.AddField("id", id);
		form.AddField("password", pw);
		form.AddField("info", infoDtStr);
		UnityWebRequest www = UnityWebRequest.Post(serverURL, form);

		yield return www.SendWebRequest();
		string res = www.downloadHandler.text;
		Debug.Log($"coAccount(command= {cmd}, infoDtStr= {infoDtStr}):: <color=yellow> res= " + res + "</color>");

		//* 結果
		if(res.Contains("Fail")) {
			string msg = res.Split(":")[1];
			HM._.ui.showErrorMsgPopUp(msg + " : " + www.downloadHandler.error);
			Debug.Log("FAIL= " + www.downloadHandler.error);
		}
		else if(res.Contains("Succeed")) {
			string msg = res.Split(":")[1];

			if(cmd == Type.save) Debug.Log("<color=blue>Save Info Data to Server!</color>");
			else HM._.ui.showSuccessMsgPopUp(LM._.localize(msg));

			//* ログイン
			if(msg == "Login success") {
				var dt = DB.Dt;
				HM._.ui.LoginPopUp.SetActive(false);

				//* 処理
				HM._.ui.LoginBtn.gameObject.SetActive(false);
				HM._.ui.LogoutBtn.gameObject.SetActive(true);
				// HM._.ui.LoginUserIDTxt.text = "ID: " + DB.Dt.AccountID;
				dt.IsLogin = true;
				dt.AccountID = id;
				dt.AccountPassword = pw;

				//? ログインできたら、一回 Myデータを保存
				reqSaveMyInfo();

				//* MyRank Info
				HM._.rm.MyRankInfoObj.SetActive(true);
				HM._.rm.NeedToLoginTxtObj.SetActive(false);
			}
			//* 新規登録
			else if(msg == "Register success") {
				HM._.ui.RegisterPopUp.SetActive(false);
				HM._.ui.LoginPopUp.SetActive(true);
				//* 新規登録したら、ログインID表示してパスワードのみ入力するように
			}
		}
	}

	public IEnumerator coAutoLogin() {
		var dt = DB.Dt;
		if(!dt.IsLogin) yield break;
		Item curSkin = Array.Find(dt.PlSkins, skin => skin.IsArranged);
		string updatedinfoDt = $"{dt.NickName}_{dt.Lv}_{dt.Fame}_{curSkin.Name}";

		WWWForm form = new WWWForm();
		form.AddField("command", Type.login.ToString());
		form.AddField("id", DB.Dt.AccountID);
		form.AddField("password", DB.Dt.AccountPassword);
		form.AddField("info", updatedinfoDt);

		UnityWebRequest www = UnityWebRequest.Post(serverURL, form);

		yield return www.SendWebRequest();
		string res = www.downloadHandler.text;
		Debug.Log("coAutoLogin():: <color=yellow>res= " + res + "</color>");

		//* 結果
		if(res.Contains("Fail")) {
			StartCoroutine(coDisplayAutoLoginLog(res, "red"));
			HM._.rm.MyRankInfoObj.SetActive(false);
			HM._.rm.NeedToLoginTxtObj.SetActive(true);
		}
		else if(res.Contains("Succeed")) {
			StartCoroutine(coDisplayAutoLoginLog(res, "blue"));

			//* SettingPanalで、ログアウトボタンに切り替え
			HM._.ui.LoginBtn.gameObject.SetActive(false);
			HM._.ui.LogoutBtn.gameObject.SetActive(true);
			// HM._.ui.LoginUserIDTxt.text = "ID: " + DB.Dt.AccountID;

			//? AUTOログインができたら、一回 Myデータを保存
			reqSaveMyInfo();

			//* MyRank Info
			HM._.rm.MyRankInfoObj.SetActive(true);
			HM._.rm.NeedToLoginTxtObj.SetActive(false);
		}
	}
	private IEnumerator coDisplayAutoLoginLog(string res, string fontClr) {
		string msg = res.Split(":")[1];
		autoLoginLogTxt.gameObject.SetActive(true);
		autoLoginLogTxt.text = $"<color={fontClr}>{msg}</color>";
		yield return Util.time2;
		autoLoginLogTxt.gameObject.SetActive(false);
	}
	public void clearAllInputFieldTxt() { //* => HUI:: displaySignInUpPopUp()
		Array.ForEach(idInputs, idInput => idInput.text = "");
		Array.ForEach(passwordInputs, pwInput => pwInput.text = "");
	}
#endregion
}