using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum {
    public enum SCENE {Title, Home, Loading, Game, MiniGame};
    public enum HOME {Room, IkeaShop, ClothShop, Inventory};
    public enum ACHIVERANK {Achivement, Mission, Rank};
    public enum FUNITURE_CATE {Funiture, Decoration, Bg, Mat};
    public enum INV_CATE {Player, Pet};
    public enum FUNITURE_BG {Wall, Floor};
    public enum MAP {
        Forest, Jungle, Tundra,
        MiniGame1_Orchard, MiniGame2_Monkeywat, MiniGame3_IceDragon
    };
    public enum MINIGAME_LV {
        Easy, Normal, Hard
    }
    public enum TAG {
        GoGame, IconBtnGroupArea, Player, Pet, Funiture, Box, Obj,
        TableDecoArea,
        Apple, Bomb, GoldApple, Diamond//* MiniGame1
    };
    public enum ANIM {
        IsWalk, DoBounce, DoSuccess, DoFail, IsSit, DoDance, IsShowGachaReward,
        BlackInOut, BlackIn, BlackOut,
        DoCamShake, DoWindMillScrollDown, 
        DoBlinkAdd, DoBlinkMinus,
        HelpFraction, HelpGCD,
        DoTalk, DoShock,
        DoSwitchBG, 
        DoFirstActive,
    };
    public enum BOX_NAME {
        Box , LeftBox, RightBox, _Blink, _QuestionMark,
    }
    public enum OPERATION {Plus, Minus, Multiply, Divide};
    public enum EXPRESSION {Idle, Fail, Success};
    public enum LAYER {Chair};
    public enum SORTING_LAYER {Mat, Default, FrontDecoObj};
    public enum HOME_EF_IDX {FunitureSetupEF};
    public enum OBJ_SPR_IDX {
        //* 小さい
        SunFlowerSeed, Cherry, Blueberry, Dotory, Leaf,
        //* 一般
        Apple, Carrot, Banana, Onion, Orange, Potato, Branch, Rock,
    };
    public enum RWD_IDX {
        Coin, Exp, WoodChair
    };
    public enum OBJ_NAME {
    }

    internal static IEnumerable<QuestManager.MQ_ID> GetValues(Type type)
    {
        throw new NotImplementedException();
    }
}