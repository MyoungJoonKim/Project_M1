using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PillarManager : MonoBehaviour
{
    [Header("Pillar Roots")]
    [SerializeField] private GameObject[] pillarRoot;

    [Header("RunePillar State")]
    [SerializeField] private bool[] setActiveRune;


    public int PillarCount => pillarRoot.Length;
    public bool[] SetActiveRune => setActiveRune;


    private void SetActiveRandRune()
    {
        int rand = Random.Range(0, pillarRoot.Length);

        while (setActiveRune[rand])
        {
            rand = Random.Range(0, pillarRoot.Length);
        }

        setActiveRune[rand] = true;
    }


}
