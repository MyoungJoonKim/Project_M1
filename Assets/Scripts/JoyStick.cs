using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    public Image IMGBALL;
    public RectTransform background;

    public Vector2 Input = Vector2.zero;
    private float radius = 100f;

    public void SetPosition(Vector2 position)
    {
        background.position = position;
        background.gameObject.SetActive(true);
    }
    public void OnDown(PointerEventData eventData)
    {
        IMGBALL.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        IMGBALL.rectTransform.anchoredPosition = Vector2.zero;

        //Á¶ÀÌ½ºÆ½ ¼û±è
        background.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Input = localPoint / radius;
            Input = (Input.magnitude > 1f) ? Input.normalized : Input;

            IMGBALL.rectTransform.anchoredPosition = Input * radius ;
        }
    }
}
