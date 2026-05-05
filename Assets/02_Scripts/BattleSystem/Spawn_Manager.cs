using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawn_Manager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Player player;

    [Header("Rounds")]
    [SerializeField] private RoundData[] rounds;

    [Header("Wave Settings")]
    [SerializeField] private float waveSpawnDuration = 5f;
    [SerializeField] private float nextWaveDelay = 20f;
    [SerializeField] private float bossSpawnDelay = 30f;
    [SerializeField] private float nextRoundDelay = 60f;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int spawnCountPerTick = 5;

    [Header("Map Spawn Range")]
    [SerializeField] private float mapMinX;
    [SerializeField] private float mapMaxX;
    [SerializeField] private float mapMinY;
    [SerializeField] private float mapMaxY;
    [SerializeField] private float safeRadius = 5f;

    [Header("Pool")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private int maxPoolSize = 50;

    private int currentRoundIndex;
    private int currentWaveIndex;
    private int spawnIndex;

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
        StartCoroutine(RoundRoutine());
    }

    private void CreatePool()
    {
        if (rounds == null || rounds.Length == 0)
            return;

        foreach (RoundData round in rounds)
        {
            if (round == null || round.waves == null)
                continue;

            foreach (WaveData wave in round.waves)
            {
                if (wave == null)
                    continue;

                foreach (MonsterData data in wave.normalMonsters)
                {
                    CreatePoolMonster(data);
                }
                if (wave.spawnBoss)
                    CreatePoolMonster(wave.bossMonster);
            }
        }
    }

    private void CreatePoolMonster(MonsterData data)
    {
        if (data == null || data.prefab == null)
            return;

        if (string.IsNullOrEmpty(data.monsterID))
        {
            Debug.LogWarning("MonsterList id가 비어있음");
            return;
        }

        if (pool.ContainsKey(data.monsterID))
            return;

        string key = data.monsterID;

        pool[key] = new ObjectPool<Monster>(
            () => CreateMonster(data),
            OnGetMonster,
            OnReleaseMonster,
            OnDestroyMonster,
            true,
            poolSize,
            maxPoolSize
        );
    }

    IEnumerator RoundRoutine()
    {
        currentRoundIndex = 0;

        while (true)
        {
            if (rounds == null || rounds.Length == 0)
                yield break;

            RoundData round = rounds[currentRoundIndex];
            Debug.Log($"{round.roundNumber} 라운드 시작");
            yield return StartCoroutine(PlayRound(round));

            Debug.Log($"{round.roundNumber} 라운드 종료");
            yield return new WaitForSeconds(nextRoundDelay);

            currentRoundIndex++;

            if (currentRoundIndex >= rounds.Length)
                currentRoundIndex = 0;
        }
    }

    private IEnumerator PlayRound(RoundData round)
    {
        if (round == null || round.waves == null)
            yield break;
        
        for (currentWaveIndex = 0; currentWaveIndex < round.waves.Count; currentWaveIndex++)
        {
            WaveData wave = round.waves[currentWaveIndex];

            Debug.Log($"{round.roundNumber} 라운드 / {wave.waveNumber} 웨이브 시작");
            yield return StartCoroutine(PlayWave(wave));

            bool isLastWave = currentWaveIndex == round.waves.Count - 1;

            if (isLastWave && wave.spawnBoss && wave.bossMonster != null)
            {
                yield return new WaitForSeconds(bossSpawnDelay);
                SpawnMonster(wave.bossMonster);
                Debug.Log($"{wave.bossMonster.monsterName} 보스 소환");
            }
            else
                yield return new WaitForSeconds(nextWaveDelay);
        }
    }


    private IEnumerator PlayWave(WaveData wave)
    {
        spawnIndex = 0;
        float timer = 0f;

        while (timer < waveSpawnDuration)
        {
            for (int i = 0; i < spawnCountPerTick; i++)
            {
                MonsterData data = GetNextMonster(wave);
                SpawnMonster(data);
            }
            yield return new WaitForSeconds(spawnInterval);
            timer += spawnInterval;
        }
    }    

    private MonsterData GetNextMonster(WaveData wave)
    {
        if (wave == null || wave.normalMonsters == null || wave.normalMonsters.Count == 0)
            return null;

        MonsterData data = wave.normalMonsters[spawnIndex];
        spawnIndex++;

        if (spawnIndex >= wave.normalMonsters.Count)
            spawnIndex = 0;

        return data;
    }
    public void SpawnMonster(MonsterData data)
    {
        if (player == null)
            return;

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

    private Monster CreateMonster(MonsterData data)
    {
        Monster monster = Instantiate(data.prefab);
        monster.name = $"{data.monsterID}_Pooled";
        monster.SetManagedPool(pool[data.monsterID]);
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
