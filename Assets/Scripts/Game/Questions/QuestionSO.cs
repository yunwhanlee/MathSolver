using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] string[] objs;      public string[] Objs {get => objs;}

    public string makeQuizSentence(List<string> analList, bool isXEquation) {
        // analList.ForEach(li => Debug.Log("li= " + li));
        string sign = analList.Find(s => s=="+"||s=="-"||s=="minus"||s=="times"||s=="frac"||s=="underline"||s=="left");
        analList.Remove(sign);

        //* キーワード 切り替え
        string result = "";
        string objName = objs[Random.Range(0, objs.Length)];
        string obj2Name = objs[Random.Range(0, objs.Length)];
        switch(sign) {
            case "+": {
                if(isXEquation) { //* 数式類推型
                    // 要らない部分削除
                    int xLastIdx = analList.FindLastIndex(str => str == "x");
                    int endLen = analList.Count - xLastIdx - 1;
                    analList.RemoveRange(xLastIdx, endLen);
                    analList.RemoveAll(str => str == "=");

                    Debug.Log($"makeQuizSentence:: (+) 数式類推型 Cnt({analList.Count})---------------------------------------------");
                    int i=0; analList.ForEach(li => Debug.Log($"makeQuizSentence li[{i++}] => {li}"));
                    
                    result = qstPlus_XEquation.Replace("OBJ1", $"<sprite name={objName}>");
                    result = result.Replace("N1", analList[0]);
                    // result = result.Replace("X", analList[1] = "몇");
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
                else { //* 正解完成型
                    result = qstPlus.Replace("OBJ1", $"<sprite name={objName}>");
                    result = result.Replace("N1", analList[0]);
                    result = result.Replace("N2", analList[1]);
                }
                break;
            }
            case "-": {
                result = qstMinus.Replace("OBJ1", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            }
            case "times": {
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
        Debug.Log($"makeQuizSentence(sign= {sign}):: result= {result}");
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
}
