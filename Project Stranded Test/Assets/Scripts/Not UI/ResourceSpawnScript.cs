using UnityEngine;
using System.Collections;
using Photon;

public class ResourceSpawnScript : PunBehaviour {

    public GameObject largeResourcePrefab;
    public GameObject medResourcePrefab;
    public GameObject smallResourcePrefab;

    public GameObject[] mediumResources;
    public GameObject[] smallResources;
    public GameObject[] largeResources;

    GameObject[] spawnPoints;

    public int spawnTime = 15;

	// Use this for initialization
	void Start ()
    {
        mediumResources = new GameObject[2];
        smallResources = new GameObject[3];
        largeResources = new GameObject[1];
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    for (int x = 0; x < mediumResources.GetLength(0); x++)
        {
            if (mediumResources[x] == null)
            {
                Debug.Log("made it into the medium resource null loop");
                StartCoroutine(Spawn(medResourcePrefab, x));
            }
            else
            {
                if (mediumResources[x] !=null)
                {
                    Debug.Log(mediumResources);
                    continue;
                }
            }
        }

        for (int x = 0; x < smallResources.GetLength(0); x++)
        {
            if (smallResources[x] == null)
            {
                StartCoroutine(Spawn(smallResourcePrefab, x));
            }
            else
            {
                if (smallResources[x] != null)
                {
                    continue;
                }
            }
        }

        for (int x = 0; x < largeResources.GetLength(0); x++)
        {
            if (largeResources[x] == null)
            {
                StartCoroutine(Spawn(largeResourcePrefab, x));
            }
            else
            {
                if (largeResources[x] != null)
                {
                    continue;
                }
            }
        }

    }

    IEnumerator Spawn(GameObject resource, int arrayPos)
    {
        Debug.Log("made it into the coroutine");
        yield return new WaitForSeconds(spawnTime);
        int spawnPoint = (int)Random.Range(0, spawnPoints.GetLength(0));
        GameObject newResource = PhotonNetwork.Instantiate(resource.name, spawnPoints[spawnPoint].transform.position, Quaternion.identity, 0);
        if (newResource.tag == "Small")
        {
            smallResources[arrayPos] = newResource;
        }
        if (newResource.tag == "Medium")
        {
            mediumResources[arrayPos] = newResource;
        }
        if (newResource.tag == "Large")
        {
            largeResources[arrayPos] = newResource;
        }
    }
}
