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
    [SerializeField] private SpriteRenderer runeSprite;

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
                SetActiveObject(basePrillar, true);
                SetActiveObject(runePrillar, false);
                SetActiveObject(brokenPillar, false);
                runeSprite.enabled = false;
                break;
            case PillarState.RuneActive:
                SetActiveObject(basePrillar, false);
                SetActiveObject(runePrillar, true);
                SetActiveObject(brokenPillar, false);
                runeSprite.enabled = true;
                break;
            case PillarState.Broken:
                SetActiveObject(basePrillar, false);
                SetActiveObject(runePrillar, false);
                SetActiveObject(brokenPillar, true);
                runeSprite.enabled = false;
                break;
        }
    }

    private void SetActiveObject(GameObject gameObject, bool isOn)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = isOn;
        gameObject.GetComponent<Collider2D>().enabled = isOn;
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
