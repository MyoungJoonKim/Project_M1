using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Player player;

    [Header("Rounds")]
    [SerializeField] private RoundData[] rounds;

    [Header("Wave Settings")]
    [SerializeField] private float waveSpawnDuration = 6f;
    [SerializeField] private float nextWaveDelay = 10f;
    [SerializeField] private float bossSpawnDelay = 20f;
    [SerializeField] private float nextRoundDelay = 30f;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int spawnCountPerTick = 6;

    [Header("Map Spawn Range")]
    [SerializeField] private float mapMinX;
    [SerializeField] private float mapMaxX;
    [SerializeField] private float mapMinY;
    [SerializeField] private float mapMaxY;
    [SerializeField] private float safeRadius = 5f;

    [Header("Pool")]
    [SerializeField] private int poolSize = 5;
    [SerializeField] private int maxPoolSize = 30;

    [Header("Limit")]
    [SerializeField] private int maxActiveMonsterCount = 80;

    private int currentRoundIndex;
    private int currentWaveIndex;
    private int spawnIndex;
    private Coroutine roundCoroutine;

    public int CurrentWaveIndex => currentWaveIndex;

    private readonly List<Monster> activeMonsters = new();
    private readonly Dictionary<string, IObjectPool<Monster>> pool = new();

    // 몬스터 ID별 현재 살아있는 수
    private readonly Dictionary<string, int> activeCountByMonsterID = new();

    // 웨이브가 끝나서 앞으로 더 이상 스폰하지 않을 몬스터 ID
    private readonly HashSet<string> spawnClosedMonsterIDs = new();

    // 풀 제거 예약 중인 몬스터 ID. Release 도중 Clear 되는 문제 방지용
    private readonly HashSet<string> pendingRemoveMonsterIDs = new();

    private void Awake()
    {
        Shared.spawnManager = this;
    }

    private void Start()
    {
        // 전체 몬스터 풀을 미리 만들지 않는다.
        // 현재 웨이브가 시작될 때 해당 웨이브 몬스터 풀만 만든다.
        roundCoroutine = StartCoroutine(RoundRoutine());
    }

    private void OnDestroy()
    {
        if (Shared.spawnManager == this)
            Shared.spawnManager = null;
    }

    private IEnumerator RoundRoutine()
    {
        currentRoundIndex = 0;

        while (true)
        {
            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            if (rounds == null || rounds.Length == 0)
                yield break;

            RoundData round = rounds[currentRoundIndex];

            yield return StartCoroutine(PlayRound(round));

            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            {
                yield return null;
                continue;
            }

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
            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
                yield break;

            WaveData wave = round.waves[currentWaveIndex];

            yield return StartCoroutine(PlayWave(wave));

            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
                yield break;

            bool isLastWave = currentWaveIndex == round.waves.Count - 1;

            if (isLastWave && wave.spawnBoss && wave.bossMonster != null)
            {
                yield return new WaitForSeconds(bossSpawnDelay);

                if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
                    yield break;

                PrepareMonsterPool(wave.bossMonster);
                SpawnMonster(wave.bossMonster);

                // 보스도 다시 안 나오는 구조라면 스폰 종료 표시.
                CloseMonsterSpawn(wave.bossMonster);
            }
            else
            {
                yield return new WaitForSeconds(nextWaveDelay);
            }
        }
    }

    private IEnumerator PlayWave(WaveData wave)
    {
        if (wave == null)
            yield break;

        PrepareWavePool(wave);

        spawnIndex = 0;
        float timer = 0f;

        while (timer < waveSpawnDuration)
        {
            if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
                yield break;

            for (int i = 0; i < spawnCountPerTick; i++)
            {
                MonsterData data = GetNextMonster(wave);
                SpawnMonster(data);
            }

            yield return new WaitForSeconds(spawnInterval);
            timer += spawnInterval;
        }

        // 이 웨이브 몬스터는 앞으로 더 이상 생성하지 않음.
        // 단, 아직 살아있는 몬스터가 있으면 풀은 유지하고, 전부 죽으면 제거됨.
        CloseWaveSpawn(wave);
    }

    private void PrepareWavePool(WaveData wave)
    {
        if (wave == null || wave.normalMonsters == null)
            return;

        foreach (MonsterData data in wave.normalMonsters)
        {
            PrepareMonsterPool(data);
        }
    }

    private void PrepareMonsterPool(MonsterData data)
    {
        CreatePoolMonster(data);
    }

    private void CloseWaveSpawn(WaveData wave)
    {
        if (wave == null || wave.normalMonsters == null)
            return;

        foreach (MonsterData data in wave.normalMonsters)
        {
            CloseMonsterSpawn(data);
        }
    }

    private void CloseMonsterSpawn(MonsterData data)
    {
        if (data == null || string.IsNullOrEmpty(data.monsterID))
            return;

        spawnClosedMonsterIDs.Add(data.monsterID);
        TryRemoveUnusedPool(data.monsterID);
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
        if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            return;

        if (activeMonsters.Count >= maxActiveMonsterCount)
            return;

        if (player == null)
            return;

        if (data == null || data.prefab == null)
            return;

        if (string.IsNullOrEmpty(data.monsterID))
            return;

        if (!pool.ContainsKey(data.monsterID))
            CreatePoolMonster(data);

        if (!pool.ContainsKey(data.monsterID))
            return;

        Monster monster = pool[data.monsterID].Get();

        // OnEnable에서 RegisterMonster가 호출되므로,
        // 반드시 SetActive(true) 전에 데이터와 위치를 먼저 세팅한다.
        monster.transform.position = GetRandomPosition();
        monster.transform.rotation = Quaternion.identity;

        monster.SetMonsterData(data);
        monster.SetTarget(player.transform);
        monster.SetPlayer(player);
        monster.ResetMonster();

        monster.gameObject.SetActive(true);
    }

    private Vector2 GetRandomPosition()
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
        while (player != null && Vector2.Distance(pos, player.transform.position) < safeRadius);

        return pos;
    }

    private void CreatePoolMonster(MonsterData data)
    {
        if (data == null || data.prefab == null)
            return;

        if (string.IsNullOrEmpty(data.monsterID))
        {
            Debug.LogWarning("MonsterData의 monsterID가 비어있음");
            return;
        }

        string key = data.monsterID;

        if (pool.ContainsKey(key))
            return;

        IObjectPool<Monster> newPool = null;

        newPool = new ObjectPool<Monster>(
            () =>
            {
                Monster monster = Instantiate(data.prefab);
                monster.name = $"{data.monsterID}_Pooled";
                monster.SetManagedPool(newPool);
                monster.gameObject.SetActive(false);
                return monster;
            },
            OnGetMonster,
            OnReleaseMonster,
            OnDestroyMonster,
            true,
            poolSize,
            maxPoolSize
        );

        pool.Add(key, newPool);
    }

    public void RegisterMonster(Monster monster)
    {
        if (monster == null)
            return;

        if (!activeMonsters.Contains(monster))
            activeMonsters.Add(monster);

        MonsterData data = monster.GetMonsterData();

        if (data == null || string.IsNullOrEmpty(data.monsterID))
            return;

        string id = data.monsterID;

        if (!activeCountByMonsterID.ContainsKey(id))
            activeCountByMonsterID[id] = 0;

        activeCountByMonsterID[id]++;
    }

    public void UnRegisterMonster(Monster monster)
    {
        if (monster == null)
            return;

        if (activeMonsters.Contains(monster))
            activeMonsters.Remove(monster);

        MonsterData data = monster.GetMonsterData();

        if (data == null || string.IsNullOrEmpty(data.monsterID))
            return;

        string id = data.monsterID;

        if (activeCountByMonsterID.ContainsKey(id))
        {
            activeCountByMonsterID[id]--;

            if (activeCountByMonsterID[id] < 0)
                activeCountByMonsterID[id] = 0;
        }

        TryRemoveUnusedPool(id);
    }

    private void TryRemoveUnusedPool(string monsterID)
    {
        if (string.IsNullOrEmpty(monsterID))
            return;

        if (!spawnClosedMonsterIDs.Contains(monsterID))
            return;

        int activeCount = 0;

        if (activeCountByMonsterID.ContainsKey(monsterID))
            activeCount = activeCountByMonsterID[monsterID];

        if (activeCount > 0)
            return;

        if (pendingRemoveMonsterIDs.Contains(monsterID))
            return;

        pendingRemoveMonsterIDs.Add(monsterID);
        StartCoroutine(RemovePoolNextFrame(monsterID));
    }

    private IEnumerator RemovePoolNextFrame(string monsterID)
    {
        // ObjectPool.Release가 완전히 끝난 다음 프레임에 Clear 한다.
        yield return null;

        pendingRemoveMonsterIDs.Remove(monsterID);

        if (!spawnClosedMonsterIDs.Contains(monsterID))
            yield break;

        int activeCount = 0;

        if (activeCountByMonsterID.ContainsKey(monsterID))
            activeCount = activeCountByMonsterID[monsterID];

        if (activeCount > 0)
            yield break;

        if (pool.ContainsKey(monsterID))
        {
            pool[monsterID].Clear();
            pool.Remove(monsterID);
        }

        activeCountByMonsterID.Remove(monsterID);
        spawnClosedMonsterIDs.Remove(monsterID);

        Debug.Log($"{monsterID} 풀 제거 완료");
    }

    public void StopSpawn()
    {
        if (roundCoroutine != null)
        {
            StopCoroutine(roundCoroutine);
            roundCoroutine = null;
        }

        StopAllCoroutines();
    }

    public void ClearMonsterTargets()
    {
        for (int i = activeMonsters.Count - 1; i >= 0; i--)
        {
            Monster monster = activeMonsters[i];

            if (monster == null)
                continue;

            monster.StopMonster();
        }
    }

    public void ClearAllMonsters()
    {
        for (int i = activeMonsters.Count - 1; i >= 0; i--)
        {
            Monster monster = activeMonsters[i];

            if (monster == null)
                continue;

            monster.ReleaseMonster(false);
        }

        foreach (var pair in pool)
        {
            pair.Value.Clear();
        }

        pool.Clear();
        activeMonsters.Clear();
        activeCountByMonsterID.Clear();
        spawnClosedMonsterIDs.Clear();
        pendingRemoveMonsterIDs.Clear();
    }

    public List<Monster> GetActiveMonsters()
    {
        return activeMonsters;
    }

    public int CurrentRoundNumber
    {
        get
        {
            if (rounds == null || rounds.Length == 0)
                return 0;

            return rounds[currentRoundIndex].roundNumber;
        }
    }

    private void OnGetMonster(Monster monster)
    {
        // 여기서 SetActive(true) 하지 않는다.
        // SpawnMonster에서 데이터 세팅 후 SetActive(true) 한다.
    }

    private void OnReleaseMonster(Monster monster)
    {
        if (monster == null)
            return;

        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(Monster monster)
    {
        if (monster != null)
            Destroy(monster.gameObject);
    }
}
