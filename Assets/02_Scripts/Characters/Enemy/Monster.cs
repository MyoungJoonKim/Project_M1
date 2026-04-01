using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : Character
{
    [Header("Monster Data")]
    [SerializeField] private MonsterData monsterData;

    [Header("Monster Reward")]
    [SerializeField] private float rewardExp = 10f;

    public Transform target;
    private Player player;
    private Monster_Ai monster_Ai;
    private Monster_Attack monster_Attack;
    private Monster_Animator monster_Animator;


    private IObjectPool<Monster> monsterPool;

    private void Awake()
    {
        monster_Ai = gameObject.GetComponent<Monster_Ai>();
        monster_Attack = gameObject.GetComponent<Monster_Attack>();
        monster_Animator = gameObject.GetComponent<Monster_Animator>();

        if (monsterData != null )
        {
            ApplyMonsterData(monsterData);
        }
    }
    private void Update()
    {
        if (isDead && !deadHandled)
        {
            deadHandled = true;
            OnDead();
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void ResetMonster()
    {
        isDead = false;
        deadHandled = false;

        float maxHp = GetMaxStat(MaxStatType.MaxHp);
        SetStat(StatType.Hp, maxHp);

        if (monster_Ai != null)
            monster_Ai.ChangeState(MonsterState.Idle);
    }

    // 몬스터 사망 함수
    public void OnDead()
    {
        Debug.Log("몬스터 처치");

        if (player != null)
            player.AddExp(GetRewardExp());
        // 2. 드랍 경험치 아이콘 처리 
        // 3. 사망 이펙트 처리

        ReleaseMonster();
    }

    public void SetMonsterData(MonsterData data)
    {
        monsterData = data;
        ApplyMonsterData(monsterData);
    }

    public MonsterData GetMonsterData()
    {
        return monsterData;
    }

    public void ApplyMonsterData(MonsterData data)
    {
        if (data == null)
            return;

        monsterData = data;
        characterName = data.monsterName;
        rewardExp = data.rewardExp;

        InitStats(
            data.maxHp,
            data.atk,
            data.def,
            data.moveSpeed,
            data.attackRange,
            data.attackCooldown,
            1f,
            0f,
            0f
            );
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetRewardExp(float exp)
    {
        this.rewardExp = exp;
    }

    public float GetRewardExp()
    {
        return rewardExp;
    }

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        this.monsterPool = pool;
    }

    public void ReleaseMonster()
    {
        this.monsterPool.Release(this);
    }
}
