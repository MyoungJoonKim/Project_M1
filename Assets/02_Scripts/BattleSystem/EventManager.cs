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
    
    [Header("Event Managers")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PillarManager pillarManager;

    private Pillar pillar;
    private Pillar currentActivePillar;
    public Pillar CurrentActivePillar => currentActivePillar;

    private bool isEventStart;
    private bool endEvent;
    private bool eventSuccess; // 이벤트 성공 시 스킬가져오기 수정할 것.
    public bool EventFail => endEvent;
    public bool EventSuccess => pillar.IsBroken; // 이벤트 성공 시 스킬가져오기 수정할 것.
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

        while(!endEvent)
        {
            durationTime -= Time.deltaTime;

            if (durationTime <= 0)
                endEvent = true;
            yield return null;
        }
            Debug.Log("이벤트 종료");
    }

}
