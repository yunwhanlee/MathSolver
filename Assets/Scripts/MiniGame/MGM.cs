using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGM : MonoBehaviour { //* MiniGame Manager

    public enum STATUS {READY, PLAY, FINISH, PAUSE};
    [SerializeField] STATUS status; public STATUS Status {get => status; set => status = value;}

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
    [SerializeField] int id;
    [SerializeField] int score;         public int Score {get => score; set => score = value;}
    [SerializeField] float curTime;
    [SerializeField] float totalTime;
    [SerializeField] float maxTime;
    [SerializeField] GameObject[] maps;
    [SerializeField] GameObject newBestScoreEF;

    //* MiniGame1 Forest
    [SerializeField] bool isStun;       public bool IsStun {get => isStun; set => isStun = value;}
    [SerializeField] float appleSpan = 1;
    [SerializeField] float plMoveSpd;    public float PlMoveSpd {get => plMoveSpd;}
    [SerializeField] int applePoint;       public int ApplePoint {get => applePoint;}
    [SerializeField] int goldApplePoint;   public int GoldApplePoint {get => goldApplePoint;}
    [SerializeField] int diamondPoint;     public int DiamondPoint {get => diamondPoint;}

    void Awake() {
        _ = this;
        cam = Camera.main.GetComponent<Cam>();
        ui = GameObject.Find("MinigameUIManager").GetComponent<MGUI>();
        mgem = GameObject.Find("MinigameEffectManager").GetComponent<MGEM>();
        mgrm = GameObject.Find("MinigameResultManager").GetComponent<MGResultManager>();

        score = 0;
        maxTime = 60;
        id = getMapIdx();
        maps[id].SetActive(true);
        mode = (DB._.MinigameLv == 0)? MODE.EASY : (DB._.MinigameLv == 1)? MODE.NORMAL : MODE.HARD;

        //* Set Obj Point
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
    }

    void Update() {
        if(status != STATUS.PLAY) return;

        //* Time
        totalTime += Time.deltaTime;
        curTime += Time.deltaTime;

        //* Score Txt
        ui.ScoreTxt.text = (id == 0)? $"<sprite name=apple>: {score}"
            : (id == 0)? $"<sprite name=banana>: {score}"
            : $"<sprite name=todo>: {score}";

        //* Timer
        float remainTime = (maxTime - totalTime);
        if(remainTime > 0) 
            ui.PlayTimerTxt.text = remainTime.ToString("N0");
        //* Finish
        else {
            status = STATUS.FINISH;

            //* Update Best Score
            if(score > DB.Dt.Minigame1BestScore) {
                DB.Dt.Minigame1BestScore = score;
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
        }

        //* Create Apples (MiniGame1 Forest)
        appleSpan = (remainTime > maxTime * 0.6f)? 1 
            : (remainTime > maxTime * 0.4f)? 0.7f
            : (remainTime > maxTime * 0.3f)? 0.5f
            : 0.3f;

        if(curTime >= appleSpan) {
            curTime = 0;
            Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 6);
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
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private int getMapIdx() {
        //! For TEST
        if(DB._ == null) return 0;

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
#endregion
}
