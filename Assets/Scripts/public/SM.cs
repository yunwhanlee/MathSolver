using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SM : MonoBehaviour {
    static public SM _;
    void Awake() => singleton();
    private void singleton(){
        if(_ == null) {
            _ = this;
            DontDestroyOnLoad(_);
        }
        else
            Destroy(this.gameObject);
    }
    public enum SFX {
        //* UI
        BtnClick, Error, Success ,
        TinyBubblePop, BubblePop,
        LevelUp, Yooo, 
        Talk, Talk2,
        Fanfare, Tada, FeatherPop, Grinding,
        GainItem, PetClick, GetReward, Transition,
        //* IN GAME
        StartDrum, WolfRoar, ChildYeah, CorrectAnswer, WrongAnswer,
        GetCoin, GetExp, 
        Ready, Start, Explosion,
    }
    [Header("UI")][Header("__________________________")]
    [SerializeField] AudioSource btnClickSFX;
    [SerializeField] AudioSource errorSFX;
    [SerializeField] AudioSource successSFX;
    [SerializeField] AudioSource tinyBubblePopSFX;
    [SerializeField] AudioSource bubblePopSFX;
    [SerializeField] AudioSource levelUpSFX;
    [SerializeField] AudioSource yoooSFX;
    [SerializeField] AudioSource talkSFX;
    [SerializeField] AudioSource talk2SFX;
    [SerializeField] AudioSource fanfareSFX;
    [SerializeField] AudioSource tadaSFX;
    [SerializeField] AudioSource featherPopSFX;
    [SerializeField] AudioSource grindingSFX;
    [SerializeField] AudioSource gainItemSFX;
    [SerializeField] AudioSource petClickSFX;
    [SerializeField] AudioSource getRewardSFX;
    [SerializeField] AudioSource transitionSFX;
    [Header("IN GAME")][Header("__________________________")]
    [SerializeField] AudioSource startDrumSFX;
    [SerializeField] AudioSource wolfRoarSFX;
    [SerializeField] AudioSource childYeahSFX;
    [SerializeField] AudioSource correctAnswerSFX;
    [SerializeField] AudioSource wrongAnswerSFX;
    [SerializeField] AudioSource getCoinSFX;
    [SerializeField] AudioSource getExpSFX;
    [SerializeField] AudioSource readySFX;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource explosionSFX;

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void sfxPlay(string name, float delay = 0) {
        //* UI
        if(name == SFX.BtnClick.ToString()) btnClickSFX.Play();
        if(name == SFX.Error.ToString()) errorSFX.Play();
        if(name == SFX.Success.ToString()) successSFX.Play();
        if(name == SFX.TinyBubblePop.ToString()) tinyBubblePopSFX.Play();
        if(name == SFX.BubblePop.ToString()) bubblePopSFX.Play();
        if(name == SFX.LevelUp.ToString()) levelUpSFX.Play();
        if(name == SFX.Yooo.ToString()) yoooSFX.Play();
        if(name == SFX.Talk.ToString()) talkSFX.Play();
        if(name == SFX.Talk2.ToString()) talk2SFX.Play();
        if(name == SFX.Fanfare.ToString()) fanfareSFX.Play();
        if(name == SFX.Tada.ToString()) tadaSFX.PlayDelayed(delay);
        if(name == SFX.FeatherPop.ToString()) featherPopSFX.Play();
        if(name == SFX.Grinding.ToString()) grindingSFX.PlayDelayed(delay);
        if(name == SFX.GainItem.ToString()) gainItemSFX.Play();
        if(name == SFX.PetClick.ToString()) petClickSFX.Play();
        if(name == SFX.GetReward.ToString()) getRewardSFX.Play();
        if(name == SFX.Transition.ToString()) transitionSFX.Play();
        //* IN GAME
        if(name == SFX.StartDrum.ToString()) startDrumSFX.Play();
        if(name == SFX.WolfRoar.ToString()) wolfRoarSFX.Play();
        if(name == SFX.ChildYeah.ToString()) childYeahSFX.Play();
        if(name == SFX.CorrectAnswer.ToString()) correctAnswerSFX.Play();
        if(name == SFX.WrongAnswer.ToString()) wrongAnswerSFX.Play();
        if(name == SFX.GetCoin.ToString()) getCoinSFX.Play();
        if(name == SFX.GetExp.ToString()) getExpSFX.Play();
        if(name == SFX.Ready.ToString()) readySFX.Play();
        if(name == SFX.Start.ToString()) startSFX.Play();
        if(name == SFX.Explosion.ToString()) explosionSFX.Play();

    }
#endregion
}
