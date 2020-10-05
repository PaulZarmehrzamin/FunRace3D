using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Transform[] points;  // Array of 4 points
    public int precision;       // Precision of curve
    public float width;         // Path with
    public float height;        // Path height
    public bool front;          // Front wall
    public bool back;           // Back wall

    private Mesh mesh;          // Mesh

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }

    private void CreateMesh()
    {
        Vector3[] vertices = new Vector3[precision * 4];
        int[] triangles = new int[6 * (precision - 1) * 3 + 4 * 3];
        int vert = 0;
        int tri = 0;

        for (int i = 0; i < precision; i++)
        {
            Vector3 forward;
            // Direction from first to second point of Bezier curve
            if (i == 0)
            {
                forward = points[1].position - points[0].position;
            }
            // Direction from before last to last point of Bezier curve
            else if (i == (precision - 1))
            {
                forward = points[3].position - points[2].position;
            }
            // Direction from previous to next point
            else
            {
                forward = BezierPoint(i + 1) - BezierPoint(i - 1);
            }
            forward.Normalize();
            // Get points on each side (left, right, down left, down right)
            Vector3 left = Vector3.Cross(forward, Vector3.up);
            vertices[vert] = BezierPoint(i) + left * width * 0.5f - transform.position;
            vertices[vert + 1] = BezierPoint(i) - left * width * 0.5f - transform.position;
            vertices[vert + 2 * precision] = vertices[vert] - Vector3.up * height;
            vertices[vert + 1 + 2 * precision] = vertices[vert + 1] - Vector3.up * height;

            // Get triangles (2 on top, 2 on the left, 2 on the right)
            if (i < precision - 1)
            {
                triangles[tri] = vert;
                triangles[tri + 1] = vert + 2;
                triangles[tri + 2] = vert + 1;

                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + 2;
                triangles[tri + 5] = vert + 3;

                triangles[tri + 6] = vert;
                triangles[tri + 7] = vert + 2 * precision;
                triangles[tri + 8] = vert + 2;

                triangles[tri + 9] = vert + 2;
                triangles[tri + 10] = vert + 2 * precision;
                triangles[tri + 11] = vert + 2 + 2 * precision;

                triangles[tri + 12] = vert + 1;
                triangles[tri + 13] = vert + 3;
                triangles[tri + 14] = vert + 1 + 2 * precision;

                triangles[tri + 15] = vert + 3;
                triangles[tri + 16] = vert + 3 + 2 * precision;
                triangles[tri + 17] = vert + 1 + 2 * precision;

                vert += 2;
                tri += 18;
            }
        }
        // Front wall
        if (front)
        {
            triangles[tri] = 0;
            triangles[tri + 1] = 1;
            triangles[tri + 2] = 2 * precision;

            triangles[tri + 3] = 1;
            triangles[tri + 4] = 1 + 2 * precision;
            triangles[tri + 5] = 2 * precision;
        }
        // Back wall
        if (back)
        {
            triangles[tri + 6] = vert;
            triangles[tri + 7] = vert + 2 * precision;
            triangles[tri + 8] = vert + 1;

            triangles[tri + 9] = vert + 1;
            triangles[tri + 10] = vert + 2 * precision;
            triangles[tri + 11] = vert + 1 + 2 * precision;
        }

        // Add everything to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    Vector3 BezierPoint(int i)
    {
        float t = ((float)i) / ((float)precision - 1);
        return Mathf.Pow(1 - t, 3) * points[0].position
                + 3 * t * Mathf.Pow(1 - t, 2) * points[1].position
                + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position
                + Mathf.Pow(t, 3) * points[3].position;
    }
}

