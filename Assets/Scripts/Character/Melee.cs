
using UnityEngine;

public class Melee : MonoBehaviour
{

    public enum MeleeType
    {
        Player,
        Enemy
    }

    public MeleeType currentType;

    public int damageValue = 1;
    private void OnTriggerEnter(Collider other)
    {
        switch (currentType)
        {
            case MeleeType.Player:
                if (other.CompareTag("Enemy"))
                {
                    if (!other.gameObject.GetComponent<Enemy>().isAlive)
                        return;
                    // Debug.Log("Hit enemy");
                    other.gameObject.GetComponent<Enemy>().DamageEnemy(damageValue);
                }
                break;

            case MeleeType.Enemy:
                if (other.CompareTag("Player"))
                {
                    if (!Health.instance.isAlive)
                        return;
                    // Debug.Log("Hit enemy");
                    Health.instance.DamagePlayer(damageValue);
                }
                break;
        }
    }
}
