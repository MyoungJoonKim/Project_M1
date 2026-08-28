using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Character Player")]
    public Player player;

    [Header("Manager")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private DropManager dropManager;
    [SerializeField] private DamageTextManager damageTextManager;
    [SerializeField] private PlayerSkillManager playerSkillManager;

    public float GameTime {  get; private set; }
    public bool isBattlePlaying;
    public int killRecord;

    private Coroutine gameTimeCoroutine;

    private void Start()
    {
        Time.timeScale = 1f;
        StartBattle();
    }


    // ¸ó½ºÅÍ Å¸°Ù ÃßÀû ¸ØÃã.
    public void EndGame(Player player)
    {
        StopBattle();

        BattleUI battleUI = FindObjectOfType<BattleUI>();
        RewardUI rewardUI = FindObjectOfType<RewardUI>(true);

        if (battleUI != null)
            battleUI.StopBattleUI();

        if (rewardUI != null)
            StartCoroutine(rewardUI.GameOverUI(!player.isDead));

        if (spawnManager != null)
        {
            spawnManager.StopSpawn();
            spawnManager.ClearMonsterTargets();
        }

        if (playerSkillManager != null)
            playerSkillManager.StopAllSkills();

        if (dropManager != null)
            dropManager.ClearAll();

        if (damageTextManager != null)
            damageTextManager.ClearAll();
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

    // À¯Àú °ñµå È¹µæ·®
    public int GetRewardGold()
    {
        return killRecord * 2;
    }

    // À¯Àú °æÇèÄ¡ È¹µæ·®
    public int GetRewardUserExp()
    {
        int rewardExp = 0;
        rewardExp += killRecord / (int)5f;
        return rewardExp;
    }
}
