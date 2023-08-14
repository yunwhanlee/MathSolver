using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGM : MonoBehaviour { //* MiniGame Manager
    const int MIN_X = -2, MAX_X =2;
    public enum STATUS {READY, PLAY, FINISH, PAUSE};
    [SerializeField] STATUS status; public STATUS Status {get => status; set => status = value;}
    public enum TYPE {MINIGAME1, MINIGAME2, MINIGAME3};
    [SerializeField] TYPE type; public TYPE Type {get => type; set => type = value;}
    public enum MODE {EASY, NORMAL, HARD};
    [SerializeField] MODE mode; public MODE Mode {get => mode;}

    //* OUTSIDE
    public static MGM _;
    public Cam cam;
    public MGUI ui;
    public MGEM mgem; //* ObjとEF生成 (pool)
    public MGResultManager mgrm;
    [SerializeField] Player pl; public Player Pl {get => pl; set => pl = value;} //* LOAD OBJECT FROM HOME
    [SerializeField] Pet pet; public Pet Pet {get => pet; set => pet = value;} //* LOAD OBJECT FROM HOME

    //TODO MiniGameTalkManager

    //* Public Value
    [SerializeField] bool isFinish;
    [SerializeField] int id;
    [SerializeField] int score;         public int Score {get => score; set => score = value;}
    [SerializeField] float curTime;
    [SerializeField] float totalTime;
    [SerializeField] float maxTime;
    [SerializeField] GameObject[] maps;
    [SerializeField] GameObject newBestScoreEF;

    [Space(10)]
    [Header("MINIGAME 2 VALUE")]
    //* MiniGame1 Forest
    [SerializeField] bool isStun;       public bool IsStun {get => isStun; set => isStun = value;}
    [SerializeField] float appleSpan = 1;
    [SerializeField] float plMoveSpd;    public float PlMoveSpd {get => plMoveSpd;}
    [SerializeField] int applePoint;       public int ApplePoint {get => applePoint;}
    [SerializeField] int goldApplePoint;   public int GoldApplePoint {get => goldApplePoint;}
    [SerializeField] int diamondPoint;     public int DiamondPoint {get => diamondPoint;}

    [Space(10)]
    [Header("MINIGAME 2 VALUE")]
    [SerializeField] Transform skyBG;
    [SerializeField] Transform createPadYSpot;
    [SerializeField] GameObject floorColliderObj;   public GameObject FloorColliderObj {get => floorColliderObj;}
    [SerializeField] float padSpan;
    [SerializeField] int jumpPower;      public int JumpPower {get => jumpPower;}
    [SerializeField] float createPadPosY;
    [SerializeField] float camUpSpeed = 30;



    void Awake() {
        _ = this;
        cam = Camera.main.GetComponent<Cam>();
        ui = GameObject.Find("MinigameUIManager").GetComponent<MGUI>();
        mgem = GameObject.Find("MinigameEffectManager").GetComponent<MGEM>();
        mgrm = GameObject.Find("MinigameResultManager").GetComponent<MGResultManager>();

        score = 0;
        maxTime = 60;

        //* 選択したゲーム(ID)
        id = getMapIdx();
        maps[id].SetActive(true);
        type = (id == 0)? TYPE.MINIGAME1 : (id == 1)? TYPE.MINIGAME2 : TYPE.MINIGAME3;

        //* 選択したモード
        if(DB._ == null) mode = MODE.EASY; //! TEST
        else mode = (DB._.MinigameLv == 0)? MODE.EASY : (DB._.MinigameLv == 1)? MODE.NORMAL : MODE.HARD;

        //* タイプ
        switch(type) {
            case TYPE.MINIGAME1: {
                //* モード データ設定
                if(mode == MODE.EASY) {
                    applePoint = Config.MINIGAME1_EASY_OBJ_DATA[0];
                    goldApplePoint = Config.MINIGAME1_EASY_OBJ_DATA[1];
                }
                else if(mode == MODE.NORMAL) {
                    applePoint = Config.MINIGAME1_NORMAL_OBJ_DATA[0];
                    goldApplePoint = Config.MINIGAME1_NORMAL_OBJ_DATA[1];
                    diamondPoint = Config.MINIGAME1_NORMAL_OBJ_DATA[2];
                }
                else if(mode == MODE.HARD) {
                    applePoint = Config.MINIGAME1_HARD_OBJ_DATA[0];
                    goldApplePoint = Config.MINIGAME1_HARD_OBJ_DATA[1];
                    diamondPoint = Config.MINIGAME1_HARD_OBJ_DATA[2];
                }
                break;
            }
            case TYPE.MINIGAME2: {
                //* Base Pads
                mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(-1.5f, createPadPosY), Util.time999);
                mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(0, createPadPosY), Util.time999);
                mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(1.5f, createPadPosY), Util.time999);

                //* Random Pads
                var pad1 = mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(Random.Range(MIN_X, MAX_X + 1), createPadPosY += 2), Util.time999);
                mgem.createObj((int)MGEM.IDX.BananaObj, new Vector2(pad1.transform.position.x, pad1.transform.position.y + 1), Util.time999);
                var pad2 = mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(Random.Range(MIN_X, MAX_X + 1), createPadPosY += 2), Util.time999);
                mgem.createObj((int)MGEM.IDX.BananaObj, new Vector2(pad2.transform.position.x, pad2.transform.position.y + 1), Util.time999);
                var pad3 = mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(Random.Range(MIN_X, MAX_X + 1), createPadPosY += 2), Util.time999);
                mgem.createObj((int)MGEM.IDX.BananaObj, new Vector2(pad3.transform.position.x, pad3.transform.position.y + 1), Util.time999);
                var pad4 = mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(Random.Range(MIN_X, MAX_X + 1), createPadPosY += 2), Util.time999);
                mgem.createObj((int)MGEM.IDX.BananaObj, new Vector2(pad4.transform.position.x, pad4.transform.position.y + 1), Util.time999);
                var pad5 = mgem.createObj((int)MGEM.IDX.JumpingPadObj, new Vector2(Random.Range(MIN_X, MAX_X + 1), createPadPosY += 2), Util.time999);
                mgem.createObj((int)MGEM.IDX.BananaObj, new Vector2(pad5.transform.position.x, pad5.transform.position.y + 1), Util.time999);
                break;
            }
            case TYPE.MINIGAME3: {
                //TODO
                break;
            }
        }
    }

    void Update() {
        if(status == STATUS.FINISH && !isFinish) {
            isFinish = true;
            //* Update Best Score
            if(type == TYPE.MINIGAME1 && score > DB.Dt.Minigame1BestScore) {
                DB.Dt.Minigame1BestScore = score;
                newBestScoreEF.SetActive(true);
                ui.PlayTimerTxt.text = $"<color=yellow>NEW !\nBEST : {score}</color>";
            }
            else if(type == TYPE.MINIGAME2 && score > DB.Dt.Minigame2BestScore) {
                DB.Dt.Minigame2BestScore = score;
                newBestScoreEF.SetActive(true);
                ui.PlayTimerTxt.text = $"<color=yellow>NEW !\nBEST : {score}</color>";
            }
            else {
                ui.PlayTimerTxt.text = $"BEST : {score}";
            }

            pl.Anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
            
            //* 残るオブジェクト全て破壊
            mgem.releaseAllObj();

            //* Exp & Coin Reward
            const int EXP_VAL = 5, COIN_VAL = 10;
            mgrm.setReward(EXP_VAL * score, COIN_VAL * score);
            StartCoroutine(mgrm.coDisplayResultPanel());

            //* Minigame2の場合
            if(type == TYPE.MINIGAME2) {
                StartCoroutine(coSetResultMinigame2()); 
            }
            return;
        }

        if(status != STATUS.PLAY) return;

        //* Time
        totalTime += Time.deltaTime;
        curTime += Time.deltaTime;

        //* Score Txt
        ui.ScoreTxt.text = (id == 0)? $"<sprite name=apple>: {score}"
            : (id == 1)? $"<sprite name=banana>: {score}"
            : $"<sprite name=todo>: {score}";

        //* Timer
        float remainTime = (maxTime - totalTime);
        if(remainTime > 0) 
            ui.PlayTimerTxt.text = remainTime.ToString("N0");
        //* Finish
        else {
            status = STATUS.FINISH;
        }

        //* タイプ
        switch(type) {
            case TYPE.MINIGAME1: {
                //* リンゴ 生成時間
                appleSpan = (remainTime > maxTime * 0.6f)? 1 
                    : (remainTime > maxTime * 0.4f)? 0.7f
                    : (remainTime > maxTime * 0.3f)? 0.5f
                    : 0.3f;

                if(curTime >= appleSpan) {
                    curTime = 0;
                    Vector2 pos = new Vector2(Random.Range(MIN_X, MAX_X), 6);
                    float spd = Random.Range(100, 200) * Time.deltaTime;

                    //* ランダム種類 設定
                    int rand = Random.Range(0, 100);
                    int objIdx = 0;
                    if(mode == MODE.EASY)
                        objIdx = (rand <= 60)? (int)MGEM.IDX.AppleObj
                            : (rand <= 80)? (int)MGEM.IDX.GoldAppleObj
                            : (int)MGEM.IDX.BombObj;
                    else
                        objIdx = (rand <= 45)? (int)MGEM.IDX.AppleObj
                            : (rand <= 70)? (int)MGEM.IDX.GoldAppleObj
                            : (rand <= 90)? (int)MGEM.IDX.BombObj
                            : (int)MGEM.IDX.DiamondObj;

                    Obj obj = mgem.createObj(objIdx, pos, Util.time8).GetComponent<Obj>();

                    obj.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
                    obj.Rigid.bodyType = RigidbodyType2D.Kinematic;
                    obj.Rigid.velocity = Vector2.down * spd;
                }
                break;
            }
            case TYPE.MINIGAME2: {
                float lvBalance = (remainTime > maxTime * 0.6f)? 0.65f
                    : (remainTime > maxTime * 0.4f)? 0.8f
                    : (remainTime > maxTime * 0.3f)? 0.9f
                    : 1.1f;

                //* 足場 生成時間
                padSpan = lvBalance;

                //* カメラー 上スクロール
                cam.transform.Translate(0, camUpSpeed * Time.deltaTime, 0);

                if(curTime >= padSpan) {
                    Debug.Log($"<color=yellow>MGM:: update():: MINIGAME2 足場生成！ createPadYSpot= {createPadYSpot.transform.position}, padSpan= {padSpan}</color>");
                    curTime = 0;
                    var pad = mgem.createObj(
                        (int)MGEM.IDX.JumpingPadObj, 
                        new Vector2(Random.Range(MIN_X, MAX_X + 1), 
                        createPadYSpot.transform.position.y), 
                        Util.time999
                    );
                    var pos = new Vector2(pad.transform.position.x, pad.transform.position.y + 1);

                    //* ランダム種類 設定
                    int rand = Random.Range(0, 100);
                    int objIdx = (rand <= 70)? (int)MGEM.IDX.BananaObj
                        : (int)MGEM.IDX.GoldBananaObj;
                    mgem.createObj(objIdx, pos, Util.time13);
                }

                if(cam.transform.position.y > 13) {
                    skyBG.Translate(0, -camUpSpeed * Time.deltaTime, 0);
                }
                
                break;
            }
            case TYPE.MINIGAME3: {
                //TODO
                break;
            }
        }
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private int getMapIdx() {
        //! For TEST
        if(DB._ == null) return 1;

        int idx = (DB._.SelectMapIdx == (int)Enum.MAP.Minigame1)? 0
            : (DB._.SelectMapIdx == (int)Enum.MAP.Minigame2)? 1
            : (DB._.SelectMapIdx == (int)Enum.MAP.Minigame3)? 2 : -1;
        if(idx == -1) Debug.LogError("存在しないマップINDEXです。０");
        return idx;
    }

    //* MiniGame1
    public IEnumerator coSetPlayerStun() {
        isStun = true;
        yield return Util.time1_5;
        isStun = false;
    }
    IEnumerator coSetResultMinigame2() {
        yield return Util.time1_5;
        pl.transform.position = new Vector2(0, -3.5f);
        pl.Rigid.bodyType = RigidbodyType2D.Static; //* 位置固定
        cam.transform.position = new Vector3(0, 0, -10);
    }
#endregion
}
