using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 knockback;
    public float health = 100;
    public AudioClip deathSound;
    public AudioClip deathSoundTwo;
    public AudioClip hitSound;
    public AudioSource audio;
    public bool dead = false;

    void Update() 
    {
      CheckHealth();
    }

    void OnCollisionEnter (Collision collisionInfo)
    {
      if (collisionInfo.collider.tag == "Ground") 
      {
        Debug.Log("Hit the Ground");
      }
    }

    public void PlayHitSound()
    {
      audio.PlayOneShot(hitSound);
    }

    private void CheckHealth()
    {
      if(health <= 0 && !dead)
      {
        dead = true;
        audio.PlayOneShot(deathSound);
        audio.PlayOneShot(deathSoundTwo);
        controller.enabled = false;
        Debug.Log("You Died");
      }
    }
}
