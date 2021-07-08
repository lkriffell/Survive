using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float forward = 1000;
    public float backward = -750;
    public float right = 500;
    public float left = -500;

    void FixedUpdate()
    {
        if ( Input.GetKey("w") ) 
        {
          rb.AddForce(0, 0, forward * Time.deltaTime);
        }
        if ( Input.GetKey("s") ) 
        {
          rb.AddForce(0, 0, backward * Time.deltaTime);
        }
        if ( Input.GetKey("a") ) 
        {
          rb.AddForce(left * Time.deltaTime, 0, 0);
        }
        if ( Input.GetKey("d") ) 
        {
          rb.AddForce(right * Time.deltaTime, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
