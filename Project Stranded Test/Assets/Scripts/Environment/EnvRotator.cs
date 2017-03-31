using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvRotator : MonoBehaviour {

    public float rotationSpeedX;
    public float rotationSpeedY;
    public float rotationSpeedZ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
	}
}
