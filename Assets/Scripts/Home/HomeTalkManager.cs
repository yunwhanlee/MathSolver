using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : MonoBehaviour {
    enum ID_IDX {FRONTOUTH_BOY, TUTORIAL_FIRST};

    [Header("BUTTON TYPE")]
    [SerializeField] GameObject TutorialBeginPanelBtn;
    // [SerializeField] Button tutorialBeginPanelBtn;

    [Header("OBJECT TYPE")]

    //* Value
    Dictionary<int, string[]> talkDt;
    [SerializeField] bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] int curId;
    [SerializeField] int talkIdx;
    [SerializeField] GameObject talkDialog;
    [SerializeField] TextMeshProUGUI talkTxt;
    [SerializeField] Image speakerImg;
    [SerializeField] Sprite frontoothBoySpr;

    // [SerializeField] Image portraitImg;

    void Awake() {
        talkDt = new Dictionary<int, string[]>();
        generateData();
    }

    void generateData() {
        //* TEST用
        talkDt.Add((int)ID_IDX.FRONTOUTH_BOY, new string[] {
            "아닛! 나를 찾아내다니.. \n옛다 10000코인!"
            , "더 필요하면 언제든 오시게나. \n담백하다!"
        });
        //* チュートリアル用
        talkDt.Add((int)ID_IDX.TUTORIAL_FIRST, new string[] {
            "반갑네! 자네가 이번에 새로 부임한 \n수학조수구만!"
            , "나는 동물마을의 수학해결사\n 늑선생이라네."
        });
    }

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region EVENT
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void onClickPlayActionBtn() => playAction();
    public void onClickRegistActionBtn(int id) => registAction(id);
#endregion

///---------------------------------------------------------------------------------------------------------------------------------------------------
#region FUNC
///---------------------------------------------------------------------------------------------------------------------------------------------------
    public void registAction(int id) {
        curId = id;
        playAction(); //* 最初スタート
        speakerImg.sprite = (id == (int)ID_IDX.FRONTOUTH_BOY)? frontoothBoySpr
            : (id == (int)ID_IDX.TUTORIAL_FIRST)? HM._.pl.IdleSpr
            : null;
    }
    public void playAction() {
        talk(curId);
        talkDialog.SetActive(isAction);

        if(curId == (int)ID_IDX.FRONTOUTH_BOY && talkIdx == 1) 
            HM._.ui.test_GetCoinFromFrontouthBoy();
        if(curId == (int)ID_IDX.TUTORIAL_FIRST && talkIdx == talkDt[(int)ID_IDX.TUTORIAL_FIRST].Length) {
            TutorialBeginPanelBtn.SetActive(false);
        }
    }
    private void talk(int id) {
        string msg = getTalk(id, talkIdx);

        if(msg == null) {
            isAction = false;
            talkIdx = 0;
            return;
        }

        else talkTxt.text = msg;
        
        isAction = true;
        talkIdx++;
    }
    private string getTalk(int id, int talkIdx) {
        if(talkIdx == talkDt[id].Length)
            return null;
        else 
            return talkDt[id][talkIdx];
    }

#endregion
}
