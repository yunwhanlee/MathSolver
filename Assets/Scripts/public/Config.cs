using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {
    public readonly static Color CATE_SELECT_COLOR = new Color(0.1f, 0.3f, 0.55f, 1);

    public readonly static string OPERATION_REGEX_PATTERN = @"[+\-]|minus|times|frac|underline|left";
    public readonly static string TEXTDRAW_REGEX_PATTERN = @"[-+x=?]|minus|times|frac|underline|left|\d+";

    public readonly static int MINIGAME1_MAX_VAL = 400;
    public readonly static int[] MINIGMAE1_PLAY_PRICES = new int[3] {500, 1000, 1500};
    public readonly static int[] MINIGAME1_REWARD_SCORES = new int[3] {100, 250, MINIGAME1_MAX_VAL};
    public readonly static int[] MINIGAME1_EASY_OBJ_DATA = new int[2] {1, 2};
    public readonly static int[] MINIGAME1_NORMAL_OBJ_DATA = new int[3] {2, 4, 8};
    public readonly static int[] MINIGAME1_HARD_OBJ_DATA = new int[3] {3, 6, 10};
}