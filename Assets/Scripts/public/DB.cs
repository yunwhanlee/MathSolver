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
    public const int EXP_MAX_UNIT = 100;


    [Header("VALUE")]
    //* Math-Pid-Info
    [SerializeField] string myAuthorization;   public string MyAuthorization {get => myAuthorization; set => myAuthorization = value;}
    [SerializeField] string myMBR_ID;   public string MyMBR_ID {get => myMBR_ID; set => myMBR_ID = value;}

    //* Player
    [SerializeField] string nickName; public string NickName {get => nickName; set => nickName = value;}
    [SerializeField] int lv; public int Lv {get => lv; set => lv = value;}
    [SerializeField] int coin; public int Coin {get => coin; set => coin = value;}
    [SerializeField] int exp; public int Exp {get => exp; set => exp = value;}
    [SerializeField] int mainQuestID; public int MainQuestID {get => mainQuestID; set => mainQuestID = value;}

    [SerializeField] int gachaCnt;  public int GachaCnt {get => gachaCnt; set => gachaCnt = value;}

    [Header("TUTORIAL TRIGGER")]
    //* Home
    [SerializeField] bool isTutoRoomTrigger;   public bool IsTutoRoomTrigger {get => isTutoRoomTrigger; set => isTutoRoomTrigger = value;}
    [SerializeField] bool isTutoFunitureShopTrigger;   public bool IsTutoFunitureShopTrigger {get => isTutoFunitureShopTrigger; set => isTutoFunitureShopTrigger = value;}
    [SerializeField] bool isTutoClothShopTrigger;   public bool IsTutoClothShopTrigger {get => isTutoClothShopTrigger; set => isTutoClothShopTrigger = value;}
    [SerializeField] bool isTutoInventoryTrigger;   public bool IsTutoInventoryTrigger {get => isTutoInventoryTrigger; set => isTutoInventoryTrigger = value;}
    [SerializeField] bool isTutoGoGameTrigger;   public bool IsTutoGoGameTrigger {get => isTutoGoGameTrigger; set => isTutoGoGameTrigger = value;}
    [SerializeField] bool isTutoWorldMapTrigger;   public bool IsTutoWorldMapTrigger {get => isTutoWorldMapTrigger; set => isTutoWorldMapTrigger = value;}
    [SerializeField] bool isTutoFinishTrigger;   public bool IsTutoFinishTrigger {get => isTutoFinishTrigger; set => isTutoFinishTrigger = value;}
    //* Game
    [SerializeField] bool isTutoDiagChoiceDiffTrigger;   public bool IsTutoDiagChoiceDiffTrigger {get => isTutoDiagChoiceDiffTrigger; set => isTutoDiagChoiceDiffTrigger = value;}
    [SerializeField] bool isTutoDiagFirstQuizTrigger;   public bool IsTutoDiagFirstQuizTrigger {get => isTutoDiagFirstQuizTrigger; set => isTutoDiagFirstQuizTrigger = value;}
    [SerializeField] bool isTutoDiagFirstAnswerTrigger;   public bool IsTutoDiagFirstAnswerTrigger {get => isTutoDiagFirstAnswerTrigger; set => isTutoDiagFirstAnswerTrigger = value;}
    [SerializeField] bool isTutoDiagResultTrigger;   public bool IsTutoDiagResultTrigger {get => isTutoDiagResultTrigger; set => isTutoDiagResultTrigger = value;}

    [Header("MAP UNLOCK TRIGGER")]
    [SerializeField] bool isMap1BG1Trigger;   public bool IsMap1BG1Trigger {get => isMap1BG1Trigger; set => isMap1BG1Trigger = value;} //* 最初だから、いつも活性化
    [SerializeField] bool isMap1BG2Trigger;   public bool IsMap1BG2Trigger {get => isMap1BG2Trigger; set => isMap1BG2Trigger = value;}
    [SerializeField] bool isMap1BG3Trigger;   public bool IsMap1BG3Trigger {get => isMap1BG3Trigger; set => isMap1BG3Trigger = value;}

    [SerializeField] bool isMap2BG1Trigger;   public bool IsMap2BG1Trigger {get => isMap2BG1Trigger; set => isMap2BG1Trigger = value;}
    [SerializeField] bool isMap2BG2Trigger;   public bool IsMap2BG2Trigger {get => isMap2BG2Trigger; set => isMap2BG2Trigger = value;}
    [SerializeField] bool isMap2BG3Trigger;   public bool IsMap2BG3Trigger {get => isMap2BG3Trigger; set => isMap2BG3Trigger = value;}

    [SerializeField] bool isMap3BG1Trigger;   public bool IsMap3BG1Trigger {get => isMap3BG1Trigger; set => isMap3BG1Trigger = value;}
    [SerializeField] bool isMap3BG2Trigger;   public bool IsMap3BG2Trigger {get => isMap3BG2Trigger; set => isMap3BG2Trigger = value;}
    [SerializeField] bool isMap3BG3Trigger;   public bool IsMap3BG3Trigger {get => isMap3BG3Trigger; set => isMap3BG3Trigger = value;}

    [FormerlySerializedAs("Funiture Items Data")] //* ⇐ Inspectorビューで全て宣言
    [Header("FUNITURE ITEM")]
    [SerializeField] Funiture[] funitures;    public Funiture[] Funitures {get => funitures; set => funitures = value;}
    [SerializeField] Funiture[] decorations;    public Funiture[] Decorations {get => decorations; set => decorations = value;}
    [SerializeField] BgFuniture[] bgs;    public BgFuniture[] Bgs {get => bgs; set => bgs = value;}
    [SerializeField] Funiture[] mats;    public Funiture[] Mats {get => mats; set => mats = value;}

    [Header("PLAYERSKIN ITEM")]
    [SerializeField] PlayerSkin[] plSkins;    public PlayerSkin[] PlSkins {get => plSkins; set => plSkins = value;}
    [SerializeField] PetSkin[] petSkins;   public PetSkin[] PtSkins {get => petSkins; set => petSkins = value;}


    public void setCoin(int value) {
        coin += value;
        if(coin < 0) coin = 0;
    }
    public float getExpPer() {
        int max = EXP_MAX_UNIT * lv;
        //* Level Up!
        if(exp >= max) {
            lv++;
            DB._.LvUpCnt++;
            exp -= max;
            return exp;
        }
        return ((float)exp) / ((float)max);
    }
}
#endregion

//* -----------------------------------------------------------------------------------------------------------------
#region 「データ管理マネージャー」
//* -----------------------------------------------------------------------------------------------------------------
public class DB : MonoBehaviour {
    public static DB _ {get; private set;}
    [SerializeField] bool isReset;
    [SerializeField] int lvUpCnt;   public int LvUpCnt {get => lvUpCnt; set => lvUpCnt = value;}
    [SerializeField] int selectMapIdx;   public int SelectMapIdx {get => selectMapIdx; set => selectMapIdx = value;}
    const string Database = "DB";
    const int DEF_WALL_IDX = 0;
    const int DEF_FLOOR_IDX = 9;
    [SerializeField] Data dt;   public static Data Dt {get => _.dt; set => _.dt = value;}
    void Awake() {
        Application.targetFrameRate = 50;
        if(isReset) reset();

        #region SINGLETON
        Debug.Log($"Awake {_ == null}");
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
            
        #endregion

        Data copyDt = null;
        copyDt = dt; //* Inspectorビューで、既に登録したデータをcopyデータとして使う

        if(load() == null) {
            Debug.Log("<color=red>ロードデータない：リセット</color>");
            reset();
        }
        else {
            Debug.Log("<color=yellow>ロードデータ有り：データロード</color>");
            //! load() PlayerPrefs方式では、「Sprite」と「Prefab:GameObject」データは保存・ロードができない
            dt = load(); //* ロードした始点、SpriteとPrefabデータはなくなる
        }

        //* ロードする前に代入したcopyDtで、保存できないタイプのデータを設定
        //* 家具
        setFunitureTypeData(dt.Funitures, copyDt.Funitures);
        setFunitureTypeData(dt.Decorations, copyDt.Decorations);
        setFunitureTypeData(dt.Mats, copyDt.Mats);
        setBgFunitureTypeData(dt.Bgs, copyDt.Bgs);
        //* プレイヤー
        setPlayerSkinData(dt.PlSkins, copyDt.PlSkins);
        //* ペット
        setPetSkinData(dt.PtSkins, copyDt.PtSkins);
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
        //* Mathpid-API-info
        dt.MyAuthorization = "";
        dt.MyMBR_ID = "";

        dt.NickName = "";
        dt.Lv = 1;
        dt.Coin = 0;
        dt.Exp = 0;
        dt.MainQuestID = 0;

        //* 服屋 値段
        dt.GachaCnt = 1;

        //* Tutorial Trigger
        dt.IsTutoRoomTrigger = true;
        dt.IsTutoFunitureShopTrigger = true;
        dt.IsTutoClothShopTrigger = true;
        dt.IsTutoInventoryTrigger = true;
        dt.IsTutoGoGameTrigger = true;
        dt.IsTutoWorldMapTrigger = true;
        dt.IsTutoFinishTrigger = true;

        dt.IsTutoDiagChoiceDiffTrigger = true;
        dt.IsTutoDiagFirstQuizTrigger = true;
        dt.IsTutoDiagFirstAnswerTrigger = true;
        dt.IsTutoDiagResultTrigger = true;

        //* MapUnlock Trigger
        dt.IsMap1BG1Trigger = true; //* 最初だから、いつも活性化
        dt.IsMap1BG2Trigger = false;
        dt.IsMap1BG3Trigger = false;
        dt.IsMap2BG1Trigger = false;
        dt.IsMap2BG2Trigger = false;
        dt.IsMap2BG3Trigger = false;
        dt.IsMap3BG1Trigger = false;
        dt.IsMap3BG2Trigger = false;
        dt.IsMap3BG3Trigger = false;

        #region FUNITURE
        //* ロック
        Array.ForEach(dt.Funitures, ft => ft.IsLock = true);
        Array.ForEach(dt.Decorations, dc => dc.IsLock = true);
        Array.ForEach(dt.Bgs, bg => bg.IsLock = true);
        Array.ForEach(dt.Mats, mt => mt.IsLock = true);

        //* 位置
        Array.ForEach(dt.Funitures, ft => ft.Pos = Vector3.zero);
        Array.ForEach(dt.Decorations, dc => dc.Pos = Vector3.zero);
        //// Array.ForEach(dt.Bgs, item => item.Pos = Vector3.zero);
        Array.ForEach(dt.Mats, mt => mt.Pos = Vector3.zero);

        //* 反転
        Array.ForEach(dt.Funitures, ft => ft.IsFlat = false);
        Array.ForEach(dt.Decorations, dc => dc.IsFlat = false);
        //// Array.ForEach(dt.Bgs, item => item.IsFlat = false);
        Array.ForEach(dt.Mats, mt => mt.IsFlat = false);

        //* Wall 最初IDXのみ ON
        BgFuniture defualtWall = Array.Find(dt.Bgs, bg => bg.Name == "Wall");
        defualtWall.IsLock = false;
        defualtWall.IsArranged = true;
        //* Floor 最初IDXのみ ON
        BgFuniture defualtFloor = Array.Find(dt.Bgs, bg => bg.Name == "Floor");
        defualtFloor.IsLock = false;
        defualtFloor.IsArranged = true;
        #endregion

        #region INVENTORY
        //* ロック
        Array.ForEach(dt.PlSkins, plSk => plSk.IsLock = true);
        Array.ForEach(dt.PtSkins, ptSk => ptSk.IsLock = true);

        //* Player 最初IDXのみ ON
        dt.PlSkins[0].IsLock = false;
        dt.PlSkins[0].IsArranged = true;

        //* Pet 最初IDXのみ ON
        dt.PtSkins[0].IsLock = false;
        dt.PtSkins[0].IsArranged = true;
        #endregion
    }
    #endregion
    /// -----------------------------------------------------------------------------------------------------------------
    #region SET UNSAVED DATA
    /// -----------------------------------------------------------------------------------------------------------------
    void setFunitureTypeData(Funiture[] dtItems, Funiture[] copyItems) {
        int i = 0;
        Array.ForEach(dtItems, item => {
            item.Prefab = copyItems[i].Prefab;
            item.Spr = copyItems[i++].Prefab.GetComponent<SpriteRenderer>().sprite;
        });
    }
    void setBgFunitureTypeData(BgFuniture[] dtItems, BgFuniture[] copyItems) {
        int i = 0;
        Array.ForEach(dt.Bgs, item => item.Spr = copyItems[i++].Spr);
    }
    void setPlayerSkinData(PlayerSkin[] dtItems, PlayerSkin[] copyItems) {
        int i = 0;
        Array.ForEach(dtItems, item => {
            item.Spr = copyItems[i].Spr;
            item.SprLibraryAsset = copyItems[i++].SprLibraryAsset;
        });
    }
    void setPetSkinData(PetSkin[] dtItems, PetSkin[] copyItems) {
        int i = 0;
        Array.ForEach(dtItems, item => {
            item.Spr = copyItems[i].Spr;
            item.SprLibraryAsset = copyItems[i++].SprLibraryAsset;
        });
    }

    #endregion
#endregion
}