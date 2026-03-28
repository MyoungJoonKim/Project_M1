using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spanwer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject prefabs_Monster;
    [SerializeField] float mapMinX, mapMaxX, mapMinY, mapMaxY;
    [SerializeField] float safeRadius;
    [SerializeField] int maxPoolsize;
    
    private IObjectPool<Monster> _Pool;

    void Start()
    {
        _Pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, true, 10, maxPoolsize);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        prefabs_Monster.gameObject.SetActive(true);
        float waveTime = 10f;
        float timer = 0f;

        while (timer < waveTime)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 spawnPosition = GetRandomPosition();

                var monster = _Pool.Get();
                monster.transform.position = spawnPosition;
            }
            yield return new WaitForSeconds(1f);
            timer += 1;
        }
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
        while (Vector2.Distance(pos, player.position) < safeRadius);

        return pos;
    }
    private Monster CreateMonster()
    {
        Monster monster = Instantiate(prefabs_Monster).GetComponent<Monster>();
        monster.SetManagedPool(_Pool);
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
