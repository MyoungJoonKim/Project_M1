using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Character
{
    private void Awake()
    {
        name = "��";
        stats[StatType.Level] = 1;
        stats[StatType.Hp] = 300;
        stats[StatType.Mp] = 100;
        stats[StatType.Atk] = 30;
        stats[StatType.Matk] = 30;
        stats[StatType.Def] = 10;
        stats[StatType.Mdef] = 10;
        stats[StatType.Exp] = 50;

        maxStats[MaxStatType.MaxHp] = 300;
        maxStats[MaxStatType.MaxMp] = 100;
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
