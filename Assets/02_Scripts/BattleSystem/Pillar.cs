using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    [Header("UIs")]
    [SerializeField] private EventSliderUI eventSliderUI;


    private PillarState currentState;
    public PillarState CurrentState => currentState;

    public bool CanActiveRune => currentState == PillarState.Base;
    public bool IsBroken => currentState == PillarState.Broken; // ÀÌº¥Æ® ¼º°ø ½Ã ½ºÅ³°¡Á®¿À±â ¼öÁ¤ÇÒ °Í.

    private Coroutine brokenCoroutine;

    private void Start()
    {
        eventSliderUI = GetComponent<EventSliderUI>();

        ChangeState(PillarState.Base);

        if (brokenCoroutine != null ) 
            StopCoroutine(brokenCoroutine);

        brokenCoroutine = StartCoroutine(PillarBroken());
    }

    private void Update()
    {
        ChangeState(currentState);
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
                eventSliderUI.SetActiveBar(false);
                break;
            case PillarState.RuneActive:
                SetActiveObject(basePrillar, false);
                SetActiveObject(runePrillar, true);
                SetActiveObject(brokenPillar, false);
                runeSprite.enabled = true;
                eventSliderUI.SetActiveBar(true);
                break;
            case PillarState.Broken:
                SetActiveObject(basePrillar, false);
                SetActiveObject(runePrillar, false);
                SetActiveObject(brokenPillar, true);
                runeSprite.enabled = false;
                eventSliderUI.SetActiveBar(false);
                break;
        }
    }

    private void SetActiveObject(GameObject gameObject, bool isOn)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = isOn;
    }

    private IEnumerator PillarBroken()
    {
        while (currentState != PillarState.Broken)
        {
            if (eventManager.EventFail)
            {
                currentState = PillarState.Base;
                Debug.Log("·é±âµÕ ÆÄ±« ½ÇÆÐ");
                yield break;
            }

            if (stats[StatType.Hp] <= 0)
            {
                currentState = PillarState.Broken;

                if (eventManager != null)
                    eventManager.StartEventSkill();

                Debug.Log("·é±âµÕ ÆÄ±« ¼º°ø");
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
