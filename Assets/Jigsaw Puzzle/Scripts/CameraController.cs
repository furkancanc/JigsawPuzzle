using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector3 cameraStartMovePosition;
    private Vector2 touch0ClickedPosition;

    [Header("Zoom")]
    private Vector3 zoomInitialPosition;
    private Vector3 zoomCenter;
    private float clickedOrthoSize;

    public void SingleTouchBeganCallback(Vector3 screenPosition)
    {
        cameraStartMovePosition = transform.position;
        touch0ClickedPosition = screenPosition;
    }

    public void SingleTouchDrag(Vector2 screenPosition)
    {
        Vector3 moveDelta = (screenPosition - touch0ClickedPosition) / Screen.width;
        Vector3 targetPosition = cameraStartMovePosition - (moveDelta * moveSpeed);

        transform.position = targetPosition;
    }

    public void DoubleTouchBeganCallback(Vector2 touch0Pos, Vector2 touch1Pos)
    {
        clickedOrthoSize = Camera.main.orthographicSize;

        this.touch0ClickedPosition = touch0Pos;
        zoomInitialPosition = transform.position;

        zoomCenter = (Camera.main.ScreenToWorldPoint(touch0Pos) - Camera.main.ScreenToWorldPoint(touch1Pos)) / 2;
        zoomCenter.z = -10;
    }

    public void DoubleTouchDrag(float distanceDelta)
    {

    }
}
