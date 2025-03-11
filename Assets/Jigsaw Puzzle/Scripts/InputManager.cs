using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PuzzleController puzzleController;

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
        worldTouchPosition.z = 0;

        TouchPhase touchPhase = Input.touches[0].phase;

        switch (touchPhase)
        {
            case TouchPhase.Began:
                // If detect a piece, start moving
                // If not move camera
                if (puzzleController.SingleTouchBeganCallback(worldTouchPosition))
                {
                    // A piece has been detected
                    return;
                }
                break;
            default:
                break;
        }
    }
}
