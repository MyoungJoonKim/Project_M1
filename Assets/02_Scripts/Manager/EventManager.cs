using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventTextUI eventTextUI;
    [SerializeField] private int eventWave = 5;
    [SerializeField] private float durationTime = 45f;

    [Header("Event Skill")]
    [SerializeField] private ActiveSkillData eventSkillData;
    [SerializeField] private Transform player;
    [SerializeField] private Transform skillRoot;
    
    [Header("Event Managers")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PillarManager pillarManager;
    [SerializeField] private EventSpawnManager eventSpawnManager;
    [SerializeField] private PlayerSkillManager playerSkillManager;

    private Pillar pillar;
    private Pillar currentActivePillar;
    public Pillar CurrentActivePillar => currentActivePillar;

    private bool isEventStart;
    private bool endEvent;
    private float currentDurationTime;
    private int lastEventRound = -1;

    public bool EventFail => endEvent;
    public float Timer => currentDurationTime;
    


    private void Start()
    {
        pillar = GetComponentInChildren<Pillar>();

        StartCoroutine(WarningEvent());
    }

    public void StartEventSkill()
    {
        if (playerSkillManager == null ||
            !playerSkillManager.gameObject.activeInHierarchy)
        {
            playerSkillManager = null;
            CreateSkillManager();
        }

        if (playerSkillManager == null)
        {
            Debug.LogError("EventManager: PlayerSkillManager null");
            return;
        }

        playerSkillManager.Init(
            eventSkillData,
            player,
            spawnManager,
            this,
            BattleManager.Instance
        );

        playerSkillManager.CreateEventSkill();
    }
    private void CreateSkillManager()
    {
        if (eventSkillData == null)
        {
            Debug.LogError("EventManager: eventSkillData null");
            return;
        }

        GameObject obj = new GameObject("EventSkillManager");

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        obj.SetActive(true);

        playerSkillManager = obj.AddComponent<PlayerSkillManager>();

        playerSkillManager.Init(
            eventSkillData,
            player,
            spawnManager,
            this,
            BattleManager.Instance
        );
    }

    private IEnumerator WarningEvent()
    {
        while (true)
        {
            if (BattleManager.Instance == null || !BattleManager.Instance.isBattlePlaying)
            {
                yield return null;
                continue;
            }

            int roundIndex = spawnManager.CurrentRoundIndex;
            int waveNumber = spawnManager.CurrentWaveIndex + 1;

            if (waveNumber == eventWave && lastEventRound != roundIndex && !isEventStart)
            {
                lastEventRound = roundIndex;
                yield return StartCoroutine(StartPillarEvent(roundIndex));
            }

            yield return null;
        }
    }

    private IEnumerator StartPillarEvent(int roundIndex)
    {
        isEventStart = true;
        endEvent = false;

        currentDurationTime = durationTime;

        eventTextUI.Open();
        SoundManager.Instance.PlayWarning();

        currentActivePillar = pillarManager.SetActiveRandRune(roundIndex);

        if (currentActivePillar == null)
        {
            Debug.Log("ÀÌº¥Æ®¿ë ·é±âµÕ ¼ÒÁø");
            isEventStart = false;
            yield return null;
        }
        Debug.Log("ÀÌº¥Æ®¿ë ·é±âµÕ ·£´ý È°¼ºÈ­");

        while (currentDurationTime > 0f)
        {
            if (BattleManager.Instance == null || !BattleManager.Instance.isBattlePlaying)
            {   
                currentActivePillar = null;
                isEventStart= false;
                yield break;
            }

            currentDurationTime -= Time.deltaTime;

            // ·é±âµÕ ÆÄ±« ½Ã ÀÌº¥Æ® ¼º°ø
            if (currentActivePillar.IsBroken)
            {
                endEvent = true;
                currentActivePillar = null;
                isEventStart = false;
                yield break;
            }
            yield return null;
        }

        // Á¦ÇÑ½Ã°£ ³»¿¡ ·é±âµÕ ÆÄ±« ¸øÇÏ¸é ÀÌº¥Æ® ½ÇÆÐ
        endEvent = true;

        if (eventSpawnManager != null)
            eventSpawnManager.SpawnEventWave(roundIndex);

        currentActivePillar = null;    
        isEventStart = false;
    }

}
