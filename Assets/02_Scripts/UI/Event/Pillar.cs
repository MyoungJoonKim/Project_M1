using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject basePrillar;
    [SerializeField] private GameObject runePrillar;
    [SerializeField] private GameObject brokenPillar;

    [Header("Timer")]
    [SerializeField] private float durationTime = 30f;

    [SerializeField] private PillarManager pillarManager;

    
    private void OnRunePillar()
    {
        bool[] bools = pillarManager.SetActiveRune;

        for (int i = 0; i < bools.Length; i++)
        {
            if (bools[i])
            {
                basePrillar.SetActive(false);
                runePrillar.SetActive(true);
            }
        }
        
    }

    private void OnBrokenPillar()
    {
        
    }

}
