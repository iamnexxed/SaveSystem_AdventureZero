
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Character>().checkPointsCrossed++;
            GameManager.Instance.SaveGame();
            gameObject.SetActive(false);
        }
    }
}
