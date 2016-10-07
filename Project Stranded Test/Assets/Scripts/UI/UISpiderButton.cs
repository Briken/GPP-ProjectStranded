using UnityEngine;
using System.Collections;

public class UISpiderButton : MonoBehaviour {

    public GameObject[] spiderButtons;
    bool isActive = false;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject spiderButton in spiderButtons)
        {
            spiderButton.SetActive(false);
        }
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
                spiderButton.SetActive(false);
                isActive = false;
            }
        }
        else
        {
            foreach (GameObject spiderButton in spiderButtons)
            {
                spiderButton.SetActive(true);
                isActive = true;
            }
        }
    }


}
