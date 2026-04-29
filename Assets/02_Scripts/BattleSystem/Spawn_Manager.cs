using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawn_Manager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Player player;

    [Header("Monster Data")]
    [SerializeField] private MonsterData[] monsterData;


    [Header("Map Spawn Range")]
    [SerializeField] float mapMinX;
    [SerializeField] float mapMaxX; 
    [SerializeField] float mapMinY; 
    [SerializeField] float mapMaxY;
    [SerializeField] float safeRadius = 5f;

    [Header("Pool")]
    [SerializeField] int poolSize = 10;
    [SerializeField] int maxPoolSize = 50;

    private readonly List<Monster> activeMonsters = new();
    private readonly Dictionary<string, IObjectPool<Monster>> pool = new();

    private void Awake()
    {
        if (Shared.spawn_Manager == null)
            Shared.spawn_Manager = this;
    }
    private void Start()
    {
        CreatePool();
        StartCoroutine(SpawnRoutine());
    }

    private void CreatePool()
    {
        if (monsterData == null || monsterData.Length == 0)
            return;

        foreach (var list in monsterData)
        {
            if (list == null)
                continue;

            if (string.IsNullOrEmpty(list.monsterID))
            {
                Debug.LogWarning("MonsterList id가 비어있음");
                continue;
            }

            if (list.prefab == null)
            {
                Debug.LogWarning($"{list.prefab} prefab이 없음");
                continue;
            }

            string key = list.monsterID;
            Monster prefab = list.prefab;

            if (pool.ContainsKey(key))
            {
                Debug.LogWarning($"중복된 몬스터 id: {key}");
                continue;
            }

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

        if (monsterData == null || monsterData.Length == 0)
            return;

        //MonsterData data = monsterData[Random.Range(0, monsterData.Length)];
        MonsterData data = monsterData[0];
        if (data == null || data.prefab == null)
            return;

        if (!pool.ContainsKey(data.monsterID))
            return;

        Monster monster = pool[data.monsterID].Get();

        monster.transform.position = GetRandomPosition();
        monster.transform.rotation = Quaternion.identity;

        monster.SetMonsterData(data);
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

    public void RegisterMonster(Monster monster)
    {
        if (monster == null)
            return;

        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);
    }

    public void UnRegisterMonster(Monster monster)
    {
        if (monster == null)
            return;

        if (activeMonsters.Contains(monster))
            activeMonsters.Remove(monster);
    }

    public List<Monster> GetActiveMonsters()
    {
        return activeMonsters;
    }

    public void ClearMonsterTargets()
    {
        foreach (var monster in activeMonsters)
        {
            if (monster == null)
                continue;

            monster.SetTarget(null);
        }
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
