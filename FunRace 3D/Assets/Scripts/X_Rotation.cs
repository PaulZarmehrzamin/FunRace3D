// Madbox Test - FunRace 3D
// By Paul Zarmehrzamin

using UnityEngine;

public class X_Rotation : MonoBehaviour
{
    public float speed;     // Rotation speed

    // ----- ----- ----- ----- ----- ----- ----- ----- ----- -----

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0f, 0f, Space.Self);
    }
}
