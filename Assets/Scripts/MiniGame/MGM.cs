using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGM : MonoBehaviour { //* MiniGame Manager

    public enum STATUS {READY, PLAY, FINISH, PAUSE};
    [SerializeField] STATUS status; public STATUS Status {get => status; set => status = value;}

    //* OUTSIDE
    public static MGM _;
    public Cam cam;
    public MGUI ui;
    public MGEM mgem; //* ObjとEF生成 (pool)

    //TODO MiniGameTalkManager

    [Header("LOAD OBJECT FROM HOME")]
    [SerializeField] Player pl; public Player Pl {get => pl; set => pl = value;}
    [SerializeField] Pet pet; public Pet Pet {get => pet; set => pet = value;}

    //* Public Value
    [Header("MAPS")]
    [SerializeField] int id;
    [SerializeField] GameObject[] maps;
    [SerializeField] int score;         public int Score {get => score; set => score = value;}
    [SerializeField] float curTime;
    [SerializeField] float totalTime;
    [SerializeField] float maxTime = 10;

    //* MiniGame1 Forest
    [SerializeField] float appleSpan = 1;
    [SerializeField] float plMoveSpd;    public float PlMoveSpd {get => plMoveSpd;}

    void Awake() {
        _ = this;
        score = 0;
        cam = Camera.main.GetComponent<Cam>();
        ui = GameObject.Find("MinigameUIManager").GetComponent<MGUI>();
        mgem = GameObject.Find("MinigameEffectManager").GetComponent<MGEM>();

        id = getMapIdx();
        maps[id].SetActive(true);
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

        //* Timer Txt & Finish Game
        float remainTime = (maxTime - totalTime);
        if(remainTime > 0) 
            ui.PlayTimerTxt.text = remainTime.ToString("N0");
        else {
            status = STATUS.FINISH;
            ui.PlayTimerTxt.text = STATUS.FINISH.ToString();
        }

        //* Create Apples (MiniGame1 Forest)
        if(curTime >= appleSpan) {
            curTime = 0;
            Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 6);
            float spd = Random.Range(50, 150) * Time.deltaTime;

            int rand = Random.Range(0, 100);

            //* ランダム種類 設定
            int objIdx = (rand <= 70)? (int)MGEM.IDX.AppleObj
                : (rand <= 20)? (int)MGEM.IDX.GoldAppleObj
                : (int)MGEM.IDX.BombObj;

            Obj obj = mgem.createObj(objIdx, pos, Util.time8).GetComponent<Obj>();
            obj.name = obj.SprRdr.sprite.name;

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

        int idx = (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame1_Orchard)? 0
            : (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame2_Monkeywat)? 1
            : (DB._.SelectMapIdx == (int)Enum.MAP.MiniGame3_IceDragon)? 2 : -1;
        if(idx == -1) Debug.LogError("存在しないマップINDEXです。０");
        return idx;
    }
#endregion
}
