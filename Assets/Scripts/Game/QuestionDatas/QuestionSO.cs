using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject { //* データ
    [Header("FOUR FUNDAMENTAL CONTENT")]
    [TextArea(2 ,6)]
    [SerializeField] string qstPlus; public string QstPlus {get => qstPlus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstPlus_XEqu; public string QstPlus_XEqu {get => qstPlus_XEqu;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMinus; public string QstMinus {get => qstMinus;}
    [TextArea(2 ,6)]
    [SerializeField] string qstMultiply; public string QstMultiply {get => qstMultiply;}
    [TextArea(2 ,6)]
    [SerializeField] string qstDivide; public string QstDivide {get => qstDivide;}
    [TextArea(2 ,6)]
    [SerializeField] string qstGreatestCommonDivisor; public string QstGreatestCommonDivisor {get => qstGreatestCommonDivisor;}

    [Header("RANDOM OBJS")]
    [SerializeField] string[] defObjNames; public string[] DefObjNames {get => defObjNames; set => defObjNames = value;}
    List<string> objNameList;      public List<string> ObjNameList {get => objNameList; set => objNameList = value;}
    [SerializeField] string obj1Name;  public string Obj1Name {get => obj1Name; set => obj1Name = value;}
    [SerializeField] string obj2Name;  public string Obj2Name {get => obj2Name; set => obj2Name = value;}
}
