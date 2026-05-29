using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BattleManager : MonoBehaviour
{
    [Header("Character Player")]
    public Player player;

    private Coroutine gameTimeCoroutine;

    public float GameTime {  get; private set; }
    public int killRecord;
    public bool isBattlePlaying;



    private void Awake()
    {
        Shared.battleManager = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartBattle();
    }

    private void OnDestroy()
    {
        if (Shared.battleManager == this)
            Shared.battleManager = null;
    }

    // ∏ÛΩ∫≈Õ ≈∏∞Ÿ √ﬂ¿˚ ∏ÿ√„.
    public void PlayerDead(Player player)
    {
        StopBattle();

        BattleUI battleUI = FindObjectOfType<BattleUI>();
        RewardUI rewardUI = FindObjectOfType<RewardUI>();
        if (battleUI != null)
        {
            battleUI.StopBattleUI();
            StartCoroutine(rewardUI.GameOverUI());
        }

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
        killRecord = 0;
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

    public int GetRewardGold()
    {
        return killRecord * 10;
    }

    public int GetRewardUserExp()
    {
        int rewardExp = 0;

        rewardExp += killRecord * 5;

        return rewardExp;
    }
}
