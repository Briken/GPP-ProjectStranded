using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCircle : MonoBehaviour {

    public Vector3 growthRate = new Vector3(0.1f, 0.1f, 0.1f);
    public float opacityReductionSpeed = 1.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localScale += growthRate*Time.deltaTime;

        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a - (opacityReductionSpeed*Time.deltaTime));

    }
}
