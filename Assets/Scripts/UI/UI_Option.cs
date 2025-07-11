using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UI_Option : MonoBehaviour
{
    public Toggle[] toggles;
    public GameObject[] contents;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < toggles.Length; i++) 
        {
            SetToggle(i);
            contents[i].SetActive(toggles[i].isOn);
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToggle(int index)
    {
        toggles[index].onValueChanged.AddListener((isOn) => OnToggleChange(index, isOn));
    }
    public void OnToggleChange(int index, bool isOn)
    {
        contents[index].SetActive(isOn);
    }

    public void OnButtonExit()
    {
        gameObject.SetActive(false);
    }
}
