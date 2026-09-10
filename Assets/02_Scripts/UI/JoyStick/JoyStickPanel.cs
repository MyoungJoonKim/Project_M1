using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickPanel : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        // 클릭한 위치에 조이스틱 생성
        joystick.transform.position = data.position;

        joystick.gameObject.SetActive(true);
        joystick.OnDown(data);
}

    public void OnPointerUp(BaseEventData eventData)
    {
        joystick.gameObject.SetActive(false);
        joystick.OnUp((PointerEventData)eventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        joystick.OnDrag((PointerEventData)eventData);
    }
}
