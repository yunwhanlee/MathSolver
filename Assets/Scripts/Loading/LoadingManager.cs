using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    AsyncOperation operation;
    [SerializeField] bool isAllowNextSceneActive;

    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI loadingTxt;
    [SerializeField] RectTransform loadingIcon;
    [SerializeField] GameObject TouchScreenPanel;
    const float MAX = 100;
    const float ROT_SPEED = 80;
    
    void Start(){
        TouchScreenPanel.SetActive(false);

        //* 処理 (非同期)
        StartCoroutine(coLoadScene(Enum.SCENE.Game.ToString()));
    }

    void Update() {
        loadingIcon.Rotate(0, 0, ROT_SPEED * Time.deltaTime);
    }
/// -------------------------------------------------------------------------------------------
#region FUNC
/// -------------------------------------------------------------------------------------------
    IEnumerator coLoadScene(string sceneName){
        yield return null;
        operation = SceneManager.LoadSceneAsync(sceneName);

        //* 読込みが完了出来たら、自動で次のシーンに進む。
        operation.allowSceneActivation = isAllowNextSceneActive;

        while(!operation.isDone){
            yield return null;
            if(loadingBar.value < 1f){
                loadingBar.value = Mathf.MoveTowards(loadingBar.value, 1f, Time.deltaTime); 
                loadingTxt.text = $"{LM._.localize("On my way to solve")}... {loadingBar.value}%";
            }
            else{
                loadingBar.value = MAX;
                loadingTxt.text = $"{LM._.localize("Arrived complete")}! {LM._.localize("Pls Touch Screen!")}";
                TouchScreenPanel.SetActive(true);
            }
        }
    }
    public void onClickTouchScreenGoGame() {
        operation.allowSceneActivation = true;
    }
#endregion
}
