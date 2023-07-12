using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static WaitForSeconds time2 = new WaitForSeconds(2);
    public static WaitForSecondsRealtime realTime0_01 = new WaitForSecondsRealtime(0.01f);
    public static GameObject instantiateObj(GameObject obj, Transform tf) {
        GameObject ins = null;
        ins = Instantiate(obj, tf);
        return ins;
    }

    public static T getRandomList<T>(List<T> list) {
        int rand = Random.Range(0, list.Count);
        T res = list[rand]; 
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
        //* Value
        bool isDecreasing = false;
        const float originSc = 1.0f;
        float spd = 1.5f * Time.deltaTime;
        const float maxSc = 1.2f;

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
    public static float getExpPer() {
        int max = Data.EXP_MAX_UNIT * DB.Dt.Lv;
        if(DB.Dt.Exp == max) {
            DB.Dt.Lv++;
            DB.Dt.Exp = 0;
            return 1;
        }
        return ((float)DB.Dt.Exp) / ((float)max);
    }
}