using UnityEngine;
// using TMPro;

public class Gun : MonoBehaviour
{
    public Transform attackPoint;

    // Gun Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold, individualReload;

    int bulletsLeft, bulletsShot;

    // Bullet Stats
    public GameObject bullet;
    public float shootForce, upwardForce;

    public bool shooting;
    bool readyToShoot, reloading;

    // Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    // Effects
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject hitMarkerEffect;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    private AudioSource audio;

    
    // public TextMeshProUGUI ammunitiionDisplay;
    public Camera fpsCam;
    public Animator animator;
    public bool allowInvoke = true;
    
    private void Start()
    {
      audio = GetComponent<AudioSource>();
      bulletsLeft = magazineSize;
      readyToShoot = true;
    }

    private void Update()
    {
      if (transform.root.name == "Player") MyInput();
      if (transform.root.name != "Player" && transform.root.tag != "Gun") CharacterInput();

      // if (ammunitionDisplay != null) 
      //   ammunitiionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
      // Semi or auto
      if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
      else shooting = Input.GetKeyDown(KeyCode.Mouse0);
      // Reloading
      if(Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
      if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
      if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
      {
        bulletsShot = 0;
        Shoot();
      }
    }

    private void CharacterInput()
    {
      if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
      if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
      {
        bulletsShot = 0;
        Shoot();
      }
    }

    public void EmptyRounds()
    {
      shooting = true;
      if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
      {
        bulletsShot = 0;
        Shoot();
        shooting = false;
        if (allowButtonHold) EmptyRounds();
      }
    }

    private void Shoot()
    {
      audio.PlayOneShot(shootSound);
      readyToShoot = false;
      // Find bullet arc
      Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
      RaycastHit hit;
      // Check if bullet hit something
      Vector3 targetPoint;
      if (Physics.Raycast(ray, out hit)) targetPoint = hit.point;
      else targetPoint = ray.GetPoint(75);
      // Direction
      Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
      // Spread
      float x = Random.Range(-spread, spread);
      float y = Random.Range(-spread, spread);
      // Add spread
      Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

      // Instantiate
      GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
      currentBullet.transform.forward = directionWithSpread;

      // Add forces to bullet
      currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
      currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

      bulletsLeft--;
      bulletsShot++;

      // Effects
      if (muzzleFlash != null)
      {
        ParticleSystem flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Destroy(flash, 0.2f);
      }

      if (allowInvoke)
      {
        Invoke(nameof(ResetShot), timeBetweenShooting);
        allowInvoke = false;

        // Add recoil to player
        // playerRb.isKinematic = false;
        // playerRb.velocity = -directionWithSpread.normalized * recoilForce;
      }
      if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
      // Multiple Projectile Gun
      {
        Invoke(nameof(Shoot), timeBetweenShots);
      }
    }

    private void ResetShot()
    {
      playerRb.isKinematic = true;
      readyToShoot = true;
      allowInvoke = true;
    }

    private void Reload()
    {
      animator.SetBool("Reloading", true);
      reloading = true;
      audio.PlayOneShot(reloadSound);
      if(individualReload)
      {
        Invoke(nameof(IndividualReload), reloadTime);
      } else
      {
        Invoke(nameof(ReloadFinished), reloadTime);
      }
    }
    private void IndividualReload()
    {
      bulletsLeft += bulletsPerTap;
      animator.SetBool("Reloading", false);
      reloading = false;
    }

    private void ReloadFinished()
    {
      animator.SetBool("Reloading", false);
      bulletsLeft = magazineSize;
      reloading = false;
    }
}
