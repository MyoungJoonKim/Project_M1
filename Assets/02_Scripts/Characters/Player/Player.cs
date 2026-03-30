using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Player Default Stats")]
    [SerializeField] private float startHp = 100f;
    [SerializeField] private float startAtk = 10f;
    [SerializeField] private float startDef = 5f;
    [SerializeField] private float startMoveSpeed = 10f;
    [SerializeField] private float startLevel = 1f;
    [SerializeField] private float startExp = 0f;
    [SerializeField] private float startMaxExp = 30f;

    private void Awake()
    {
        characterName = "Player";

        InitStats(
            startHp, 
            startAtk, 
            startDef, 
            startMoveSpeed, 
            startLevel, 
            startExp, 
            startMaxExp
            );
    }

    void Update()
    {
        if (isDead && !deadHandled)
        {
            deadHandled = true;
            OnDead();
        }
    }

    public void OnDead()
    {
        Debug.Log("플레이어 사망");
    }
}
