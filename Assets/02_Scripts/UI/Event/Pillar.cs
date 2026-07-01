using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PillarState
{
    Base,
    RuneActive,
    Broken
}

public class Pillar : Prop
{
    [Header("State Prefabs")]
    [SerializeField] private GameObject basePrillar;
    [SerializeField] private GameObject runePrillar;
    [SerializeField] private GameObject brokenPillar;

    [Header("Manager")]
    [SerializeField] private EventManager eventManager;


    private PillarState currentState;
    public PillarState CurrentState => currentState;

    public bool CanActiveRune => currentState == PillarState.Base;
    public bool IsBroken => currentState == PillarState.Broken;

    private Coroutine brokenCoroutine;

    private void Start()
    {
        ChangeState(PillarState.Base);

        if (brokenCoroutine != null ) 
            StopCoroutine(brokenCoroutine);

        brokenCoroutine = StartCoroutine(PillarBroken());
    }

    private void ChangeState(PillarState state)
    {
        currentState = state;

        switch (state)
        {
            case PillarState.Base:
                basePrillar.SetActive(true);
                runePrillar.SetActive(false);
                brokenPillar.SetActive(false);
                break;
            case PillarState.RuneActive:
                basePrillar.SetActive(false);
                runePrillar.SetActive(true);
                brokenPillar.SetActive(false);
                break;
            case PillarState.Broken:
                basePrillar.SetActive(false);
                runePrillar.SetActive(false);
                brokenPillar.SetActive(true);
                break;
        }
    }

    private IEnumerator PillarBroken()
    {
        while (currentState == PillarState.RuneActive)
        {
            if (eventManager.EndEvent)
            {
                currentState = PillarState.Base;
                yield break;
            }

            if (stats[StatType.Hp] <= 0)
            {
                currentState = PillarState.Broken;
            }
            yield return null;
        }
        brokenCoroutine = null;
    }


    public void ActiveRune()
    {
        if (currentState != PillarState.Base)
            return;
        currentState = PillarState.RuneActive;
    }


    

}
