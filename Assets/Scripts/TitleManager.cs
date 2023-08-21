using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button screenPanelBtn;
    public Animator canvasAnim;

    public void onClickScreenPanelBtn() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
    public void onClickTitleAnimSkipBtn() {
        canvasAnim.SetTrigger(Enum.ANIM.DoSkipTitleAnim.ToString());
    }
}