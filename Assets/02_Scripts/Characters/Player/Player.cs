using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Player Default Stats")]
    [SerializeField] private float startHp = 500f;
    [SerializeField] private float startAtk = 1f;
    [SerializeField] private float startDef = 1f;
    [SerializeField] private float startMoveSpeed = 10f;
    [SerializeField] private float startLevel = 1f;
    [SerializeField] private float startExp = 0f;
    [SerializeField] private float startMaxExp = 20f;

    [Header("Player prefab")]
    [SerializeField] private GameObject grave;
    [SerializeField] private GameObject skillRoot;

    [SerializeField] private GameObject joyStick; 

    private Player_Controller player_Controller;
    private Player_Animator player_Animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;


    private void Awake()
    {
        characterName = "Player";

        InitStats(
            startHp, 
            startAtk, 
            startDef, 
            startMoveSpeed, 
            0f,
            0f,
            startLevel, 
            startExp, 
            startMaxExp
            );

        player_Controller = GetComponent<Player_Controller>();
        player_Animator = GetComponent<Player_Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Shared.skillSelectUI.Open();
        grave.SetActive(false);
    }

    void Update()
    {
        if (isDead && !deadHandled)
        {
            deadHandled = true;
            OnDead();
        }
    }


    public void AddExp(float amount)
    {
        if (isDead)
            return;

        AddStat(StatType.Exp, amount);

        while (GetStat(StatType.Exp) >= GetMaxStat(MaxStatType.MaxExp))
        {
            float remainExp = GetStat(StatType.Exp) - GetMaxStat(MaxStatType.MaxExp);

            SetStat(StatType.Exp, remainExp);
            LevelUp();
        }
    }

    public void LevelUp()
    {
        AddStat(StatType.Level, 1f);

        AddMaxStat(MaxStatType.MaxHp, 50f);
        this.Heal(50f);

        float newMaxExp = GetMaxStat(MaxStatType.MaxExp) + 50f;
        SetMaxStat(MaxStatType.MaxExp, newMaxExp);

        Debug.Log("플레이어 현재 레벨" + GetStat(StatType.Level));
        Debug.Log("플레이어 현재 최대체력" + GetMaxStat(MaxStatType.MaxHp));
        Debug.Log("플레이어 현재 필요 경험치" + GetMaxStat(MaxStatType.MaxExp));

        Shared.skillSelectUI.Open();

    }
    public void OnDead()
    {
        Debug.Log("플레이어 사망");
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }

        if (player_Controller != null)
        {
            joyStick.SetActive(false);
            player_Controller.enabled = false;
        }

        if (Shared.battle_Manager != null)
            Shared.battle_Manager.PlayerDead(this);
        
        if (player_Animator != null)
        {
            player_Animator.SetDead(isDead);
            player_Animator.enabled = false;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (skillRoot != null)
            skillRoot.SetActive(false);

        if (grave != null)
            grave.SetActive(true);
        
    }

    public void OnHit()
    {
        player_Animator.Hit();
    }
}
