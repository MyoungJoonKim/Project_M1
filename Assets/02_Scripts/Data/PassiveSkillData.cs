using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveType
{
    ExpBonus,
    MoveSpeed,
    PickupRange,
    DamageBonus,
    DamageReduction
}


[CreateAssetMenu(fileName = "PassiveSkillData", menuName ="Game Data/PassiveSkill Data")]
public class PassiveSkillData : ScriptableObject
{
    [Header("Info")]
    public string passiveSkillName;
    public string skillInfo;
    public PassiveType passiveType;
    public Sprite icon;

    [Header("LevelUp Base")]
    public int maxLevel = 5;
    public float valuePerLevel;
}
