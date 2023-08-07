using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour {
    public enum ID {
        CorrectAnswerCnt,
        SkinCnt,
        PetCnt,
        CoinAmount,
    }

    //* Achieve
    [SerializeField] Achieve[] achieves;    public Achieve[] Achieves {get => achieves;}
}
