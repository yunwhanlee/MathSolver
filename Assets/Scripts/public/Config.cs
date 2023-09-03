using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {
    //* Version
    public readonly static string VER_STATUS = "demo";
    public readonly static string VER_MAJOR = "1";
    public readonly static string VER_MINOR = "0";
    public readonly static string VER_RESOLUTION = "1";
    //* PLAYER
    public readonly static int LV_EXP_UNIT = 60;
    public readonly static float LV_BONUS_PER = 0.1f;
    public readonly static float LEGACY_BONUS_PER = 0.2f;
    //* CLOTH
    public readonly static int GACHA_SWEETPOTATO_PER = 40;
    public readonly static int GACHA_PET_PER = 30;
    //* HOME
    public readonly static Color CATE_SELECT_COLOR = new Color(0.1f, 0.3f, 0.55f, 1);
    public readonly static int CLOTH_PRICE_UNIT = 300;
    //* GAME
    public readonly static int FOREST_EXP_RWD_UNIT = 10;
    public readonly static int FOREST_COIN_RWD_UNIT = 100;

    public readonly static int JUNGLE_EXP_RWD_UNIT = 15;
    public readonly static int JUNGLE_COIN_RWD_UNIT = 150;

    public readonly static int TUNDRA_EXP_RWD_UNIT = 18;
    public readonly static int TUNDRA_COIN_RWD_UNIT = 180;

    public readonly static int COMBO_ACTIVE_CNT = 2;
    public readonly static int[] MAP1_BG_UNLOCK_LVS = new int[3] {1, 3, 5};
    public readonly static int[] MAP2_BG_UNLOCK_LVS = new int[3] {7, 9, 11};
    public readonly static int[] MAP3_BG_UNLOCK_LVS = new int[3] {13, 15, 17};

    public readonly static string OPERATION_REGEX_PATTERN = @"[+\-]|minus|times|frac|underline|left";
    public readonly static string TEXTDRAW_REGEX_PATTERN = @"[-+x=?]|minus|times|frac|underline|left|\d+";
    //* MINIGAME
    public readonly static int[] MINIGMAE_PLAY_PRICES = new int[3] {500, 1000, 1500};
    public readonly static int MINGAME_RES_EXP_UNIT = 2;
    public readonly static int MINGAME_RES_COIN_UNIT = 5;

    public readonly static string MINIGAME1_TITLE = "Catch falling apples!";
    public readonly static string MINIGAME1_CONTENT = "Collect as many apples as you can.";
    public readonly static int MINIGAME1_MAX_VAL = 130;
    public readonly static int[] MINIGAME1_UNLOCK_SCORES = new int[3] {30, 80, MINIGAME1_MAX_VAL};
    public readonly static int[] MINIGAME1_EASY_OBJ_DATA = new int[2] {1, 2};
    public readonly static int[] MINIGAME1_NORMAL_OBJ_DATA = new int[3] {2, 4, 8};
    public readonly static int[] MINIGAME1_HARD_OBJ_DATA = new int[3] {3, 6, 10};

    public readonly static string MINIGAME2_TITLE = "Jump to the sky!";
    public readonly static string MINIGAME2_CONTENT = "Collect bananas without falling off.";
    public readonly static int MINIGAME2_MAX_VAL = 130;
    public readonly static int[] MINIGAME2_UNLOCK_SCORES = new int[3] {30, 80, MINIGAME1_MAX_VAL};
    public readonly static int[] MINIGAME2_EASY_OBJ_DATA = new int[2] {1, 3};
    public readonly static int[] MINIGAME2_NORMAL_OBJ_DATA = new int[3] {2, 5, 9};
    public readonly static int[] MINIGAME2_HARD_OBJ_DATA = new int[3] {3, 7, 10};

    public readonly static string MINIGAME3_TITLE = "Snow sledding!";
    public readonly static string MINIGAME3_CONTENT = "Collect blueberries avoiding obstacles.";
    public readonly static int MINIGAME3_MAX_VAL = 130;
    public readonly static int[] MINIGAME3_UNLOCK_SCORES = new int[3] {30, 80, MINIGAME3_MAX_VAL};
    public readonly static int[] MINIGAME3_EASY_OBJ_DATA = new int[2] {1, 2};
    public readonly static int[] MINIGAME3_NORMAL_OBJ_DATA = new int[3] {2, 4, 8};
    public readonly static int[] MINIGAME3_HARD_OBJ_DATA = new int[3] {3, 6, 10};
}