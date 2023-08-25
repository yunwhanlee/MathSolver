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
    public enum BGM {
        Title, Home, Forest
    }

    public enum SFX {
        //* UI
        BtnClick, Error, Success ,
        TinyBubblePop, BubblePop,
        LevelUp, Yooo, 
        Talk, Talk2, Talk3, Talk4, Talk5,
        Fanfare, Tada, FeatherPop, Grinding,
        GainItem, PetClick, GetReward, Transition,
        //* IN GAME
        StartDrum, WolfRoar, ChildYeah, CorrectAnswer, WrongAnswer,
        GetCoin, GetExp, Result,
        Ready, Start, Explosion, Jump,
        SceneSpawn, Stun, PaperScroll, Fall,
    }
    [Header("BGM")][Header("__________________________")]
    [SerializeField] AudioSource bgmAudio;  public AudioSource BgmAudio {get => bgmAudio;}
    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip homeBGM;
    [SerializeField] AudioClip forestBGM;

    [Header("UI")][Header("__________________________")]
    [SerializeField] GameObject soundGroup; public GameObject SoundGroup {get => soundGroup;}
    [SerializeField] AudioSource btnClickSFX;
    [SerializeField] AudioSource errorSFX;
    [SerializeField] AudioSource successSFX;
    [SerializeField] AudioSource tinyBubblePopSFX;
    [SerializeField] AudioSource bubblePopSFX;
    [SerializeField] AudioSource levelUpSFX;
    [SerializeField] AudioSource yoooSFX;
    [SerializeField] AudioSource talkSFX;
    [SerializeField] AudioSource talk2SFX;
    [SerializeField] AudioSource talk3SFX;
    [SerializeField] AudioSource talk4SFX;
    [SerializeField] AudioSource talk5SFX;
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
    [SerializeField] AudioSource resultSFX;
    [SerializeField] AudioSource readySFX;
    [SerializeField] AudioSource startSFX;
    [SerializeField] AudioSource explosionSFX;
    [SerializeField] AudioSource jumpSFX;
    [SerializeField] AudioSource sceneSpawnSFX;
    [SerializeField] AudioSource stunSFX;
    [SerializeField] AudioSource paperScrollSFX;
    [SerializeField] AudioSource fallSFX;

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void bgmPlay(string name) {
        bgmAudio.clip = (name == BGM.Title.ToString())? titleBGM
            : (name == BGM.Home.ToString())? homeBGM
            : (name == BGM.Forest.ToString())? forestBGM : null;
        bgmAudio.volume = (name == BGM.Title.ToString())? 0.5f
            : (name == BGM.Home.ToString())? 0.2f
            : (name == BGM.Forest.ToString())? 0.4f : 0;
        bgmAudio.time = 0;
        bgmAudio.Play();
    }
    public void setBgmTime(float _time) => bgmAudio.time = _time;

    public void sfxPlay(string name, float delay = 0) {
        //* UI
        if(name == SFX.BtnClick.ToString()) btnClickSFX.Play();
        if(name == SFX.Error.ToString()) errorSFX.Play();
        if(name == SFX.Success.ToString()) successSFX.Play();
        if(name == SFX.TinyBubblePop.ToString()) tinyBubblePopSFX.Play();
        if(name == SFX.BubblePop.ToString()) bubblePopSFX.Play();
        if(name == SFX.LevelUp.ToString()) levelUpSFX.Play();
        if(name == SFX.Yooo.ToString()) yoooSFX.Play();
        if(name == SFX.Talk.ToString()) {talkSFX.Play();} 
        if(name == SFX.Talk2.ToString()) {talk2SFX.Play();} 
        if(name == SFX.Talk3.ToString()) {talk3SFX.Play();} 
        if(name == SFX.Talk4.ToString()) {talk4SFX.Play();} 
        if(name == SFX.Talk5.ToString()) {talk5SFX.Play();} 

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
        if(name == SFX.Result.ToString()) resultSFX.Play();
        if(name == SFX.Ready.ToString()) readySFX.Play();
        if(name == SFX.Start.ToString()) startSFX.Play();
        if(name == SFX.Explosion.ToString()) explosionSFX.Play();
        if(name == SFX.Jump.ToString()) jumpSFX.Play();
        if(name == SFX.SceneSpawn.ToString()) sceneSpawnSFX.Play();
        if(name == SFX.Stun.ToString()) stunSFX.Play();
        if(name == SFX.PaperScroll.ToString()) paperScrollSFX.Play();
        if(name == SFX.Fall.ToString()) fallSFX.Play();
    }
    public void enabledTalk(string name) {
        talkSFX.enabled = (name == SM.SFX.Talk.ToString())? true : false;
        talk2SFX.enabled = (name == SM.SFX.Talk2.ToString())? true : false;
        talk3SFX.enabled = (name == SM.SFX.Talk3.ToString())? true : false;
        talk4SFX.enabled = (name == SM.SFX.Talk4.ToString())? true : false;
        talk5SFX.enabled = (name == SM.SFX.Talk5.ToString())? true : false;
    }
    public void disableTalk() {
        talkSFX.Stop();  talkSFX.enabled = false;
        talk2SFX.Stop(); talk2SFX.enabled = false;
        talk3SFX.Stop(); talk3SFX.enabled = false;
        talk4SFX.Stop(); talk4SFX.enabled = false;
        talk5SFX.Stop(); talk5SFX.enabled = false;
    }

    // public void sfxStop(string name){
    //     Debug.Log($"sfxStop:: name= {name}");
    //     //* UI
    //     if(name == SFX.BtnClick.ToString()) btnClickSFX.Stop();
    //     if(name == SFX.Error.ToString()) errorSFX.Stop();
    //     if(name == SFX.Success.ToString()) successSFX.Stop();
    //     if(name == SFX.TinyBubblePop.ToString()) tinyBubblePopSFX.Stop();
    //     if(name == SFX.BubblePop.ToString()) bubblePopSFX.Stop();
    //     if(name == SFX.LevelUp.ToString()) levelUpSFX.Stop();
    //     if(name == SFX.Yooo.ToString()) yoooSFX.Stop();
    //     if(name == SFX.Talk.ToString()) talkSFX.Stop();
    //     if(name == SFX.Talk2.ToString()) talk2SFX.Stop();
    //     if(name == SFX.Talk3.ToString()) talk3SFX.Stop();
    //     if(name == SFX.Talk4.ToString()) talk4SFX.Stop();
    //     if(name == SFX.Talk5.ToString()) talk5SFX.Stop();

    //     if(name == SFX.Fanfare.ToString()) fanfareSFX.Stop();
    //     if(name == SFX.Tada.ToString()) tadaSFX.Stop();
    //     if(name == SFX.FeatherPop.ToString()) featherPopSFX.Stop();
    //     if(name == SFX.Grinding.ToString()) grindingSFX.Stop();
    //     if(name == SFX.GainItem.ToString()) gainItemSFX.Stop();
    //     if(name == SFX.PetClick.ToString()) petClickSFX.Stop();
    //     if(name == SFX.GetReward.ToString()) getRewardSFX.Stop();
    //     if(name == SFX.Transition.ToString()) transitionSFX.Stop();
    //     //* IN GAME
    //     if(name == SFX.StartDrum.ToString()) startDrumSFX.Stop();
    //     if(name == SFX.WolfRoar.ToString()) wolfRoarSFX.Stop();
    //     if(name == SFX.ChildYeah.ToString()) childYeahSFX.Stop();
    //     if(name == SFX.CorrectAnswer.ToString()) correctAnswerSFX.Stop();
    //     if(name == SFX.WrongAnswer.ToString()) wrongAnswerSFX.Stop();
    //     if(name == SFX.GetCoin.ToString()) getCoinSFX.Stop();
    //     if(name == SFX.GetExp.ToString()) getExpSFX.Stop();
    //     if(name == SFX.Result.ToString()) resultSFX.Stop();
    //     if(name == SFX.Ready.ToString()) readySFX.Stop();
    //     if(name == SFX.Start.ToString()) startSFX.Stop();
    //     if(name == SFX.Explosion.ToString()) explosionSFX.Stop();
    //     if(name == SFX.Jump.ToString()) jumpSFX.Stop();
    //     if(name == SFX.SceneSpawn.ToString()) sceneSpawnSFX.Stop();
    //     if(name == SFX.Stun.ToString()) stunSFX.Stop();
    //     if(name == SFX.PaperScroll.ToString()) paperScrollSFX.Stop();
    //     if(name == SFX.Fall.ToString()) fallSFX.Stop();
    // }
#endregion
}
