using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Character
{
    [Header("Prop Data")]
    [SerializeField] private PropData propData;
    [SerializeField] private Rigidbody2D rigidbody;


    private void Awake()
    {
        propData = GetComponent<PropData>();
        rigidbody = GetComponent<Rigidbody2D>();

        if (propData != null)
            PropData(propData);
    }
    public void PropData(PropData data)
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

    public void OnBroken()
    {

    }

}
