using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum State { None, PuzzlePiece, Camera }
    private State state;

    [Header("Elements")]
    [SerializeField] private PuzzleController puzzleController;
    [SerializeField] private CameraController cameraController;

    [Header("Double Touch")]
    private Vector2 touch0clickedPosition, touch1clickedPosition;
    private Vector2 initialDelta;

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
        else if (Input.touchCount == 2)
            ManageDoubleInput();
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
                    cameraController.SingleTouchBeganCallback(touchPosition);
                    state = State.Camera;
                }
                break;

            case TouchPhase.Moved:
                if (state == State.PuzzlePiece)
                {
                    puzzleController.SingleTouchDrag(worldTouchPosition);
                }
                else if (state == State.Camera)
                {
                    cameraController.SingleTouchDrag(touchPosition);
                }
                break;

            case TouchPhase.Stationary:
                if (state == State.PuzzlePiece)
                {
                    puzzleController.SingleTouchDrag(worldTouchPosition);
                }
                else if (state == State.Camera)
                {
                    cameraController.SingleTouchDrag(touchPosition);
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
    
    private void ManageDoubleInput()
    {
        switch (state)
        {
            case State.None:
                ManageCameraDoubleInput();
                break;
            case State.Camera:
                ManageCameraDoubleInput();
                break;
            case State.PuzzlePiece:
                break;

        }
    }

    private void ManageCameraDoubleInput()
    {
        Touch[] touches = Input.touches;

        if (touches[0].phase == TouchPhase.Began)
        {
            touch0clickedPosition = touches[0].position;
        }
        if (touches[1].phase == TouchPhase.Began)
        {
            touch0clickedPosition = touches[0].position;
            touch1clickedPosition = touches[0].position;

            initialDelta = touch1clickedPosition - touch0clickedPosition;

            cameraController.DoubleTouchBeganCallback(touch0clickedPosition, touch1clickedPosition);

            return;
        }

        Vector2 currentDelta = touches[1].position - touches[0].position;
        float distanceDelta = (currentDelta.magnitude - initialDelta.magnitude) / Screen.width;

        cameraController.DoubleTouchDrag(distanceDelta);
    }
}
