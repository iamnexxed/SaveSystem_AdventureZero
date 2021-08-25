using UnityEngine;
using System.Collections;
using TMPro;

public class WeaponPickUp : MonoBehaviour 
{
    [HideInInspector]
    public Weapon weapon;               // Used to store the Weapon that gets picked up, must have "Weapon.CS" attached

    [HideInInspector]
    public Weapon primaryWeapon;
    [HideInInspector]
    public Weapon secondaryWeapon;

    public GameObject activeWeaponAttach;     // Used to place Weapon Object in Player
    public GameObject passiveWeaponAttach;

    public float weaponDropForce;       // Used to add a force to the 'Weapon' when dropped

    public TMP_Text ammoText;           // Used to show ammo left when a 'Weapon' is active

    float nextTimeToFire = 0f;

	// Use this for initialization
	void Start () 
    {
        // Weapon should be left empty
        // - Remove this line if the weapon is manually added as a child of "WeaponPlacement"
        weapon = null;

        // Do not display ammo because there is no weapon on "Character"
        ammoText.text = string.Empty;

        // Check if weaponAttach exists and was dragged into variable
        if (!activeWeaponAttach)
        {
            // Find the point to attach the weapon to
            // - WeaponPlacement is an Empty GameObject used to connect weapon to
            activeWeaponAttach = GameObject.Find("WeaponPlacement");
        }

        if (weaponDropForce <= 0)
        {
            weaponDropForce = 10.0f;

            Debug.Log("WeaponDropForce not set on " + name + ". Defaulting to " + weaponDropForce);
        }

    }

    // Update is called once per frame
    void Update () {
	    
        // Drop weapon when 'T' is pressed
	    if(Input.GetKeyDown(KeyCode.T))
	    {
		    // Is there a weapon to drop
		    if(weapon)
		    {
                // Remove weapon as a Child of Player
                activeWeaponAttach.transform.DetachChildren();

                // Turn collision back on
                StartCoroutine(EnableCollisions(weapon, 1f));

                // Turn Physics back on	
                weapon.GetComponent<Rigidbody>().isKinematic = false;
                weapon.GetComponent<Rigidbody>().useGravity = true;

                // Throw Weapon forward
                weapon.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * weaponDropForce, ForceMode.Impulse);

                // Do not display ammo because there is no weapon on "Character"
                ammoText.text = string.Empty;

            }

            // If object exists on passive
            if(passiveWeaponAttach.transform.childCount > 0)
            {
                if(passiveWeaponAttach.transform.GetChild(0).CompareTag("PrimaryWeapon"))
                {
                    // Move Weapon to weaponAttach position
                    primaryWeapon.transform.position = activeWeaponAttach.transform.position;
                    // Make weaponAttach the parent of Weapon
                    primaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                    // Rotate it to the parent identity
                    primaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                    // Show Ammo
                    ammoText.text = primaryWeapon.ammo.ToString();

                    weapon = primaryWeapon;
                    secondaryWeapon = null;
                }
                else if (passiveWeaponAttach.transform.GetChild(0).CompareTag("SecondaryWeapon"))
                {
                    // Move Weapon to weaponAttach position
                    secondaryWeapon.transform.position = activeWeaponAttach.transform.position;
                    // Make weaponAttach the parent of Weapon
                    secondaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                    // Rotate it to the parent identity
                    secondaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                    // Show Ammo
                    ammoText.text = secondaryWeapon.ammo.ToString();

     
                    weapon = secondaryWeapon;
                    primaryWeapon = null;
                }
            }
            else
            {
                weapon = null;
            }
	    }

	    // Check if the Fire key was pressed
	    if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
	    {
            // Check if there is weapon attached to the Player
            // Add a fire rate which can be controlled through the power ups
            if (!weapon)
                return; 
            nextTimeToFire = Time.time + 1/weapon.fireRate;
		    

            // Print ammo to screen
            ammoText.text = weapon.Shoot().ToString();
		    
	    }

        // Check if Tab key was pressed
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon();
        }
	}

    // Must set Collider to isTrigger to function
    void OnTriggerEnter(Collider other)
    {
        // Did the player collide with a weapon
        if (other.gameObject.CompareTag("Weapon"))
        {
            // Store a copy of Weapon
            

            // Stop applying Physics to Weapon	
            

            // Move Weapon to weaponAttach position
            

            // Make weaponAttach the parent of Weapon
            

            // Rotate it to the parent identity
            

            // Stop Collision between Player and Weapon
            
        }	
    }

    // Does not work with the Character Controller
    void OnCollisionEnter(Collision c)
    {
	
	
    }

    // Used when working with a Character Controller to check for collision
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
	   
        // Did 'Player' collide with a GameObject tagged as "Weapon"
        if (hit.collider.CompareTag("PrimaryWeapon"))
        {
            // Store a copy of Weapon
            primaryWeapon = hit.gameObject.GetComponent<Weapon>();

            // Stop applying Physics to Weapon	
            primaryWeapon.GetComponent<Rigidbody>().isKinematic = true;
            primaryWeapon.GetComponent<Rigidbody>().useGravity = false;
            if(!weapon)
            {
                // Move Weapon to weaponAttach position
                primaryWeapon.transform.position = activeWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                primaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                // Rotate it to the parent identity
                primaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                // Show Ammo
                ammoText.text = primaryWeapon.ammo.ToString();

                // Stop Collision between Player and Weapon
                Physics.IgnoreCollision(primaryWeapon.GetComponent<Collider>(), GetComponent<Collider>());
                weapon = primaryWeapon;

                // Debug.Log("Active weapon will be : " + hit.collider.tag);
            }
            else
            {
                // Move Weapon to weaponAttach position
                primaryWeapon.transform.position = passiveWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                primaryWeapon.transform.SetParent(passiveWeaponAttach.transform);

                // Rotate it to the parent identity
                primaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
                // Debug.Log("Passive weapon will be : " + hit.collider.tag);
            }

            

        }

        // Did 'Player' collide with a GameObject tagged as "Weapon"
        if (hit.collider.CompareTag("SecondaryWeapon"))
        {
            // Store a copy of Weapon
            secondaryWeapon = hit.gameObject.GetComponent<Weapon>();

            // Stop applying Physics to Weapon	
            secondaryWeapon.GetComponent<Rigidbody>().isKinematic = true;
            secondaryWeapon.GetComponent<Rigidbody>().useGravity = false;
            if(!weapon)
            {
                // Move Weapon to weaponAttach position
                secondaryWeapon.transform.position = activeWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                secondaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                // Rotate it to the parent identity
                secondaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                // Show Ammo
                ammoText.text = secondaryWeapon.ammo.ToString();

                // Stop Collision between Player and Weapon
                Physics.IgnoreCollision(secondaryWeapon.GetComponent<Collider>(), GetComponent<Collider>());
                weapon = secondaryWeapon;
                // Debug.Log("Active weapon will be : " + hit.collider.tag);
            }
            else
            {
                // Move Weapon to weaponAttach position
                secondaryWeapon.transform.position = passiveWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                secondaryWeapon.transform.SetParent(passiveWeaponAttach.transform);

                // Rotate it to the parent identity
                secondaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
                // Debug.Log("Passive weapon will be : " + hit.collider.tag);
            }



        }
    }

    IEnumerator EnableCollisions(Weapon currentWeapon, float timeToDisable)
    {
        // Wait for a specified amount of time
        yield return new WaitForSeconds(timeToDisable);

        // Turn collision back on after timeToDisable seconds
        Physics.IgnoreCollision(currentWeapon.GetComponent<Collider>(), GetComponent<Collider>(), false);

        // Reset weapon to null so a new weapon can be collected
        // weapon = null;
    }

 

    void ChangeWeapon()
    {
        // If object exists on passive
        if (passiveWeaponAttach.transform.childCount > 0)
        {
            if (passiveWeaponAttach.transform.GetChild(0).CompareTag("PrimaryWeapon"))
            {
                // Move Weapon to weaponAttach position
                primaryWeapon.transform.position = activeWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                primaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                // Rotate it to the parent identity
                primaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                // Show Ammo
                ammoText.text = primaryWeapon.ammo.ToString();

                weapon = primaryWeapon;

                // Move Weapon to weaponAttach position
                secondaryWeapon.transform.position = passiveWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                secondaryWeapon.transform.SetParent(passiveWeaponAttach.transform);

                // Rotate it to the parent identity
                secondaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
               

            }
            else if (passiveWeaponAttach.transform.GetChild(0).CompareTag("SecondaryWeapon"))
            {
                // Move Weapon to weaponAttach position
                secondaryWeapon.transform.position = activeWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                secondaryWeapon.transform.SetParent(activeWeaponAttach.transform);

                // Rotate it to the parent identity
                secondaryWeapon.transform.localRotation = activeWeaponAttach.transform.localRotation;

                // Show Ammo
                ammoText.text = secondaryWeapon.ammo.ToString();


                weapon = secondaryWeapon;

                // Move Weapon to weaponAttach position
                primaryWeapon.transform.position = passiveWeaponAttach.transform.position;
                // Make weaponAttach the parent of Weapon
                primaryWeapon.transform.SetParent(passiveWeaponAttach.transform);

                // Rotate it to the parent identity
                primaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);

            }
        }
    }

    
}
