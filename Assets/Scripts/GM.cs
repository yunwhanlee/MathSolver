using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public static GM _;
    public Player pl;
    public UIManager ui;
    public GameObject pet;
    public TouchControl touchCtr;

    void Awake() {
        _ = this;
    }

    public void GoToLoadingScene() => SceneManager.LoadScene(Enum.SCENE.Loading.ToString());
}
