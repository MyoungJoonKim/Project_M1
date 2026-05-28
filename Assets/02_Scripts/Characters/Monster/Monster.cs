using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


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


public class Monster : Character
{
    [Header("Monster Data")]
    [SerializeField] private MonsterData monsterData;

    [Header("Monster Reward")]
    [SerializeField] private float rewardExp = 10f;

    public Transform target;
    private Player player;
    private MonsterAi monsterAi;
    private MonsterAnimator monsterAnimator;


    private IObjectPool<Monster> monsterPool;

    private void Awake()
    {
        monsterAi = gameObject.GetComponent<MonsterAi>();
        monsterAnimator = gameObject.GetComponent<MonsterAnimator>();

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

    private void OnEnable()
    {
        if (Shared.spawnManager != null)
        {
            Shared.spawnManager.RegisterMonster(this);
        }
    }

    private void OnDisable()
    {
        if (Shared.spawnManager != null)
        {
            Shared.spawnManager.UnRegisterMonster(this);
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

        if (monsterAi != null)
            monsterAi.ChangeState(MonsterState.Idle);
    }

    // 몬스터 사망 함수
    public void OnDead()
    {
        Debug.Log("몬스터 처치");

        if (player != null)
            Shared.expDropManager.SpawnExpGem(transform.position, GetRewardExp());
        
        ReleaseMonster();
    }
    public void OnHit()
    {
        if (monsterAnimator != null)
            monsterAnimator.Hit();
    }

    public void StopMonster()
    {
        SetTarget(null);

        MonsterAi ai = GetComponent<MonsterAi>();
        if (ai != null)
            ai.ChangeState(MonsterState.Idle);

        MonsterAttack attack = GetComponent<MonsterAttack>();
        if (attack != null)
            attack.StopAttack();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (monsterAnimator != null)
            monsterAnimator.SetMove(false);
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
        Shared.battleManager.killRecord++;
    }
}
