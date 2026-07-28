using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Pool;

public class EventSpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject eventMonsterPrefab;

    [Header("Monster Data")]
    [SerializeField] private MonsterData eventMonsterData;

    [Header("Spawn Position")]
    [SerializeField] private float leftX = 471f;
    [SerializeField] private float rightX = 609f;
    [SerializeField] private float startY = 1025f;
    [SerializeField] private float endY = 890f;

    [Header("Settings")]
    [SerializeField] private int monsterCount = 28;

    private List<GameObject> monsters = new List<GameObject>();
    private Coroutine moveCoroutine;


    private void Awake()
    {
            
    }

    private void CreatePool()
    {
        if (eventMonsterPrefab ==  null) 
            return;

        for (int i = 0; i < monsterCount; i++)
        {
            GameObject monster = Instantiate(eventMonsterPrefab, transform);
            monster.SetActive(false);
            monsters.Add(monster);
        }
    }

    public void SpawnEventWave()
    {
        if (eventMonsterPrefab == null)
            return;

        if (eventMonsterData == null)
            return;

        if (monsters.Count == 0) 
            CreatePool();

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        SetMonsterPosition();
        SetMonsterData();
        SetMonsterActive(true);

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
                monster.SetMonsterData(eventMonsterData);
                monster.ResetMonster();
            }
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
                monsters[i].transform.position += Vector3.down * eventMonsterData.moveSpeed * Time.deltaTime;
            }

            if (monsters.Count > 0 && monsters[0].transform.position.y <= endY)
                break;

            yield return null;
        }

        SetMonsterActive(false);
        moveCoroutine = null;
    }
}
