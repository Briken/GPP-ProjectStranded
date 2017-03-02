using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCircle : MonoBehaviour {

    public Vector3 growthRate = new Vector3(0.1f, 0.1f, 0.1f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localScale += growthRate;
	}
}
