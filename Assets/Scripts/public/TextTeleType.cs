using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTeleType : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI teleTxt;   public TextMeshProUGUI TeleTxt {get => teleTxt; set => teleTxt = value;}
    [SerializeField] float speed;

    public IEnumerator coTextVisible(TextMeshProUGUI teleTxt) {
        teleTxt.ForceMeshUpdate();
        int totalVisibleChars = teleTxt.textInfo.characterCount;
        int cnt = 0;

        while(true) {
            int visibleCnt = cnt % (totalVisibleChars + 1);
            teleTxt.maxVisibleCharacters = visibleCnt;
            // Debug.Log($"coTextVisible:: visibleCnt= {visibleCnt}, totalVisibleChars= {totalVisibleChars}");

            if (visibleCnt >= totalVisibleChars) {
                break;
            }

            cnt += 1;
            yield return new WaitForSeconds(speed);
        }
    }
}