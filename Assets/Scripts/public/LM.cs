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

public class LM : MonoBehaviour {
    public static LM _;
    [Header("VALUE")]
    const int EN = 0, KR = 1, JP = 2;
    const string LangIndex = "LangIndex";
    const string langURL = "https://docs.google.com/spreadsheets/d/1wQ_3T5is4x7eUoq68VBMmJmRRuZyF8Gjg0TiQUyj1cA/export?format=tsv";
    public event Action actLocalizechanged = () => {};
    public int curLangIndex;
    public List<Lang> langs;
    public TMP_FontAsset krFt;
    public TMP_FontAsset jpFt;

    #region SINGLETON
    void Awake() {
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        initLang();
    }
    #endregion

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
    void initLang() {
        //* LOAD APPLICATION SYSTEM LANGUAGE DATA
        Debug.Log("initLang():: Application.systemLanguage= " + Application.systemLanguage);
        const int NOTHING = -1;
        int langIndex = PlayerPrefs.GetInt(LangIndex, NOTHING);
        int systemIndex = langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if(systemIndex == NOTHING) systemIndex = EN;
        int index = (langIndex == NOTHING)? systemIndex : langIndex;

        setLangIndex(index);
    }

    public void setLangIndex(int index) {
        curLangIndex = index;
        PlayerPrefs.SetInt(LangIndex, curLangIndex);
        actLocalizechanged();
    }

    IEnumerator coGetLang() {
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();
        setLangList(www.downloadHandler.text);
    }
    void setLangList(string tsv) {
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
    public string localize(string key) {
        int keyIndex = langs[0].value.FindIndex(i => i.ToLower() == key.ToLower());
        return langs[curLangIndex].value[keyIndex];
    }
#endregion
}
