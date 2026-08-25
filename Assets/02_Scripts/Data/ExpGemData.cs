using UnityEngine;

[CreateAssetMenu(fileName = "ExpGemData", menuName = "Game Data/ExpGem Data")]

public class ExpGemData : ScriptableObject
{
    [Header("Prefab")]
    public ExpGem prefab;

    [Header("Stats")]
    public float minExp;
}
