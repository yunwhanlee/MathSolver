using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LM;

public class Localize : MonoBehaviour

{
    [TextArea(2 ,6)]
    public string textKey;

    void Start() {
        localizeChanged();
        _.actLocalizechanged += localizeChanged;
    }
    void OnDestroy() {
        _.actLocalizechanged -= localizeChanged;
    }
///------------------------------------------------------------------------------------------
    #region FUNC
///------------------------------------------------------------------------------------------
    private string localize(string key) {
        int keyIndex = _.langs[0].value.FindIndex(i => i.ToLower() == key.ToLower());
        return _.langs[_.curLangIndex].value[keyIndex];
    }
    private void localizeChanged() {
        if(GetComponent<TextMeshProUGUI>() != null) {
            GetComponent<TextMeshProUGUI>().text = localize(textKey);
        }
    }
    #endregion
}
