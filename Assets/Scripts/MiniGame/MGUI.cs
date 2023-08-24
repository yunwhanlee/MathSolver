using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System;

public class MGUI : MonoBehaviour {
    [SerializeField] GameObject resultPanel;    public GameObject ResultPanel {get => resultPanel; set => resultPanel = value;}
    [SerializeField] GameObject giveUpPopUp;    public GameObject GiveUpPopUp {get => giveUpPopUp; set => giveUpPopUp = value;}
    [SerializeField] Animator switchScreenAnim; public Animator SwitchScreenAnim {get => switchScreenAnim;}
    [SerializeField] Image[] switchScreenImgs;
    [SerializeField] Sprite[] switchScreenSprs;

    [SerializeField] Button startScrBtn;
    [SerializeField] TextMeshProUGUI playerLvTxt;
    [SerializeField] TextMeshProUGUI modeTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;   public TextMeshProUGUI ScoreTxt {get => scoreTxt;}
    [SerializeField] TextMeshProUGUI playTimerTxt;  public TextMeshProUGUI PlayTimerTxt {get => playTimerTxt;}
    [SerializeField] TextMeshProUGUI titleTxt;      public TextMeshProUGUI TitleTxt {get => titleTxt;}
    [SerializeField] TextMeshProUGUI contentTxt;    public TextMeshProUGUI ContentTxt {get => contentTxt;}

    [SerializeField] bool isLeftArrowBtnPressed;
    [SerializeField] bool isRightArrowBtnPressed;

    //* MiniGame1 Forest
    [SerializeField] Button leftArrowBtn;   public Button LeftArrowBtn {get => leftArrowBtn; set => leftArrowBtn = value;}
    [SerializeField] Button rightArrowBtn;   public Button RightArrowBtn {get => rightArrowBtn; set => rightArrowBtn = value;}

    //* MiniGame3 Tundra
    [SerializeField] bool isRotating = false;  // 각도 보간 진행 여부
    [SerializeField] float targetRotationZ = 0f;  // 목표 각도 (오른쪽일 때 5, 왼쪽일 때 -5)
    private float rotationStep = 90f;  // 회전 각도의 변화 속도 (각도/초)

    void Start() {
        var mgm = MGM._;
        resultPanel.SetActive(false);
        SM._.sfxPlay(SM.SFX.SceneSpawn.ToString());
        Array.ForEach(switchScreenImgs, img => 
            img.sprite = (mgm.Type == MGM.TYPE.MINIGAME1)? switchScreenSprs[0]
            : (mgm.Type == MGM.TYPE.MINIGAME2)? switchScreenSprs[1]
            : switchScreenSprs[2]
        );

        if(DB._ == null) playerLvTxt.text = "99"; //! TEST
        else playerLvTxt.text = DB.Dt.Lv.ToString();

        if(DB._ == null) modeTxt.text = LM._.localize(MGM.MODE.Normal.ToString()); //! TEST
        else modeTxt.text = (DB._.MinigameLv == 0)? LM._.localize(MGM.MODE.Easy.ToString())
            : (DB._.MinigameLv == 1)? LM._.localize(MGM.MODE.Normal.ToString())
            : LM._.localize(MGM.MODE.Hard.ToString());

        scoreTxt.text = "";
        leftArrowBtn.gameObject.SetActive(false);
        rightArrowBtn.gameObject.SetActive(false);

        //* タイトル
        titleTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? LM._.localize(Config.MINIGAME1_TITLE)
            : (mgm.Type == MGM.TYPE.MINIGAME2)? LM._.localize(Config.MINIGAME2_TITLE)
            : LM._.localize(Config.MINIGAME3_TITLE);

        //* コンテンツ
        contentTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? LM._.localize(Config.MINIGAME1_CONTENT)
            : (mgm.Type == MGM.TYPE.MINIGAME2)? LM._.localize(Config.MINIGAME2_CONTENT)
            : LM._.localize(Config.MINIGAME3_CONTENT);

        //* Score Txt
        scoreTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? $"<sprite name=apple>: {mgm.Score}"
            : (mgm.Type == MGM.TYPE.MINIGAME2)? $"<sprite name=banana>: {mgm.Score}"
            : $"<sprite name=blueberry>: {mgm.Score}";
    }

    void Update() {
        #region MINIGAME1 & MINIGAME 2
        if(MGM._.Status != MGM.STATUS.PLAY) return;
        if(MGM._.IsStun) return;
        var mgm = MGM._;
        
        //* Score Txt
        scoreTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? $"<sprite name=apple>: {mgm.Score}"
            : (mgm.Type == MGM.TYPE.MINIGAME2)? $"<sprite name=banana>: {mgm.Score}"
            : $"<sprite name=blueberry>: {mgm.Score}";

        //* Btn onPressed
        isLeftArrowBtnPressed = leftArrowBtn.targetGraphic.canvasRenderer.GetColor() == new Color(0.7f, 0.7f, 0.7f);
        isRightArrowBtnPressed = rightArrowBtn.targetGraphic.canvasRenderer.GetColor() == new Color(0.7f, 0.7f, 0.7f);
        
        //* Player Moving Control
        if(isLeftArrowBtnPressed)    movePlayer(isLeft: true);
        else if(isRightArrowBtnPressed)    movePlayer(isLeft: false);
        else { //* Moving Stop
            switch(MGM._.Type) {
                case MGM.TYPE.MINIGAME1:
                case MGM.TYPE.MINIGAME2:
                    MGM._.Pl.Anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
                    break;
                case MGM.TYPE.MINIGAME3:
                    //* 徐々に角度０に戻す
                    float newRotationZ = Mathf.MoveTowardsAngle(MGM._.Pl.transform.localRotation.eulerAngles.z, 0f, rotationStep * Time.deltaTime);
                    MGM._.Pl.transform.localRotation = Quaternion.Euler(0f, 0f, newRotationZ);
                    break;
            }
        }
        #endregion
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickStartBtn() {
        Debug.Log("onClickStartBtn():: Status: READY ➝ PLAY");
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        startScrBtn.gameObject.SetActive(false);
        leftArrowBtn.gameObject.SetActive(true);
        rightArrowBtn.gameObject.SetActive(true);
        StartCoroutine(coReadyStartCount());
    }
    public void onClickExitIconBtn() {
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        Time.timeScale = 0;
        giveUpPopUp.SetActive(true);
    }
    public void onClickGiveUpPopUpYesBtn() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
        Time.timeScale = 1;
        StartCoroutine(MGM._.mgrm.coGoHome());
    }
    public void onClickGiveUpPopUpNoBtn() {
        SM._.sfxPlay(SM.SFX.BtnClick.ToString());
        Time.timeScale = 1;
        giveUpPopUp.SetActive(false);
    }
#endregion
///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    private void movePlayer(bool isLeft) {
        const float minX = -2, maxX = 2;
        var pl = MGM._.Pl;
        var plPos = pl.transform.localPosition;
        float spd = MGM._.PlMoveSpd * Time.deltaTime;

        
        pl.Sr.flipX = isLeft;
        float x = plPos.x + (isLeft? -spd : spd);

        switch(MGM._.Type) {
            case MGM.TYPE.MINIGAME1:
                pl.walk();
                pl.transform.localPosition = new Vector2(Mathf.Clamp(x, minX, maxX) , plPos.y);
                break;
            case MGM.TYPE.MINIGAME2:
                pl.walk();
                pl.transform.localPosition = new Vector2(x, plPos.y);
                break;
            case MGM.TYPE.MINIGAME3:
                setPlayerMovingRot(isLeft, pl.gameObject); //* 動きによって、Playerの角度も少し回転
                pl.transform.localPosition = new Vector2(Mathf.Clamp(x, minX, maxX) , plPos.y);
                break;
        }
    }

    private void setPlayerMovingRot(bool isLeft, GameObject pl) {
        if (isLeftArrowBtnPressed || isRightArrowBtnPressed) {
            const int leftAngleMax = 10, rightAngleMax = 350;
            float rotationDelta = isLeft? rotationStep * Time.deltaTime : -rotationStep * Time.deltaTime;
            float zAngle = pl.transform.localRotation.eulerAngles.z + rotationDelta;
            if(isLeft) zAngle = Mathf.Clamp(zAngle, 0, leftAngleMax);
            else zAngle = Mathf.Clamp(zAngle, rightAngleMax, 360);
            pl.transform.localRotation = Quaternion.Euler(0, 0, zAngle);
        }
    }
    
    private IEnumerator coReadyStartCount() {
        SM._.sfxPlay(SM.SFX.Ready.ToString());
        titleTxt.text = LM._.localize("Ready");
        contentTxt.text = "";

        yield return Util.time1;
        SM._.sfxPlay(SM.SFX.Start.ToString());
        MGM._.Status = MGM.STATUS.PLAY;
        titleTxt.text = $"{LM._.localize("Start")}!";

        //* ミニーゲーム 最初のアクション
        if(MGM._.Type == MGM.TYPE.MINIGAME2) {
            MGM._.Pl.jump();
            MGM._.FloorColliderObj.SetActive(false);
        }
        else if(MGM._.Type == MGM.TYPE.MINIGAME3) {
            MGM._.Pl.Anim.SetTrigger(Enum.ANIM.DoSuccess.ToString());
            MGM._.PlSnowParticleEF.SetActive(true);
        }

        yield return Util.time1;
        titleTxt.gameObject.SetActive(false);
        contentTxt.gameObject.SetActive(false);


    }
#endregion
}
