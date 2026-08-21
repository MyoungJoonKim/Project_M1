using UnityEngine;

public class Prop : Character
{
    [Header("Prop Data")]
    [SerializeField] private PropData propData;
    [SerializeField] private Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();

        if (propData != null)
            Init(propData);
    }
    public void Init(PropData data)
    {
        if (data == null)
            return;

        propData = data;
        characterName = data.propName;
        propData.propType = data.propType;

        InitStats(
            data.maxHp,
            0f,
            1f,
            0f,
            0f,
            0f,
            1f,
            0f,
            0f
        );
    }

    public bool GetPropType()
    {
        if (propData == null)
            return false;

        return propData.propType == PropType.Attackable;
    }

}
