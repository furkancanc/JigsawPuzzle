using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Renderer renderer;

    public void Configure(float scale, Vector2 tiling, Vector2 offset)
    {
        transform.localScale = Vector3.one * scale;

        renderer.material.mainTextureScale = tiling;
        renderer.material.mainTextureOffset = offset;
    }
}
