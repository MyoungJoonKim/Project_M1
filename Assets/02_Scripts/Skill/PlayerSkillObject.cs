using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillObject : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform effectRoot;

    [Header("Skill Object")]
    [SerializeField] private GameObject effectObject;
    [SerializeField] private Collider2D collider2D;


    private SkillType skillType;
    private Transform target;

    private int objectIndex;
    private int objectCount;

    private float damage;
    private float range;
    private float radius;
    private float speed;
    private float hitInterval;

    private float angle;

    private bool canAttack = true;
    private bool isActivated = false;

    private Coroutine skillUpdateCoroutine;
    private SpawnManager spawnManager;
    private BattleManager battleManager;
    private PassiveSkillManager passiveSkillManager;

    private Dictionary<Prop, float> propLastHitTimes = new Dictionary<Prop, float>();
    private Dictionary<Monster, float> monsterLastHitTimes = new Dictionary<Monster, float>();

    public void SetUp(
        Transform playerTransform,
        Transform targetTransform,
        SpawnManager _spawnManager,
        BattleManager _battleManager,
        int _objectIndex,
        int _objectCount,
        float damageValue,
        float rangeValue,
        float radiusValue,
        float speedValue,
        float hitIntervalValue,
        SkillType type)
    {
        player = playerTransform;
        target = targetTransform;

        passiveSkillManager = player.GetComponentInParent<PassiveSkillManager>();

        spawnManager = _spawnManager;
        battleManager = _battleManager;
        objectIndex = _objectIndex;
        objectCount = _objectCount;
        damage = damageValue;
        range = rangeValue;
        radius = radiusValue;
        speed = speedValue;
        hitInterval = hitIntervalValue;
        skillType = type;

        SetEffectSize(radius);
        angle = (360f / objectCount) * objectIndex;

        if (skillUpdateCoroutine != null)
        {
            StopCoroutine(skillUpdateCoroutine);
            skillUpdateCoroutine = null;
        }
        skillUpdateCoroutine = StartCoroutine(SkillUpdateRoutine());
    }

    private IEnumerator SkillUpdateRoutine()
    {
        while (true)
        {
            if (battleManager == null || !battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }
            UpdateSkillBehavior();
            yield return null;
        }
    }

    public void SetAttackEnabled(bool value)
    {
        canAttack = value;
    }


    private void UpdateSkillBehavior()
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
            case SkillType.Projectile:
                ProjectileSkill();
                break;
            case SkillType.EventSummon:
                EventSummonSkill();
                break;
        }
    }
    private void SFXSkillType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Rotation:
                SoundManager.Instance.PlayElectricSkill(hitInterval);
                break;
            case SkillType.Area:
                SoundManager.Instance.PlaySunAreaSkill(hitInterval);
                break;
            case SkillType.Summon:
                SoundManager.Instance.PlayPoisonCloudSkill(hitInterval);
                break;
            case SkillType.TargetExplosion:
                SoundManager.Instance.PlayExplosionSkill();
                break;
            case SkillType.Direction:
                SoundManager.Instance.PlayCrystalWave();
                break;
            case SkillType.Projectile:
                break;
            case SkillType.EventSummon:
                SoundManager.Instance.PlayLightningStrikeSkill();
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (battleManager == null || !battleManager.isBattlePlaying)
            return;

        if (!canAttack) 
            return;

        float finalDamage = damage;

        if (passiveSkillManager != null)
        {
            finalDamage *= passiveSkillManager.SkillDamageRate;
        }

        Monster monster = collision.GetComponentInParent<Monster>();
        Pillar pillar = collision.GetComponentInParent<Pillar>();
        Prop prop = collision.GetComponentInParent<Prop>();

        if (monster != null && !monster.isDead)
        {
            if (!monsterLastHitTimes.ContainsKey(monster))
                monsterLastHitTimes[monster] = -999f;

            if (Time.time >= monsterLastHitTimes[monster] + hitInterval)
            {
                monster.TakeDamage(finalDamage, true);
                monster.HandleHit();
                SFXSkillType(this.skillType);

                monsterLastHitTimes[monster] = Time.time;
            }
            return;
        }

        if (pillar != null && pillar.GetPropType())
        {
            if (!pillar.CanTakeDamage())
                return;
            
            if (!propLastHitTimes.ContainsKey(pillar))
                propLastHitTimes[pillar] = -999f;

            if (Time.time >= propLastHitTimes[pillar] + hitInterval)
            {
                pillar.TakeDamage(finalDamage, true);
                SFXSkillType(this.skillType);

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
                prop.TakeDamage(finalDamage, true);
                SFXSkillType(this.skillType);

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
        if (isActivated)
            return;
        isActivated = true;
        SetEffectActive(true);
    }

    private void TargetExplosionSkill()
    {
        if (isActivated)
            return;
        isActivated = true;
        SetEffectActive(true);
    }

    private void DirectionSkill()
    {
        if (isActivated)
            return;

        if (target == null)
        {
            SetEffectActive(false);
            return;
        }
        isActivated = true;

        Vector3 startPosition = player.position;
        transform.position = startPosition;

        Vector2 direction = (target.transform.position - startPosition).normalized;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, directionAngle);

        SetEffectActive(true);
        StartCoroutine(DirectionSkillClear());
    }

    private void ProjectileSkill()
    {
    }

    private void EventSummonSkill()
    {
        if (isActivated)
            return;
        isActivated = true;
        SetEffectActive(true);
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
        if (effectRoot == null)
            return;

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
        isActivated = false;
        monsterLastHitTimes.Clear();
        propLastHitTimes.Clear();

        if (skillUpdateCoroutine != null)
        {
            StopCoroutine(skillUpdateCoroutine);
            skillUpdateCoroutine = null;
        }

        if (collider2D != null)
            collider2D.enabled = false;

        if (effectObject != null)
            effectObject.SetActive(false);
    }

    private IEnumerator DirectionSkillClear()
    {
        yield return new WaitForSeconds(2);
        SetEffectActive(false);
        isActivated = false;
    }

}
