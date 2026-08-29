using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillObject : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform boss;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Transform effectRoot;

    [Header("Skill Object")]
    [SerializeField] private GameObject effectObject;
    [SerializeField] private Collider2D collider2D;

    private SkillType skillType;

    private int index;
    private int totalCount;

    private float damage;
    private float range;
    private float speed;
    private float hitInterval;

    private float angle;

    private Vector2 moveDirection;

    private bool canAttack = true;
    private bool isTrigger = false;
    private Coroutine skillCoroutine;

    private Dictionary<Player, float> playerLastHitTimes = new Dictionary<Player, float>();


    public void SetUp(
        Transform bossTransform,
        Transform playerTransform,
        BattleManager _battleManager,
        int objectIndex,
        int objectCount,
        float damageValue,
        float rangeValue,
        float speedValue,
        float hitIntervalValue,
        SkillType type)
    {
        boss = bossTransform;
        targetPlayer = playerTransform;
        BattleManager.Instance = _battleManager;
        index = objectIndex;
        totalCount = objectCount;
        damage = damageValue;
        range = rangeValue;
        speed = speedValue;
        hitInterval = hitIntervalValue;
        skillType = type;

        angle = (360f / totalCount) * index;

        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }
        skillCoroutine = StartCoroutine(SkillTypeCoroutine());


    }

    private IEnumerator SkillTypeCoroutine()
    {
        while (true)
        {
            if (BattleManager.Instance == null || !BattleManager.Instance.isBattlePlaying)
            {
                yield return null;
                continue;
            }
            UpdateSkillType();
            yield return null;
        }
    }

    public void SetAttack(bool value)
    {
        canAttack = value;
    }


    private void UpdateSkillType()
    {
        if (boss == null)
            return;

        switch (skillType)
        {
            case SkillType.Projection:
                ProjectionSkill();
                break;
            case SkillType.Summon:
                SummonSkill();
                break;
            case SkillType.TargetExplosion:
                TargetExplosionSkill();
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (BattleManager.Instance == null || !BattleManager.Instance.isBattlePlaying)
            return;

        if (!canAttack)
            return;

        Player player = collision.GetComponentInParent<Player>();

        if (player != null && !player.isDead)
        {
            if (!playerLastHitTimes.ContainsKey(player))
                playerLastHitTimes[player] = -999f;

            if (Time.time >= playerLastHitTimes[player] + hitInterval)
            {
                player.TakeDamage(damage, true);
                player.OnHit();

                playerLastHitTimes[player] = Time.time;
            }
            return;
        }
    }

    private void SummonSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;

        float radius = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(radius) * range;
        float y = Mathf.Sin(radius) * range;

        transform.position = boss.position + new Vector3(x, y, 0f);

        SetEffectActive(true);
    }

    private void TargetExplosionSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;
        SetEffectActive(true);
    }

    private void ProjectionSkill()
    {
        if (targetPlayer == null)
        {
            SetEffectActive(false);
            return;
        }
        if (!isTrigger)
        {
            isTrigger = true;

            Vector3 startPosition = boss.position;
            transform.position = startPosition;

            moveDirection = (targetPlayer.position - startPosition).normalized;
            float directionAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, directionAngle);
            
            SetEffectActive(true);
        }
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    private void SetEffectActive(bool value)
    {
        if (effectObject != null)
            effectObject.SetActive(value);

        if (skillType == SkillType.TargetExplosion)
        {
            if (collider2D != null)
                StartCoroutine(TargetExplosionColliderTimer(value));
        }
        else
        {
            if (collider2D != null)
                collider2D.enabled = value;
        }
    }

    public void StopSkill()
    {
        canAttack = false;
        isTrigger = false;
        playerLastHitTimes.Clear();

        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }

        if (collider2D != null)
            collider2D.enabled = false;

        if (effectObject != null)
            effectObject.SetActive(false);
    }

    private IEnumerator TargetExplosionColliderTimer(bool value)
    {
        yield return new WaitForSeconds(0.9f);
        collider2D.enabled = value;
    }

}
