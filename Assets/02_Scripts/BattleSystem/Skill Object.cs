using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkillObject : MonoBehaviour
{
    [SerializeField] private Transform player;
    private SkillType skillType;

    private int index;
    private int totalCount;

    private float damage;
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
        float radiusValue,
        float speedValue,
        float hitIntervalValue,
        SkillType type)
    {
        player = playerTarget;
        index = objectIndex;
        totalCount = objectCount;
        damage = damageValue;
        radius = radiusValue;
        speed = speedValue;
        hitInterval = hitIntervalValue;
        skillType = type;

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
        float range = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(range) * radius;
        float y = Mathf.Sin(range) * radius;

        transform.position = player.position + new Vector3(x, y, 0f);
    }

    private void AreaSkill()
    {
        transform.position = player.position;
    }

    private void SummonSkill()
    {

    }

    private void TargetExplosionSkill()
    {
        if (isTrigger)
            return;
        isTrigger = true;

        //Destroy(gameObject, 1f);
    }

    private void DirectionSkill()
    {
        transform.position = player.position;
    }

    private void ProjectionSkill()
    {

    }

}
