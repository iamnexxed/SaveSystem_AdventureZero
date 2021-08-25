
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        SPEED,
        JUMP,
        HEALTH,
        AMMO,
        RAPIDFIRE
    }

    public CollectibleType itemType;

    [HideInInspector]
    public string itemAction;

    // public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // To increase the power in terms of percentage
    public float incrementMultiplierAmount = 0.1f;

    // Increase ammo
    public int ammoAmount = 10;

    public float rapidFireTime = 5f;

    private void Start()
    {
        
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        // transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(itemType)
        {
            case CollectibleType.SPEED:
                if(other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    Debug.Log("Speed increased by : " + incrementMultiplierAmount);
                    other.GetComponent<Character>().speed += other.GetComponent<Character>().speed * incrementMultiplierAmount;
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.JUMP:
                if (other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    Debug.Log("Jump increased by : " + incrementMultiplierAmount);
                    other.GetComponent<Character>().jumpSpeed += other.GetComponent<Character>().jumpSpeed * incrementMultiplierAmount;
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.HEALTH:
                if (other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    Health.instance.IncreaseHealth(1);
                    Debug.Log("Health pickup collected. Current health : " + Health.instance.life);
                    Destroy(gameObject);
                }
                break;
            case CollectibleType.AMMO:
                if (other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    if (!other.GetComponent<WeaponPickUp>().weapon)
                        return;

                    other.GetComponent<WeaponPickUp>().weapon.AddAmmo(ammoAmount);
                    other.GetComponent<WeaponPickUp>().ammoText.text = other.GetComponent<WeaponPickUp>().weapon.ammo.ToString();
                    Debug.Log("Ammo pickup collected.");
                    Destroy(gameObject);
                }
                break;

            case CollectibleType.RAPIDFIRE:
                if (other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    if (!other.GetComponent<WeaponPickUp>().weapon)
                        return;

                    other.GetComponent<WeaponPickUp>().weapon.RapidFire(rapidFireTime);
                    other.GetComponent<WeaponPickUp>().ammoText.text = other.GetComponent<WeaponPickUp>().weapon.ammo.ToString();


                    Debug.Log("Activate Rapid Fire!");
                    Destroy(gameObject);
                }
                break;
        }
    }
}
