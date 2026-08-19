using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickPanel : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private JoyStick joyStick;

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData data = (PointerEventData)eventData;

        // 클릭한 위치에 조이스틱 생성
        joyStick.transform.position = data.position;

        joyStick.gameObject.SetActive(true);
        joyStick.OnDown(data);
}

    public void OnPointerUp(BaseEventData eventData)
    {
        joyStick.gameObject.SetActive(false);
        joyStick.OnUp((PointerEventData)eventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        joyStick.OnDrag((PointerEventData)eventData);
    }
}
