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

    [Header("Event Points")]
    [SerializeField] private Transform[] eventPositions;

    [Header("Event Pillar Prefabs")]
    [SerializeField] private GameObject[] pillarProps;
    

    [SerializeField] private SpawnManager spawnManager;
    
    private void Start()
    {
        StartCoroutine(WarningEvent());
    }

    private IEnumerator WarningEvent()
    {
        while (spawnManager.CurrentWaveIndex <= eventWave)
        {
            if (spawnManager.CurrentWaveIndex == eventWave)
            {
                eventUI.Open();
                break;
            }
            yield return null;
        }
    }

    private void PillarSpawn()
    {
        for (int i = 0; i < eventPositions.Length; i++)
        {
            

        }
    }
}
