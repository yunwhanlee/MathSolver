using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTeleType : MonoBehaviour {
    [SerializeField] GameObject endCursor;
    public IEnumerator coTextVisible(TextMeshProUGUI teleTxt, string voice) {
        Debug.Log($"coTextVisible:: charLen= {teleTxt.text.Length}, teleTxt= {teleTxt}, voice= {voice}");
        teleTxt.ForceMeshUpdate();
        int charLen = teleTxt.text.Length;
        int cnt = 0;
        const int voiceSpan = 4;
        if(endCursor) endCursor.SetActive(false); //* QuizTxtの場合は、endCursor要らない。

        //* Tele Type Anim
        while(true) {
            if(Mathf.Clamp(cnt, 0, charLen - voiceSpan) % voiceSpan == 0) SM._.sfxPlay(voice);
                
            int visibleCnt = cnt % (charLen + 1);
            teleTxt.maxVisibleCharacters = visibleCnt;

            if (visibleCnt >= charLen) {
                break;
            }

            cnt += 1;
            yield return Util.realTime0_025;
        }

        //* Tele Type Done
        Debug.Log($"coTextVisible:: TeleType Done!");
        if(endCursor) endCursor.SetActive(true); //* QuizTxtの場合は、endCursor要らない。
    }
}