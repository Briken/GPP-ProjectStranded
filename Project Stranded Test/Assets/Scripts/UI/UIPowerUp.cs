using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPowerUp : MonoBehaviour {

    public GameObject powerUpSlip;
    public GameObject powerUpSticker;
    public GameObject powerUpName;

    /* Power-ups are numbered as such:
     * 0 = Mystery
     * (BUFFS)
     * 1 = Speed Boost
     * 2 = Bonus Fuel
     * 3 = Shield
     * 4 = Reflect
     * (DEBUFFS)
     * 5 = Freeze
     * 6 = Slow Down
     * 7 = Fuel Leak
     * 8 = Punch
     * 9 = Outta Control
     * (UTILITY)
     * 10 = Hook
     * 11 = Switcharoo
     * 12 = Trap
     * (GLOBAL)
     * 13 = Chicken
     * 14 = Spy
     */
    public int powerUpNumber = 0;
    public Sprite[] powerUpStickers;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RevealPowerUp(int requestedPowerUpNumber)
    {
        powerUpNumber = requestedPowerUpNumber;

        switch (powerUpNumber)
        {
            case 0:
                powerUpSlip.GetComponent<Image>().color = new Color(140.0f / 255.0f, 0.0f / 255.0f, 150.0f / 255.0f);
                powerUpSticker.GetComponent<Image>().sprite = powerUpStickers[0];
                powerUpName.GetComponent<Text>().text = "???";
                break;

            case 1:
                powerUpSlip.GetComponent<Image>().color = Color.red;
                powerUpSticker.GetComponent<Image>().sprite = powerUpStickers[1];
                powerUpName.GetComponent<Text>().text = "SPEED BOOST";
                break;

            case 2:
                powerUpSlip.GetComponent<Image>().color = Color.magenta;
                powerUpSticker.GetComponent<Image>().sprite = powerUpStickers[2];
                powerUpName.GetComponent<Text>().text = "IT WORKS!!!";
                break;
        }

        gameObject.GetComponent<UILerpMovement>().ActivateLerp();
    }

    public void HidePowerUp()
    {
        gameObject.GetComponent<UILerpMovement>().ReverseLerp();
    }
}
