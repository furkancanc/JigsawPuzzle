using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Renderer renderer;

    List<Vector3> vertices = new List<Vector3>();

    private void Start()
    {
        Vector3 topRight = (Vector3.right + Vector3.up) * .5f;
        Vector3 bottomRight = (Vector3.right + Vector3.down) * .5f;
        Vector3 topLeft = (Vector3.left + Vector3.up) * .5f;
        Vector3 bottomLeft = (Vector3.left + Vector3.down) * .5f;

        vertices.AddRange(new[] { topRight, bottomRight, bottomLeft, topLeft });

        List<int> triangles = new List<int>();

        triangles.AddRange(new int[] { 0, 1, 2 });
        triangles.AddRange(new int[] { 0, 2, 3 });

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        renderer.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < vertices.Count; ++i)
        {
            Gizmos.DrawWireSphere(vertices[i], .1f);
        }
    }
}
