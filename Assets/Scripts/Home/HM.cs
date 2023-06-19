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
    public InventoryUIManager iUI;
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
        setCharaSpriteLibraryBySaveData(DB.Dt.PlSkins);
        setCharaSpriteLibraryBySaveData(DB.Dt.PtSkins);
    }
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public void GoToLoadingScene() => SceneManager.LoadScene(Enum.SCENE.Loading.ToString());

    private void createFunitureItemsBySaveData(Item[] itemDts) {
        Item[] arrangedItems = Array.FindAll(itemDts, item => item.IsArranged);

        //* 系変換
        Array.ForEach(arrangedItems, item => {
            if(item is Funiture ft) {
                Debug.Log($"createFunitureItemsBySaveData():: funitures.len= {arrangedItems.Length}, item= {item.Name}");
                GameObject ins = Instantiate(ft.Prefab, HM._.ui.RoomObjectGroupTf);
                //* 名(Clone) 削除
                ins.name = ins.name.Split('(')[0];
                //* 位置
                ins.transform.position = ft.Pos;
                //* 反転
                Vector2 sc = ins.transform.localScale;
                ins.transform.localScale = new Vector2((ft.IsFlat ? -sc.x : sc.x), sc.y);
                //* レイヤー
                ins.GetComponent<RoomObject>().setSortingOrderByPosY();
            }
            else if (item is BgFuniture bg) {
                if (bg.Type == BgFuniture.TYPE.Wall)
                    wallSr.sprite = bg.Spr;
                else if (bg.Type == BgFuniture.TYPE.Floor)
                    floorSr.sprite = bg.Spr;
            }
        });
    }
    private void setCharaSpriteLibraryBySaveData(Item[] itemDts) {
        Item curSkin = Array.Find(itemDts, item => item.IsArranged);
        //* Pattern Matching
        switch(curSkin) {
            case PlayerSkin plSk:
                HM._.pl.SprLib.spriteLibraryAsset = plSk.SprLibraryAsset;
                break;
            case PetSkin ptSk:
                HM._.pet.SprLib.spriteLibraryAsset = ptSk.SprLibraryAsset;
                //* Sprite Libraryがなかったら、SpriteRenderer 非表示
                if(!ptSk.SprLibraryAsset)
                    HM._.pet.Sr.sprite = null;
                break;
        }
    }
#endregion
}
