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
    [SerializeField] string[] defObjNames;
    List<string> objNameList;      public List<string> ObjNameList {get => objNameList;}
    [SerializeField] string obj1Name;  public string Obj1Name {get => obj1Name; set => obj1Name = value;}
    [SerializeField] string obj2Name;  public string Obj2Name {get => obj2Name; set => obj2Name = value;}

//-------------------------------------------------------------------------------------------------------------
#region FUNC
//-------------------------------------------------------------------------------------------------------------
    private void initObjList() {
        objNameList = new List<string>(defObjNames);
        foreach(Transform obj in GM._.ObjGroupTf)
            Destroy(obj.gameObject);
    }
    public string makeQuizSentence(List<string> analList) {
        List<string> leftSideList = new List<string>(); //* 左辺
        List<string> rightSideList = new List<string>(); //* 右辺
        string lOpr = null; //* 左演算子
        string rOpr = null; //* 右演算子
        List<string> lNums = new List<string>(); //* 左定数
        List<string> rNums = new List<string>(); //* 右定数
        
        //* X方程式か？
        bool isXEquation = analList.Exists(li => li == "x");
        if(isXEquation) analList.RemoveAll(str => str == "x");

        Debug.Log($"makeQuizSentence(analList.Cnt= {analList.Count}, isXEquation= {isXEquation})::");
        Debug.Log($"makeQuizSentence:: analList: <color=white>{string.Join(", ", analList.ToArray())}</color>");
        /*【 TYPE 分析 】
            ※ "="が有る：(横)、ない：(縦)。
            ※ (縦)は「X方程式」がない。
            ※ 「x, =, ?」：三つは分離してから削除。

            ② (縦)：38, -, 13, ? (引く)
            ③ (縦)：12, times, 4, ? (掛け)
            ⑤ (縦)：underline, 3, 9, ? || left, 8, 10, ? (最大公約数)
            ① (横)：4, +, 3, =, ? (足す)
            ④ (横)：frac, 12, 4, =, ? (分数) 
            ⑥ (横)：2, +, x, =, 8, 「x, =, ?」 (X方程式)
            ⑦ (横)：1, +, x, =, 8, minus, 4, 「x, =, ?」 (X方程式＋右式⊖定数)
            ⑧ (横)：4, +, x, =, 7, +, 1, 「x, =, ?」 (X方程式＋右式⊕定数)
        */

        //* (横)：左・右辺、(縦)：左辺のみ
        bool isHrzEqu = analList.Exists(li => li == "=");

        //* 左辺・右辺 分離
        int splitIdx = (isHrzEqu)? analList.FindIndex(c=>c=="=") : analList.FindIndex(c=>c=="?");
        for(int i = 0; i < analList.Count; i++) {
            if(i < splitIdx) leftSideList.Add(analList[i]);
            else             rightSideList.Add(analList[i]);
        }

        //* 要らない部分 削除
        leftSideList.Remove("?");
        rightSideList.Remove("?");
        rightSideList.Remove("=");

        //* (横)：演算子と定数に分ける
        if(isHrzEqu) {
            lNums = separateOperatorAndNumbers(out lOpr, leftSideList);
            rNums = separateOperatorAndNumbers(out rOpr, rightSideList);
        }
        //* (縦)：演算子と定数に分ける
        else {
            lNums = separateOperatorAndNumbers(out lOpr, leftSideList);
        }

        Debug.Log($"makeQuizSentence:: (横): leftOpr= <color=yellow>{lOpr}</color>, rightOpr= <color=yellow>{rOpr}</color>");
        Debug.Log($"makeQuizSentence:: (横): lNums= <color=green>{string.Join(", ", lNums.ToArray())}</color>, rNums= <color=green>{string.Join(", ", rNums.ToArray())}</color>");

        //* キーワード 切り替え
        string result = "미 지원..";
        initObjList();
        obj1Name = Util.getRandomList(objNameList);
        obj2Name = Util.getRandomList(objNameList);
        int lN1 = int.Parse(lNums[0]); 
        int lN2 = lNums.Count > 1? int.Parse(lNums[1]) : 0;
        int rN1 = rNums.Count > 0? int.Parse(rNums[0]) : 0;
        int rN2 = rNums.Count > 1? int.Parse(rNums[1]) : 0;
        Debug.Log($"makeQuizSentence:: lN1= {lN1}, lN2= {lN2}, rN1= {rN1}, rN2= {rN2}");
        switch(lOpr) {
            case "+": {
                //* (定数式) N1 + N2 = ?
                if(!isXEquation) {
                    result = replaceTxtKeyword(qstPlus, new string[]{obj1Name, lNums[0], lNums[1]});
                    GM._.createObj(obj1Name, lN1);
                    GM._.OnAnswerObjAction += () => GM._.addObj(obj1Name, befNum: lN1, lN2);
                }
                //* (X方程式) N1 + X = N2
                else {
                    const float POS_X = 0.65f;
                    result = replaceTxtKeyword(qstPlus_XEquation, new string[]{obj1Name, lNums[0], rNums[0]});
                    GM._.createQuestionMarkBox(obj1Name, lN1, -POS_X);
                    GM._.createObj(obj1Name, rN1, POS_X);
                    GM._.OnAnswerBoxAction = GM._.showQuestionMarkAnswerBox;
                    
                    //* ± N3
                    if(rNums.Count > 1) {
                        rOpr = (rOpr == "minus")? "-" : rOpr; //* 言語➝記号に変更
                        result += replaceExtraOprKeyword(rOpr, rNums[1]);
                        GM._.createExtraOprBox(rOpr, obj1Name, rN2, POS_X);
                    }
                    else {
                        result += "가 됫어요.";
                    }

                    result += "\n친구는 몇 개를 주었나요?";
                }
                break;
            }
            case "-": { //* 38 - 13 = ?
                result = replaceTxtKeyword(qstMinus, new string[]{obj1Name, lNums[0], lNums[1]});
                GM._.createObj(obj1Name, lN1);
                GM._.OnAnswerObjAction += () => GM._.substractObj(lN2);
                break;
            }
            case "times": { //* 31 times 2
                result = replaceTxtKeyword(qstMultiply, new string[]{obj1Name, lNums[0], lNums[1]});
                GM._.createObj(obj1Name, lN1);
                GM._.OnAnswerObjAction += () => GM._.multiplyObj(obj1Name, befNum: lN1, lN2);
                break;
            }
            case "frac": {
                int value = lN1 / lN2;
                int rest = lN1 % lN2;
                Debug.Log($"value= {value}, rest= {rest}");

                result = replaceTxtKeyword(qstDivide, new string[]{obj1Name, lNums[0], lNums[1]});
                GM._.createObj(obj1Name, lN1);
                GM._.OnAnswerObjAction += () => GM._.divideObj(obj1Name, befNum: lN1, lN2);

                //* 残りが有ったら、分数で表記
                if(rest != 0)
                    result += " 나머지는요?\n(분수로 알려주세요!)";
                break;
            }
            case "underline":
            case "left": { //* 最大公約数
                const float POS_X = 0.65f;
                int gcd = Util.getGreatestCommonDivisor(lN1, lN2);
                result = replaceTxtKeyword(qstGreatestCommonDivisor, new string[]{obj1Name, lNums[0], lNums[1], obj2Name});
                GM._.createObj(obj1Name, lN1, posX: -POS_X);
                GM._.createObj(obj2Name, lN2, posX: POS_X);
                GM._.OnAnswerObjAction += () => GM._.greatestCommonDivisorObj(obj1Name, lN1, gcd, -POS_X);
                GM._.OnAnswerObjAction += () => GM._.greatestCommonDivisorObj(obj2Name, lN2, gcd, POS_X);
                break;
            }
        }
        Debug.Log($"makeQuizSentence(sign= {lOpr}):: result= {result}");
        return result;
    }

    private List<string> separateOperatorAndNumbers(out string oprerator, List<string> equationList) {
        //* 演算子
        oprerator = equationList.Find(str => Regex.IsMatch(str, Config.OPERATION_REGEX_PATTERN));
        if(oprerator != null) equationList.Remove(oprerator);
        //* 残った定数を返す
        return equationList;
    }

    private string replaceTxtKeyword(string sentence, string[] keys) {
        Debug.Log($"replaceTxtKeyword:: keys.Length= {keys.Length} : {string.Join(", ", keys)}");
        const int OBJ1 = 0, N1 = 1, N2 = 2, OBJ2 = 3;
        //* Keyword 変換
        string res = sentence.Replace("OBJ1", $"<sprite name={keys[OBJ1]}>");
        res = res.Replace("N1", keys[N1]);
        res = res.Replace("N2", keys[N2]);
        if(OBJ2 < keys.Length) 
            res = res.Replace("OBJ2", $"<sprite name={keys[OBJ2]}>");

        return res;
    }

    private string replaceExtraOprKeyword(string rOpr, string key) {
        switch(rOpr) {
            case "+":
                return $"...\n<color=blue>앗! {key}개 더 있네요.</color>";
            case "-": // case "minus":
                return $"...\n<color=red>앗! 죄송.. {key}개 빼야되요.</color>";
        }
        return "";
    }
#endregion
}
