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
    public HEM em;

    [Header("GROUP")]
    [SerializeField] Transform effectGroup;   public Transform EffectGroup {get => effectGroup; set => effectGroup = value;}

    [Header("MATERIAL")]
    public Material sprUnlitMt;
    public Material outlineAnimMt;

    [Header("GAME OBJECT")]
    public GameObject funitureModeShadowFrameObj;
    public GameObject funitureModeItem;
    public GameObject roomObjectGroup;
    public SpriteRenderer wallSr;
    public SpriteRenderer floorSr;

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
            //* 名(Clone) 削除
            ins.name = ins.name.Split('(')[0]; 
            //* 位置
            ins.transform.position = item.Pos;
            //* 反転
            Vector2 sc = ins.transform.localScale;
            ins.transform.localScale = new Vector2((item.IsFlat? -sc.x : sc.x), sc.y);
            //* レイヤー
            ins.GetComponent<RoomObject>().setSortingOrderByPosY();
        });
    }
#endregion
}
