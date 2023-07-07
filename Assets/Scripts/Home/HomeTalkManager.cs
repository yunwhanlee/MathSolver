using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeTalkManager : MonoBehaviour {
    enum ID_IDX {FRONTOUTH_BOY, TUTORIAL_FIRST};

    [Header("BUTTON TYPE")]
    // [SerializeField] Button tutorialBeginPanelBtn;

    [Header("OBJECT TYPE")]

    //* Value
    Dictionary<int, string[]> talkDt;
    [SerializeField] bool isAction; public bool IsAction {get => isAction;}
    [SerializeField] int curId;
    [SerializeField] int talkIdx;
    [SerializeField] GameObject talkDialog;
    [SerializeField] TextMeshProUGUI talkTxt;

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
            "어서오시게! 자네가 이번에 새로온 \n수학조수이구만!"
            , "나는 찬란한 동물마을의 수학해결사인 울프강 강늑대라네. 반가워!"
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
    }
    public void playAction() {
        talk(curId);
        talkDialog.SetActive(isAction);

        if(curId == (int)ID_IDX.FRONTOUTH_BOY && talkIdx == 1) 
            HM._.ui.test_GetCoinFromFrontouthBoy();
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
