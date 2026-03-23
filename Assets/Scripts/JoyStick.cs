using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class JoyStick : MonoBehaviour
{
    public Image IMGBALL;
    private float radius = 30f;

    public Vector2 Input = Vector2.zero;

    public void OnDown(PointerEventData eventData)
    {
        IMGBALL.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        IMGBALL.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(IMGBALL.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Input = localPoint / radius;
            Input = (Input.magnitude > 1f) ? Input.normalized : Input;

            IMGBALL.rectTransform.anchoredPosition = Input * radius;
        }
    }
}
