using UnityEngine;
using System.Collections;

// To-do:
// - Initially deactivate the buttons after getting their position

public class UISpiderButton : MonoBehaviour {

    public GameObject[] spiderButtons;
    bool isActive = false;

    // Use this for initialization
    void Start(){

    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    // Make all objects in the array active when active and vice versa
    public void ToggleSpiderButtons()
    {
        if (isActive)
        {
            foreach (GameObject spiderButton in spiderButtons)
            {
                if (spiderButton.GetComponent(typeof(UILerpMovement)) != null)
                {
                    spiderButton.GetComponent<UILerpMovement>().ReverseLerp();
                }

                if (spiderButton.transform.position == spiderButton.GetComponent<UILerpMovement>().objectLocationToTarget.transform.position)
                {
                    spiderButton.SetActive(false);
                }
                
                isActive = false;
            }
        }
        else
        {
            foreach (GameObject spiderButton in spiderButtons)
            {
                spiderButton.SetActive(true);

                if (spiderButton.GetComponent(typeof(UILerpMovement)) != null)
                {
                    spiderButton.GetComponent<UILerpMovement>().ActivateLerp();
                }

                isActive = true;
            }
        }
    }


}
