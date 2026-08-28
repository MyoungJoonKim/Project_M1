using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DropManager : MonoBehaviour
{
    [Header("Dead Effect")]
    [SerializeField] private GameObject deadEffect;

    [Header("ExpGem Data")]
    [SerializeField] private ExpGemData[] expGemData;

    [Header("Pool Root")]
    [SerializeField] private Transform expGemRoot;

    [Header("Pool")]
    [SerializeField] private int startPoolSizePerType = 25;
    [SerializeField] private int maxActiveGemCount = 60;

    //private readonly Dictionary<int, Queue<ExpGem>> effectPool = new();
    private readonly Dictionary<int, Queue<ExpGem>> expPool = new();
    private readonly List<ExpGem> activeGems = new();


    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        if (expGemData == null)
            return;

        for (int i = 0; i < expGemData.Length; i++)
        {
            expPool[i] = new Queue<ExpGem>();

            for (int j = 0; j < startPoolSizePerType; j++)
            {
                ExpGem gem = Instantiate(expGemData[i].prefab, expGemRoot);
                gem.gameObject.SetActive(false);
                gem.SetManager(this);
                gem.SetPoolIndex(i);
                expPool[i].Enqueue(gem);
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
        if (expGemData == null || index < 0 || index >= expGemData.Length)
            return null;

        if (!expPool.ContainsKey(index))
            expPool[index] = new Queue<ExpGem>();

        if (expPool[index].Count > 0)
            return expPool[index].Dequeue();

        ExpGem gem = Instantiate(expGemData[index].prefab, expGemRoot);
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

        if (!expPool.ContainsKey(index))
            expPool[index] = new Queue<ExpGem>();

        expPool[index].Enqueue(gem);
    }

    private int GetExpGemIndex(float expAmount)
    {
        if (expGemData == null || expGemData.Length == 0)
            return -1;

        int index = 0;
        for (int i = 0; i < expGemData.Length; i++)
        {
           if (expAmount >= expGemData[i].minExp)
                index = i;
           else
                break;
        }
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
