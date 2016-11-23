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

    bool sRoutine;
    bool mRoutine;
    bool lRoutine;

    GameObject[] spawnPoints;

    public int spawnTime = 15;

	// Use this for initialization
	void Start ()
    {
        Debug.Log("this network manager's user id is " + this.photonView.ownerId.ToString());
        mediumResources = new GameObject[3];
        smallResources = new GameObject[1];
        largeResources = new GameObject[2];
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (this.photonView.ownerId == 0)
        {
            for (int x = 0; x < mediumResources.GetLength(0); x++)
            {
                if (mediumResources[x] == null && !mRoutine)
                {
                    mRoutine = true;

                    StartCoroutine(Spawn(medResourcePrefab, x));
                }
                else
                {
                    if (mediumResources[x] != null)
                    {
                        continue;
                    }
                }
            }

            for (int x = 0; x < smallResources.GetLength(0); x++)
            {
                if (smallResources[x] == null && !sRoutine)
                {
                    sRoutine = true;
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
                if (largeResources[x] == null && !lRoutine)
                {
                    lRoutine = true;
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
    }

    IEnumerator Spawn(GameObject resource, int arrayPos)
    {
        
        yield return new WaitForSeconds(spawnTime);
        int spawnPoint = (int)Random.Range(0, spawnPoints.GetLength(0));
        GameObject newResource = PhotonNetwork.Instantiate(resource.name, spawnPoints[spawnPoint].transform.position, Quaternion.identity, 0);

        if (newResource.tag == "Small")
        {
            smallResources[arrayPos] = newResource;
            sRoutine = false;
        }
        if (newResource.tag == "Medium")
        {
            mediumResources[arrayPos] = newResource;
            mRoutine = false;
        }
        if (newResource.tag == "Large")
        {
            largeResources[arrayPos] = newResource;
            lRoutine = false;
        }
    }
}
