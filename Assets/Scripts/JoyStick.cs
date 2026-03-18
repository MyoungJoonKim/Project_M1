using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    //public Player_Controller player_Controller;
    public Image IMGBALL;

    Vector3 Input = Vector3.zero;
    Vector3 Position = Vector3.zero;
    public Vector2 vec = Vector2.zero;



    public void OnDown(PointerEventData eventData)
    {
        IMGBALL.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnUp(PointerEventData eventData)
    {
        Input = Vector3.zero;
        IMGBALL.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(IMGBALL.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            localPoint.x = localPoint.x / IMGBALL.rectTransform.sizeDelta.x;
            localPoint.y = localPoint.y / IMGBALL.rectTransform.sizeDelta.y;

            Input.x = localPoint.x;
            Input.y = localPoint.y;

            Input = (Input.magnitude > 1.0f) ? Input.normalized : Input;

            Position.x = Input.x * IMGBALL.rectTransform.sizeDelta.x / 2f;
            Position.y = Input.y * IMGBALL.rectTransform.sizeDelta.y / 2f;

            vec = new Vector2(Position.x, Position.y);

            IMGBALL.rectTransform.anchoredPosition = vec;

            // 캐릭터 이동
        }
    }
}
