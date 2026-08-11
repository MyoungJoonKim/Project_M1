using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDropManager : MonoBehaviour
{
    [Header("Exp Gem Prefab")]
    public ExpGem[] expGemPrefab;

    [Header("Pool")]
    [SerializeField] private int startPoolSizePerType = 25;
    [SerializeField] private int maxActiveGemCount = 200;

    private readonly Dictionary<int, Queue<ExpGem>> pool = new();
    private readonly List<ExpGem> activeGems = new();


    private void Awake()
    {
        Shared.expDropManager = this;
        CreatePool();
    }

    private void OnDestroy()
    {
        if (Shared.expDropManager == this)
            Shared.expDropManager = null;
    }


    private void CreatePool()
    {
        if (expGemPrefab == null)
            return;

        for (int i = 0; i < expGemPrefab.Length; i++)
        {
            pool[i] = new Queue<ExpGem>();

            for (int j = 0; j < startPoolSizePerType; j++)
            {
                ExpGem gem = Instantiate(expGemPrefab[i], transform);
                gem.gameObject.SetActive(false);
                gem.SetManager(this);
                gem.SetPoolIndex(i);
                pool[i].Enqueue(gem);
            }
        }
    }

    public void SpawnExpGem(Vector3 position, float expAmount)
    {
        int index = GetExpGemIndex(expAmount);

        if (index < 0)
            return;

        // 맵에 경험치잼이 너무 많이 쌓이면 오래된 잼부터 회수
        if (activeGems.Count >= maxActiveGemCount)
            Release(activeGems[0]);

        ExpGem gem = GetGem(index);

        if (gem == null)
            return;

        gem.transform.position = position;
        gem.transform.rotation = Quaternion.identity;
        gem.Init(expAmount);

        gem.gameObject.SetActive(true);
        activeGems.Add(gem);
    }

    private ExpGem GetGem(int index)
    {
        if (expGemPrefab == null || index < 0 || index >= expGemPrefab.Length)
            return null;

        if (!pool.ContainsKey(index))
            pool[index] = new Queue<ExpGem>();

        if (pool[index].Count > 0)
            return pool[index].Dequeue();

        ExpGem gem = Instantiate(expGemPrefab[index], transform);
        gem.gameObject.SetActive(false);
        gem.SetManager(this);
        gem.SetPoolIndex(index);
        return gem;
    }

    public void Release(ExpGem gem)
    {
        if (gem == null)
            return;

        activeGems.Remove(gem);

        int index = gem.GetPoolIndex();

        gem.gameObject.SetActive(false);

        if (!pool.ContainsKey(index))
            pool[index] = new Queue<ExpGem>();

        pool[index].Enqueue(gem);
    }

    private int GetExpGemIndex(float expAmount)
    {
        if (expGemPrefab == null || expGemPrefab.Length == 0)
            return -1;

        int index = Mathf.FloorToInt((expAmount - 1f) / 50f);

        if (index < 0)
            index = 0;

        if (index >= expGemPrefab.Length)
            index = expGemPrefab.Length - 1;

        return index;
    }

    public void ClearAll()
    {
        for (int i = activeGems.Count - 1; i >= 0; i--)
        {
            Release(activeGems[i]);
        }
    }




}
