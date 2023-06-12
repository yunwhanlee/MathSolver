using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum : MonoBehaviour
{
    public enum SCENE {Title, Home, Loading, Game};
    public enum HOME {Room, IkeaShop, ClothShop, Inventory};
    public enum ACHIVERANK {Achivement, Mission, Rank};
    public enum FUNITURE_CATE {Funiture, Decoration, Bg, Mat};
    public enum TAG {GoGame};
    public enum ANIM {DoIdle, DoWalk, DoBounce};
    public enum OPERATION {Plus, Minus, Multiply, Divide};
    public enum EXPRESSION {Idle, Fail, Success};
    public enum LAYER {IconBtnGroupArea};
    public enum SORTINGLAYER {Mat, Default};
}   