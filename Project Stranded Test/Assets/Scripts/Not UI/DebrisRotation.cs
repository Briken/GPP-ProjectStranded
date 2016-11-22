using UnityEngine;
using System.Collections;

public class DebrisRotation : MonoBehaviour {

    public float rotationAmountX = 50.0f;
    public float rotationAmountY = 45.0f;
    public float rotationAmountZ = 40.0f;

    public float translateAmountX = 0.1f;
    public float translateAmountY = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Rotate(rotationAmountX * Time.deltaTime, rotationAmountY * Time.deltaTime, rotationAmountZ * Time.deltaTime);
        gameObject.transform.Translate(translateAmountX * Time.deltaTime, translateAmountY * Time.deltaTime, 0);
    }
}
