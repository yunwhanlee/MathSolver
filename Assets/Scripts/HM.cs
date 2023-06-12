using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class HM : MonoBehaviour {
    public static HM _;
    public enum STATE {NORMAL, DECORATION_MODE};
    public STATE state;

    [Header("OUTSIDE")]
    public Player pl;
    public Pet pet;
    public UIManager ui;
    public TouchControl touchCtr;
    public FunitureUIManager fUI;

    [Header("MATERIAL")]
    public Material sprUnlitMt;
    public Material outlineAnimMt;

    [Header("GAME OBJECT")]
    public GameObject funitureModeShadowFrameObj;
    public GameObject funitureModeItem;
    public GameObject roomObjectGroup;

    void Awake() => _ = this;

    void Start() {
        Debug.Log("ロードデータ:: 配置した家具を生成");
        createFunitureItemsBySaveData(DB.Dt.Funitures);
        createFunitureItemsBySaveData(DB.Dt.Decorations);
        createFunitureItemsBySaveData(DB.Dt.Bgs);
        createFunitureItemsBySaveData(DB.Dt.Mats);
    }
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void GoToLoadingScene() => SceneManager.LoadScene(Enum.SCENE.Loading.ToString());
    private void createFunitureItemsBySaveData(Funiture[] itemDts) {
        Funiture[] arrangedItems = Array.FindAll(itemDts, item => item.IsArranged);
        Array.ForEach(arrangedItems, item => {
            GameObject ins = Instantiate(item.Prefab, HM._.ui.RoomObjectGroupTf);
            ins.name = ins.name.Split('(')[0]; //* 名(Clone) 削除
            ins.transform.position = item.Pos;
            ins.GetComponent<RoomObject>().setSortingOrderByPosY();
        });
    }
#endregion
}
