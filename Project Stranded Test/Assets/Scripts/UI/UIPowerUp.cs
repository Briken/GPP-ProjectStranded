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
        powerUpSticker.GetComponent<Image>().sprite = powerUpStickers[powerUpNumber];

        gameObject.GetComponent<UILerpMovement>().ActivateLerp();

        switch (powerUpNumber)
        {
            case 0: // Mystery
                powerUpSlip.GetComponent<Image>().color = new Color(140.0f / 255.0f, 0.0f / 255.0f, 150.0f / 255.0f);
                powerUpName.GetComponent<Text>().text = "???";
                break;

            case 1: // Speed Boost
                powerUpSlip.GetComponent<Image>().color = Color.red;
                powerUpName.GetComponent<Text>().text = "SPEED BOOST";
                break;

            case 2: // Bonus Fuel
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "BONUS FUEL";
                break;

            case 3: // Shield
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "SHIELD";
                break;

            case 4: // Reflect
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "REFLECT";
                break;

            case 5: // Freeze
                powerUpSlip.GetComponent<Image>().color = new Color(0.0f / 255.0f, 135.0f / 255.0f, 255.0f / 255.0f);
                powerUpName.GetComponent<Text>().text = "FREEZE";
                break;

            case 6: // Slow Down
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "SLOW DOWN";
                break;

            case 7: // Fuel Leak
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "FUEL LEAK";
                break;

            case 8: // Punch
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "PUNCH";
                break;

            case 9: // Outta Control
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "OUTTA CONTROL";
                break;

            case 10: // Hook
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "HOOK";
                break;

            case 11: // Switcharoo
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "SWITCHAROO";
                break;

            case 12: // Trap
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "TRAP";
                break;

            case 13: // Chicken
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "CHICKEN";
                break;

            case 14: // Spy
                powerUpSlip.GetComponent<Image>().color = Color.grey;
                powerUpName.GetComponent<Text>().text = "SPY";
                break;
        }
    }

    public void HidePowerUp()
    {
        gameObject.GetComponent<UILerpMovement>().ReverseLerp();
    }
}
