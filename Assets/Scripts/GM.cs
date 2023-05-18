using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM _;
    public Player pl;
    public TouchControl touchCtr;

    void Awake() {
        _ = this;
    }
}
