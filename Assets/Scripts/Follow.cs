using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform objectToFollow;
    public Vector3 offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = objectToFollow.position + offset;
    }
}
