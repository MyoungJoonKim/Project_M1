using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Character
{
    private void Awake()
    {
        
        stats[StatType.Level] = 1;
        stats[StatType.Hp] = 1000;
        stats[StatType.Mp] = 500;
        stats[StatType.Atk] = 30;
        stats[StatType.Matk] = 30;
        stats[StatType.Def] = 10;
        stats[StatType.Mdef] = 10;
        stats[StatType.Exp] = 0;

        maxStats[MaxStatType.MaxHp] = 1000;
        maxStats[MaxStatType.MaxMp] = 500;
        maxStats[MaxStatType.MaxExp] = 100;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
