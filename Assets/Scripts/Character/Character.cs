using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string name;
    public Dictionary<StatType, int> stats = new();
    public Dictionary<BaseStatType, int> baseStats = new();
    public Dictionary<MaxStatType, int> maxStats = new();


}
