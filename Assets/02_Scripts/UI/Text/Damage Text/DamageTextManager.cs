using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;

    private void Awake()
    {
        Shared.damageTextManager = this;
    }

    private void OnDestroy()
    {
        if (Shared.damageTextManager == this)
            Shared.damageTextManager = null;
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        DamageText text = Instantiate(damageTextPrefab, position, Quaternion.identity);

        text.SetUp(damage);
    }
}
