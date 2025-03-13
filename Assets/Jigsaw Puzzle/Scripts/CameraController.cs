using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    private Vector3 cameraStartMovePosition;
    private Vector3 startMoveTouchWorldPosition;
    public void SingleTouchBeganCallback(Vector3 worldPosition)
    {
        cameraStartMovePosition = transform.position;
        startMoveTouchWorldPosition = worldPosition;
    }

    public void SingleTouchDrag(Vector3 worldPosition)
    {
        Vector3 moveDelta = worldPosition - startMoveTouchWorldPosition;
        Vector3 targetPosition = cameraStartMovePosition + moveDelta;

        transform.position = targetPosition;
    }
}
