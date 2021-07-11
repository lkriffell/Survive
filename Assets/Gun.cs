using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 500f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject hitMarkerEffect;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    public AudioSource audio;

    public Camera fpsCam;

    private float nextTimeToFire = 0f;

    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {   
        if (isReloading)
        {
          return;
        }
        if (currentAmmo <= 0)
        {
          StartCoroutine(Reload());
          return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
          nextTimeToFire = Time.time + 1f / fireRate;
          Shoot();
        }
    }

    void Shoot()
    { 
        audio.PlayOneShot(shootSound);
        muzzleFlash.Play();
        currentAmmo --;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
          Target target = hit.transform.GetComponentInParent<Target>();
          Debug.Log(target);

          if (target != null)
          {
            target.TakeDamage(damage);
          }
          GameObject impactGO;
          if (hit.rigidbody != null)
          {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
            impactGO = Instantiate(hitMarkerEffect, hit.point, Quaternion.LookRotation(hit.normal));
          } else
          {
            impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
          }
          Destroy(impactGO, 2f);
        }
    }

    IEnumerator Reload ()
    {
      animator.SetBool("Reloading", true);
      isReloading = true;

      yield return new WaitForSeconds(reloadTime - .25f);
      audio.PlayOneShot(reloadSound);
      animator.SetBool("Reloading", false);
      yield return new WaitForSeconds(.25f);

      currentAmmo = maxAmmo;
      isReloading = false;
    }
}
