using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon;

public class UIBarScript : PunBehaviour {

    public GameObject fuelCarriedText;
    public GameObject fuelCarriedBar;
    public float barMaxValue = 20.0f;
    public float barValue = 0.0f;
    // Use this for initialization
    void Start () {
        fuelCarriedText = GameObject.Find("Text - Fuel");
        fuelCarriedBar = GameObject.Find("Slider - Fuel");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (photonView.isMine)
        {
            fuelCarriedText.GetComponent<Text>().text = "FUEL: " + GetComponent<PlayerResource>().resource.ToString();

            // Fuel bar handling:
            if (barValue > this.gameObject.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue - (0.4f * Time.deltaTime), 0.0f, 1.0f);
            }
            else if (barValue <= this.gameObject.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue + (0.15f * Time.deltaTime), 0.0f, 1.0f);
            }

            fuelCarriedBar.GetComponent<Slider>().value = barValue;
        }
    }
}
