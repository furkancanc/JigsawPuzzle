using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Renderer renderer;

    [Header("Movement")]
    private Vector3 startMovePosition;
    public void Configure(float scale, Vector2 tiling, Vector2 offset)
    {
        transform.localScale = Vector3.one * scale;

        renderer.material.mainTextureScale = tiling;
        renderer.material.mainTextureOffset = offset;
    }

    public void StartMoving()
    {
        startMovePosition = transform.position;
    }

    public void Move(Vector3 moveDelta)
    {
        Vector3 targetPosition = startMovePosition + moveDelta;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 60 * .3f);
        //transform.position = targetPosition;
    }

    public void StopMoving()
    {

    }
}
