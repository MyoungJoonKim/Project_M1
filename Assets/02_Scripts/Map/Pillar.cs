using System.Collections;
using UnityEngine;

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

    [Header("UI")]
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
        while (currentState == PillarState.RuneActive)
        {
            // 제한시간 초과 실패
            if (eventManager != null && eventManager.EventFail)
            {
                ChangeState(PillarState.Base);
                brokenCoroutine = null;
                yield break;
            }

            // 룬기둥 파괴 성공
            if (stats[StatType.Hp] <= 0)
            {
                ChangeState(PillarState.Broken);

                if (eventManager != null)
                    eventManager.StartEventSkill();

                brokenCoroutine = null;
                yield break;
            }
            yield return null;
        }
        brokenCoroutine = null;
    }

    public void ActiveRune(int roundIndex)
    {
        if (currentState != PillarState.Base)
            return;

        float hp = GetMaxStat(MaxStatType.MaxHp);

        if (roundIndex == 1)
        {
            hp *= 2.7f;
        }

        SetMaxStat(MaxStatType.MaxHp, hp);
        SetStat(StatType.Hp, hp);

        ChangeState(PillarState.RuneActive);

        if (brokenCoroutine != null)
        {
            StopCoroutine(brokenCoroutine);
            brokenCoroutine = null;
        }

        brokenCoroutine = StartCoroutine(PillarBroken());
    }

    public bool CanTakeDamage()
    {
        return currentState == PillarState.RuneActive;
    }

}
