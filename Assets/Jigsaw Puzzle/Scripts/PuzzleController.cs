using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("Elements")]
    private PuzzleGenerator puzzleGenerator;

    [Header("Settings")]
    private float detectionRadius;

    public void Configure(PuzzleGenerator puzzleGenerator, float gridScale)
    {
        this.puzzleGenerator = puzzleGenerator;
        detectionRadius = gridScale * 1.5f;
    }

    public bool SingleTouchBeganCallback(Vector3 worldPosition)
    {
        PuzzlePiece[] puzzlePieces = puzzleGenerator.GetPuzzlePieces();
        PuzzlePiece closestPiece = GetClosesPiece(puzzlePieces, worldPosition);

        if (closestPiece == null)
            return false;

        Destroy(closestPiece.gameObject);
        return true;
    }

    private PuzzlePiece GetClosesPiece(PuzzlePiece[] puzzlePieces, Vector3 worldPosition)
    {
        float minDistance = 50000;
        int closestIndex = -1;

        for (int i = 0; i < puzzlePieces.Length; ++i)
        {
            float distance = Vector3.Distance(puzzlePieces[i].transform.position, worldPosition);
            if (distance > detectionRadius)
                continue;

            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        if (closestIndex < 0)
            return null;

        return puzzlePieces[closestIndex];
    }
}
