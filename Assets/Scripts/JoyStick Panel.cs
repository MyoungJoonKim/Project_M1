using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickPanel : MonoBehaviour, IPointerDownHandler
{
    public JoyStick joyStick;

    public void OnPointerDown(PointerEventData eventData)
    {
        joyStick.SetPosition(eventData.position);
    }
}
