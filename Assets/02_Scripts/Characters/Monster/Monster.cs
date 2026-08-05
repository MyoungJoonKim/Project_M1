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
    private MonsterAttack monsterAttack;
    private Rigidbody2D rb;

    private IObjectPool<Monster> monsterPool;

    private void Awake()
    {
        monsterAi = GetComponent<MonsterAi>();
        monsterAnimator = GetComponent<MonsterAnimator>();
        monsterAttack = GetComponent<MonsterAttack>();
        rb = GetComponent<Rigidbody2D>();

        if (monsterData != null)
            ApplyMonsterData(monsterData);
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
            Shared.spawnManager.RegisterMonster(this);
    }

    private void OnDisable()
    {
        if (Shared.spawnManager != null)
            Shared.spawnManager.UnRegisterMonster(this);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        if (player == null)
            return;

        if (monsterData == null)
            return;

        if (monsterData.monsterType != MonsterType.Boss)
            return;

        BossSkill(monsterData.projectionSkill);
        BossSkill(monsterData.summonSkill);
        BossSkill(monsterData.targetExplosionSkill);
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
    private void BossSkill(SkillData skill)
    {
        if (skill == null)
            return;

        if (player == null) 
            return;

        Transform bossMonster = this.gameObject.transform;

        BossSkillManager[] managers = bossMonster.GetComponentsInChildren<BossSkillManager>(true);

        foreach (BossSkillManager manager in managers)
        {
            if (manager.Data == skill)
            {
                manager.Init(monsterData, skill, bossMonster, player.transform);
                return;
            }
        }

        GameObject obj = new GameObject(skill.skillName);
        obj.transform.parent = bossMonster;
        obj.transform.localPosition = Vector3.zero;

        BossSkillManager newSkill = obj.AddComponent<BossSkillManager>();
        newSkill.Init(monsterData, skill, bossMonster, player.transform);
    }

    public void ResetMonster()
    {
        isDead = false;
        deadHandled = false;

        float maxHp = GetMaxStat(MaxStatType.MaxHp);
        SetStat(StatType.Hp, maxHp);

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (monsterAttack != null)
            monsterAttack.StopAttack();

        if (monsterAnimator != null)
            monsterAnimator.SetMove(false);

        if (monsterAi != null)
            monsterAi.ResetAI();
    }

    public void OnDead()
    {
        Debug.Log("몬스터 처치");

        if (monsterAi != null)
            monsterAi.StopAI();

        if (player != null && Shared.expDropManager != null)
            Shared.expDropManager.SpawnExpGem(transform.position, GetRewardExp());

        ReleaseMonster(true);
    }

    public void OnHit()
    {
        if (monsterAnimator != null)
            monsterAnimator.Hit();
    }

    public void StopMonster()
    {
        SetTarget(null);

        if (monsterAi != null)
            monsterAi.StopAI();

        if (monsterAttack != null)
            monsterAttack.StopAttack();

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (monsterAnimator != null)
            monsterAnimator.SetMove(false);
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
        rewardExp = exp;
    }

    public float GetRewardExp()
    {
        return rewardExp;
    }

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        monsterPool = pool;
    }

    public void ReleaseMonster(bool addKillCount = true)
    {
        StopMonster();

        if (addKillCount && Shared.battleManager != null)
            Shared.battleManager.killRecord++;

        if (monsterPool != null)
            monsterPool.Release(this);
        else
            gameObject.SetActive(false);
    }
}
