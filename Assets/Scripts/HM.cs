using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HM : MonoBehaviour {
    public static HM _;
    public enum STATE {NORMAL, DECORATION_MODE};
    public STATE state;
    public Player pl;
    public UIManager ui;
    public TouchControl touchCtr;

    public GameObject pet;
    public GameObject funitureModeShadowFrameObj;
    public GameObject funitureModeItem;

    void Awake() => _ = this;    

    public void GoToLoadingScene() => SceneManager.LoadScene(Enum.SCENE.Loading.ToString());
}
