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
    public enum MAP {Forest, Jungle, Tundra};
    public enum MG {Minigame1, Minigame2, Minigame3}
    public enum MINIGAME_LV {Easy, Normal, Hard}
    public enum TAG {
        GoGame, IconBtnGroupArea, Player, Pet, Funiture, Box, Obj,
        TableDecoArea,
        Apple, Bomb, GoldApple, Diamond, //* MiniGame1
        JumpingPad, EraseObjLine, Banana, GoldBanana, PlayerNoCollideArea,  //* MiniGame2
        Obstacle, Blueberry, GoldBlueberry,
    };
    public enum ANIM {
        DoSkipTitleAnim,
        IsWalk, DoBounce, DoSuccess, DoFail, IsSit, DoDance, 
        IsShowGachaReward, DoSweetPotato, IsGoldSweetPotato,
        BlackInOut, BlackIn, BlackOut,
        DoCamShake, DoWindMillScrollDown,
        DoBlinkAdd, DoBlinkMinus,
        HelpFraction, HelpGCD,
        DoTalk, DoShock,
        DoSwitchBG,
        DoFirstActive
    };
    public enum BOX_NAME {
        Box , LeftBox, RightBox, _Blink, _QuestionMark,
    }
    public enum OPERATION {Plus, Minus, Multiply, Divide};
    public enum EXPRESSION {Idle, Fail, Success};
    public enum LAYER {Chair};
    public enum SORTING_LAYER {Mat, Default, FrontDecoObj};
    public enum HOME_EF_IDX {FunitureSetupEF};
    public enum OBJ_SPR_IDX { //! GMスクリプトのobjSprsと順番を合わせること！
        Apple,
        Banana,
        Blueberry,
        Branch,
        Carrot,
        Cherry,
        Clover,
        Coconut,
        Coin,
        Dotory,
        DragonFruit,
        Feather,
        Grape,
        GreenApple,
        Leaf,
        Lemon,
        Mango,
        Mushroom,
        MushroomDot,
        Onion,
        Orange,
        Pear,
        Potato,
        Pumkin,
        Rock,
        SilverCoin,
        Strawberry,
        SunFlowerSeed,
        WaterMelon,
    };
    public enum RWD_IDX {
        Coin, Exp, WoodChair, FrogChair, 
        WoodenWolfStatue, GoldenMonkeyStatue, IceDragonStatue
    };
    public enum SPC_PET {
        GoldApple, BabyMonkey, BabyDragon
    }

    internal static IEnumerable<QuestManager.MQ_ID> GetValues(Type type)
    {
        throw new NotImplementedException();
    }
}