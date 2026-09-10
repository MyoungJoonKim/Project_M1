using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    [Header("Joystick Icon")]
    [SerializeField] private Image joystickCircle;
    [SerializeField] private Image joystickBall;

    private float radius = 30f;

    public Vector2 Input = Vector2.zero;


    public void SetJoystickVisible(bool visible)
    {
        joystickCircle.enabled = visible;
        joystickBall.enabled = visible;
    }

    public void OnDown(PointerEventData eventData)
    {
        joystickBall.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        joystickBall.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBall.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Input = localPoint / radius;
            Input = (Input.magnitude > 1f) ? Input.normalized : Input;

            joystickBall.rectTransform.anchoredPosition = Input * radius;
        }
    }
}
