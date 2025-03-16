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

        List<Vector2> v2Vertices = new List<Vector2>();
        v2Vertices.AddRange(new Vector2[] { topRight, Vector3.zero, bottomRight, bottomLeft, topLeft });

        MeshTriangulator triangulator = new MeshTriangulator(v2Vertices.ToArray());
        int[] triangles = triangulator.Triangulate();

        for (int i = 0; i < v2Vertices.Count; ++i)
        {
            vertices.Add(v2Vertices[i]);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

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
