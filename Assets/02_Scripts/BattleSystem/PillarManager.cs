 using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PillarManager : MonoBehaviour
{
    [Header("Pillars")]
    [SerializeField] private Pillar[] pillars;


    public Pillar SetActiveRandRune()
    {
        List<Pillar> list = new List<Pillar>();

        for (int i = 0; i < pillars.Length; i++)
        {
            if (pillars[i].CanActiveRune)
            {
                list.Add(pillars[i]);
            }
        }

        if (list.Count == 0)
            return null;

        int rand = Random.Range(0, list.Count);
        Pillar selectedPillar = list[rand];

        selectedPillar.ActiveRune();    

        return selectedPillar;
    }



}
