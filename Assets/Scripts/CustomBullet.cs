using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public AudioClip impactSound;
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisionNum;
    PhysicMaterial physics_mat;
    

    private void FixedUpdate()
    {
      // When to explode
      if(collisionNum > maxCollisions) Explode();

      // Count down time
      maxLifetime -= Time.deltaTime;
      if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
      if (explosion != null) 
      {
        // AudioSource.PlayClipAtPoint(impactSound, transform.position);
        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionEffect, 0.2f);
      }

      Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
      foreach (Collider objectHit in colliders)
      {
        Rigidbody rb = objectHit.GetComponent<Rigidbody>();
        Target targetHit = objectHit.GetComponent<Target>();
        if (targetHit != null && rb != null && !targetHit.damageApplied) 
        {
          rb.AddExplosionForce(explosionForce, transform.position, explosionRange);
          targetHit.TakeDamage(explosionDamage);
        }
      }
      Invoke(nameof(Delay), 0.05f);
    }

    private void Delay()
    {
      Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Gun")) return;

      if (explodeOnTouch) Explode();

      if (!collision.collider.CompareTag("Bullet") && !collision.collider.CompareTag("Gun")) 
      {
        collisionNum++;
      }
    }

    private void Setup()
    {
      physics_mat = new PhysicMaterial();
      physics_mat.bounciness = bounciness;
      physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
      physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
      
      GetComponent<SphereCollider>().material = physics_mat;

      rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
