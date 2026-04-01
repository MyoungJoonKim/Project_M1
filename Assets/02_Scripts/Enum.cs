using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    TITLE,
    LOADING,
    LOBBY,
    BATTLE,
    END
}

public enum StatType
{
    Hp,                 // 현재 체력
    Atk,                // 공격력
    Def,                // 방어력

    Level,              // 레벨
    Exp,                // 현재 경험치

    MoveSpeed,          // 이동속도
    AttackRange,        // 공격범위
    AttackCooldown      // 공격 쿨타임
}

public enum MaxStatType
{
    MaxHp,              // 최대 체력
    MaxExp              // 레벨업 필요 경험치
}

public enum MonsterState
{
    Idle,
    Move,
    Attack,
    Dead,

    Stun,
    Freeze,
    Burn,
    Knockback,
}

