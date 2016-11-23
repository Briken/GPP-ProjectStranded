using UnityEngine;
using System.Collections;

public class SpawnPointIconRemover : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // Remove that sprite
        Destroy(gameObject.GetComponent<SpriteRenderer>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
