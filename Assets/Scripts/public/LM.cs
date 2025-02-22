using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using TMPro;

[System.Serializable]
public class Lang {
    public string lang, langLocalize;
    public List<string> value = new List<string>();
}

public class LM : MonoBehaviour { //* Language Manager
    public enum LANG_IDX {EN, KR, JP}
    public static LM _;
    [Header("VALUE")]
    const string LangIndex = "LangIndex";
    const string langURL = "https://docs.google.com/spreadsheets/d/1wQ_3T5is4x7eUoq68VBMmJmRRuZyF8Gjg0TiQUyj1cA/export?format=tsv";
    public event Action actLocalizechanged = () => {};
    public int curLangIndex;
    public List<Lang> langs;

    #region SINGLETON
    void Awake() {
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(this);
            initLang();
            getLang();
        }
        else Destroy(this);
    }
    #endregion

    //! Titleシーンで最新化しても、際読み込むと元に戻る。。
    //* HomeシーンのLM(Script)Componentをコピーして、Titleシーンに貼り付けると解決できる。
    [ContextMenu("GOOGLE スプレッドシート 最新化")]
    void getLang() => StartCoroutine(coGetLang());

///------------------------------------------------------------------------------------------
#region EVENT
///------------------------------------------------------------------------------------------
    public void onClickLanguageSettingBtn(int index) {
        // Inspectorビュー：EN = 0, KR = 1, JP = 2
        setLangIndex(index);
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
#endregion
///------------------------------------------------------------------------------------------
#region FUNC
///------------------------------------------------------------------------------------------
    private void initLang() {
        //* LOAD APPLICATION SYSTEM LANGUAGE DATA
        Debug.Log("initLang():: Application.systemLanguage= " + Application.systemLanguage);
        const int NOTHING = -1;
        int langIndex = PlayerPrefs.GetInt(LangIndex, NOTHING);
        int systemIndex = langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if(systemIndex == NOTHING) systemIndex = (int)LM.LANG_IDX.KR; //* 設定言語なかったら、初期設定言語
        int index = (langIndex == NOTHING)? systemIndex : langIndex;
        setLangIndex(index);
    }

    public void setLangIndex(int index) {
        curLangIndex = index;
        PlayerPrefs.SetInt(LangIndex, curLangIndex);
        actLocalizechanged();
    }

    IEnumerator coGetLang() {
        Debug.Log("<color=green>LM:: coGetLang():: Google Sheet 最新化スタート</color>");
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();
        setLangList(www.downloadHandler.text);
    }
    private void setLangList(string tsv) {
        //* 2次元配列
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        string[,] sentence = new string[rowSize, columnSize];

        for(int i = 0; i < rowSize; i++) {
            string[] column = row[i].Split('\t');
            for(int j = 0; j < columnSize; j++)
                sentence[i, j] = column[j];
        }

        //* クラス リスト
        langs = new List<Lang>();
        for( int i = 0; i < columnSize; i++) {
            Lang lang = new Lang();
            lang.lang = sentence[0, i];
            lang.langLocalize = sentence[1, i];

            //* Values
            for(int j = 2; j < rowSize; j++) lang.value.Add(sentence[j, i]);
            langs.Add(lang);
        }
    }
    /// <summary>
    /// 言語切り替え
    /// </summary>
    /// <param name="key">テキスト</param>
    /// <param name="standardlangIdx">国 : (0=EN default, 1=KR, 2=JP)</param>
    /// <returns></returns>
    public string localize(string key, int standardlangIdx = 0) {
        int keyIndex = langs[standardlangIdx].value.FindIndex(i => i.ToLower() == key.ToLower());
        try {
            string str = langs[curLangIndex].value[keyIndex];
            if(str.Contains('\r')) str = str.Replace("\r", "");
            return str;
        }
        catch(Exception err) {
            Debug.Log($"localize(key= <b>{key}</b>):: curLangIndex= {curLangIndex}, keyIndex= <color=red>{keyIndex}</color>");
            Debug.LogError($"{err} :最新化 必要! ➝「GoogleスプレッドシートのA列にある文字」と合うのがないです!");
            return "ERROR";
        }
    }
#endregion
}
