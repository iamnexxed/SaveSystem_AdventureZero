using UnityEngine;

public class PickUpSpawn : MonoBehaviour
{
    public GameObject[] pickupPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        int randomNum = Random.Range(0, pickupPrefabs.Length);

        Instantiate(pickupPrefabs[randomNum], transform.localPosition, transform.rotation);
        Destroy(gameObject);
    }
}