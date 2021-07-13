using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public bool dead;
    public bool explodes;
    public float force;
    public float damage;
    public float radius;
    public GameObject explosionEffect;
    private AudioSource audio;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public bool isPlayer;
    public CharacterController controller;

    void Start()
    {
      audio = GetComponent<AudioSource>();
    }
    
    public void TakeDamage (float amount)
    {
      health -= amount;
      audio.PlayOneShot(hitSound);
      if (health <= 0)
      {
        
        Die();
      }
    }

    void Die()
    {
      if (isPlayer) DisableController();
      else 
      {
        GetComponent<CharacterAI>().enabled = false;
        GetComponent<Animator>().enabled = false;
        SetRigidbodyState(false);
      }  
      AudioSource.PlayClipAtPoint (deathSound, transform.position);
      gameObject.layer = 10;
      gameObject.tag = "Untagged";
      dead = true;
      if (explodes)
      {
        Explode();
      } else{
        Destroy(gameObject, 5f);
      }
    }

    void Explode()
    {
      GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        
      Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

      Destroy(gameObject);
      Destroy(explosion, 2f);

      foreach (Collider nearbyObject in colliders)
      {
        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
        Debug.Log(rb);
        if (rb != null)
        {
          Debug.Log(nearbyObject.transform.root.tag);
          rb.AddExplosionForce(force, transform.position, radius);
        }
      }
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

    public void SetRigidbodyState(bool state)
    {
      Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
      foreach (Rigidbody rigidbody in rigidBodies)
      {
        rigidbody.isKinematic = state;
      }
      GetComponent<Rigidbody>().isKinematic = !state;
    }

    public void SetColliderState(bool state)
    {
      Collider[] colliders = GetComponentsInChildren<Collider>();
      foreach (Collider collider in colliders)
      {
        collider.enabled = state;
      }
      GetComponent<Collider>().enabled = !state;
    }
}
