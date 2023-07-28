using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward Item", fileName = "RewardItem")]
public class RewardItemSO : ScriptableObject { //* データ
    public enum ItemType {Equipment, Consumables, Etc};
    [SerializeField] ItemType itemType;
    [SerializeField] string name;
    [SerializeField] Sprite spr;     public Sprite Spr {get => spr;} //* ChildIdx[0]
    [SerializeField] Color clr;      public Color Clr {get => clr;}
    [SerializeField] bool isSpecial; public bool IsSpecial {get => isSpecial;} //* ChildIdx[2]
}