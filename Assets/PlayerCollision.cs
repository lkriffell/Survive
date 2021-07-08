using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public Vector3 knockback;
    public int health = 100;

    void OnCollisionEnter (Collision collisionInfo)
    {
      if (collisionInfo.collider.tag == "Ground") 
      {
        Debug.Log("Hit the Ground");
      }

      if (collisionInfo.collider.tag == "Enemy") 
      {
        movement.rb.AddForce(movement.rb.velocity * -5, ForceMode.Impulse);
        health -= 10;
        if(health <= 0)
        {
          movement.enabled = false;
          Debug.Log("You Died");
        }
      }
    }
}
