using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("Elements")]
    private PuzzleGenerator puzzleGenerator;

    [Header("Settings")]
    private float detectionRadius;

    [Header("Piece Movement")]
    private Vector3 clickedPosition;
    private PuzzlePiece currentPiece;

    public void Configure(PuzzleGenerator puzzleGenerator, float gridScale)
    {
        this.puzzleGenerator = puzzleGenerator;
        detectionRadius = gridScale * 1.5f;
    }

    public bool SingleTouchBeganCallback(Vector3 worldPosition)
    {
        PuzzlePiece[] puzzlePieces = puzzleGenerator.GetPuzzlePieces();
        currentPiece = GetTopClosestPiece(puzzlePieces, worldPosition);

        if (currentPiece == null)
            return false;

        ManagePiecesOrder(puzzlePieces);

        clickedPosition = worldPosition;
        currentPiece.StartMoving();


        return true;
    }

    public void SingleTouchDrag(Vector3 worldPosition)
    {
        Vector3 moveDelta = worldPosition - clickedPosition;

        if (currentPiece != null)
        {
            currentPiece.Move(moveDelta);
        }
    }

    private void ManagePiecesOrder(PuzzlePiece[] puzzlePieces)
    {
        float highestZ = puzzlePieces.Length * Constants.pieceZOffset;
        float currentPieceZ = currentPiece.transform.position.z;

        Vector3 currentPieceTargetPosition = currentPiece.transform.position;
        currentPieceTargetPosition.z = -highestZ;
        currentPiece.transform.position = currentPieceTargetPosition;

        for (int i = 0; i < puzzlePieces.Length; ++i)
        {
            if (puzzlePieces[i] == currentPiece)
                continue;

            if (puzzlePieces[i].transform.position.z < currentPieceZ)
            {
                puzzlePieces[i].transform.position += Vector3.forward * Constants.pieceZOffset;
            }
        }

        currentPiece.transform.position = new Vector3(currentPiece.transform.position.x, currentPiece.transform.position.y, -highestZ);
    }

    private PuzzlePiece GetTopClosestPiece(PuzzlePiece[] puzzlePieces, Vector3 worldPosition)
    {
        // 1. Create a list of potential pieces
        List<PuzzlePiece> potentialPieces = new List<PuzzlePiece>();

        for (int i = 0; i < puzzlePieces.Length; ++i)
        {
            float distance = Vector3.Distance((Vector2)puzzlePieces[i].transform.position, worldPosition);
            if (distance > detectionRadius) continue;

            potentialPieces.Add(puzzlePieces[i]);
        }
        // 2. sort these pieces by z position
        if (potentialPieces.Count < 0) return null;

        potentialPieces.Sort();
        // 3. Return the first element of the list
        return potentialPieces[0];
    }

    private PuzzlePiece GetClosestPiece(PuzzlePiece[] puzzlePieces, Vector3 worldPosition)
    {
        float minDistance = 50000;
        int closestIndex = -1;

        for (int i = 0; i < puzzlePieces.Length; ++i)
        {
            float distance = Vector3.Distance((Vector2)puzzlePieces[i].transform.position, worldPosition);
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
