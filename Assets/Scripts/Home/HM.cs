using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class HM : MonoBehaviour {
    public static HM _;
    public enum STATE {NORMAL, DECORATION_MODE, SETTING};
    public STATE state;

    [Header("OUTSIDE")]
    public Player pl;
    public Pet pet;
    public HUI ui;
    public TouchControl touchCtr;
    public FunitureUIManager fUI;
    public InventoryUIManager iUI;
    public HEM em;
    public HomeTalkManager htm;
    public WorldMapManager wmm;

    [Header("MATERIAL")]
    public Material sprUnlitMt;
    public Material outlineAnimMt;
    public Material outlineMt;

    [Header("SPRITE")]
    public Sprite[] conturiesIcons; //* EN=0, KR=1, JP=2

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
    public IEnumerator GoToLoadingScene() {
        Debug.Log("GoToLoadingScene()::");
        pl.transform.SetParent(DB._.transform);
        pet.transform.SetParent(DB._.transform);
        pl.transform.gameObject.SetActive(false);
        pet.transform.gameObject.SetActive(false);

        HM._.ui.SwitchScreenAnim.gameObject.SetActive(true);
        HM._.ui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackIn.ToString());
        yield return Util.time0_5;
        SceneManager.LoadScene(Enum.SCENE.Loading.ToString());

    }

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
                HM._.pl.IdleSpr = plSk.Spr;
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
    public bool isChair(GameObject obj) {
        bool isFuniture = obj.CompareTag(Enum.TAG.Funiture.ToString());
        bool isChair = (obj.layer == LayerMask.NameToLayer(Enum.LAYER.Chair.ToString()));
        return (isFuniture && isChair);
    }
    public void clearAllChairOutline() {
        for(int i = 0; i < roomObjectGroup.transform.childCount; i++) {
            GameObject rObj = roomObjectGroup.transform.GetChild(i).gameObject;
            if(isChair(rObj))
                rObj.GetComponent<SpriteRenderer>().material = sprUnlitMt;
        }
    }
#endregion
}
