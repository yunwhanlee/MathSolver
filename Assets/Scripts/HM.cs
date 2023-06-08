using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HM : MonoBehaviour {
    public static HM _;
    public enum STATE {NORMAL, DECORATION_MODE};
    public STATE state;

    [Header("OUTSIDE")]
    public Player pl;
    public UIManager ui;
    public TouchControl touchCtr;
    public FunitureUIManager fUI;

    [Header("MATERIAL")]
    public Material sprUnlitMt;
    public Material outlineAnimMt;

    [Header("GAME OBJECT")]
    public GameObject pet;
    public GameObject funitureModeShadowFrameObj;
    public GameObject funitureModeItem;
    public GameObject roomObjectGroup;

    void Awake() => _ = this;

    public void GoToLoadingScene() => SceneManager.LoadScene(Enum.SCENE.Loading.ToString());
}
