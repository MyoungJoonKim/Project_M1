using UnityEngine;

public enum MonsterType
{
    Normal,
    Boss
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Game Data/Monster Data")]

public class MonsterData : ScriptableObject
{
    [Header("Info")]
    public string monsterID;
    public string monsterName;
    public MonsterType monsterType;

    [Header("Prefab")]
    public Monster prefab;

    [Header("Stats")]
    public float maxHp;
    public float atk;
    public float def;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldown;

    [Header("Reward")]
    public int rewardExp;

    [Header("Boss Skills")]
    public ActiveSkillData projectionSkill;
    public ActiveSkillData targetExplosionSkill;
    public ActiveSkillData summonSkill;
}
