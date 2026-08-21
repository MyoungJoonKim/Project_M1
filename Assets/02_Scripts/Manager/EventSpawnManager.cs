using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject[] eventMonsterPrefabs;

    [Header("Monster Data")]
    [SerializeField] private MonsterData[] eventMonsterDatas;

    [Header("Spawn Position")]
    [SerializeField] private float leftX = 471f;
    [SerializeField] private float rightX = 609f;
    [SerializeField] private float startY = 1025f;
    [SerializeField] private float endY = 890f;

    [Header("Settings")]
    [SerializeField] private int monsterCount = 28;

    private GameObject currentMonsterPrefab;
    private MonsterData currentMonsterData;
    private List<GameObject> monsters = new List<GameObject>();
    private Coroutine moveCoroutine;

    private void CreatePool()
    {
        if (currentMonsterPrefab ==  null) 
            return;

        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(currentMonsterPrefab, transform);
            monster.SetActive(false);
            monsters.Add(monster);
        }
    }

    private void ClearPool()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] != null)
                Destroy(monsters[i]);
        }
        monsters.Clear();
    }

    public void SpawnEventWave(int roundIndex)
    {
        if (Shared.battleManager == null || !Shared.battleManager.isBattlePlaying)
            return;

        if (eventMonsterPrefabs == null|| eventMonsterDatas == null)
            return;

        if (roundIndex < 0)
            return;

        currentMonsterPrefab = eventMonsterPrefabs[roundIndex];
        currentMonsterData = eventMonsterDatas[roundIndex];

        if (currentMonsterPrefab == null|| currentMonsterData == null) 
            return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        ClearPool();
        CreatePool();

        EventMonsterAttack.ResetAttackTime();

        if (monsters.Count == 0)
            return;
            
        SetMonsterPosition();
        SetMonsterActive(true);
        SetMonsterData();

        moveCoroutine = StartCoroutine(MoveWave());
    }
    
    private void SetMonsterPosition()
    {
        float spacing = 0f;

        if (monsterCount > 1)
            spacing = (rightX - leftX) / (monsterCount - 1);

        for (int i = 0;i < monsterCount; i++)
        {
            if (monsters[i] == null)
                continue;

            float x = leftX + spacing * i;
            monsters[i].transform.position = new Vector3(x, startY, 0f);
            monsters[i].transform.rotation = Quaternion.identity;
        }
    }
    private void SetMonsterData()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
                continue;

            Monster monster = monsters[i].GetComponent<Monster>();

            if (monster != null)
            {
                monster.SetMonsterData(currentMonsterData);
                monster.ResetMonster(false);
            }

            MonsterAi monsterAi = monsters[i].GetComponent<MonsterAi>();

            if (monsterAi != null)
                monsterAi.enabled = false;
        }
    }

    private void SetMonsterActive(bool value)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] != null)
                monsters[i].SetActive(value);
        }
    }

    private IEnumerator MoveWave()
    {
        while (true)
        {
            for (int i = 0; i < monsters.Count; i++)
            { 
                if (monsters[i] == null)
                    continue;
                monsters[i].transform.position += Vector3.down * currentMonsterData.moveSpeed * Time.deltaTime;
            }

            if (monsters.Count > 0 && monsters[0].transform.position.y <= endY)
                break;

            yield return null;
        }

        SetMonsterActive(false);
        moveCoroutine = null;
    }
}
