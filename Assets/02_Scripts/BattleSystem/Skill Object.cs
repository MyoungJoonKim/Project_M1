using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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

    private Dictionary<Monster, float> lastHitTimes = new Dictionary<Monster, float>();


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
        if (!canAttack) 
            return;

        Monster monster = collision.GetComponent<Monster>();

        if (monster == null || monster.isDead)
            return;

        if (!lastHitTimes.ContainsKey(monster))
            lastHitTimes[monster] = -999f;

        if (Time.time >= lastHitTimes[monster] + hitInterval)
        {
            monster.TakeDamage(damage, true);
            monster.OnHit();

            lastHitTimes[monster] = Time.time;
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
        transform.position = player.position;

        Monster target = Shared.skill_Manager.GetRandomMonster();

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

    
}
