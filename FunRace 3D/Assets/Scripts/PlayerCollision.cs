// Madbox Test - FunRace 3D
// By Paul Zarmehrzamin

using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float force;     // Force after collision

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Check if we collided with an obstacle
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            // Enable gravity and apply force
            Vector3 direction = (transform.position - collision.contacts[0].point).normalized;
            Rigidbody r = GetComponent<Rigidbody>();
            r.useGravity = true;
            r.AddForce(collision.contacts[0].normal * force);
            // Restart at checkpoint
            FindObjectOfType<GameManager>().RestartCheckpoint();
        }
    }
}
