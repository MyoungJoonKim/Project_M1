using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventTextUI eventTextUI;
    [SerializeField] private int eventWave = 2;
    [SerializeField] private float durationTime = 30f;

    [Header("Event Skill")]
    [SerializeField] private ActiveSkillData eventSkillData;
    [SerializeField] private Transform player;
    [SerializeField] private Transform skillRoot;
    
    [Header("Event Managers")]
    [SerializeField] private PlayerSkillManager playerSkillManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PillarManager pillarManager;
    [SerializeField] private EventSpawnManager eventSpawnManager;


    private Pillar pillar;
    private Pillar currentActivePillar;
    public Pillar CurrentActivePillar => currentActivePillar;

    private bool isEventStart;
    private bool endEvent;
    public bool EventFail => endEvent;
    public float Timer => durationTime;


    private void Start()
    {
        pillar = GetComponentInChildren<Pillar>();

        StartCoroutine(WarningEvent());
    }

    private void Awake()
    {
        if (Shared.eventManager == null)
            Shared.eventManager = this;
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
        playerSkillManager.Init(eventSkillData, player);
    }

    private IEnumerator WarningEvent()
    {
        while (!isEventStart)
        {
            if (spawnManager.CurrentWaveIndex == eventWave)
            {
                StartCoroutine(StartPillarEvent());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator StartPillarEvent()
    {
        isEventStart = true;

        eventTextUI.Open();

        currentActivePillar = pillarManager.SetActiveRandRune();

        if (currentActivePillar == null)
        {
            Debug.Log("이벤트용 룬기둥 소진");
            yield return null;
        }
        Debug.Log("이벤트용 룬기둥 랜덤 활성화");

        while (durationTime > 0f)
        {
            durationTime -= Time.deltaTime;

            // 룬기둥 파괴 시 이벤트 성공
            if (currentActivePillar.IsBroken)
            {
                endEvent = true;
                currentActivePillar = null;
                yield break;
            }
            yield return null;
        }

        // 제한시간 내에 룬기둥 파괴 못하면 이벤트 실패
        endEvent = true;

        if (eventSpawnManager != null)
            eventSpawnManager.SpawnEventWave();

        currentActivePillar = null;    
    }

}
