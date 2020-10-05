// Madbox Test - FunRace 3D
// By Paul Zarmehrzamin

using UnityEngine;

public class DrawPath : MonoBehaviour
{
    public Transform[] points;  // Array of 4 points

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Draw path in editor
    private void OnDrawGizmos()
    {
        // Draw path with 21 spheres
        for (float t = 0f; t <= 1f; t += 0.05f)
        {
            // Bezier curve formula
            Vector3 gizmosPosition = Mathf.Pow(1 - t, 3) * points[0].position
                + 3 * t * Mathf.Pow(1 - t, 2) * points[1].position
                + 3 * (1 - t) * Mathf.Pow(t, 2) * points[2].position
                + Mathf.Pow(t, 3) * points[3].position;
            Gizmos.DrawSphere(gizmosPosition, 0.1f);
        }

        // Draw segments from point0 to point1 and from point2 to point3
        Gizmos.DrawLine(points[0].position, points[1].position);
        Gizmos.DrawLine(points[2].position, points[3].position);
    }
}
