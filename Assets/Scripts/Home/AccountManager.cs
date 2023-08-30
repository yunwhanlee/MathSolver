using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;

public class AccountManager : MonoBehaviour {
	const int LOGIN = 0, REGISTER = 1;
	public enum Type {login, register, save};
	[SerializeField] TMP_InputField[] idInputs;
	[SerializeField] TMP_InputField[] passwordInputs;
	[Header("ユーザデータ：NICKNAME_LEVEL_FAME_SKIN")]
	[SerializeField] string infoDtStr;
	[SerializeField] TextMeshProUGUI autoLoginLogTxt;
	private string serverURL = "https://4ruh0zv0zf.execute-api.ap-northeast-1.amazonaws.com/default/Lambda";
/// -----------------------------------------------------------------------------------------------------------------
#region EVENT
/// -----------------------------------------------------------------------------------------------------------------
	public void onClickSignInLoginBtn() => reqLogin();
	public void onClickSignUpRegisterBtn() => reqRegister();
	public void onClickSettingLogoutBtn() {
		HM._.ui.LoginBtn.gameObject.SetActive(true);
		HM._.ui.LogoutBtn.gameObject.SetActive(false);
		DB.Dt.IsLogin = false;
		DB.Dt.AccountID = "";
		DB.Dt.AccountPassword = "";
	}
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
	public void reqLogin() => StartCoroutine(coAccount(Type.login));
	public void reqRegister() => StartCoroutine(coAccount(Type.register));
	public void reqSaveInfo(string infoDtStr) => StartCoroutine(coAccount(Type.save, infoDtStr));
	public void reqAutoLogin() => StartCoroutine(coAutoLogin());
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
				dt.IsLogin = true;
				dt.AccountID = idInputs[idx].text;
				dt.AccountPassword = passwordInputs[idx].text;

				//* Infoデータ サーバへ保存
				Item curSkin = Array.Find(dt.PlSkins, skin => skin.IsArranged);
				infoDtStr = $"{dt.NickName}_{dt.Lv}_{dt.Fame}_{curSkin.Name}";
				print($"onClickSignInLoginBtn():: infoDtStr= {infoDtStr}");
				reqSaveInfo(infoDtStr);
			}
			//* 新規登録
			else if(msg == "Register success") {
				HM._.ui.RegisterPopUp.SetActive(false);
				HM._.ui.LoginPopUp.SetActive(true);
			}
		}
	}
	public IEnumerator coAutoLogin() {
		Debug.Log("<color=red>coAutoLogin()::</color>");
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
		Debug.Log("AccountManager():: <color=yellow>res= " + res + "</color>");

		if(res.Contains("Fail")) {
			string msg = res.Split(":")[1];
			HM._.ui.showErrorMsgPopUp(msg);
		}
		else if(res.Contains("Succeed")) {
			string msg = res.Split(":")[1];
			//* AutoLoginログ 表示
			autoLoginLogTxt.gameObject.SetActive(true);
			autoLoginLogTxt.text = msg;
			yield return Util.time3;
			autoLoginLogTxt.gameObject.SetActive(false);

			//* SettingPanalで、ログアウトボタンに切り替え
			HM._.ui.LoginBtn.gameObject.SetActive(false);
			HM._.ui.LogoutBtn.gameObject.SetActive(true);
		}
	}
#endregion
}