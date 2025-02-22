using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cam : MonoBehaviour {
    const int W = 9, H = 16;
    [SerializeField] Animator anim; public Animator Anim {get => anim; set => anim = value;}

    void Awake() {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)W / H);
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
        OnPreCull();
    }

/// -----------------------------------------------------------------------------------------------------------------
#region FUNCTION
/// -----------------------------------------------------------------------------------------------------------------
    private void OnPreCull() {
        GL.Clear(true, true, Color.black);
    }
#endregion
}