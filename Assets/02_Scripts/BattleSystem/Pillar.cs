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

    [Header("Ui")]
    [SerializeField] private EventSliderUI eventSliderUI;


    private PillarState currentState;
    public PillarState CurrentState => currentState;

    public bool CanActiveRune => currentState == PillarState.Base;
    public bool IsBroken => currentState == PillarState.Broken; // 이벤트 성공 시 스킬가져오기 수정할 것.

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
                Debug.Log("룬기둥 파괴 실패");
                yield break;
            }

            if (stats[StatType.Hp] <= 0)
            {
                currentState = PillarState.Broken; // 이벤트 성공 시 스킬가져오기 수정할 것.
                Debug.Log("룬기둥 파괴 성공");
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
