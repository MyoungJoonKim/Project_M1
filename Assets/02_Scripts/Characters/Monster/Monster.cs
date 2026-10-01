using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Manager")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private DropManager dropManager;


    private Player player;
    private MonsterAI monsterAI;
    private MonsterAttack monsterAttack;
    private MonsterAnimator monsterAnimator;

    private BossHealthUI bossSliderUI;

    private Rigidbody2D rigidbody2D;
    private Coroutine deathCheckCoroutine;

    private IObjectPool<Monster> monsterPool;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        monsterAI = GetComponent<MonsterAI>();
        monsterAttack = GetComponent<MonsterAttack>();
        monsterAnimator = GetComponent<MonsterAnimator>();

        if (monsterData != null)
            ApplyMonsterData(monsterData);

        deathCheckCoroutine = StartCoroutine(DeathCheckRoutine());
    }

    private IEnumerator DeathCheckRoutine()
    {
        while (true)
        {
            if (isDead && !deadHandled)
            {
                deadHandled = true;
                HandleDeath();

                deathCheckCoroutine = null;

                if (monsterData.monsterID == "B2")
                {
                    Debug.Log("보스처치");
                    player.HandleVictory();
                }
                yield break;
            }
            yield return null;
        }
    }

    private void OnEnable()
    {
        if (spawnManager != null)
            spawnManager.RegisterMonster(this);
    }

    private void OnDisable()
    {
        if (spawnManager != null)
            spawnManager.UnregisterMonster(this);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        if (player == null)
        {
            Debug.Log("Monster: player null");
            return;
        }

        if (monsterData == null)
        {
            Debug.Log("Monster: monsterData null");
            return;
        } 

        if (monsterData.monsterType != MonsterType.Boss)
            return;

        SetBossSkill(monsterData.summonSkill);
        SetBossSkill(monsterData.projectionSkill);
        SetBossSkill(monsterData.targetExplosionSkill);
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
        {
            Debug.Log("Monster: data null");
            return;
        }

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
    private void SetBossSkill(ActiveSkillData skill)
    {
        if (skill == null)
        {
            Debug.Log("Monster: skill null");
            return;
        }

        if (player == null)
        {
            Debug.Log("Monster: player null");
            return;
        }

        Transform bossMonster = this.gameObject.transform;

        BossSkillManager[] managers = bossMonster.GetComponentsInChildren<BossSkillManager>(true);

        foreach (BossSkillManager manager in managers)
        {
            if (manager.Data == skill)
            {
                manager.Init(monsterData, skill, bossMonster, player.transform, BattleManager.Instance);
                return;
            }
        }

        GameObject obj = new GameObject(skill.skillName);
        obj.transform.parent = bossMonster;
        obj.transform.localPosition = Vector3.zero;

        BossSkillManager newSkill = obj.AddComponent<BossSkillManager>();
        newSkill.Init(monsterData, skill, bossMonster, player.transform, BattleManager.Instance);
    }

    public void ResetMonster(bool useAI = true)
    {
        isDead = false;
        deadHandled = false;

        float maxHp = GetMaxStat(MaxStatType.MaxHp);
        SetStat(StatType.Hp, maxHp);

        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }

        if (monsterAttack != null)
            monsterAttack.StopAttack();

        if (monsterAnimator != null)
            monsterAnimator.SetMove(false);

        // 일반 몬스터만 AI 초기화 (이벤트몬스터만 제외)
        if (useAI && monsterAI != null)
            monsterAI.ResetAI();

        if (deathCheckCoroutine != null)
        {
            StopCoroutine(deathCheckCoroutine);
            deathCheckCoroutine = null;
        }

        deathCheckCoroutine = StartCoroutine(DeathCheckRoutine());
    }

    public void SetBossSliderUI(BossHealthUI sliderUI)
    {
        bossSliderUI = sliderUI;
    }

    public void HandleDeath()
    {
        SoundManager.Instance.PlayMonsterDead();

        if (monsterAI != null)
            monsterAI.StopAI();

        if (monsterData.monsterType == MonsterType.Boss)
        {
            BossSkillManager[] bossSkills = GetComponentsInChildren<BossSkillManager>(true);
            for (int i = 0; i < bossSkills.Length; i++)
            {
                bossSkills[i].StopAllSkills();
            }
            bossSliderUI.SetActiveBar(false);
        }

        if (player != null && dropManager != null)
        {
            dropManager.SpawnExpGem(transform.position, GetRewardExp());
            dropManager.SpawnDeathEffect(transform.position);
        }

        ReleaseMonster(true);
    }

    public void HandleHit()
    {
        if (monsterAnimator != null)
            monsterAnimator.Hit();
    }

    public void StopMonster()
    {
        SetTarget(null);

        if (monsterAI != null)
            monsterAI.StopAI();

        if (monsterAttack != null)
            monsterAttack.StopAttack();

        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
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

    public float GetRewardExp()
    {
        return rewardExp;
    }

    public void SetPool(IObjectPool<Monster> pool)
    {
        monsterPool = pool;
    }

    public void SetSpawnManager(SpawnManager manager)
    {
        spawnManager = manager;
    }

    public void ReleaseMonster(bool addKillCount = true)
    {
        StopMonster();

        if (addKillCount && BattleManager.Instance != null)
            BattleManager.Instance.killRecord++;

        if (monsterPool != null)
            monsterPool.Release(this);
        else
            gameObject.SetActive(false);
    }
}
