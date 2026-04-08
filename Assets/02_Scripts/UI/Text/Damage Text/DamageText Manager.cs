using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;

    private void Awake()
    {
        if (Shared.damageTextManager == null)
        {
            Shared.damageTextManager = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        DamageText text = Instantiate(damageTextPrefab, position, Quaternion.identity);

        text.SetUp(damage);
    }
}
