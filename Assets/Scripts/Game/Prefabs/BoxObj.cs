using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxObj : MonoBehaviour {
    [SerializeField] Animator anim;  public Animator Anim {get => anim;}

    [SerializeField] int val;     public int Val {get => val; set => val = value;}
    [SerializeField] bool isBlockMerge;  public bool IsBlockMerge {get => isBlockMerge; set => isBlockMerge = value;}
    [SerializeField] TextMeshProUGUI valueTxt;  public TextMeshProUGUI ValueTxt {get => valueTxt; set => valueTxt = value;}
    [SerializeField] Image objImg;  public Image ObjImg {get => objImg; set => objImg = value;}
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Image iconCardImg;
    [SerializeField] Image txtCardImg;
    [SerializeField] Sprite[] boxSprs;   public Sprite[] BoxSprs {get => boxSprs; set => boxSprs = value;}
    [SerializeField] Sprite[] boxNameCardSprs;

    void Start() {
        anim = GetComponent<Animator>();
        setBoxSpr(DB._.SelectMapIdx);
    }

    void Update() {
        if(valueTxt.text == "?") return;
        if(valueTxt.text.Contains("+")) return;
        if(valueTxt.text.Contains("minus") || valueTxt.text.Contains("-")) return;

        valueTxt.text = val.ToString();
    }
//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    void setBoxSpr(int selectMapIdx) {
        Debug.Log($"setBoxSpr({selectMapIdx})");
        sr.sprite = boxSprs[selectMapIdx];
        iconCardImg.sprite = boxNameCardSprs[selectMapIdx];
        txtCardImg.sprite = boxNameCardSprs[selectMapIdx];
    }
#endregion
//-------------------------------------------------------------------------------------------------------------
#region COLLIDER
//-------------------------------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col) {
        if(!isBlockMerge && col.gameObject.CompareTag(Enum.TAG.Obj.ToString())) {
            SM._.sfxPlay(SM.SFX.BubblePop.ToString());
            val++;
            Destroy(col.gameObject);
        }
    }
#endregion
}
