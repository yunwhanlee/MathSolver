using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static readonly Util _;
    public static WaitForSeconds time0_05 = new WaitForSeconds(0.05f);
    public static WaitForSeconds time0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds time0_5 = new WaitForSeconds(0.5f);
    public static WaitForSeconds time1 = new WaitForSeconds(1);
    public static WaitForSeconds time2 = new WaitForSeconds(2);
    public static WaitForSecondsRealtime realTime0_01 = new WaitForSecondsRealtime(0.01f);
    public static GameObject instantiateObj(GameObject obj, Transform tf) {
        GameObject ins = null;
        ins = Instantiate(obj, tf);
        return ins;
    }

    public static T GetRandomList<T>(List<T> list) {
        int rand = Random.Range(0, list.Count);
        T res = list[rand]; 
        list.RemoveAt(rand);
        return res;
    }
}