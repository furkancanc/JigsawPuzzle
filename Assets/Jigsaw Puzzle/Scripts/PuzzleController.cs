using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("Settings")]
    private float detectionRadius;

    public void Configure(float gridScale)
    {
        detectionRadius = gridScale * 1.5f;
    }

    public bool SingleTouchBeganCallback(Vector3 worldPosition)
    {

    }
}
