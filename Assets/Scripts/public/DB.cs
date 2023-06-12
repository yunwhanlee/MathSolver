using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System;

//* -----------------------------------------------------------------------------------------------------------------
#region 「実際のデータ保存場所」
//* -----------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Data {
    //* Value
    [FormerlySerializedAs("Funiture Items Data")] //* ⇐ Inspectorビューで全て宣言
    [Header("FUNITURE ITEM DATA")]
    [SerializeField] Funiture[] funitures;    public Funiture[] Funitures {get => funitures; set => funitures = value;}
    [SerializeField] Funiture[] decorations;    public Funiture[] Decorations {get => decorations; set => decorations = value;}
    [SerializeField] Funiture[] bgs;    public Funiture[] Bgs {get => bgs; set => bgs = value;}
    [SerializeField] Funiture[] mats;    public Funiture[] Mats {get => mats; set => mats = value;}

    [Header("VALUE")]
    [SerializeField] int coin; public int Coin {get => coin; set => coin = value;}
    [SerializeField] int playerId; public int PlayerId {get => playerId; set => playerId = value;}
    [SerializeField] int petId; public int PetId {get => petId; set => petId = value;}
    [SerializeField] int bestScore; public int BestScore {get => bestScore; set => bestScore = value;}

    public void setCoin(int value) {
        coin += value;
        if(coin < 0) coin = 0;
    }
}
#endregion

//* -----------------------------------------------------------------------------------------------------------------
#region 「データ管理マネージャー」
//* -----------------------------------------------------------------------------------------------------------------
public class DB : MonoBehaviour {
    public static DB _ {get; private set;}
    [SerializeField] bool isReset;
    const string Database = "DB";
    [SerializeField] Data dt;   public static Data Dt {get => _.dt; set => _.dt = value;}
    void Awake() {
        if(isReset) reset();

    #region SINGLETON
        Debug.Log($"Awake {_ == null}");
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    #endregion

        if(load() == null) reset();
        else dt = load();

        //? SINGLETONの場合、DontDestroyOnLoad
        Debug.Log("AAA Spr割り当て");
        //* Funiture型のオブジェクトのSpr変数を設定
        Array.ForEach(dt.Funitures, item => item.Spr = item.Prefab.GetComponent<SpriteRenderer>().sprite);
        Array.ForEach(dt.Decorations, item => item.Spr = item.Prefab.GetComponent<SpriteRenderer>().sprite);
        Array.ForEach(dt.Bgs, item => item.Spr = item.Prefab.GetComponent<SpriteRenderer>().sprite);
        Array.ForEach(dt.Mats, item => item.Spr = item.Prefab.GetComponent<SpriteRenderer>().sprite);
    }
/// -----------------------------------------------------------------------------------------------------------------
#region QUIT APP EVENT
/// -----------------------------------------------------------------------------------------------------------------
    #if UNITY_EDITOR
        void OnApplicationQuit() {
            Debug.Log("<color=yellow>QUIT APP(PC)::OnApplicationQuit():: SAVE</color>");
            this.save();
        }
    #elif UNITY_ANDROID
        void OnApplicationPause(bool paused){
            //* ゲームが開くとき（paused == true）にも起動されるので注意が必要。
            if(paused == true) {
                Debug.Log("<color=yellow>QUIT APP(Mobile)::OnApplicationPause( "+paused+" ):: Scene= " + SceneManager.GetActiveScene().name);
                this.save();
            }
        }
    #endif
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region SAVE
/// -----------------------------------------------------------------------------------------------------------------
    public void save() {
        PlayerPrefs.SetString(Database, JsonUtility.ToJson(dt, true)); //* Serialize To Json
        //* Print
        string json = PlayerPrefs.GetString(Database);
        Debug.Log($"★SAVE:: The Key: {Database} Exists? {PlayerPrefs.HasKey(Database)}, Data ={json}");
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region LOAD
/// -----------------------------------------------------------------------------------------------------------------
    public Data load() {
        //* (BUG)最初の実行だったら、ロードデータがないから、リセットして初期化。
        if(!PlayerPrefs.HasKey(Database)){            
            return null;
        }
        //* Json 読み込み
        string json = PlayerPrefs.GetString(Database);
        Debug.Log($"★LOAD:: (json == null)? {json == null}, \ndata= {json}");
        //* Json クラス化
        Data dt = JsonUtility.FromJson<Data>(json); 
        return dt;
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region RESET
/// -----------------------------------------------------------------------------------------------------------------
    public void reset() {
        Debug.Log($"★RESET:: The Key: {Database} Exists? {PlayerPrefs.HasKey(Database)}");
        PlayerPrefs.DeleteAll();
        //* 選択したキャラ
        dt.PlayerId = 0;
        dt.PetId = 0;
        //* 財貨
        dt.Coin = 5000;
        //* ロック
        Array.ForEach(dt.Funitures, item => item.IsLock = true);
        Array.ForEach(dt.Decorations, item => item.IsLock = true);
        Array.ForEach(dt.Bgs, item => item.IsLock = true);
        Array.ForEach(dt.Mats, item => item.IsLock = true);
        //* 位置
        Array.ForEach(dt.Funitures, item => item.Pos = Vector3.zero);
        Array.ForEach(dt.Decorations, item => item.Pos = Vector3.zero);
        Array.ForEach(dt.Bgs, item => item.Pos = Vector3.zero);
        Array.ForEach(dt.Mats, item => item.Pos = Vector3.zero);
    }
#endregion
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
#endregion
}
#endregion