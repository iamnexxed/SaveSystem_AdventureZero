using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
 
    public Rigidbody projectile;            // Used to Create projectile
    public int ammo;                        // Used to keep track of how much ammo there is
    public Transform projectileSpawnPoint;  // Used to position the bullet once spawned
    public float projectileForce;           // Used to apply force to the bullet being fired
    public float fireRate = 4f;
    public float fireRateMultiplier = 3f;

    int currentAmmo;
    // Use this for initialization
    void Start () {

        if (ammo <= 0)
        {
            // Set the ammo count to 20
            ammo = 20;
        }

        if (projectileForce <= 0)
        {
            // Set the bullet force
            projectileForce = 3.0f;
        }
	}

    public int Shoot()
    {
        // Check if there is enough ammo
        if (projectile && ammo > 0)
        {
            // Create the bullet if there is enough ammo
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;

            // Add the force to fire the bullet
            temp.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            // Remove one ammo count
            ammo--;
        }
        // Do something if there isnt enough ammo
        else
        {
            // Play audio for reload

            // Print message
            // Debug.Log("Reload");
        }

        return ammo;
    }

    public void AddAmmo(int value = 5)
    {
        ammo += value;
    }

    public void RapidFire(float timeToDisable)
    {
        InfiniteFire();
        StartCoroutine(StartRapidFire(timeToDisable));
    }

    IEnumerator StartRapidFire(float timeToDisable)
    {
        
        // Wait for a specified amount of time
        yield return new WaitForSeconds(timeToDisable);
        Debug.Log("Restore Ammo");
        RestoreAmmo();
    }

    void InfiniteFire()
    {
        currentAmmo = ammo;
        ammo = 9999999;
        fireRate *= fireRateMultiplier;
    }

    void RestoreAmmo()
    {
        ammo = currentAmmo;
        fireRate /= fireRateMultiplier;
    }
}

/*
abstract class Weapon
{
    public abstract int Shoot();
}
public class RangedWeapon : Weapon
{
    public override int Shoot()
    {
        return base.Shoot();
    }
}
*/