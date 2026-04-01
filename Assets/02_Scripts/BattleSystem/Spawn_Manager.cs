using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawn_Manager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Player player;

    [Header("Monster List")]
    [SerializeField] private MonsterList[] monsterList;

    [Header("Map Spawn Range")]
    [SerializeField] float mapMinX;
    [SerializeField] float mapMaxX; 
    [SerializeField] float mapMinY; 
    [SerializeField] float mapMaxY;
    [SerializeField] float safeRadius = 5f;

    [Header("Pool")]
    [SerializeField] int poolSize = 10;
    [SerializeField] int maxPoolSize = 50;

    
    private readonly Dictionary<string, IObjectPool<Monster>> pool = new();

    void Start()
    {
        CreatePool();
        StartCoroutine(SpawnRoutine());
    }

    private void CreatePool()
    {
        if (monsterList == null || monsterList.Length == 0)
            return;

        foreach (var list in monsterList)
        {
            if (list == null || list.prefab == null || list.data == null) 
                continue;

            string key = list.id;
            Monster prefab = list.prefab;

            pool[key] = new ObjectPool<Monster>(
            () => CreateMonster(prefab, key),
            OnGetMonster,
            OnReleaseMonster,
            OnDestroyMonster,
            true,
            poolSize,
            maxPoolSize
            );
        }
    }

    IEnumerator SpawnRoutine()
    {
        float waveTime = 10f;
        float timer = 0f;

        while (timer < waveTime)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnMonster();
            }
            yield return new WaitForSeconds(1f);
            timer += 1;
        }
    }

    public void SpawnMonster()
    {
        if (player == null)
            return;

        if (monsterList == null || monsterList.Length == 0)
            return;

        MonsterList list = monsterList[0];

        if (list == null || list.prefab == null || list.data == null)
            return;

        if (!pool.ContainsKey(list.id))
            return;

        Monster monster = pool[list.id].Get();

        monster.transform.position = GetRandomPosition();
        monster.transform.rotation = Quaternion.identity;

        monster.SetMonsterData(list.data);
        monster.SetTarget(player.transform);
        monster.SetPlayer(player);
        monster.ResetMonster();
    }


    Vector2 GetRandomPosition()
    {
        Vector2 pos;
        int count = 0;

        do
        {
            float x = Random.Range(mapMinX, mapMaxX);
            float y = Random.Range(mapMinY, mapMaxY);
            pos = new Vector2(x, y);

            count++;
            if (count > 30)
                break;
        }
        while (Vector2.Distance(pos, player.transform.position) < safeRadius);

        return pos;
    }
    private Monster CreateMonster(Monster prefab, string key)
    {
        Monster monster = Instantiate(prefab);
        monster.name = $"{key}_Pooled";
        monster.SetManagedPool(pool[key]);
        monster.gameObject.SetActive(false);
        return monster;
    }

    private void OnGetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(Monster monster)
    {
        Destroy(monster.gameObject);
    }
}
