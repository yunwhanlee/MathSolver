using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button screenPanelBtn;
    public Animator canvasAnim;
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