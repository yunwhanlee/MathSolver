using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssetKits.ParticleImage;

public class TitleManager : MonoBehaviour
{
    public static TitleManager _;
    public Image helpLogoImg;
    public Image logoWgiteImg;
    public Image titleLogoImg;
    public ParticleImage helpLogoTxtPtcImg;
    public Button screenPanelBtn;
    public Animator canvasAnim;

    void Awake() {
        _ = this;
    }

    void Start() {
        SM._.bgmPlay(SM.BGM.Title.ToString());
    }

    public void onClickScreenPanelBtn() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public void onClickTitleAnimSkipBtn() {
        SM._.setBgmTime(21.0f);
        canvasAnim.SetTrigger(Enum.ANIM.DoSkipTitleAnim.ToString());
    }
}