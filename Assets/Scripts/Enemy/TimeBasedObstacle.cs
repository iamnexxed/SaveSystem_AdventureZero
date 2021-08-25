using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedObstacle : MonoBehaviour
{
    public int damage;
    public float damageTime;
    float timeSinceLastDamage;
    // Start is called before the first frame update
    void Start()
    {
        if (damage <= 0)
        {
            damage = 2;
        }
        if(damageTime <= 0f)
        {
            damageTime = 2f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Health.instance.isAlive)
                return;

            if (Time.time > timeSinceLastDamage + damageTime)
            {
                Health character = other.GetComponent<Health>();
                if (character)
                {
                    character.DamagePlayer(damage);
                }
                timeSinceLastDamage = Time.time;
            }
           
        }
    }
}
