using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject {
    [Header("FOUR FUNDAMENTAL CONTENT")]
    [TextArea(2 ,6)]
    [SerializeField] string qstPlus; public string QuestionPlus {get => qstPlus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstPlus_XEquation; public string QstPlus_XEquation {get => qstPlus_XEquation;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMinus; public string QstMinus {get => qstMinus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMultiply; public string QstMultiply {get => qstMultiply;}
    [TextArea(2 ,6)]
    [SerializeField] string qstDivide; public string QstDivide {get => qstDivide;}
    [TextArea(2 ,6)]
    [SerializeField] string qstGreatestCommonDivisor; public string QstGreatestCommonDivisor {get => qstGreatestCommonDivisor;}

    [Header("RANDOM OBJS")]
    [SerializeField] string[] defObjs;
    List<string> objList;      public List<string> ObjList {get => objList;}

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private void initObjList() => objList = new List<string>(defObjs);
    public string makeQuizSentence(List<string> analList) {
        string lOpr = "";
        string rOpr = "";
        List<string> lNums = new List<string>();
        List<string> rNums = new List<string>();
        bool isXEquation = analList.Exists(li => li == "x");
        if(isXEquation) analList.RemoveAll(str => str == "x");
        Debug.Log($"makeQuizSentence(analList.Cnt= {analList.Count}, isXEquation= {isXEquation})::");
        Debug.Log($"makeQuizSentence:: analList: <color=white>{string.Join(", ", analList.ToArray())}</color>");
        /*【 TYPE 分析 】
            ※ "="が有る：(横)、ない：(縦)。
            ※ (縦)は、１次 方程式がない。
            ※ 「x, =, ?」：三つの情報は要らない。

            ① 4, +, 3, =, ? (横：足す)
            ② 38, -, 13, ? (縦：引く)
            ③ 12, times, 4, ? (縦：掛け)
            ④ frac, 12, 4, =, ? (横：分数) 
            ⑤ underline, 3, 9, ? || left, 8, 10, ? (縦：最大公約数)
            ⑥ 2, +, x, =, 8, 「x, =, ?」 (横：１次 方程式)
            ⑦ 1, +, x, =, 8, minus, 4, 「x, =, ?」 (横：１次 方程式＋右式⊖定数)
            ⑧ 4, +, x, =, 7, +, 1, 「x, =, ?」 (横：１次 方程式＋右式⊕定数)
        */

        //* 横・縦 ?
        bool isHorizontalEquation = analList.Exists(li => li == "=");
        //* 横 (正解完成型) とか (数式推論型)
        if(isHorizontalEquation) {
            //* 左・右式 分ける
            int equalIdx = analList.FindIndex(li => li == "=");
            int len = analList.Count - 1;
            List<string> leftEquList = new List<string>();
            List<string> rightEquList = new List<string>();
            for(int i = 0; i < analList.Count; i++) {
                if(i < equalIdx)    leftEquList.Add(analList[i]);
                else    rightEquList.Add(analList[i]);
            }
            //* 要らない部分 削除
            rightEquList.Remove("=");
            rightEquList.Remove("?");

            Debug.Log($"makeQuizSentence:: (横  Left式): <color=white>{string.Join(", ", leftEquList.ToArray())}</color>"); 
            Debug.Log($"makeQuizSentence:: (横 Right式): <color=white>{string.Join(", ", rightEquList.ToArray())}</color>"); 

            //* 左 演算子
            lOpr = leftEquList.Find(str => Regex.IsMatch(str, Config.OPERATION_REGEX_PATTERN));
            leftEquList.Remove(lOpr);
            lNums = leftEquList;
            
            //* 右 演算子
            rOpr = rightEquList.Find(str => Regex.IsMatch(str, Config.OPERATION_REGEX_PATTERN));
            if(rOpr != "") {
                rightEquList.Remove(rOpr);
                rNums = rightEquList;
            }
        }
        //* 縦 (正解完成型) のみ
        else {
            lOpr = analList.Find(str => Regex.IsMatch(str, Config.OPERATION_REGEX_PATTERN));
            analList.Remove(lOpr);
        }

        // analList.Remove(lOpr);
        // analList.RemoveAll(str => str == "?");

        //* キーワード 切り替え
        string result = "미 지원..";
        initObjList();
        string objName = Util.GetRandomList(objList);
        string obj2Name = Util.GetRandomList(objList);
        switch(lOpr) {
            case "+": {
                if(isXEquation) { //* (数式推論型)
                    // 要らない部分削除
                    int xLastIdx = analList.FindLastIndex(str => str == "x");
                    int endLen = analList.Count - xLastIdx - 1;
                    analList.RemoveRange(xLastIdx, endLen);
                    analList.RemoveAll(str => str == "=");
                    
                    result = qstPlus_XEquation.Replace("OBJ1", $"<sprite name={objName}>");
                    result = result.Replace("N1", analList[0]);
                    result = result.Replace("N2", analList[2]);

                    // EXTRA 演算子
                    if(analList.Count >= 4) {
                        if(analList[3] != null) {
                            string extraSign = analList[3];
                            switch(extraSign) {
                                case "+":
                                    result += $"...\n<color=blue>앗! {analList[4]}개 더 있네요.</color>";
                                    break;
                                case "-": case "minus":
                                    result += $"...\n<color=red>앗! 죄송.. {analList[4]}개 빼야되요.</color>";
                                    break;
                            }
                        }
                    }
                    else {
                        result += "가 됫어요.";
                    }
                    result += "\n친구는 몇 개를 주었나요?";
                }
                else { //* 正解完成型 (2 + 3 = ?)
                    result = qstPlus.Replace("OBJ1", $"<sprite name={objName}>");
                    result = result.Replace("N1", analList[0]);
                    result = result.Replace("N2", analList[1]);
                }
                break;
            }
            case "-": { // 38 - 13 = ?
                result = qstMinus.Replace("OBJ1", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            }
            case "times": { // 31 times 2
                result = qstMultiply.Replace("OBJ1", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            }
            case "frac": {
                int n1 = int.Parse(analList[0]);
                int n2 = int.Parse(analList[1]);
                int value = n1 / n2;
                int rest =  n1 % n2;
                Debug.Log($"value= {value}, rest= {rest}");
                result = qstDivide.Replace("OBJ1", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                //* 残りが有ったら、分数で表記
                if(rest != 0)
                    result += " 나머지는요?\n(분수로 알려주세요!)";
                break;
            }
            case "underline": case "left": { //* 最大公約数
                result = qstGreatestCommonDivisor.Replace("OBJ1", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("OBJ2", $"<sprite name={obj2Name}>");
                result = result.Replace("N2", analList[1]);
                break;
            }
        }
        Debug.Log($"makeQuizSentence(sign= {lOpr}):: result= {result}");
        return result;
    }

    public IEnumerator coCreateObj(string sign, string objName, List<string> analList) {
        switch(sign) {
            case "+": 

                break;
            case "-":

                break;
            case "times":

                break;
            case "frac": {
                // int cnt = 50 / 10;
                // for(int i = 0; i < cnt; i++){
                //     yield return new WaitForSeconds(0.1f);
                //     var stuff = Instantiate(stuffObjPf, GM._.StuffGroupTf);
                //     stuff.transform.position = new Vector2(0, 5);
                // }
                break;
            }
            yield return null;
        }
    }
#endregion
}
