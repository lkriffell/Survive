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
    public bool damageApplied;

    public bool isPlayer;
    public CharacterController controller;

    void Start()
    {
      audio = GetComponent<AudioSource>();
    }
    
    public void TakeDamage (float amount)
    {
      health -= amount;
      damageApplied = true;
      Invoke(nameof(ResetDamageApplied), 0.05f);
      audio.PlayOneShot(hitSound);
      if (health <= 0 && !dead)
      {
        Die();
      }
    }

    private void ResetDamageApplied()
    {
      damageApplied = false;
    }

    void Die()
    {
      SetRigidbodyState(false);

      AudioSource.PlayClipAtPoint(deathSound, transform.position);
      gameObject.layer = 10;
      gameObject.tag = "Untagged";
      dead = true;

      DropWeapons();
      if (isPlayer) DisableController();
      else 
      {
        GetComponent<CharacterAI>().enabled = false;
        GetComponent<Animator>().enabled = false;
      }  
      
      if (explodes)
      {
        Explode();
      } else{
        if (!isPlayer) Destroy(gameObject, 5f);
      }
    }

    void Explode()
    {
      GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        
      Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

      Destroy(gameObject);
      Destroy(explosion, 2f);

      foreach (Collider objectHit in colliders)
      {
        Rigidbody rb = objectHit.GetComponent<Rigidbody>();
        Target targetHit = objectHit.GetComponent<Target>();
        if (targetHit != null && rb != null && !targetHit.damageApplied) rb.AddExplosionForce(force, transform.position, radius);
        if (targetHit != null && !targetHit.damageApplied) targetHit.TakeDamage(damage);
      }
    }

    private void DisableController() 
    {
      controller.enabled = false;
    }

    private void DropWeapons() 
    {
      Gun[] guns = GetComponentsInChildren<Gun>();
      foreach (Gun gun in guns)
      {
        Rigidbody gunRb = gun.GetComponent<Rigidbody>();
        gunRb.AddForce(transform.forward * Random.Range(0, 30), ForceMode.Impulse);
        gunRb.AddTorque(transform.up * Random.Range(0, 30) * Random.Range(0, 50));
        gun.transform.parent = null;
        gun.EmptyRoundsOnDeath();
        Destroy(gun, 20f);
        // gun.enabled = false;
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
