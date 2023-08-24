using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTeleType : MonoBehaviour {
    [SerializeField] GameObject endCursor;
    public IEnumerator coTextVisible(TextMeshProUGUI teleTxt, string voice) {
        teleTxt.ForceMeshUpdate();
        int totalVisibleChars = teleTxt.text.Length;
        int cnt = 0;
        if(endCursor) endCursor.SetActive(false); //* QuizTxtの場合は、endCursor要らない。

        //* Tele Type Anim
        while(true) {
            if(cnt % 4 == 0) SM._.sfxPlay(voice);
                
            int visibleCnt = cnt % (totalVisibleChars + 1);
            teleTxt.maxVisibleCharacters = visibleCnt;

            if (visibleCnt >= totalVisibleChars) {
                break;
            }

            cnt += 1;
            yield return Util.realTime0_025;
        }

        //* Tele Type Done
        Debug.Log($"coTextVisible:: TeleType Done!");
        SM._.sfxStop(voice);
        if(endCursor) endCursor.SetActive(true); //* QuizTxtの場合は、endCursor要らない。
    }
}