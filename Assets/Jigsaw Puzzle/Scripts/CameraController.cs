using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector3 cameraStartMovePosition;
    private Vector2 touch0ClickedPosition;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomMultiplier;
    [SerializeField] private Vector2 minMaxOrthoSize;
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
        // Set ortho size
        SetOrthoSize(distanceDelta);

        // Move towards the center
        MoveTowardsZoomCenter();
    }

    private void SetOrthoSize(float distanceDelta)
    {
        float targetOrthoSize = clickedOrthoSize - distanceDelta * zoomMultiplier;
        targetOrthoSize = Mathf.Clamp(targetOrthoSize, minMaxOrthoSize.x, minMaxOrthoSize.y);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthoSize, Time.deltaTime * 60 * .3f);
    }

    private void MoveTowardsZoomCenter()
    {
        float percent = Mathf.InverseLerp(minMaxOrthoSize.x, minMaxOrthoSize.y, clickedOrthoSize - Camera.main.orthographicSize);
        percent *= zoomSpeed;
        Vector3 targetPosition = Vector3.Lerp(zoomInitialPosition, zoomCenter, percent);
        transform.position = targetPosition;
        cameraStartMovePosition = transform.position;
    }
}
