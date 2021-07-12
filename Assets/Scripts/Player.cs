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
    }

    void OnCollisionEnter (Collision collisionInfo)
    {
      if (collisionInfo.collider.tag == "Ground") 
      {
        Debug.Log("Hit the Ground");
      }
    }

    public void TakeDamage (float amount)
    {
      Debug.Log("You got hit");
      health -= amount;
      audio.PlayOneShot(hitSound);
      if (health <= 0 && !dead)
      {
        Die();
      }
    }

    private void Die()
    {
      dead = true;
      DisableController();
      audio.PlayOneShot(deathSound);
      audio.PlayOneShot(deathSoundTwo);
      Debug.Log("You Died");
    }

    private void DisableController() 
    {
      controller.enabled = false;
      Gun[] guns = GetComponentsInChildren<Gun>();
      foreach (Gun gun in guns)
      {
        gun.enabled = false;
      }
    }
}
