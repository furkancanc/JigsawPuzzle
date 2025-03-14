using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PuzzleController puzzleController;
    [SerializeField] private PuzzlePiece puzzlePiecePrefab;

    [Header("Settings")]
    [SerializeField] private int gridSize;
    private float gridScale;
    private List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();


    private void Start()
    {

        gridScale = Constants.puzzleWorldSize / gridSize;
        puzzleController.Configure(this, gridScale);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        puzzlePieces.Clear();

        Vector3 startPosition = Vector2.left * (gridScale * gridSize / 2) + Vector2.down * (gridScale * gridSize  / 2);

        startPosition.x += gridScale / 2;
        startPosition.y += gridScale / 2;

        for (int x = 0; x < gridSize; ++x)
        {
            for (int y = 0; y < gridSize; ++y)
            {
                Vector3 correctPosition = startPosition + new Vector3(x, y) * gridScale;
                correctPosition.z -= Constants.pieceZOffset * GridIndexFromPosition(x, y);

                Vector3 randomPosition = Random.insideUnitSphere * 2;
                randomPosition.z = correctPosition.z;

                PuzzlePiece puzzlePieceInstance = Instantiate(puzzlePiecePrefab, randomPosition, Quaternion.identity, transform);

                puzzlePieceInstance.name = "Puzzle Piece (" + x + "-" + y + ")"; 

                puzzlePieces.Add(puzzlePieceInstance);

                Vector2 tiling = new Vector2(1f / gridSize, 1f / gridSize);
                Vector2 offset = new Vector2((float)x / gridSize, (float)y / gridSize);

                puzzlePieceInstance.Configure(gridScale, tiling, offset, correctPosition);
            }
        }

        ConfigureNeighbors();
    }

    private void ConfigureNeighbors()
    {
        for (int i = 0; i < puzzlePieces.Count; ++i)
        {
            ConfigurePieceNeigbors(puzzlePieces[i], i);
        }
    }

    private void ConfigurePieceNeigbors(PuzzlePiece piece, int index)
    {
        Vector2Int gridPosition = IndexToGridPosition(index);

        int x = gridPosition.x;
        int y = gridPosition.y;

        PuzzlePiece rightPiece      = IsValidGridPosition(x + 1, y) ? transform.GetChild(GridIndexFromPosition(x + 1 ,y)).GetComponent<PuzzlePiece>() : null;
        PuzzlePiece bottomPiece     = IsValidGridPosition(x, y - 1) ? transform.GetChild(GridIndexFromPosition(x, y - 1)).GetComponent<PuzzlePiece>() : null;
        PuzzlePiece leftPiece       = IsValidGridPosition(x - 1, y) ? transform.GetChild(GridIndexFromPosition(x - 1, y)).GetComponent<PuzzlePiece>() : null;
        PuzzlePiece topPiece        = IsValidGridPosition(x, y + 1) ? transform.GetChild(GridIndexFromPosition(x, y + 1)).GetComponent<PuzzlePiece>() : null;

        piece.SetNeighbors(rightPiece, bottomPiece, leftPiece, topPiece);
    }

    private bool IsValidGridPosition(int x, int y) => x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    private int GridIndexFromPosition(int x, int y) => y + gridSize * x;

    private Vector2Int IndexToGridPosition(int index)
    {
        int x = index / gridSize;
        int y = (int)((float)index) % gridSize;

        return new Vector2Int(x, y);
    }

    public PuzzlePiece[] GetPuzzlePieces()
    {
        return puzzlePieces.ToArray();
    }
}
