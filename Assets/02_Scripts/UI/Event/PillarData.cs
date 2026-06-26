using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarData : MonoBehaviour
{
    [Header("Pillar Info")]
    [SerializeField] private float hp = 100f;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject standardPrefab;
    [SerializeField] private GameObject eventPrefab;
    [SerializeField] private GameObject destroyPrefab;


}
