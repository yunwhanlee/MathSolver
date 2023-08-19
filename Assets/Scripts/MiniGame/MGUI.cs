using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class MGUI : MonoBehaviour {

    [SerializeField] GameObject resultPanel;    public GameObject ResultPanel {get => resultPanel; set => resultPanel = value;}
    [SerializeField] GameObject giveUpPopUp;    public GameObject GiveUpPopUp {get => giveUpPopUp; set => giveUpPopUp = value;}
    [SerializeField] Animator switchScreenAnim; public Animator SwitchScreenAnim {get => switchScreenAnim;}

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

        if(DB._ == null) playerLvTxt.text = "99"; //! TEST
        else playerLvTxt.text = DB.Dt.Lv.ToString();

        if(DB._ == null) modeTxt.text = MGM.MODE.NORMAL.ToString(); //! TEST
        else modeTxt.text = (DB._.MinigameLv == 0)? MGM.MODE.EASY.ToString()
            : (DB._.MinigameLv == 1)? MGM.MODE.NORMAL.ToString()
            : MGM.MODE.HARD.ToString();

        scoreTxt.text = "";
        leftArrowBtn.gameObject.SetActive(false);
        rightArrowBtn.gameObject.SetActive(false);

        //* タイトル
        titleTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? "Catch falling apples!"
            : (mgm.Type == MGM.TYPE.MINIGAME2)? "Jump to the sky!"
            : "Snow sledding!";

        //* コンテンツ
        contentTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? "Collect as many apples as you can!"
            : (mgm.Type == MGM.TYPE.MINIGAME2)? "Collect bananas without falling off!"
            : "Collect blueberries avoiding obstacles.";

        //* Score Txt
        scoreTxt.text = (mgm.Type == MGM.TYPE.MINIGAME1)? $"<sprite name=apple>: {mgm.Score}"
            : (mgm.Type == MGM.TYPE.MINIGAME2)? $"<sprite name=banana>: {mgm.Score}"
            : $"<sprite name=blueberry>: {mgm.Score}";
    }

    void Update() {
        #region MINIGAME1 & MINIGAME 2
        if(MGM._.Status != MGM.STATUS.PLAY) return;
        if(MGM._.IsStun) return;

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
        startScrBtn.gameObject.SetActive(false);
        leftArrowBtn.gameObject.SetActive(true);
        rightArrowBtn.gameObject.SetActive(true);
        StartCoroutine(coReadyStartCount());
    }
    public void onClickExitIconBtn() {
        Time.timeScale = 0;
        giveUpPopUp.SetActive(true);
    }
    public void onClickGiveUpPopUpYesBtn() {
        Time.timeScale = 1;
        StartCoroutine(MGM._.mgrm.coGoHome());
    }
    public void onClickGiveUpPopUpNoBtn() {
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
        titleTxt.text = "Ready";
        contentTxt.text = "";

        yield return Util.time1;
        MGM._.Status = MGM.STATUS.PLAY;
        titleTxt.text = "Start!";

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
