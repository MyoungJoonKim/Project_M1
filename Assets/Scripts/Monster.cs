using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 1.5f;
    private Rigidbody2D rb;

    private IObjectPool<Monster> monsterPool;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        MonsterMove();
    }

    void MonsterMove()
    {
        if (target == null)
            return;
        Vector2 dir = (target.position - transform.position).normalized;
        rb.velocity = dir * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ãæµ¹");
        }
    }
    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        this.monsterPool = pool;
    }

    public void DestoryMonster()
    {
        this.monsterPool.Release(this);
    }
}
