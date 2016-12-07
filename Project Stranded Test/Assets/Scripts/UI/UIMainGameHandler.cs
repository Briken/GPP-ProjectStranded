﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMainGameHandler : MonoBehaviour {

    public GameObject timeRemainingText;
    public GameObject fuelCarriedText;
    public GameObject fuelCarriedBar;
    public float barMaxValue = 20.0f;
    public float barValue = 0.0f;

    GameObject mainPlayer;
    float gameTimeRemaining;

    bool noMain = true;

    [Header("Power-up Indicators")]
    public GameObject powerUpIndicatorSpeedBoost;
    public GameObject powerUpIndicatorFreeze;
    Color powerUpIndicatorInactiveColour = new Color (1.0f, 1.0f, 1.0f, 0.2f);
    Color powerUpIndicatorActiveColour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (noMain)
        {
            foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (n.GetComponent<MovementScript>().playerNum == 1)
                {
                    mainPlayer = n;
                    noMain = false;
                }
            }
        }
        // Game time handling:
        //   mainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (mainPlayer != null)
        {
            gameTimeRemaining = mainPlayer.GetComponent<GameTimer>().timer;
        }
        int minutes = Mathf.FloorToInt(gameTimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(gameTimeRemaining - (minutes * 60));
        string gameTimeString = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeRemainingText.GetComponent<Text>().text = "TIME: " + gameTimeString;
        if (mainPlayer != null)
        {
            fuelCarriedText.GetComponent<Text>().text = "FUEL: " + mainPlayer.GetComponent<PlayerResource>().resource.ToString();

            if (barValue > mainPlayer.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue - (0.4f * Time.deltaTime), 0.0f, 1.0f);
            }
            else if (barValue <= mainPlayer.GetComponent<PlayerResource>().resource / barMaxValue)
            {
                barValue = Mathf.Clamp(barValue + (0.15f * Time.deltaTime), 0.0f, 1.0f);
            }

            fuelCarriedBar.GetComponent<Slider>().value = barValue;

            if (mainPlayer.GetComponent<MovementScript>().isFrozen)
            {
                powerUpIndicatorFreeze.GetComponent<Image>().color = powerUpIndicatorActiveColour;
            }
            else
            {
                powerUpIndicatorFreeze.GetComponent<Image>().color = powerUpIndicatorInactiveColour;
            }

            if (mainPlayer.GetComponent<MovementScript>().isSpedUp)
            {
                powerUpIndicatorSpeedBoost.GetComponent<Image>().color = powerUpIndicatorActiveColour;
            }
            else
            {
                powerUpIndicatorSpeedBoost.GetComponent<Image>().color = powerUpIndicatorInactiveColour;
            }
        }
    }
}
