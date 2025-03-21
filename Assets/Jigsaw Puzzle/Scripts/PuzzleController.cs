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

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    private Quaternion pieceStartRotation;

    public void Configure(PuzzleGenerator puzzleGenerator, float gridScale)
    {
        this.puzzleGenerator = puzzleGenerator;
        detectionRadius = gridScale / 2 * 1.5f;
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

    public void SingleTouchEnded()
    {
        if (currentPiece == null) return;

        currentPiece.StopMoving();
        currentPiece = null;
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

        if (currentPiece.Group == null)
        {
            return;
        }

        foreach (Transform piece in currentPiece.Group)
        {
            piece.position = new Vector3(piece.position.x, piece.position.y, -highestZ);
        }
    }

    private PuzzlePiece GetTopClosestPiece(PuzzlePiece[] puzzlePieces, Vector3 worldPosition)
    {
        // 1. Create a list of potential pieces
        List<PuzzlePiece> potentialPieces = new List<PuzzlePiece>();

        for (int i = 0; i < puzzlePieces.Length; ++i)
        {
            if (puzzlePieces[i].IsValid) continue;

            float distance = Vector3.Distance((Vector2)puzzlePieces[i].transform.position, worldPosition);
            if (distance > detectionRadius) continue;

            potentialPieces.Add(puzzlePieces[i]);
        }
        // 2. sort these pieces by z position
        if (potentialPieces.Count <= 0) return null;

        potentialPieces.Sort();
        // 3. Return the first element of the list
        return potentialPieces[0];
    }

    public void StartRotatingPiece()
    {
        if (currentPiece == null) return;
        if (currentPiece.Group == null)
        {
            pieceStartRotation = currentPiece.transform.rotation;
        }
    }

    public void RotatePiece(float xDelta)
    {
        if (currentPiece == null) return;

        float targetAdditionalZAngle = xDelta * rotationSpeed;
        Quaternion targetRotation = pieceStartRotation * Quaternion.Euler(0, 0, targetAdditionalZAngle);

        if (currentPiece.Group == null)
        {
            currentPiece.transform.rotation = targetRotation;
        }
        else
        {
            currentPiece.Group.rotation = targetRotation;
        }
    }
}
