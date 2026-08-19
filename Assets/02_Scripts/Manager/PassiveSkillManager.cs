using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillManager : MonoBehaviour
{
    [Header("Player Pickup Collider")]
    [SerializeField] private CircleCollider2D pickupCollider2D;

    private Dictionary<PassiveSkillData, int> passiveLevels = new Dictionary<PassiveSkillData, int>();

    public float ExpBonusRate { get; private set; } = 1f;
    public float SkillDamageRate { get; private set; } = 1f;
    public float DamageReductionRate { get; private set; } = 0f;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();

    }

    public void LevelUp(PassiveSkillData data)
    {
        if (data == null) 
            return;

        int level = GetLevel(data);

        if (level >= data.maxLevel)
            return;

        passiveLevels[data] = level + 1;

        ApplyPassive(data);
    }

    public int GetLevel(PassiveSkillData data)
    {
        if (passiveLevels.ContainsKey(data))
            return passiveLevels[data];

        return 0;
    }

    private void ApplyPassive(PassiveSkillData data)
    {
        PassiveType type = data.passiveType;

        int level = GetLevel(data);

        float value = data.valuePerLevel;

        switch (type)
        {
            case PassiveType.ExpBonus:
                ExpBonusRate = 1f + (value * level);
                break;
            case PassiveType.DamageBonus:
                SkillDamageRate = 1f + (value * level);
                break;
            case PassiveType.DamageReduction:
                DamageReductionRate = value * level;
                break;
            case PassiveType.PickupRange:
                PickupRangeRate(value, level);
                break;
            case PassiveType.MoveSpeed:
                MoveSpeedRate(value, level);
                break;
        }
    }

    private void MoveSpeedRate(float value, int level)
    {
        float rate = 1f + (value * level);

        player.stats[StatType.MoveSpeed] *= rate;
    }

    private void PickupRangeRate(float value, int level)
    {
        float rate = 1f + (value * level);

        pickupCollider2D.radius *= rate;
        Debug.Log($"기본 범위{value}, 범위증가 적용{pickupCollider2D.radius}");
    }

    public Dictionary<PassiveSkillData, int> GetPassiveSkills()
    {
        return passiveLevels;
    }
}
