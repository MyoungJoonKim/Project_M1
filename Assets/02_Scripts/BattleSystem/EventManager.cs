using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventUI eventUI;
    [SerializeField] private int eventWave = 2;

    [Header("Event Managers")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PillarManager pillarManager;

    private bool isEventStart;

    private void Start()
    {
        StartCoroutine(WarningEvent());
    }

    private IEnumerator WarningEvent()
    {
        while (!isEventStart)
        {
            if (spawnManager.CurrentWaveIndex == eventWave)
            {
                StartPillarEvent();
                yield break;
            }
            yield return null;
        }
    }

    private void StartPillarEvent()
    {
        isEventStart = true;

        eventUI.Open();

        Pillar activePillar = pillarManager.SetActiveRandRune();

        if (activePillar == null)
        {
            Debug.Log("ÀÌº¥Æ®¿ë ·é±âµÕ ¼ÒÁø");
            return;
        }
        Debug.Log("ÀÌº¥Æ®¿ë ·é±âµÕ ·£´ý È°¼ºÈ­");
    }

}
