using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public bool dead;
    public AudioSource audio;
    public AudioClip hitSound;

    void Start()
    {
    }
    
    public void TakeDamage (float amount)
    {
      Debug.Log("it got hit");
      health -= amount;
      audio.PlayOneShot(hitSound);
      if (health <= 0)
      {
        Die();
      }
    }

    void Die()
    {
      Debug.Log("it died");
      gameObject.layer = 10;
      gameObject.tag = "Untagged";
      dead = true;
      GetComponent<Animator>().enabled = false;
      GetComponent<CharacterAI>().enabled = false;
      SetRigidbodyState(false);
      // SetColliderState(true);
      Destroy(gameObject, 5f);
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
