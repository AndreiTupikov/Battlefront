using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private RectTransform manipulator;
    private Vector2 joystickPosition;
    private Vector2 direction;
    private int joystickFingerId = -1;

    private void Start()
    {
        joystickPosition = transform.position;
    }

    private void Update()
    {
        direction = GetJoystickDirection();
        manipulator.position = direction * 0.55f + joystickPosition;
        controller.moveDirection = direction;
    }

    public void JoystickControl(bool isPressed)
    {
        if (isPressed)
        {
#if UNITY_EDITOR
            joystickFingerId = 1;
#else
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (joystickFingerId < 0) joystickFingerId = touch.fingerId;
                    else joystickFingerId = GetNearestTouch(joystickFingerId, touch.fingerId);
                }
            }
#endif
        }
        else joystickFingerId = -1;
    }

    private Vector2 GetJoystickDirection()
    {
        Vector2 dir = new Vector2();
        if (joystickFingerId > -1)
        {
#if UNITY_EDITOR
            dir.x = Input.mousePosition.x;
            dir.y = Input.mousePosition.y;
#else
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == joystickFingerId)
                {
                    dir.x = touch.position.x;
                    dir.y = touch.position.y;
                    break;
                }
            }
#endif
            dir = (Vector2)Camera.main.ScreenToWorldPoint(dir) - joystickPosition;
        }
        return dir.normalized;
    }

    private int GetNearestTouch(int a, int b)
    {
        float distanceA = 0;
        float distanceB = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == a) distanceA = Vector2.Distance(touch.position, joystickPosition);
            if (touch.fingerId == b) distanceB = Vector2.Distance(touch.position, joystickPosition);
        }
        return distanceB > distanceA ? a : b;
    }
}
