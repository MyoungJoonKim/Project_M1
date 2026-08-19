using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header("Pool")]
    [SerializeField] private int startPoolSize = 50;
    [SerializeField] private int maxPoolSize = 100;

    private readonly Queue<DamageText> pool = new();
    private readonly List<DamageText> activeTexts = new();

    private void Awake()
    {
        Shared.damageTextManager = this;
        CreatePool();
    }

    private void OnDestroy()
    {
        if (Shared.damageTextManager == this)
            Shared.damageTextManager = null;
    }

    private void CreatePool()
    {
        if (damageTextPrefab == null)
            return;

        for (int i = 0; i < startPoolSize; i++)
        {
            DamageText text = Instantiate(damageTextPrefab, transform);
            text.gameObject.SetActive(false);
            text.SetManager(this);
            pool.Enqueue(text);
        }
    }

    public void ShowDamage(float damage, Vector3 position)
    {
        DamageText text = GetText();

        if (text == null)
            return;

        text.transform.position = position;
        text.transform.rotation = Quaternion.identity;
        text.gameObject.SetActive(true);

        activeTexts.Add(text);
        text.SetUp(damage);
    }

    private DamageText GetText()
    {
        if (pool.Count > 0)
            return pool.Dequeue();

        int totalCount = pool.Count + activeTexts.Count;

        if (totalCount >= maxPoolSize)
            return null;

        DamageText text = Instantiate(damageTextPrefab, transform);
        text.gameObject.SetActive(false);
        text.SetManager(this);
        return text;
    }

    public void Release(DamageText text)
    {
        if (text == null)
            return;

        activeTexts.Remove(text);

        text.StopEffect();
        text.gameObject.SetActive(false);

        pool.Enqueue(text);
    }

    public void ClearAll()
    {
        for (int i = activeTexts.Count - 1; i >= 0; i--)
        {
            Release(activeTexts[i]);
        }
    }
}
