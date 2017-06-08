using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNumberSwitcher : MonoBehaviour {

    public Sprite[] numberSprites;
    public Sprite timeSprite;
    public Sprite voteSprite;
    public GameObject spriteNumber;
    public int initialNumber = 0;
    public int currentNumber;
    public GameObject fillBarImage;
    
    ResourceScript attachedResourceScript;
    float initialWaitTime;

    // Use this for initialization
    void Start ()
    {

        attachedResourceScript = gameObject.GetComponent<ResourceScript>();
        initialWaitTime = attachedResourceScript.waitTimer;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (attachedResourceScript != null)
        {
            if (initialNumber - attachedResourceScript.nearby.Count == 0)
            {
                spriteNumber.GetComponent<SpriteRenderer>().sprite = timeSprite;
                fillBarImage.GetComponent<Image>().fillAmount = attachedResourceScript.waitTimer / initialWaitTime;
            }
            else if (initialNumber - attachedResourceScript.nearby.Count < 0)
            {
                spriteNumber.GetComponent<SpriteRenderer>().sprite = voteSprite;
            }
            else
            {
                spriteNumber.GetComponent<SpriteRenderer>().sprite = numberSprites[initialNumber - attachedResourceScript.nearby.Count];
            }
        }       
	}
}
