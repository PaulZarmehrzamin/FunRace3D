using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    public float speed;
    public float up;
    public Vector3 offset0;
    public Vector3 offset1;
    public Vector3 offset5;
    public Vector3 offset7;

    // Update is called once per frame
    void Update()
    {
        // Pick offset according to current position on path for a better view
        float t = transform.parent.GetComponent<GameManager>().tPath;
        if (t >= 7.75f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, offset7, speed * Time.deltaTime);
        }
        else if (t >= 5f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, offset5, speed * Time.deltaTime);
        }
        else if (t >= 1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, offset1, speed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, offset0, speed * Time.deltaTime);
        }
        // Look at player
        transform.LookAt(transform.parent.position + Vector3.up * up);
    }
}
