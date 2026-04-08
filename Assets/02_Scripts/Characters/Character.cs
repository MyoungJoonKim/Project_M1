using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;

    [Header("Character Stats")]
    public Dictionary<StatType, float> stats = new();
    public Dictionary<MaxStatType, float> maxStats = new();

    public bool isDead;
    protected bool deadHandled;

    public event Action StatBarChange;

    // 초기 스탯 초기화 함수
    public void InitStats(
        float maxHp,
        float atk,
        float def,
        float moveSpped = 0f,
        float attackRange = 0f,
        float attackCooldown = 0f,
        float level = 1f,
        float exp = 0f,
        float maxExp = 100f)
    {
        maxStats[MaxStatType.MaxHp] = maxHp;
        maxStats[MaxStatType.MaxExp] = maxExp;

        stats[StatType.Hp] = maxHp;
        stats[StatType.Atk] = atk;
        stats[StatType.Def] = def;
        stats[StatType.MoveSpeed] = moveSpped;
        stats[StatType.AttackRange] = attackRange;
        stats[StatType.AttackCooldown] = attackCooldown;
        stats[StatType.Level] = level;
        stats[StatType.Exp] = exp;

        isDead = false;
        deadHandled = false;
    }

    // 스탯 반환 함수
    public float GetStat(StatType type)
    {
        if (stats.TryGetValue(type, out float value))
            return value;
        return 0f;
    } 
    
    // 스탯 설정 함수
    public void SetStat(StatType type, float value)
    {
        stats[type] = value;
        StatBarChange?.Invoke();
    }

    // 스탯 증가 함수
    public void AddStat(StatType type, float value)
    {
        if (!stats.ContainsKey(type))
            stats[type] = 0f;

        stats[type] += value;
        StatBarChange?.Invoke();
    }

    // 스탯 최댓값 반환 함수
    public float GetMaxStat(MaxStatType type)
    {
        if (maxStats.TryGetValue(type,out float value)) 
            return value;
        return 0f;
    }

    // 스탯 최댓값 설정 함수
    public void SetMaxStat(MaxStatType type, float value)
    {
        maxStats[type] = value;
        StatBarChange?.Invoke();
    }

    // 스탯 최댓값 증가 함수
    public void AddMaxStat(MaxStatType type, float value)
    {
        if (!maxStats.ContainsKey(type))
            maxStats[type]= 0f;

        maxStats[type] += value;
        StatBarChange?.Invoke();
    }

    // 데미지 처리 함수
    public void TakeDamage(float damage, bool text)
    {
        if (isDead)
            return;

        float def = GetStat(StatType.Def);
        float finalDamage = Mathf.Max(1f, damage - def);

        stats[StatType.Hp] -= finalDamage;
        StatBarChange?.Invoke();

        if (text)
        {
            Vector3 offset = new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(3f, 6f), 0f);

            Vector3 textPositon = transform.position + offset;
            Shared.damageTextManager.ShowDamage(damage, textPositon);
        }    

        if (stats[StatType.Hp] <= 0)
        {
            stats[StatType.Hp] = 0;
            isDead = true;
        }
    }

    // 회복 함수
    public void Heal(float amount)
    {
        if (isDead) 
            return;

        float currentHp = GetStat(StatType.Hp);
        float maxHp = GetMaxStat(MaxStatType.MaxHp);

        stats[StatType.Hp] = Mathf.Min(currentHp + amount, maxHp);
    }

    // 부활 함수
    public void Revive(float hpPercent = 0.7f)
    {
        hpPercent = Mathf.Clamp01(hpPercent);

        float maxHp = GetMaxStat(MaxStatType.MaxHp);
        stats[StatType.Hp] = maxHp * hpPercent;

        isDead = false;
        deadHandled = false;
    }

    // 생존 여부 반환 함수
    public bool IsAlive()
    {
        return !isDead; 
    }

    // 체력 비율 반환 함수 [보스 페이즈 패턴 활용]
    public float GetHpPercent()
    {
        float maxHp = GetMaxStat(MaxStatType.MaxHp);
        if (maxHp <= 0f)
            return 0f;

        return GetStat(StatType.Hp) / maxHp;
    }
}
