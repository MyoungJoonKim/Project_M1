using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Game Data/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("Localization Key")]
    public string abilityName;
    public string abilityDescription;

    [Header("Info")]
    public Sprite icon;
    public PassiveType passiveType;

    [Header("LevelUp Base")]
    public int[] unlockLevel;
    public int[] unlockCost;
    public float[] bonusValue;
}
