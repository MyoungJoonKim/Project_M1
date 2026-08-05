using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossSkillObject : MonoBehaviour
{
    [SerializeField] private Transform boss;
    [SerializeField] private Transform effectRoot;
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

    private bool canAttack = true;
    private bool isTrigger = false;
    private Coroutine skillCoroutine;

    private Dictionary<Player, float> playerLastHitTimes = new Dictionary<Player, float>();

    private Player player;

    

    public void SetUp(
        Transform bossTransform,
        int objectIndex,
        int objectCount,
        float damageValue,
        float rangeValue,
        float speedValue,
        float hitIntervalValue,
        SkillType type)
    {
        boss = bossTransform;
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
            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
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
        if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            return;

        if (!canAttack)
            return;

        player = collision.GetComponentInParent<Player>();

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


    private void RotationSkill()
    {
        angle += speed * Time.deltaTime;
        float rotationRange = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rotationRange) * range;
        float y = Mathf.Sin(rotationRange) * range;

        transform.position = boss.position + new Vector3(x, y, 0f);
    }


    private void SummonSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;
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
        if (isTrigger)
            return;

        Transform target = player.transform;

        if (target == null)
        {
            SetEffectActive(false);
            return;
        }
        isTrigger = true;

        Vector3 startPosition = boss.position;
        transform.position = startPosition;

        Vector2 direction = (target.transform.position - startPosition).normalized;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, directionAngle);

        SetEffectActive(true);
        StartCoroutine(DirectionSkillEnd());
    }

    private void SetEffectActive(bool value)
    {
        if (effectObject != null)
            effectObject.SetActive(value);

        if (collider2D != null)
            collider2D.enabled = value;
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

    private IEnumerator DirectionSkillEnd()
    {
        yield return new WaitForSeconds(3);
        SetEffectActive(false);
        isTrigger = false;
    }

}
