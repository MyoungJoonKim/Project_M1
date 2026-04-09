using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText_Manager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;

    private void Awake()
    {
        if (Shared.damageText_Manager == null)
        {
            Shared.damageText_Manager = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        DamageText text = Instantiate(damageTextPrefab, position, Quaternion.identity);

        text.SetUp(damage);
    }
}
