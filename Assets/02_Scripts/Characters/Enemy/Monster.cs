using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    private IObjectPool<Monster> monsterPool;

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        this.monsterPool = pool;
    }

    public void DestoryMonster()
    {
        this.monsterPool.Release(this);
    }
}
