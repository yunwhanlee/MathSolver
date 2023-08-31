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
    [Header("VALUE")]
    //* Math-Pid-Info
    [SerializeField] bool isFinishDiagnosis;   public bool IsFinishDiagnosis {get => isFinishDiagnosis; set => isFinishDiagnosis = value;}
    [SerializeField] string myAuthorization;   public string MyAuthorization {get => myAuthorization; set => myAuthorization = value;}
    [SerializeField] string myMBR_ID;   public string MyMBR_ID {get => myMBR_ID; set => myMBR_ID = value;}

    //* Setting
    [SerializeField] bool isActiveSound;    public bool IsActiveSound {get => isActiveSound; set => isActiveSound = value;}
    [SerializeField] bool isActiveMusic;    public bool IsActiveMusic {get => isActiveMusic; set => isActiveMusic = value;}
    [Header("ACCOUNT")]
    [SerializeField] bool isLogin;          public bool IsLogin {get => isLogin; set => isLogin = value;}
    [SerializeField] string accountID;      public string AccountID {get => accountID; set => accountID = value;}
    [SerializeField] string accountPassword;      public string AccountPassword {get => accountPassword; set => accountPassword = value;}

    //* Player
    [SerializeField] string nickName; public string NickName {get => nickName; set => nickName = value;}
    [SerializeField] int lv; public int Lv {get => lv; set => lv = value;}
    [SerializeField] int coin; public int Coin {get => coin; set => coin = value;}
    [SerializeField] int exp; public int Exp {get => exp; set => exp = value;}
    [SerializeField] int maxExp; public int MaxExp {get => maxExp; set => maxExp = value;}
    [SerializeField] int fame; public int Fame {get => fame; set => fame = value;}

    //* Home
    [SerializeField] int mainQuestID; public int MainQuestID {get => mainQuestID; set => mainQuestID = value;}
    [SerializeField] int gachaCnt;  public int GachaCnt {get => gachaCnt; set => gachaCnt = value;}
    [SerializeField] int minigame1BestScore;    public int Minigame1BestScore {get => minigame1BestScore; set => minigame1BestScore = value;}
    [SerializeField] int minigame2BestScore;    public int Minigame2BestScore {get => minigame2BestScore; set => minigame2BestScore = value;}
    [SerializeField] int minigame3BestScore;    public int Minigame3BestScore {get => minigame3BestScore; set => minigame3BestScore = value;}

    [Header("MINIGAME REWARD")]
    [SerializeField] bool[] minigame1RewardTriggers; public bool[] Minigame1RewardTriggers {get => minigame1RewardTriggers; set => minigame1RewardTriggers = value;}
    [SerializeField] bool[] minigame2RewardTriggers; public bool[] Minigame2RewardTriggers {get => minigame2RewardTriggers; set => minigame2RewardTriggers = value;}
    [SerializeField] bool[] minigame3RewardTriggers; public bool[] Minigame3RewardTriggers {get => minigame3RewardTriggers; set => minigame3RewardTriggers = value;}

    [Header("ACHIEVEMENT")]
    [SerializeField] int acvCorrectAnswerLv;  public int AcvCorrectAnswerLv {get => acvCorrectAnswerLv; set => acvCorrectAnswerLv = value;}
    [SerializeField] int acvCorrectAnswerCnt;  public int AcvCorrectAnswerCnt {get => acvCorrectAnswerCnt; set => acvCorrectAnswerCnt = value;}
    [SerializeField] int acvSkinLv;  public int AcvSkinLv {get => acvSkinLv; set => acvSkinLv = value;}
    [SerializeField] int acvSkinCnt;  public int AcvSkinCnt {get => acvSkinCnt; set => acvSkinCnt = value;}
    [SerializeField] int acvPetLv;  public int AcvPetLv {get => acvPetLv; set => acvPetLv = value;}
    [SerializeField] int acvPetCnt;  public int AcvPetCnt {get => acvPetCnt; set => acvPetCnt = value;}
    [SerializeField] int acvCoinAmountLv;  public int AcvCoinAmountLv {get => acvCoinAmountLv; set => acvCoinAmountLv = value;}
    [SerializeField] int acvCoinAmount;  public int AcvCoinAmount {get => acvCoinAmount; set => acvCoinAmount = value;}

    [Header("QUEST TUTORIAL TRIGGER")]
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

    [Header("QUEST UNLOCK MAP:BG → [0]: Accept, [1]: Reward")]
    [SerializeField] bool[] isUnlockMap1BG2Arr;  public bool[] IsUnlockMap1BG2Arr {get => isUnlockMap1BG2Arr; set => isUnlockMap1BG2Arr = value;}
    [SerializeField] bool[] isUnlockMap1BG3Arr;  public bool[] IsUnlockMap1BG3Arr {get => isUnlockMap1BG3Arr; set => isUnlockMap1BG3Arr = value;}
    [SerializeField] bool isUnlockMinigame1;     public bool IsUnlockMinigame1 {get => isUnlockMinigame1; set => isUnlockMinigame1 = value;}
    //* Jungle
    [SerializeField] bool[] isOpenMap2UnlockBG1Arr;  public bool[] IsOpenMap2UnlockBG1Arr {get => isOpenMap2UnlockBG1Arr; set => isOpenMap2UnlockBG1Arr = value;}
    [SerializeField] bool[] isUnlockMap2BG2Arr;  public bool[] IsUnlockMap2BG2Arr {get => isUnlockMap2BG2Arr; set => isUnlockMap2BG2Arr = value;}
    [SerializeField] bool[] isUnlockMap2BG3Arr;  public bool[] IsUnlockMap2BG3Arr {get => isUnlockMap2BG3Arr; set => isUnlockMap2BG3Arr = value;}
    [SerializeField] bool isUnlockMinigame2;     public bool IsUnlockMinigame2 {get => isUnlockMinigame2; set => isUnlockMinigame2 = value;}
    //* Tundra
    [SerializeField] bool[] isOpenMap3UnlockBG1Arr;  public bool[] IsOpenMap3UnlockBG1Arr {get => isOpenMap3UnlockBG1Arr; set => isOpenMap3UnlockBG1Arr = value;}
    [SerializeField] bool[] isUnlockMap3BG2Arr;  public bool[] IsUnlockMap3BG2Arr {get => isUnlockMap3BG2Arr; set => isUnlockMap3BG2Arr = value;}
    [SerializeField] bool[] isUnlockMap3BG3Arr;  public bool[] IsUnlockMap3BG3Arr {get => isUnlockMap3BG3Arr; set => isUnlockMap3BG3Arr = value;}
    [SerializeField] bool isUnlockMinigame3;     public bool IsUnlockMinigame3 {get => isUnlockMinigame3; set => isUnlockMinigame3 = value;}

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
        if(value > 0) {
            Debug.Log($"setCoin({value}):: Earn Coin +acvCoinAmount");
            acvCoinAmount += value;
        }
        coin += value;
        if(coin < 0) coin = 0;
    }
    public float 
    getExpPer() {
        maxExp = Config.LV_EXP_UNIT * lv;
        //* Level Up!
        if(exp >= maxExp) {
            lv++;
            DB._.LvUpCnt++;
            exp -= maxExp;
            return 1; //* 必ず１を渡すことでレベルアップ確保！
            // return exp;
        }
        return ((float)exp) / ((float)maxExp);
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
    [SerializeField] int selectMinigameIdx;   public int SelectMinigameIdx {get => selectMinigameIdx; set => selectMinigameIdx = value;}
    [SerializeField] int minigameLv;    public int MinigameLv {get => minigameLv; set => minigameLv = value;} //* Minigame 難易度
    [SerializeField] int legacyCnt; public int LegacyCnt {get => legacyCnt; set => legacyCnt = value;}

    const string Database = "DB";
    [SerializeField] Data dt;   public static Data Dt {get => _.dt; set => _.dt = value;}
    void Awake() {
        Application.targetFrameRate = 60;
        if(isReset) reset();

        #region SINGLETON
        Debug.Log($"Awake {_ == null}");
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
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
        resetItem(copyDt);
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
    public void resetItem(Data copyDt) {
        Debug.Log("resetItem()::");
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
    public void reset() {
        Debug.Log($"★RESET:: The Key: {Database} Exists? {PlayerPrefs.HasKey(Database)}");
        PlayerPrefs.DeleteAll();
        //* Mathpid-API-info
        dt.IsFinishDiagnosis = false;
        dt.MyAuthorization = "";
        dt.MyMBR_ID = "";

        //* Setting
        dt.IsActiveSound = true;
        dt.IsActiveMusic = true;
        dt.IsLogin = false;
        dt.AccountID = "";
        dt.AccountPassword = "";

        dt.NickName = "";
        dt.Lv = 1;
        dt.Coin = 0;
        dt.Exp = 0;
        dt.MaxExp = Config.LV_EXP_UNIT * dt.Lv;
        dt.Fame = 0;
        dt.MainQuestID = 0;

        //* 服屋 値段
        dt.GachaCnt = 1;
        dt.Minigame1BestScore = 0;
        dt.Minigame2BestScore = 0;
        dt.Minigame3BestScore = 0;

        #region MINIGAME REWARD
        dt.Minigame1RewardTriggers = new bool[3] {false, false, false};
        dt.Minigame2RewardTriggers = new bool[3] {false, false, false};
        dt.Minigame3RewardTriggers = new bool[3] {false, false, false};
        #endregion

        #region ACHIEVE
        dt.AcvCorrectAnswerLv = 1;
        dt.AcvCorrectAnswerCnt = 0;
        dt.AcvSkinLv = 1;
        dt.AcvSkinCnt = 0;
        dt.AcvPetLv = 1;
        dt.AcvPetCnt = 0;
        dt.AcvCoinAmountLv = 1;
        dt.AcvCoinAmount = 0;
        #endregion

        #region QUEST
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

        
        //* Unlock Map:BG
        dt.IsUnlockMap1BG2Arr = new bool[2] {false, false};
        dt.IsUnlockMap1BG3Arr = new bool[2] {false, false};
        dt.IsUnlockMinigame1 = false;
        dt.IsOpenMap2UnlockBG1Arr = new bool[2] {false, false};
        dt.IsUnlockMap2BG2Arr = new bool[2] {false, false};
        dt.IsUnlockMap2BG3Arr = new bool[2] {false, false};
        dt.IsUnlockMinigame2 = false;
        dt.IsOpenMap3UnlockBG1Arr = new bool[2] {false, false};
        dt.IsUnlockMap3BG2Arr = new bool[2] {false, false};
        dt.IsUnlockMap3BG3Arr = new bool[2] {false, false};
        dt.IsUnlockMinigame3 = false;

        #endregion

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

        //* Notify
        Array.ForEach(dt.Funitures, ft => ft.IsNotify = false);
        Array.ForEach(dt.Decorations, dc => dc.IsNotify = false);
        Array.ForEach(dt.Bgs, bg => bg.IsNotify = false);
        Array.ForEach(dt.Mats, mt => mt.IsNotify = false);

        //* Arrange
        Array.ForEach(dt.Funitures, ft => ft.IsArranged = false);
        Array.ForEach(dt.Decorations, dc => dc.IsArranged = false);
        Array.ForEach(dt.Bgs, bg => bg.IsArranged = false);
        Array.ForEach(dt.Mats, mt => mt.IsArranged = false);

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