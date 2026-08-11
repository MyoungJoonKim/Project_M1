using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PropType
{
    Attackable,
    NonAttackable
}

[CreateAssetMenu(fileName = "PropData", menuName = "Game Data/Prop Data")]
public class PropData : ScriptableObject
{
    [Header("Info")]
    public string propID;
    public string propName;
    public PropType propType;

    [Header("Prefab")]
    public Prop prefab;

    [Header("Stats")]
    public float maxHp;
}
