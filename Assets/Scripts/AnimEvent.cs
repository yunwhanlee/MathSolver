using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimEvent : MonoBehaviour {   
    //? PARAM 可能：int, float, string
    const int NORMAL = 0, GOLD = 1;

    //* GCDFrame
    [SerializeField] TextMeshProUGUI GCD_ContentTxt;
    [SerializeField] TextMeshProUGUI GCD_Val1CommonDivisorTxt;
    [SerializeField] TextMeshProUGUI GCD_Val2CommonDivisorTxt;
    [SerializeField] Button btn;

    [Header("TITLE")]
    [SerializeField] Sprite engHelpLogoSpr;
    [SerializeField] Sprite engLogoWgiteSpr;
    [SerializeField] Sprite engTitleLogoSpr;

    //* Title
    //* Intro Anim
    public void playOpenBoxSFX() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
    }
    public void playShinyGlassesSFX() {
        SM._.sfxPlay(SM.SFX.GetReward.ToString());
    }
    public void playOpenDoorSFX() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
    }
    public void playHitLegSFX() {
        SM._.sfxPlay(SM.SFX.Explosion.ToString());
    }
    public void playFallSFX() {
        SM._.sfxPlay(SM.SFX.Fall.ToString());
        SM._.sfxPlay(SM.SFX.WolfRoar.ToString());
    }
    public void playHitCameraSFX() {
        SM._.sfxPlay(SM.SFX.FeatherPop.ToString());
    }
    //* Title Anim
    public void setTitleEnglishSprs() {
        Debug.Log($"playHelpLogoSpawnSFX():: curLangIndex= {LM._.curLangIndex}");
        if(LM._.curLangIndex == (int)LM.LANG_IDX.EN) {
            TitleManager._.helpLogoImg.sprite = engHelpLogoSpr;
            TitleManager._.logoWgiteImg.sprite = engLogoWgiteSpr;
            TitleManager._.titleLogoImg.sprite = engTitleLogoSpr;
            TitleManager._.helpLogoTxtPtcImg.texture = engHelpLogoSpr.texture;
        }
    }

    public void playHelpLogoSpawnSFX() {
        
        SM._.sfxPlay(SM.SFX.Jump.ToString());
    }
    public void playTitleLogoSpawnSFX() {
        SM._.sfxPlay(SM.SFX.StartDrum.ToString());
    }
    public void playFrogComeToCameraSFX() {
        SM._.sfxPlay(SM.SFX.CorrectAnswer.ToString());
    }

    //* GAME
    public void setGCD_ContentTxt(string str) {
        GCD_ContentTxt.text = LM._.localize(str);
    }
    public void setGCD_Val1CommonDivisorTxt(string str) {
        GCD_Val1CommonDivisorTxt.text = str;
    }
    public void setGCD_Val2CommonDivisorTxt(string str) {
        GCD_Val2CommonDivisorTxt.text = str;
    }
    public void playJumpSFX() {
        SM._.sfxPlay(SM.SFX.Jump.ToString());
    }
    public void playBubblePopSFX() {
        SM._.sfxPlay(SM.SFX.BubblePop.ToString());
    }
    public void playStartDrumSFX() {
        SM._.sfxPlay(SM.SFX.Success.ToString());
    }

    //* HOME
    public void setBtnInteractable(int active) {
        btn.interactable = (active == 0)? false : true;
    }
    public void playUnlockMapBgSFX() {
        SM._.sfxPlay(SM.SFX.CorrectAnswer.ToString());
    }
}