using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform effectRoot;
    [SerializeField] private GameObject effectObject;
    [SerializeField] private Collider2D collider2D;
    private SkillType skillType;

    private int index;
    private int totalCount;

    private float damage;
    private float range;
    private float radius;
    private float speed;
    private float hitInterval;

    private float angle;

    private bool canAttack = true;
    private bool isTrigger = false;

    private Dictionary<Monster, float> monsterLastHitTimes = new Dictionary<Monster, float>();
    private Dictionary<Prop, float> propLastHitTimes = new Dictionary<Prop, float>();

    public void SetUp(
        Transform playerTarget,
        int objectIndex,
        int objectCount,
        float damageValue,
        float rangeValue,
        float radiusValue,
        float speedValue,
        float hitIntervalValue,
        SkillType type)
    {
        player = playerTarget;
        index = objectIndex;
        totalCount = objectCount;
        damage = damageValue;
        range = rangeValue;
        radius = radiusValue;
        speed = speedValue;
        hitInterval = hitIntervalValue;
        skillType = type;

        SetEffectSize(radius);
        angle = (360f / totalCount) * index;
    }

    public void SetAttack(bool value)
    {
        canAttack = value;
    }

    private void Update()
    {
        if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            return;

        UpdateSkillType();
    }

    private void UpdateSkillType()
    {
        if (player == null)
            return;

        switch (skillType)
        {
            case SkillType.Rotation:
                RotationSkill();
                break;
            case SkillType.Area:
                AreaSkill();
                break;
            case SkillType.Summon:
                SummonSkill();
                break;
            case SkillType.TargetExplosion:
                TargetExplosionSkill();
                break;
            case SkillType.Direction:
                DirectionSkill();
                break;
            case SkillType.Projection:
                ProjectionSkill();
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            return;

        if (!canAttack) 
            return;

        Monster monster = collision.GetComponentInParent<Monster>();
        Pillar pillar = collision.GetComponentInParent<Pillar>();
        Prop prop = collision.GetComponentInParent<Prop>();

        if (monster != null && !monster.isDead)
        {
            if (!monsterLastHitTimes.ContainsKey(monster))
                monsterLastHitTimes[monster] = -999f;

            if (Time.time >= monsterLastHitTimes[monster] + hitInterval)
            {
                monster.TakeDamage(damage, true);
                monster.OnHit();

                monsterLastHitTimes[monster] = Time.time;
            }
            return;
        }

        if (pillar != null && pillar.GetPropType())
        {
            if (!propLastHitTimes.ContainsKey(pillar))
                propLastHitTimes[pillar] = -999f;

            if (Time.time >= propLastHitTimes[pillar] + hitInterval)
            {
                pillar.TakeDamage(damage, true);

                propLastHitTimes[pillar] = Time.time;
            }
            return;
        }

        if (prop != null && prop.GetPropType())
        {
            if (!propLastHitTimes.ContainsKey(prop))
                propLastHitTimes[prop] = -999f;

            if (Time.time >= propLastHitTimes[prop] + hitInterval)
            {
                prop.TakeDamage(damage, true);
                Debug.Log("prop attack");

                propLastHitTimes[prop] = Time.time;
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

        transform.position = player.position + new Vector3(x, y, 0f);
    }

    private void AreaSkill()
    {
        transform.position = player.position;
    }

    private void SummonSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;
    }

    private void TargetExplosionSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;
    }

    private void DirectionSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;

        transform.position = player.position;

        Transform target = GetDirectionTarget();

        if (target == null)
        {
            SetEffectActive(false);
            return;
        }
        SetEffectActive(true);

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, directionAngle - 90f);
    }

    private void ProjectionSkill()
    {

    }

    private Transform GetDirectionTarget()
    {
        Monster monster = Shared.skillManager.GetRandomMonster();

        if (monster != null)
            return monster.transform;

        Pillar pillar = Shared.eventManager.CurrentActivePillar;

        if (pillar != null && pillar.CurrentState == PillarState.RuneActive)
            return pillar.transform;

        return null;
    }

    private void SetEffectActive(bool value)
    {
        if (effectObject != null)
            effectObject.SetActive(value);

        if (collider2D != null)
            collider2D.enabled = value;
    }

    private void SetEffectSize(float size)
    {
        effectRoot.localScale = new Vector3(size, size, size);
        effectObject.transform.localScale = effectRoot.localScale;
        
        if (collider2D is CircleCollider2D circle)
        {
            circle.radius = effectObject.transform.localScale.x;
        }
        else if (collider2D is CapsuleCollider2D capsule)
        {
            capsule.size = new Vector2(effectObject.transform.localScale.x / 2, effectObject.transform.localScale.y + 0.5f);
        }
    }

    public void StopSkill()
    {
        canAttack = false;
        isTrigger = false;
        monsterLastHitTimes.Clear();
        propLastHitTimes.Clear();

        if (collider2D != null)
            collider2D.enabled = false;

        if (effectObject != null)
            effectObject.SetActive(false);

        Destroy(gameObject);
    }

}
