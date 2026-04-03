using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    //public float EffectRange = 8f;

    public Transform target;
    public float radius = 7f;
    public float speed = 180f;
    private float angle;
    private float damage = 15f;



    private void Update()
    {
        //RotateEffect();
        RatationEffect();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();

        if (monster == null || monster.isDead)
            return;
        
        monster.TakeDamage(damage);
    }

    void RatationEffect()
    {
        if (target == null)
            return;

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float y = Mathf.Sin(rad) * radius;

        transform.position = target.position + new Vector3(x, y, 0f);
    }

    //void RotateEffect()
    //{
    //    Monster[] monsters = FindObjectsOfType<Monster>();

    //    Monster targetMonster = null;
    //    float targetDistance = Mathf.Infinity;

    //    transform.position = Shared.battle_Manager.player.transform.position;

    //    foreach (Monster monster in monsters)
    //    {
    //        if (monster == null) continue;
    //        if (monster.isDead) continue;

    //        float distance = Vector2.Distance(transform.position, monster.transform.position);

    //        if (distance <targetDistance && distance <= EffectRange)
    //        {
    //            targetDistance = distance;
    //            targetMonster = monster;    
    //        }
    //    }

    //    if (targetMonster == null)
    //        return;

    //    Vector2 dir = targetMonster.transform.position - transform.position;
    //    float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    //}

}
