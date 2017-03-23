using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNumberSwitcher : MonoBehaviour {

    public Sprite[] numberSprites;
    public GameObject spriteNumber;
    public int initialNumber;
    public int currentNumber;
    ResourceScript attachedResourceScript;

	// Use this for initialization
	void Start ()
    {

        attachedResourceScript = gameObject.GetComponent<ResourceScript>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        spriteNumber.GetComponent<SpriteRenderer>().sprite = numberSprites[initialNumber - attachedResourceScript.nearby.Count];
	}
}
