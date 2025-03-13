using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum State { None, PuzzlePiece, Camera }
    private State state;

    [Header("Elements")]
    [SerializeField] private PuzzleController puzzleController;
    [SerializeField] private CameraController cameraController;

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
                    // A piece has been detected
                    state = State.PuzzlePiece;
                }
                else
                {
                    cameraController.SingleTouchBeganCallback(worldTouchPosition);
                    state = State.Camera;
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
            case TouchPhase.Ended:
                if (state == State.PuzzlePiece)
                {
                    puzzleController.SingleTouchEnded();
                }
                break;
            default:
                break;
        }
    }
}
