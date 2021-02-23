using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectile;
    public float ProjectileForwardForce = 20f;
    public float ProjectileUpwardForce = 4.2f;
    public void ShootProjectile()
    {
        Rigidbody rb = Instantiate(projectile, transform.position + 1 * transform.forward, transform.rotation).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * ProjectileForwardForce, ForceMode.Impulse);
        rb.AddForce(transform.up * ProjectileUpwardForce, ForceMode.Impulse);
    }
}
