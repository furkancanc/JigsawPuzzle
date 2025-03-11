using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Elements")]

    private void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        if (Input.touchCount == 1)
            ManageSingleInput();
    }
    
    private void ManageSingleInput()
    {
        Vector2 touchPosition = Input.touches[0].position;
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        TouchPhase touchPhase = Input.touches[0].phase;

        switch (touchPhase)
        {
            case TouchPhase.Began:
                // If detect a piece, start moving
                // If not move camera
                break;
            default:
                break;
        }
    }
}
