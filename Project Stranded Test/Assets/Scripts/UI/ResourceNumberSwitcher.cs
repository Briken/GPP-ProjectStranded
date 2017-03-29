using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNumberSwitcher : MonoBehaviour {

    public Sprite[] numberSprites;
    public Sprite timeSprite;
    public Sprite voteSprite;
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

        //{
        //    spriteNumber.GetComponent<SpriteRenderer>().sprite = timeSprite;
        //}
        //else if (initialNumber - attachedResourceScript.nearby.Count < 0)
        //{
        //    spriteNumber.GetComponent<SpriteRenderer>().sprite = voteSprite;
        //}
        //else
        //{
        //    spriteNumber.GetComponent<SpriteRenderer>().sprite = numberSprites[initialNumber - attachedResourceScript.nearby.Count];
        //}
        
	}
}
