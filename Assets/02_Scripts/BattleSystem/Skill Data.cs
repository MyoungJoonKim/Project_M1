using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="Game/Skill Data")]
public class SkillData : ScriptableObject
{
    public GameObject skillObjectPrefabs;

    [Header("Info")]
    public string skillName;

    [Header("Base")]
    public float damage;
    public float cooldown;

    
}
