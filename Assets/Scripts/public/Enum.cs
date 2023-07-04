using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum {
    public enum SCENE {Title, Home, Loading, Game};
    public enum HOME {Room, IkeaShop, ClothShop, Inventory};
    public enum ACHIVERANK {Achivement, Mission, Rank};
    public enum FUNITURE_CATE {Funiture, Decoration, Bg, Mat};
    public enum INV_CATE {Player, Pet};
    public enum FUNITURE_BG {Wall, Floor};
    public enum TAG {GoGame, IconBtnGroupArea, Player, Pet, Funiture, Box, Obj};
    public enum ANIM {
        IsWalk, DoBounce, DoSuccess, DoFail, IsSit, DoDance, IsShowGachaReward,
        BlackInOut, BlackIn, BlackOut,
    };
    public enum OPERATION {Plus, Minus, Multiply, Divide};
    public enum EXPRESSION {Idle, Fail, Success};
    public enum LAYER {Chair};
    public enum SORTINGLAYER {Mat, Default};
    public enum HOME_EF_IDX {FunitureSetupEF};
    public enum OBJ_SPR_IDX {Apple, Carrot, Banana, Onion, Orange, Potato};
}   