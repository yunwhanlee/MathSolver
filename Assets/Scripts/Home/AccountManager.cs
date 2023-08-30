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
	[SerializeField] string infoDtStr;	public string InfoDtStr {get => infoDtStr; set => infoDtStr = value;}
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
		SM._.sfxPlay(SM.SFX.BtnClick.ToString());
		reqLogin();
	}
	public void onClickSignUpRegisterBtn() {
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
	public void reqSaveInfo(string infoDtStr) => StartCoroutine(coAccount(Type.save, infoDtStr));
	public void reqAutoLogin() => StartCoroutine(coAutoLogin());
	public void reqGetAllUsers() => StartCoroutine(coGetAllUsers());

	IEnumerator coGetAllUsers() {
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

        	UserData userDt = JsonUtility.FromJson<UserData>("{\"data\":" + res + "}");
			foreach (UserInfo userInfo in userDt.data) {
				Debug.Log("id: " + userInfo.Id);
				Debug.Log("info: " + userInfo.Info);
				userInfoList.Add(new UserInfo(userInfo.Id, userInfo.Info));
			}
		}
	}

	IEnumerator coAccount(Type command, string infoDtStr = "") {
		WWWForm form = new WWWForm();
		int idx = (command == Type.login || command == Type.save)? LOGIN: REGISTER;
		form.AddField("command", command.ToString());
		form.AddField("id", idInputs[idx].text);
		form.AddField("password", passwordInputs[idx].text);
		form.AddField("info", infoDtStr);

		UnityWebRequest www = UnityWebRequest.Post(serverURL, form);

		yield return www.SendWebRequest();
		string res = www.downloadHandler.text;
		Debug.Log("AccountManager():: <color=yellow>res= " + res + "</color>");

		//* 結果
		if(res.Contains("Fail")) {
			string msg = res.Split(":")[1];
			HM._.ui.showErrorMsgPopUp(msg);
		}
		else if(res.Contains("Succeed")) {
			string msg = res.Split(":")[1];

			if(command == Type.save) Debug.Log("<color=blue>Save Info Data to Server!</color>");
			else HM._.ui.showSuccessMsgPopUp(msg);

			//* ログイン
			if(msg == "Login success") {
				var dt = DB.Dt;
				HM._.ui.LoginPopUp.SetActive(false);

				//* 処理
				HM._.ui.LoginBtn.gameObject.SetActive(false);
				HM._.ui.LogoutBtn.gameObject.SetActive(true);
				HM._.ui.LoginUserIDTxt.text = "ID: " + DB.Dt.AccountID;
				dt.IsLogin = true;
				dt.AccountID = idInputs[idx].text;
				dt.AccountPassword = passwordInputs[idx].text;

				//* Infoデータ サーバへ保存
				Item curSkin = Array.Find(dt.PlSkins, skin => skin.IsArranged);
				infoDtStr = $"{dt.NickName}_{dt.Lv}_{dt.Fame}_{curSkin.Name}";
				print($"onClickSignInLoginBtn():: infoDtStr= {infoDtStr}");
				reqSaveInfo(infoDtStr);

				//* MyRank Info
				HM._.rm.MyRankInfoObj.SetActive(true);
				HM._.rm.NeedToLoginTxtObj.SetActive(false);
				HM._.rm.setMyRankInfo(infoDtStr);
			}
			//* 新規登録
			else if(msg == "Register success") {
				HM._.ui.RegisterPopUp.SetActive(false);
				HM._.ui.LoginPopUp.SetActive(true);
			}
		}
	}

	public IEnumerator coAutoLogin() {
		var dt = DB.Dt;
		Item curSkin = Array.Find(dt.PlSkins, skin => skin.IsArranged);
		infoDtStr = $"{dt.NickName}_{dt.Lv}_{dt.Fame}_{curSkin.Name}";

		WWWForm form = new WWWForm();
		form.AddField("command", Type.login.ToString());
		form.AddField("id", DB.Dt.AccountID);
		form.AddField("password", DB.Dt.AccountPassword);
		form.AddField("info", infoDtStr);

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
			HM._.ui.LoginUserIDTxt.text = "ID: " + DB.Dt.AccountID;

			//* MyRank Info
			HM._.rm.MyRankInfoObj.SetActive(true);
			HM._.rm.NeedToLoginTxtObj.SetActive(false);
			HM._.rm.setMyRankInfo(infoDtStr);
		}
	}
	private IEnumerator coDisplayAutoLoginLog(string res, string fontClr) {
		string msg = res.Split(":")[1];
		autoLoginLogTxt.gameObject.SetActive(true);
		autoLoginLogTxt.text = $"<color={fontClr}>{msg}</color>";
		yield return Util.time2;
		autoLoginLogTxt.gameObject.SetActive(false);
	}
	public void clearAllInputFieldTxt() {
		Array.ForEach(idInputs, idInput => idInput.text = "");
		Array.ForEach(passwordInputs, pwInput => pwInput.text = "");
	}
#endregion
}