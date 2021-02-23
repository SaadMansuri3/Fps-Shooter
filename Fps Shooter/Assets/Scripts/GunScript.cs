using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem smokeEffect;
    public GameObject bulletImpact;
    public float Damage = 10f;
    public float Range = 100f;
    public float impactForce = 50f;
    public float fireRate = 15f;

    public int maxAmmo = 10;
    public float reloadTime = 1f;

    private int currentAmmo;
    private float nextTimeForFire = 0f;
    private bool isReloading = false;

    public Animator anim;

    private void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeForFire)
        {
            nextTimeForFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
  
    void Shoot()
    {
        muzzleFlash.Play();
        smokeEffect.Play();

        currentAmmo--;
         
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, Range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            
            if(target != null)
            {
                target.TakeDamage(Damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal*impactForce);
            }

            GameObject impactGO = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        anim.SetBool("Reloading",true);
        yield return new WaitForSeconds(reloadTime-.25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = maxAmmo;
    }
}
