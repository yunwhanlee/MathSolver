using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Button screenPanelBtn;

    public void onClickScreenPanelBtn() {
        SceneManager.LoadScene(Enum.SCENE.Home.ToString());
    }
}