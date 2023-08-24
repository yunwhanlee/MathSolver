using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.Mathematics;

public class MGM : MonoBehaviour { //* MiniGame Manager
    const int MIN_X = -2, MAX_X =2;
    public enum STATUS {READY, PLAY, FINISH, PAUSE};
    [SerializeField] STATUS status; public STATUS Status {get => status; set => status = value;}
    public enum TYPE {MINIGAME1, MINIGAME2, MINIGAME3};
    [SerializeField] TYPE type; public TYPE Type {get => type; set => type = value;}
    public enum MODE {Easy, Normal, Hard};
    [SerializeField] MODE mode; public MODE Mode {get => mode;}

    //* OUTSIDE
    public static MGM _;
    public Cam cam;
    public MGUI ui;
    public MGEM mgem; //* ObjとEF生成 (pool)
    public MGResultManager mgrm;
    [SerializeField] Player pl; public Player Pl {get => pl; set => pl = value;} //* LOAD ID SPRTIE
    [SerializeField] Pet pet; public Pet Pet {get => pet; set => pet = value;} //* LOAD  ID SPRTIE

    //TODO MiniGameTalkManager

    //* Public Value
    [SerializeField] bool isFinish;
    [SerializeField] int idx;
    [SerializeField] int score;         public int Score {get => score; set => score = value;}
    [SerializeField] float curTime;
    [SerializeField] float totalTime;
    [SerializeField] float maxTime;
    [SerializeField] GameObject[] mapGroups;
    [SerializeField] GameObject newBestScoreEF;
    [SerializeField] int generalPoint;       public int GeneralPoint {get => generalPoint;}
    [SerializeField] int goldPoint;   public int GoldPoint {get => goldPoint;}
    [SerializeField] int diamondPoint;     public int DiamondPoint {get => diamondPoint;}
    [Space(10)]
    [Header("MINIGAME 1 VALUE")]
    //* MiniGame1 Forest
    [SerializeField] bool isStun;       public bool IsStun {get => isStun; set => isStun = value;}
    [SerializeField] float appleSpan = 1;
    [SerializeField] float plMoveSpd;    public float PlMoveSpd {get => plMoveSpd;}

    [Space(10)]
    [Header("MINIGAME 2 VALUE")]
    [SerializeField] Transform skyBG;
    [SerializeField] Transform createPadYSpot;
    [SerializeField] GameObject camMinigame2Group;
    [SerializeField] GameObject floorColliderObj;   public GameObject FloorColliderObj {get => floorColliderObj;}
    [SerializeField] GameObject Parachute;
    [SerializeField] float padSpan;
    [SerializeField] int jumpPower;      public int JumpPower {get => jumpPower;}
    [SerializeField] float createPadPosY;
    [SerializeField] float camUpSpeed = 30;

    [Space(10)]
    [Header("MINIGAME 3 VALUE")]
    [SerializeField] GameObject minigame3ResultBG;  public GameObject Minigame3ResultBG {get => minigame3ResultBG;}
    [SerializeField] GameObject snowFloorBG;
    [SerializeField] GameObject plSnowParticleEF;   public GameObject PlSnowParticleEF {get => plSnowParticleEF;}
    [SerializeField] float snowFloorSpd;

    void Awake() {
        _ = this;
        cam = Camera.main.GetComponent<Cam>();
        ui = GameObject.Find("MinigameUIManager").GetComponent<MGUI>();
        mgem = GameObject.Find("MinigameEffectManager").GetComponent<MGEM>();
        mgrm = GameObject.Find("MinigameResultManager").GetComponent<MGResultManager>();

        score = 0;
        // maxTime = 60;

        //* 選択したゲーム(ID)
        idx = (DB._ == null)? (int)TYPE.MINIGAME3 : DB._.SelectMinigameIdx;
        type = (idx == 0)? TYPE.MINIGAME1 : (idx == 1)? TYPE.MINIGAME2 : TYPE.MINIGAME3;

        //* マップ 表示
        for(int i = 0; i < mapGroups.Length; i++)
            mapGroups[i].SetActive(i == idx); 

        //* ミニゲームによって、オブジェクト処理
        pl = mapGroups[idx].GetComponentInChildren<Player>(); //* Set Player
        camMinigame2Group.SetActive(type == TYPE.MINIGAME2); //* MiniGame2：CamChildGroup 活性化
        cam.Anim.enabled = !(type == TYPE.MINIGAME2); //* カメラー Yを動かすために、MiniGame2ならOFF

        //* 選択した 難易度(モード)
        if(DB._ == null) mode = mode; //! TEST
        else mode = (DB._.MinigameLv == 0)? MODE.Easy : (DB._.MinigameLv == 1)? MODE.Normal : MODE.Hard;

        //* タイプ
        switch(type) {
            case TYPE.MINIGAME1: {
                //* モード Info データ設定
                setPoint(Config.MINIGAME1_EASY_OBJ_DATA
                    , Config.MINIGAME1_NORMAL_OBJ_DATA
                    , Config.MINIGAME1_HARD_OBJ_DATA
                );
                break;
            }
            case TYPE.MINIGAME2: {
                //* モード Info データ設定
                setPoint(Config.MINIGAME2_EASY_OBJ_DATA
                    , Config.MINIGAME2_NORMAL_OBJ_DATA
                    , Config.MINIGAME2_HARD_OBJ_DATA
                );

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
        //* ゲーム終了
        if(status == STATUS.FINISH && !isFinish) {
            SM._.sfxPlay(SM.SFX.CorrectAnswer.ToString());
            isFinish = true;
            if(DB._ == null) {
                Debug.Log("<color=red>FINISH!</color>");
                //* 残るオブジェクト全て破壊
                mgem.releaseAllObj();
                return; //! TEST
            }
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
            else if(type == TYPE.MINIGAME3 && score > DB.Dt.Minigame3BestScore) {
                DB.Dt.Minigame3BestScore = score;
                newBestScoreEF.SetActive(true);
                ui.PlayTimerTxt.text = $"<color=yellow>NEW !\nBEST : {score}</color>";
            }

            pl.Anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
            //* 残るオブジェクト全て破壊
            mgem.releaseAllObj();

            //* Exp & Coin Reward
            const int EXP_VAL = 5, COIN_VAL = 10;
            mgrm.setReward(EXP_VAL * score, COIN_VAL * score);
            StartCoroutine(mgrm.coDisplayResultPanel());

            //* Result画面 準備
            if(type == TYPE.MINIGAME2) {
                StartCoroutine(coSetResultMinigame2()); 
            }
            else if(type == TYPE.MINIGAME3) {
                StartCoroutine(coSetResultMinigame3());
            }
            return;
        }

        if(status != STATUS.PLAY) return;

        //* Time
        totalTime += Time.deltaTime;
        curTime += Time.deltaTime;

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
                    if(mode == MODE.Easy)
                        objIdx = (rand <= 60)? (int)MGEM.IDX.AppleObj
                            : (rand <= 80)? (int)MGEM.IDX.GoldAppleObj
                            : (int)MGEM.IDX.BombObj;
                    else
                        objIdx = (rand <= 45)? (int)MGEM.IDX.AppleObj
                            : (rand <= 70)? (int)MGEM.IDX.GoldAppleObj
                            : (rand <= 90)? (int)MGEM.IDX.BombObj
                            : (int)MGEM.IDX.DiamondObj;
                    //* 生成
                    Obj obj = mgem.createObj(objIdx, pos, Util.time8).GetComponent<Obj>();

                    obj.transform.rotation = Quaternion.Euler(0,0,Random.Range(0, 360));
                    obj.Rigid.bodyType = RigidbodyType2D.Kinematic;
                    obj.Rigid.velocity = Vector2.down * spd;
                }
                break;
            }
            case TYPE.MINIGAME2: {
                float lvBalance = (remainTime > maxTime * 0.6f)? 0.5f
                    : (remainTime > maxTime * 0.4f)? 0.6f
                    : (remainTime > maxTime * 0.3f)? 0.7f
                    : 0.8f;

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
                    int objIdx = 0;
                    if(mode == MODE.Easy)
                        objIdx = (rand <= 70)? (int)MGEM.IDX.BananaObj
                            : (int)MGEM.IDX.GoldBananaObj;
                    else
                        objIdx = (rand <= 60)? (int)MGEM.IDX.BananaObj
                            : (rand <= 80)? (int)MGEM.IDX.GoldBananaObj
                            : (int)MGEM.IDX.DiamondObj;
                    //* 生成
                    var obj = mgem.createObj(objIdx, pos, Util.time13);
                    if(obj.name == MGEM.IDX.DiamondObj.ToString()) {
                        obj.GetComponent<Rigidbody2D>().gravityScale = 0.025f;
                        obj.GetComponent<Rigidbody2D>().mass = 0.05f;
                    }
                }

                if(cam.transform.position.y > 13) {
                    skyBG.Translate(0, -camUpSpeed * Time.deltaTime, 0);
                }
                
                break;
            }
            case TYPE.MINIGAME3: {
                //* SnowFloor PosY Speed
                const float startBgPosY = -24, maxBgPosY = 24;
                const float createPosY = -9;
                snowFloorSpd = (mode == MODE.Easy)? 4 : (mode == MODE.Normal)? 5 : 6;
                snowFloorBG.transform.Translate(0, snowFloorSpd * Time.deltaTime, 0);
                if(snowFloorBG.transform.localPosition.y > maxBgPosY) 
                    snowFloorBG.transform.localPosition = new Vector2(0, startBgPosY);

                //* Create Object
                float createSpan = (mode == MODE.Easy)? 1 : (mode == MODE.Normal)? 0.75f : 0.5f;

                if(curTime >= createSpan) {
                    curTime = 0;
                    List<int> posXList = new List<int>() {-2, -1, 0, 1, 2};
                    //* Create Obstacle 
                    int randCnt = Random.Range(0, 100 + 1); 
                    int cnt = (randCnt < 5)? 0 : (randCnt < 70)? 1 : 2;
                    for(int i = 0; i < cnt; i++) {
                        //* PosX
                        int obstacleRandIdx = Random.Range(0, posXList.Count);
                        float obstaclePosX = posXList[obstacleRandIdx];
                        //* PosY
                        int randY = Random.Range(-3, 3 + 1);
                        float posY = createPosY + randY * 0.25f;
                        //* Create
                        Obj obstacle = mgem.createObj((int)MGEM.IDX.ObstacleObj, new Vector2(obstaclePosX, posY), Util.time8).GetComponent<Obj>();
                        obstacle.activeMoving(snowFloorSpd);
                        //* Scale
                        int randSc = Random.Range(0, 2 + 1);
                        float sc = randSc * 0.2f;
                        Vector2 objSc = obstacle.transform.localScale;
                        obstacle.transform.localScale = new Vector3(objSc.x + sc, objSc.y + sc, 1);
                        //* Random List Remove
                        posXList.RemoveAt(obstacleRandIdx);
                    }

                    //* Create Item
                    randCnt = Random.Range(0, 100 + 1); 
                    cnt = (randCnt < 20)? 0 : (randCnt < 80)? 1 : 2;

                    //* Create Obstacle 
                    for(int i = 0; i < cnt; i++) {
                        //* PosX
                        int itemRandIdx = Random.Range(0, posXList.Count);
                        float posX = posXList[itemRandIdx];
                        //* Create
                        int rand = Random.Range(0, 100);
                        int objIdx = 0;
                        if(mode == MODE.Easy)
                            objIdx = (rand <= 70)? (int)MGEM.IDX.BlueberryObj
                                : (int)MGEM.IDX.GoldBlueberryObj;
                        else
                            objIdx = (rand <= 60)? (int)MGEM.IDX.BlueberryObj
                                : (rand <= 80)? (int)MGEM.IDX.GoldBlueberryObj
                                : (int)MGEM.IDX.DiamondObj;

                        Obj item = mgem.createObj(objIdx, new Vector2(posX, createPosY * 1.25f), Util.time8).GetComponent<Obj>();
                        if(objIdx == (int)MGEM.IDX.DiamondObj) {
                            item.Rigid.bodyType = RigidbodyType2D.Static;
                        }
                        item.activeMoving(snowFloorSpd);
                        //* Random List Remove
                        posXList.RemoveAt(itemRandIdx);
                    }
                }
                break;
            }
        }
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private void setPoint(int[] easyDts, int[] normalDts, int[] hardDts) {
        if(mode == MODE.Easy) {
            generalPoint = easyDts[0];
            goldPoint = easyDts[1];
        }
        else if(mode == MODE.Normal) {
            generalPoint = normalDts[0];
            goldPoint = normalDts[1];
            diamondPoint = normalDts[2];
        }
        else if(mode == MODE.Hard) {
            generalPoint = hardDts[0];
            goldPoint = hardDts[1];
            diamondPoint = hardDts[2];
        }
    }
    //* MiniGame1
    public IEnumerator coSetPlayerStun() {
        isStun = true;
        yield return Util.time1_5;
        isStun = false;
    }
    //* MiniGame2
    IEnumerator coSetResultMinigame2() {
        yield return Util.time1_5;
        cam.Anim.enabled = true;
        cam.transform.position = new Vector3(0, 0, -10);
        floorColliderObj.SetActive(true);
        pl.Rigid.velocity = Vector3.zero;
        pl.transform.GetChild(0).gameObject.SetActive(false);

        //* Fail
        if(totalTime < maxTime) {
            pl.transform.position = new Vector2(0, 15f);
            pl.transform.rotation = Quaternion.Euler(0, 0, 95);
            pl.Sr.flipX = false;

            yield return Util.time1;
            yield return Util.time0_3;
            SM._.sfxPlay(SM.SFX.Explosion.ToString());
            pl.Anim.SetTrigger(Enum.ANIM.DoFail.ToString());
            cam.Anim.SetTrigger(Enum.ANIM.DoCamShake.ToString());
            Util.coPlayBounceAnim(pl.transform);
            yield return Util.time0_5;
            pl.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //* Success
        else {
            Parachute.SetActive(true);
            pl.transform.position = new Vector2(0, 5);
            pl.Rigid.gravityScale = 0.1f;
        }
    }
    IEnumerator coSetResultMinigame3() {
        yield return Util.time1_5;
        minigame3ResultBG.SetActive(true);
        snowFloorBG.SetActive(false);
        plSnowParticleEF.SetActive(false);  
        pl.transform.rotation = quaternion.identity;
        pl.Rigid.bodyType = RigidbodyType2D.Static;
        pl.transform.localPosition = new Vector2(0, -4.5f);
        pl.transform.localScale = Vector2.one;
    }
#endregion
}
