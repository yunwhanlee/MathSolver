using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static readonly Util _;
    public static WaitForSeconds delay0_05 = new WaitForSeconds(0.05f);
    public static WaitForSeconds delay0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds delay0_5 = new WaitForSeconds(0.5f);
    public static WaitForSeconds delay1 = new WaitForSeconds(1);
    public static WaitForSeconds delay2 = new WaitForSeconds(2);
    public static WaitForSecondsRealtime delayRT0_01 = new WaitForSecondsRealtime(0.01f);
}