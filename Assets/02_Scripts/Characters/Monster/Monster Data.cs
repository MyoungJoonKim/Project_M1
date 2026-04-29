using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

    [Header("Prefab")]
    public Monster prefab;

    [Header("Type")]
    public MonsterType monsterType;

    [Header("Stats")]
    public float maxHp = 200f;
    public float atk = 30f;
    public float def = 10f;
    public float moveSpeed = 7f;
    public float attackRange = 5f;
    public float attackCooldown = 1.5f;

    [Header("Reward")]
    public int rewardExp = 10;
}
