using System.Collections;
using UnityEngine;

public class Player : Character
{
    [Header("Player Default Stats")]
    [SerializeField] private float startHp = 50f;
    [SerializeField] private float startAtk = 1f;
    [SerializeField] private float startDef = 1f;
    [SerializeField] private float startMoveSpeed = 10f;
    [SerializeField] private float startLevel = 1f;
    [SerializeField] private float startExp = 0f;
    [SerializeField] private float startMaxExp = 100f;

    [Header("Player prefab")]
    [SerializeField] private GameObject grave;
    [SerializeField] private GameObject skillRoot;
    [SerializeField] private GameObject hitEffect;

    [Header("Player JoyStick Panel")]
    [SerializeField] private GameObject joyStick;

    [Header("Manager")]
    [SerializeField] private PassiveSkillManager passiveSkillManager;

    [Header("UI")]
    [SerializeField] private SkillSelectUI skillSelectUI;

    private PlayerController playerController;
    private PlayerAnimator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private Coroutine deadCheckCoroutine;
    private Coroutine hitEffectCoroutine;


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

        passiveSkillManager = GetComponent<PassiveSkillManager>();
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        skillSelectUI.Open();
        grave.SetActive(false);

        if (hitEffect != null)
            hitEffect.SetActive(false);

        if (deadCheckCoroutine != null)
            StopCoroutine(deadCheckCoroutine);

        deadCheckCoroutine = StartCoroutine(DeadCheck());
    }

    private IEnumerator DeadCheck()
    {
        while (true)
        {
            if (isDead && !deadHandled)
            {
                deadHandled = true;
                OnDead();

                deadCheckCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }

    public void AddExp(float amount)
    {
        if (isDead)
            return;

        if (passiveSkillManager != null)
        {
            amount *= passiveSkillManager.ExpBonusRate;
        }

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

        AddMaxStat(MaxStatType.MaxHp, 250f);
        this.Heal(GetMaxStat(MaxStatType.MaxHp) / 2);

        float newMaxExp = GetMaxStat(MaxStatType.MaxExp) * 1.5f;
        SetMaxStat(MaxStatType.MaxExp, newMaxExp);

        Debug.Log("플레이어 현재 레벨" + GetStat(StatType.Level));
        Debug.Log("플레이어 현재 최대체력" + GetMaxStat(MaxStatType.MaxHp));
        Debug.Log("플레이어 현재 필요 경험치" + GetMaxStat(MaxStatType.MaxExp));

        skillSelectUI.Open();
    }

    public void GameWin()
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }

        if (playerController != null)
        {
            joyStick.SetActive(false);
            playerController.enabled = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.enabled = false;
        }

        if (skillRoot != null)
            skillRoot.SetActive(false);

        if (BattleManager.Instance != null)
            BattleManager.Instance.EndGame(this);
    }

    public void OnDead()
    {
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }

        if (playerController != null)
        {
            joyStick.SetActive(false);
            playerController.enabled = false;
        }

        if (BattleManager.Instance != null)
            BattleManager.Instance.EndGame(this);
        
        if (playerAnimator != null)
        {
            playerAnimator.SetDead(isDead);
            playerAnimator.enabled = false;
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
        if (hitEffect != null)
        {
            ParticleSystem particle = hitEffect.GetComponentInChildren<ParticleSystem>();

            if (particle != null)
            {
                if (hitEffectCoroutine != null)
                {
                    StopCoroutine(hitEffectCoroutine);
                    hitEffectCoroutine = null;
                }
                hitEffect.SetActive(true);
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.Play();

                hitEffectCoroutine = StartCoroutine(ReleaseHitEffect());
            }
        }

        if (playerAnimator != null)
            playerAnimator.Hit();
    }

    private IEnumerator ReleaseHitEffect()
    {
        yield return new WaitForSeconds(1f);

        if (hitEffect != null)
            hitEffect.SetActive(false);

        hitEffectCoroutine = null;
    }
}
