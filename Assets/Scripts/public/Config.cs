using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {
    //* PLAYER
    public readonly static int LV_EXP_UNIT = 100;
    public readonly static float LV_BONUS_PER = 0.1f;
    //* HOME
    public readonly static Color CATE_SELECT_COLOR = new Color(0.1f, 0.3f, 0.55f, 1);
    //* GAME
    public readonly static string OPERATION_REGEX_PATTERN = @"[+\-]|minus|times|frac|underline|left";
    public readonly static string TEXTDRAW_REGEX_PATTERN = @"[-+x=?]|minus|times|frac|underline|left|\d+";
    //* MINIGAME
    public readonly static int[] MINIGMAE_PLAY_PRICES = new int[3] {500, 1000, 1500};

    public readonly static string MINIGAME1_TITLE = "Catch falling apples!";
    public readonly static string MINIGAME1_CONTENT = "Collect as many apples as you can.";
    public readonly static int MINIGAME1_MAX_VAL = 200;
    public readonly static int[] MINIGAME1_UNLOCK_SCORES = new int[3] {50, 100, MINIGAME1_MAX_VAL};
    public readonly static int[] MINIGAME1_EASY_OBJ_DATA = new int[2] {1, 2};
    public readonly static int[] MINIGAME1_NORMAL_OBJ_DATA = new int[3] {2, 4, 8};
    public readonly static int[] MINIGAME1_HARD_OBJ_DATA = new int[3] {3, 6, 10};

    public readonly static string MINIGAME2_TITLE = "Jump to the sky!";
    public readonly static string MINIGAME2_CONTENT = "Collect bananas without falling off.";
    public readonly static int MINIGAME2_MAX_VAL = 200;
    public readonly static int[] MINIGAME2_UNLOCK_SCORES = new int[3] {50, 100, MINIGAME1_MAX_VAL};
    public readonly static int[] MINIGAME2_EASY_OBJ_DATA = new int[2] {1, 3};
    public readonly static int[] MINIGAME2_NORMAL_OBJ_DATA = new int[3] {2, 5, 9};
    public readonly static int[] MINIGAME2_HARD_OBJ_DATA = new int[3] {3, 7, 10};

    public readonly static string MINIGAME3_TITLE = "Snow sledding!";
    public readonly static string MINIGAME3_CONTENT = "Collect blueberries avoiding obstacles.";
    public readonly static int MINIGAME3_MAX_VAL = 200;
    public readonly static int[] MINIGAME3_UNLOCK_SCORES = new int[3] {50, 100, MINIGAME3_MAX_VAL};
    public readonly static int[] MINIGAME3_EASY_OBJ_DATA = new int[2] {1, 2};
    public readonly static int[] MINIGAME3_NORMAL_OBJ_DATA = new int[3] {2, 4, 8};
    public readonly static int[] MINIGAME3_HARD_OBJ_DATA = new int[3] {3, 6, 10};
}