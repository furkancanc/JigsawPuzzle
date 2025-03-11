using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PuzzlePiece puzzlePiecePrefab;

    [Header("Settings")]
    [SerializeField] private int gridSize;
    [SerializeField] private float gridScale;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector3 startPosition = Vector2.left * (gridScale * gridSize / 2) + Vector2.down * (gridScale * gridSize  / 2);

        startPosition.x += gridScale / 2;
        startPosition.y += gridScale / 2;

        for (int x = 0; x < gridSize; ++x)
        {
            for (int y = 0; y < gridSize; ++y)
            {
                Vector3 spawnPosition = startPosition + new Vector3(x, y) * gridScale;
                PuzzlePiece puzzlePieceInstance = Instantiate(puzzlePiecePrefab, spawnPosition, Quaternion.identity, transform);

                Vector2 tiling = new Vector2(1f / gridSize, 1f / gridSize);
                Vector2 offset = new Vector2((float)x / gridSize, (float)y / gridSize);

                puzzlePieceInstance.Configure(gridScale, tiling, offset);
            }
        }
        
    }
}
