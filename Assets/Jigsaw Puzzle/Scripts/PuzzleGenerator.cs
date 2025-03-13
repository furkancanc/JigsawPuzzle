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
        puzzleController.Configure(this, gridScale);

        gridScale = Constants.puzzleWorldSize / gridSize;

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


                puzzlePieces.Add(puzzlePieceInstance);

                Vector2 tiling = new Vector2(1f / gridSize, 1f / gridSize);
                Vector2 offset = new Vector2((float)x / gridSize, (float)y / gridSize);

                puzzlePieceInstance.Configure(gridScale, tiling, offset, correctPosition);
            }
        }
        
    }

    private int GridIndexFromPosition(int x, int y) => y + gridSize * x;

    public PuzzlePiece[] GetPuzzlePieces()
    {
        return puzzlePieces.ToArray();
    }
}
