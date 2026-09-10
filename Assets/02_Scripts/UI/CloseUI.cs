using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panel;

    public void OnClickCloseUI()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
