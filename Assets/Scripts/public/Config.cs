using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {
    public readonly static string OPERATION_REGEX_PATTERN = @"[+\-]|minus|times|frac|underline|left";
    public readonly static string TEXTDRAW_REGEX_PATTERN = @"[-+x=?]|minus|times|frac|underline|left|\d+";
}