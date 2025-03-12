using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum State { None, PuzzlePiece, Camera }
    private State state;

    [Header("Elements")]
    [SerializeField] private PuzzleController puzzleController;

    private void Start()
    {
        state = State.None;
    }

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
                    state = State.PuzzlePiece;
                    // A piece has been detected
                    return;
                }
                break;

            case TouchPhase.Moved:
                if (state == State.PuzzlePiece)
                {
                    puzzleController.SingleTouchDrag(worldTouchPosition);
                }
                break;

            case TouchPhase.Stationary:
                if (state == State.PuzzlePiece)
                {
                    puzzleController.SingleTouchDrag(worldTouchPosition);
                }
                break;
            default:
                break;
        }
    }
}
