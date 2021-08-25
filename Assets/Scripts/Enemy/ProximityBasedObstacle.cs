
using UnityEngine;

public class ProximityBasedObstacle : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        if(damage <= 0)
        {
            damage = 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Health character = other.GetComponent<Health>();
            if(character)
            {
                character.DamagePlayer(damage);
            }
        }
    }
}
