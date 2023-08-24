using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button screenPanelBtn;
    public Animator canvasAnim;
    public AudioSource titleBGM;

    public void onClickScreenPanelBtn() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public void onClickTitleAnimSkipBtn() {
        titleBGM.time = 21.0f;
        canvasAnim.SetTrigger(Enum.ANIM.DoSkipTitleAnim.ToString());
    }
}