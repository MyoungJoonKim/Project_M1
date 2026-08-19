using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "Game Data/Round Data")]
public class RoundData : ScriptableObject
{
    [Header("Round Info")]
    public int roundNumber;

    [Header("Waves")]
    public List<WaveData> waves = new();
}

[System.Serializable]
public class WaveData
{
    [Header("Wave Info")]
    public int waveNumber;

    [Header("Normal Monsters")]
    public List<MonsterData> normalMonsters = new();

    [Header("Boss")]
    public bool spawnBoss;
    public MonsterData bossMonster;
}
