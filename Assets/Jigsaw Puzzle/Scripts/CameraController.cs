using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector3 cameraStartMovePosition;
    private Vector3 touch0ClickedPos;
    public void SingleTouchBeganCallback(Vector3 screenPosition)
    {
        cameraStartMovePosition = transform.position;
        touch0ClickedPos = screenPosition;
    }

    public void SingleTouchDrag(Vector3 screenPosition)
    {
        Vector3 moveDelta = (screenPosition - touch0ClickedPos) / Screen.width;
        Vector3 targetPosition = cameraStartMovePosition - (moveDelta * moveSpeed);

        transform.position = targetPosition;
    }
}
