using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] int numberOfSpawnPoints = 5;
    [SerializeField] int maxX = 7;
    [SerializeField] int maxZ = 7;
    [SerializeField] float yPos = 0.5f;
    [SerializeField] PickUpSpawn spawner;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numberOfSpawnPoints; i++)
        {
            float xPos;
            float zPos;
            do
            {
                xPos = (float)Random.Range(-maxX, maxX);
                zPos = (float)Random.Range(-maxZ, maxZ);
            } while (xPos == 0f && zPos == 0f);
            
            Vector3 spawnPos = new Vector3(xPos, yPos, zPos);
            Instantiate(spawner, spawnPos, spawner.transform.rotation);
        }

        try
        {
            if(numberOfSpawnPoints < 0)
            {
                throw new UnityException("Spawn Points should be greater than 0");
            }
            if(yPos < 1f)
            {
                throw new UnityException("Set appropriate Y Spawn position so objects don't overlap with the ground");
            }
        }
        catch(UnityException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

   
}
