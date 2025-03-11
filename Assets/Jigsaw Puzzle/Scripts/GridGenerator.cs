using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject spherePrefab;

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
                GameObject sphereInstance = Instantiate(spherePrefab, spawnPosition, Quaternion.identity, transform);
                sphereInstance.transform.localScale = Vector3.one * gridScale;

                Vector2 tiling = new Vector2(1f / gridSize, 1f / gridSize);
                Vector2 offset = new Vector2((float)x / gridSize, (float)y / gridSize);

                sphereInstance.GetComponent<Renderer>().material.mainTextureScale = tiling;
                sphereInstance.GetComponent<Renderer>().material.mainTextureOffset = offset;
            }
        }
        
    }
}
