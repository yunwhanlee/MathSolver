using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject {
    [Header("FOUR FUNDAMENTAL CONTENT")]
    [TextArea(2 ,6)]
    [SerializeField] string qstPlus; public string QuestionPlus {get => qstPlus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMinus; public string QstMinus {get => qstMinus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMultiply; public string QstMultiply {get => qstMultiply;}
    [TextArea(2 ,6)]
    [SerializeField] string qstDivide; public string QstDivide {get => qstDivide;}

    [Header("RANDOM OBJS")]
    [SerializeField] string[] objs;      public string[] Objs {get => objs;}

    public string makeQuizSentence(string sign, List<string> analList) {
        // analList.ForEach(li => Debug.Log("li= " + li));

        //* キーワード 切り替え
        string result = "";
        string objName = objs[Random.Range(0, objs.Length)];
        switch(sign) {
            case "+":
                result = qstPlus.Replace("OBJ", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            case "-":
                result = qstMinus.Replace("OBJ", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            case "times":
                result = qstMultiply.Replace("OBJ", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
            case "frac":
                result = qstDivide.Replace("OBJ", $"<sprite name={objName}>");
                result = result.Replace("N1", analList[0]);
                result = result.Replace("N2", analList[1]);
                break;
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
