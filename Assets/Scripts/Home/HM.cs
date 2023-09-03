using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class HM : MonoBehaviour {
    public static HM _;
    public enum STATE {NORMAL, DECORATION_MODE, POPUP};
    public STATE state;

    [Header("OUTSIDE")]
    public Player pl;
    public Pet pet;
    public HUI ui;
    public TouchControl touchCtr;
    public FunitureUIManager fUI;
    public ClothShopUIManager cUI;
    public InventoryUIManager iUI;
    public HEM em;
    public HomeTalkManager htm;
    public AchieveManager am;
    public QuestManager qm;
    public RankManager rm;
    public WorldMapManager wmm;
    public HomeMinigameManager hmgm;
    public AccountManager actm;


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
        SM._.bgmPlay(SM.BGM.Home.ToString());
        Debug.Log("ロードデータ:: 配置した家具を生成");
        createFunitureItemsBySaveData(DB.Dt.Funitures);
        createFunitureItemsBySaveData(DB.Dt.Decorations);
        createFunitureItemsBySaveData(DB.Dt.Bgs);
        createFunitureItemsBySaveData(DB.Dt.Mats);
        setCharaSpriteLibraryBySaveData(DB.Dt.PlSkins);
        setCharaSpriteLibraryBySaveData(DB.Dt.PtSkins);

        actm.reqAutoLogin();

        //* LEGACY BONUS VAL
        int legacyCnt = 0;
        Array.ForEach(DB.Dt.Decorations, deco => {
            if(deco.IsLock == false && deco.Grade == Item.GRADE.Special)
                legacyCnt++;
        });
        Array.ForEach(DB.Dt.PtSkins, pet => {
            if(pet.IsLock == false && pet.Grade == Item.GRADE.Special)
                legacyCnt++;
        });
        //* Apply
        DB._.LegacyCnt = legacyCnt;
        
    }
/// -----------------------------------------------------------------------------------------------------------------
#region FUNC
/// -----------------------------------------------------------------------------------------------------------------
    public IEnumerator GoToLoadingScene(string name) {
        Debug.Log("GoToLoadingScene()::");

        //* 初期化 (MiniGame移動の場合は、PlayerとPetデータを移すものではなく、PlayerのSpriteLibraryAssetのみ使うので)
        for(int i = 0; i < DB._.transform.childCount; i++)
            Destroy(DB._.transform.GetChild(i).gameObject);

        //* PlayerとPetデータDBへ移動。
        pl.transform.SetParent(DB._.transform);
        pet.transform.SetParent(DB._.transform);
        pl.transform.gameObject.SetActive(false);
        pet.transform.gameObject.SetActive(false);

        SM._.sfxPlay(SM.SFX.Transition.ToString());
        HM._.ui.SwitchScreenAnim.gameObject.SetActive(true);
        HM._.ui.SwitchScreenAnim.SetTrigger(Enum.ANIM.BlackIn.ToString());
        yield return Util.time0_5;
        SceneManager.LoadScene(name);
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
                pl.IdleSpr = plSk.Spr;
                pl.SprLib.spriteLibraryAsset = plSk.SprLibraryAsset;
                em.createPlayerSkinAuraEF();
                break;
            case PetSkin ptSk:
                pet.SprLib.spriteLibraryAsset = ptSk.SprLibraryAsset;
                //* Sprite Libraryがなかったら、SpriteRenderer 非表示
                if(!ptSk.SprLibraryAsset)
                    pet.Sr.sprite = null;
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
