using System.Collections;
using UnityEngine;

public class Player : Character
{
    [Header("Player Default Stats")]
    [SerializeField] private float startHp = 500f;
    [SerializeField] private float startAtk = 1f;
    [SerializeField] private float startDef = 5f;
    [SerializeField] private float startMoveSpeed = 12f;
    [SerializeField] private float startLevel = 1f;
    [SerializeField] private float startExp = 0f;
    [SerializeField] private float startMaxExp = 100f;

    [Header("Player prefab")]
    [SerializeField] private GameObject grave;
    [SerializeField] private GameObject skillRoot;
    [SerializeField] private GameObject hitEffect;

    [Header("Player JoyStick Panel")]
    [SerializeField] private GameObject joyStick;

    [Header("Camera Shake Effect")]
    [SerializeField] private CameraShake cameraShake;

    [Header("UI")]
    [SerializeField] private SkillSelectUI skillSelectUI;

    [Header("Manager")]
    [SerializeField] private PassiveSkillManager passiveSkillManager;


    private Rigidbody2D rigidbody2D;
    private PlayerAnimator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    private Coroutine deathCheckCoroutine;
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

        AddAbilityStat();

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

        if (deathCheckCoroutine != null)
            StopCoroutine(deathCheckCoroutine);

        deathCheckCoroutine = StartCoroutine(DeathCheckRoutine());
    }

    private IEnumerator DeathCheckRoutine()
    {
        while (true)
        {
            if (isDead && !deadHandled)
            {
                deadHandled = true;
                HandleDeath();

                deathCheckCoroutine = null;
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

        //Debug.Log("플레이어 현재 레벨" + GetStat(StatType.Level));
        //Debug.Log("플레이어 현재 최대체력" + GetMaxStat(MaxStatType.MaxHp));
        //Debug.Log("플레이어 현재 필요 경험치" + GetMaxStat(MaxStatType.MaxExp));

        skillSelectUI.Open();
    }

    private void AddAbilityStat()
    {
        if (UserManager.Instance == null)
            return;

        float hpBonus = UserManager.Instance.GetAbilityBonus(PassiveType.HpBonus);

        if (hpBonus > 0f)
        {
            float maxHp = GetMaxStat(MaxStatType.MaxHp);
            maxHp += hpBonus * 100f;

            SetMaxStat(MaxStatType.MaxHp, maxHp);
            SetStat(StatType.Hp, maxHp);
        }

        float moveSpeedBonus = UserManager.Instance.GetAbilityBonus(PassiveType.MoveSpeed);

        if (moveSpeedBonus > 0f)
        {
            stats[StatType.MoveSpeed] *= 1f + (moveSpeedBonus * 0.1f);
        }
    }

    public void HandleVictory()
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

    public void HandleDeath()
    {
        SoundManager.Instance.PlayPlayerDead();

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

    public void HandleHit()
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

        if (cameraShake != null)
            cameraShake.Shake(0.2f, 0.1f);

        if (playerAnimator != null)
            playerAnimator.Hit();

        Handheld.Vibrate();
        SoundManager.Instance.PlayPlayerHit();
    }

    private IEnumerator ReleaseHitEffect()
    {
        yield return new WaitForSeconds(1f);

        if (hitEffect != null)
            hitEffect.SetActive(false);

        hitEffectCoroutine = null;
    }
}
