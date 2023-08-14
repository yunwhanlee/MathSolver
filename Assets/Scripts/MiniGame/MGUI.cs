using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    //* MiniGame1 Forest
    [SerializeField] Button leftArrowBtn;   public Button LeftArrowBtn {get => leftArrowBtn; set => leftArrowBtn = value;}
    [SerializeField] Button rightArrowBtn;   public Button RightArrowBtn {get => rightArrowBtn; set => rightArrowBtn = value;}

    void Start() {
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
        titleTxt.text = (MGM._.Type == MGM.TYPE.MINIGAME1)? "Catch falling apples"
            : (MGM._.Type == MGM.TYPE.MINIGAME2)? "Jump to the sky"
            : "TODO MINIGAME3 TITLE";

        //* コンテンツ
        contentTxt.text = (MGM._.Type == MGM.TYPE.MINIGAME1)? "Collect as many apples as you can!"
            : (MGM._.Type == MGM.TYPE.MINIGAME2)? "collect bananas without falling off!"
            : "TODO MINIGAME3 CONTENT";
    }

    void Update() {
        #region MINIGAME1 & MINIGAME 2
        if(MGM._.Status != MGM.STATUS.PLAY) return;
        if(MGM._.IsStun) return;

        //* Btn onPressed
        bool isLeftArrowBtnPressed = leftArrowBtn.targetGraphic.canvasRenderer.GetColor() == new Color(0.7f, 0.7f, 0.7f);
        bool isRightArrowBtnPressed = rightArrowBtn.targetGraphic.canvasRenderer.GetColor() == new Color(0.7f, 0.7f, 0.7f);
        
        //* Player Moving Control
        if(isLeftArrowBtnPressed)
            movePlayer(isLeft: true);
        else if(isRightArrowBtnPressed) 
            movePlayer(isLeft: false);
        else
            MGM._.Pl.Anim.SetBool(Enum.ANIM.IsWalk.ToString(), false);
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

        pl.walk();
        pl.Sr.flipX = isLeft;
        float x = plPos.x + (isLeft? -spd : spd);
        if(MGM._.Type == MGM.TYPE.MINIGAME1)
            pl.transform.localPosition = new Vector2(Mathf.Clamp(x, minX, maxX) , plPos.y);
        else 
            pl.transform.localPosition = new Vector2(x, plPos.y);
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

        yield return Util.time1;
        titleTxt.gameObject.SetActive(false);
        contentTxt.gameObject.SetActive(false);


    }
#endregion
}
