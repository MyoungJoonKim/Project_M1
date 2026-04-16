using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Rotation,       // 회전형
    Area,           // 범위형
    Summon,         // 소환형
    Projection      // 투사체
}

[CreateAssetMenu(menuName ="Game Data/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Info")]
    public string skillName;
    public SkillType skillType;
    public GameObject skillPrefab;
    public Sprite icon;
    public string skillInfo;

    [Header("LevelUp Base")]
    public int maxLevel = 5;
    public int[] count;
    public float[] damage;
    public float[] radius;
    public float[] speed;
    public float[] hitInterval;

    public float duration;
    public float cooldown;

    
}
