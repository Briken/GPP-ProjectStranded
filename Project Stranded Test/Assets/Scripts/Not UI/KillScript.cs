using UnityEngine;
using System.Collections;
using Photon;

public class KillScript : PunBehaviour {
    
    public float spawnTime = 10;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SurvivalTime(spawnTime));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [PunRPC]
    void Kill()
    {
        Destroy(this.gameObject);
    }

    IEnumerator SurvivalTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        this.photonView.RPC("Kill", PhotonTargets.All);
    }
}
