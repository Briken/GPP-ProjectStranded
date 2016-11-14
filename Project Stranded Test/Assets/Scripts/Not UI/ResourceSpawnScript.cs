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

    int mcount;
    int scount;
    int lcount;

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
            if (mediumResources[x] == null && mcount < mediumResources.GetLength(0))
            {
                mcount++;
                StartCoroutine(Spawn(medResourcePrefab, x));
            }
            else
            {
                if (mediumResources[x] !=null)
                {
                    if (mcount == mediumResources.GetLength(0))
                    {
                        mcount = 0;
                    }
                    Debug.Log("made it into the medium resource not null");
                    Debug.Log(mediumResources);
                    continue;
                }
            }
        }

        for (int x = 0; x < smallResources.GetLength(0); x++)
        {
            if (smallResources[x] == null && scount < smallResources.GetLength(0))
            {
                StartCoroutine(Spawn(smallResourcePrefab, x));
            }
            else
            {
                if (smallResources[x] != null)
                {
                    if (scount == smallResources.GetLength(0))
                    {
                        scount = 0;
                    }
                    continue;
                }
            }
        }

        for (int x = 0; x < largeResources.GetLength(0); x++)
        {
            if (largeResources[x] == null && lcount < largeResources.GetLength(0))
            {
                StartCoroutine(Spawn(largeResourcePrefab, x));
            }
            else
            {
                if (largeResources[x] != null)
                {
                    if (lcount == largeResources.GetLength(0))
                    {
                        lcount = 0;
                    }
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
