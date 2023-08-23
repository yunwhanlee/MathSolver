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
        LevelUp, Yooo, Talk, Fanfare,
        Tada, FeatherPop, Grinding,
        GainItem,
        //* PLAY
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
    [SerializeField] AudioSource fanfareSFX;
    [SerializeField] AudioSource tadaSFX;
    [SerializeField] AudioSource featherPopSFX;
    [SerializeField] AudioSource grindingSFX;
    [SerializeField] AudioSource gainItemSFX;
    // [Header("PLAY")][Header("__________________________")]

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    public void sfxPlay(string name, float delay = 0) {
        if(name == SFX.BtnClick.ToString()) btnClickSFX.Play();
        if(name == SFX.Error.ToString()) errorSFX.Play();
        if(name == SFX.Success.ToString()) successSFX.Play();
        if(name == SFX.TinyBubblePop.ToString()) tinyBubblePopSFX.Play();
        if(name == SFX.BubblePop.ToString()) bubblePopSFX.Play();
        if(name == SFX.LevelUp.ToString()) levelUpSFX.Play();
        if(name == SFX.Yooo.ToString()) yoooSFX.Play();
        if(name == SFX.Talk.ToString()) talkSFX.Play();
        if(name == SFX.Fanfare.ToString()) fanfareSFX.Play();
        if(name == SFX.Tada.ToString()) tadaSFX.PlayDelayed(delay);
        if(name == SFX.FeatherPop.ToString()) featherPopSFX.Play();
        if(name == SFX.Grinding.ToString()) grindingSFX.PlayDelayed(delay);
        if(name == SFX.GainItem.ToString()) gainItemSFX.Play();
    }
#endregion
}
