// Madbox Test - FunRace 3D
// By Paul Zarmehrzamin

using UnityEngine;

public class Translation : MonoBehaviour
{
    public float speed;         // Movement speed
    public Vector3 distance;    // Movement distance
    [Range(0f, 360f)]
    public float angle;         // Angle for cosine formula

    private Vector3 position;   // Initial position

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;
        angle %= 360f;
        transform.position = position + distance * Mathf.Cos(angle * Mathf.PI / 180);
    }
}
