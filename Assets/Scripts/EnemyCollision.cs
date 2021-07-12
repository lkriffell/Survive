using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioSource audio;

    void OnCollisionEnter (Collision collisionInfo)
    {
      if (collisionInfo.collider.tag == "Player") 
      {
          audio.PlayOneShot(hitSound);
      }

      // if (collisionInfo.collider.tag == "Enemy") 
      // {
      //   // movement.rb.AddForce(movement.rb.velocity * -1);
      //   Debug.Log(health);
      //   health -= 10;
      //   if(health <= 0)
      //   {
      //     controller.enabled = false;
      //     Debug.Log("You Died");
      //   }
      // }
    }
}
