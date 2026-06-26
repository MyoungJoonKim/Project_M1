using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventUI eventUI;
    [SerializeField] private int eventWave = 2;

    [Header("Event Objects")]
    [SerializeField] private GameObject[] eventPoints;
    

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
}
