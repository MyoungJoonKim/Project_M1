using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Character Player")]
    public Player player;

    private Coroutine gameTimeCoroutine;
    public float GameTime {  get; private set; }
    public bool isBattlePlaying;


    private void Awake()
    {
        if (Shared.battleManager != null && Shared.battleManager != this)
        {
            Destroy(gameObject);
            return;
        }

        Shared.battleManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartBattle();
    }

    // ∏ÛΩ∫≈Õ ≈∏∞Ÿ √ﬂ¿˚ ∏ÿ√„.
    public void PlayerDead(Player player)
    {
        StopBattle();

        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI != null)
            battleUI.StopBattleUI();

        if (Shared.spawnManager != null)
        {
            Shared.spawnManager.StopSpawn();
            Shared.spawnManager.ClearMonsterTargets();
        }

        if (Shared.skillManager != null)
            Shared.skillManager.StopAllSkills();
    }

    IEnumerator GameTimeUpdate()
    {
        while (isBattlePlaying)
        {
            GameTime += Time.deltaTime;
            yield return null;
        }
        gameTimeCoroutine = null;
    }
    public void StartBattle()
    {
        GameTime = 0f;
        isBattlePlaying = true;

        if (gameTimeCoroutine != null)
            StopCoroutine(gameTimeCoroutine);

        gameTimeCoroutine = StartCoroutine(GameTimeUpdate());
    }

    public void StopBattle()
    {
        isBattlePlaying = false;

        if (gameTimeCoroutine != null)
        {
            StopCoroutine(gameTimeCoroutine);
            gameTimeCoroutine = null;
        }
    }
}
