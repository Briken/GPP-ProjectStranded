using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebrisSpawner : MonoBehaviour {

    public GameObject[] debrisGameObjects;
    public GameObject[] debrisSpawnPoints;

    public float debrisObjectAmount;
    float currentDebrisObjectAmount = 0;

    bool spawningComplete = false;

    public float minScaleAmount = 1.0f;
    public float maxScaleAmount = 1.0f;

	// Use this for initialization
	void Start ()
    {
        debrisSpawnPoints = GameObject.FindGameObjectsWithTag("Debris Spawn Point");

        // Check if we have enough spawn points for the amount requested, otherwise set as max
        if (debrisObjectAmount > debrisSpawnPoints.Length)
        {
            debrisObjectAmount = debrisSpawnPoints.Length;
        }

        foreach (GameObject debrisSpawnPoint in debrisSpawnPoints)
        {
            // Throw that spawn point sprite in the trash
            Destroy(debrisSpawnPoint.GetComponent<SpriteRenderer>());
        }
      
        while (!spawningComplete)
        {
            int randomDebrisSpawnObject = Random.Range(0, debrisSpawnPoints.Length - 1);

            if (debrisSpawnPoints[randomDebrisSpawnObject].GetComponent<DebrisSpawnPoint>().hasBeenUsed == false)
            {
                float randomScaleAmount = Random.Range(minScaleAmount, maxScaleAmount);

                GameObject spawnedDebris = (GameObject)Instantiate(debrisGameObjects[Random.Range(0, debrisGameObjects.Length - 1)], debrisSpawnPoints[randomDebrisSpawnObject].transform.localPosition, Random.rotation);
                spawnedDebris.transform.localScale = new Vector3(randomScaleAmount, randomScaleAmount, randomScaleAmount);

                debrisSpawnPoints[randomDebrisSpawnObject].GetComponent<DebrisSpawnPoint>().hasBeenUsed = true;
                currentDebrisObjectAmount++;
            }

            if (currentDebrisObjectAmount == debrisObjectAmount)
            {
                spawningComplete = true;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        // debrisSpawnPoints = GameObject.FindGameObjectsWithTag("Debris Spawn Point");
    }
}
