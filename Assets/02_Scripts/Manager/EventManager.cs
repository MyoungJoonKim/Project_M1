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
    [SerializeField] private PlayerSkillManager playerSkillManager;
    [SerializeField] private EventSpawnManager eventSpawnManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PillarManager pillarManager;


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
        if (playerSkillManager == null)
            CreateSkillManager();

        if (playerSkillManager == null)
            return;

        playerSkillManager.CreateEventSkill();
    }
    private void CreateSkillManager()
    {
        if (eventSkillData == null)
        {
            Debug.Log("스킬 데이터가 없습니다.");
            return;
        }

        if (skillRoot == null)
        {
            Debug.Log("SkillRoot 연결되지 않았습니다.");
            return;
        }

        PlayerSkillManager[] managers = skillRoot.GetComponentsInChildren<PlayerSkillManager>(true);

        foreach (PlayerSkillManager manager in managers)
        {
            if (manager.Data == eventSkillData)
            {
                playerSkillManager = manager;
                return;
            }
        }

        GameObject obj = new GameObject(eventSkillData.skillName);
        obj.transform.parent = skillRoot;
        obj.transform.localPosition = Vector3.zero;

        playerSkillManager = obj.AddComponent<PlayerSkillManager>();
        playerSkillManager.Init(eventSkillData, player, spawnManager, battleManager);
    }

    private IEnumerator WarningEvent()
    {
        while (true)
        {
            if (battleManager == null || !battleManager.isBattlePlaying)
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

        currentActivePillar = pillarManager.SetActiveRandRune(roundIndex);

        if (currentActivePillar == null)
        {
            Debug.Log("이벤트용 룬기둥 소진");
            isEventStart = false;
            yield return null;
        }
        Debug.Log("이벤트용 룬기둥 랜덤 활성화");

        while (currentDurationTime > 0f)
        {
            if (battleManager == null || !battleManager.isBattlePlaying)
            {
                currentActivePillar = null;
                isEventStart= false;
                yield break;
            }

            currentDurationTime -= Time.deltaTime;

            // 룬기둥 파괴 시 이벤트 성공
            if (currentActivePillar.IsBroken)
            {
                endEvent = true;
                currentActivePillar = null;
                isEventStart = false;
                yield break;
            }
            yield return null;
        }

        // 제한시간 내에 룬기둥 파괴 못하면 이벤트 실패
        endEvent = true;

        if (eventSpawnManager != null)
            eventSpawnManager.SpawnEventWave(roundIndex);

        currentActivePillar = null;    
        isEventStart = false;
    }

}
