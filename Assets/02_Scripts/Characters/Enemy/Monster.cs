using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : Character
{

    private void Update()
    {
        if (isDead && !deadHandled)
        {
            deadHandled = true;
            OnDead();
        }
    }
    public void OnDead()
    {
        Debug.Log("몬스터 처치");

        // TODO:
        // 1. 플레이어 경험치 지급
        // 2. 드랍 아이템 처리
        // 3. 사망 이펙트 처리

        ReleaseMonster();
    }
    private IObjectPool<Monster> monsterPool;

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        this.monsterPool = pool;
    }

    public void ReleaseMonster()
    {
        this.monsterPool.Release(this);
    }
}
