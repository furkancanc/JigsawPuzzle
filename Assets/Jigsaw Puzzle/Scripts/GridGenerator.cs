using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject spherePrefab;

    [Header("Settings")]
    [SerializeField] private int gridSize;
    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector3 startPosition = Vector2.left * (float)gridSize / 2 + Vector2.down * (float)gridSize / 2;

        startPosition.x += .5f;
        startPosition.y += .5f;

        for (int x = 0; x < gridSize; ++x)
        {
            for (int y = 0; y < gridSize; ++y)
            {
                Vector3 spawnPosition = startPosition + Vector3.right * x + Vector3.up * y;
                Instantiate(spherePrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
        
    }
}
