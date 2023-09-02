using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Util : MonoBehaviour
{
    public static readonly Util _;
    public static WaitForSeconds time0_005 = new WaitForSeconds(0.005f);
    public static WaitForSeconds time0_01 = new WaitForSeconds(0.01f);
    public static WaitForSeconds time0_025 = new WaitForSeconds(0.025f);
    public static WaitForSeconds time0_05 = new WaitForSeconds(0.05f);
    public static WaitForSeconds time0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds time0_2 = new WaitForSeconds(0.2f);
    public static WaitForSeconds time0_3 = new WaitForSeconds(0.3f);
    public static WaitForSeconds time0_5 = new WaitForSeconds(0.5f);
    public static WaitForSeconds time0_8 = new WaitForSeconds(0.8f);
    public static WaitForSeconds time1 = new WaitForSeconds(1);
    public static WaitForSeconds time1_5 = new WaitForSeconds(1.5f);
    public static WaitForSeconds time2 = new WaitForSeconds(2);
    public static WaitForSeconds time3 = new WaitForSeconds(3);
    public static WaitForSeconds time8 = new WaitForSeconds(8);
    public static WaitForSeconds time13 = new WaitForSeconds(13);
    public static WaitForSeconds time999 = new WaitForSeconds(999);
    public static WaitForSecondsRealtime realTime0_01 = new WaitForSecondsRealtime(0.01f);
    public static WaitForSecondsRealtime realTime0_025 = new WaitForSecondsRealtime(0.025f);
    public static WaitForSecondsRealtime realTime1 = new WaitForSecondsRealtime(1);
    public static GameObject instantiateObj(GameObject obj, Transform tf) {
        GameObject ins = null;
        ins = Instantiate(obj, tf);
        return ins;
    }

    public static T getStuffObjRandomList<T>(List<T> list) {
        int rand = Random.Range(0, list.Count);
        T res = list[rand]; 
        Debug.Log($"getStuffObjRandomList():: GM._.BgStatus= {GM._.BgStatus}, list[{rand}]= {res}");
        list.RemoveAt(rand);
        return res;
    }

    public static int getGreatestCommonDivisor(int a, int b) {
        // 整数
        a = Mathf.Abs(a);
        b = Mathf.Abs(b);

        // 例外
        if (a == 0 || b == 0)
            return a + b;

        // Euclidean algorithm 最大公約数
        while (b != 0) {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static IEnumerator coPlayBounceAnim(Transform objTf) {
        Debug.Log($"coPlayBounceAnim:: {objTf}");
        //* Value
        bool isDecreasing = false;
        float originSc = 1.0f;
        float spd = 2f * Time.deltaTime;
        float maxSc = originSc * 1.15f;

        //* Scale Bounce
        while(true) {
            if(!isDecreasing) {
                objTf.localScale = new Vector2(objTf.localScale.x + spd, objTf.localScale.y + spd);
                if(objTf.localScale.x > maxSc) isDecreasing = true;
            }
            else {
                objTf.localScale = new Vector2(objTf.localScale.x - spd, objTf.localScale.y - spd);
                if(objTf.localScale.x <= originSc) break;
            }
            yield return null;
        }
    }

    public static bool preventInputTxtBug(string str, bool isOnlyEng = false) {
        bool res = false;
        if(str.Contains(" ")) {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Can not use Space."));
            res = true;
        }
        else if(str == "") {
            HM._.ui.showErrorMsgPopUp(LM._.localize("Please Input Text."));
            res = true;
        }
        else if(isOnlyEng) {
            if(!Regex.IsMatch(str, "^[a-zA-Z0-9]*$")) {
                HM._.ui.showErrorMsgPopUp(LM._.localize("Only the English and Number."));
                res = true;
            }
        }
        return res;
    }

    public static int getExpUnitByMap() {
        int unit =(DB._.SelectMapIdx == (int)Enum.MAP.Forest)? Config.FOREST_EXP_RWD_UNIT
            : (DB._.SelectMapIdx == (int)Enum.MAP.Jungle)? Config.JUNGLE_EXP_RWD_UNIT
            : (DB._.SelectMapIdx == (int)Enum.MAP.Tundra)? Config.TUNDRA_EXP_RWD_UNIT : -1;
        if(unit == -1) Debug.LogError("getExpRewardUnitByMap():: Exp Set Value ERROR");
        return unit;
    }
    public static int getCoinUnitByMap() {
        int unit = (DB._.SelectMapIdx == (int)Enum.MAP.Forest)? Config.FOREST_COIN_RWD_UNIT
            : (DB._.SelectMapIdx == (int)Enum.MAP.Jungle)? Config.JUNGLE_COIN_RWD_UNIT
            : (DB._.SelectMapIdx == (int)Enum.MAP.Tundra)? Config.TUNDRA_COIN_RWD_UNIT : -1;
        if(unit == -1) Debug.LogError("getCoinRewardUnitByMap():: Coin Set Value ERROR");
        return unit;
    }
}