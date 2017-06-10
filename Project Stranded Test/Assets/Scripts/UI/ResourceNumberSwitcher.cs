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
    public GameObject fillBarOuterLight;
    float fillBarOuterScale;
    
    ResourceScript attachedResourceScript;
    float initialWaitTime;

    public GameObject[] lightBeams;

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
                fillBarOuterScale = (1 - (attachedResourceScript.waitTimer / initialWaitTime)) * 0.06f + 0.005f;

                spriteNumber.GetComponent<SpriteRenderer>().sprite = timeSprite;
                fillBarImage.GetComponent<Image>().fillAmount = 1 - (attachedResourceScript.waitTimer / initialWaitTime);
                fillBarOuterLight.GetComponent<Image>().fillAmount = 1 - (attachedResourceScript.waitTimer / initialWaitTime);
                fillBarOuterLight.GetComponent<RectTransform>().localScale = new Vector3(fillBarOuterScale, fillBarOuterScale, fillBarOuterScale);
            }
            else if (initialNumber - attachedResourceScript.nearby.Count < 0)
            {
                spriteNumber.GetComponent<SpriteRenderer>().sprite = voteSprite;
            }
            else
            {
                spriteNumber.GetComponent<SpriteRenderer>().sprite = numberSprites[initialNumber - attachedResourceScript.nearby.Count];
            }

            // Change light beam colour based on nearby player count
            foreach (GameObject lightBeam in lightBeams)
            {
                if (initialNumber - attachedResourceScript.nearby.Count < 0 == true)
                {
                    lightBeam.GetComponent<FuelCrateLightBeam>().voteStarted = true;
                }

                if (System.Array.IndexOf(lightBeams, lightBeam) < attachedResourceScript.nearby.Count)
                {
                    lightBeam.GetComponent<FuelCrateLightBeam>().isActive = true;
                }
                else
                {
                    lightBeam.GetComponent<FuelCrateLightBeam>().isActive = false;
                }
            }
        }       
	}
}
