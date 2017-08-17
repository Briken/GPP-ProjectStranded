using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class CommScript : PunBehaviour {

    public GameObject ellipsisPrefab;
    public GameObject exclamationPrefab;
    public GameObject thisPlayer;
    public GameObject resourceTemp;
    public GameObject pulse;

    float crateResetDist;
    public float crateID;

    UISpiderButton menu;

    bool canComm = true;
    public float silenceTime = 5.0f;

    public float colourChangeTimer = 2.0f;
    public float colourBlinkTime = 0.25f;
    float defaultColourChangeTimer;
    bool isPlayerColour = false;

	// Use this for initialization
	void Start ()
    {
        crateResetDist = resourceTemp.GetComponent<ResourceScript>().resourceDistance;
        defaultColourChangeTimer = colourChangeTimer;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (canComm && colourChangeTimer >= 0.0f)
        {
            colourChangeTimer -= Time.deltaTime;
        }

        if (canComm && colourChangeTimer <= 0.0f)
        {
            ChangeButtonColour();
        }
    }

    public void Alert()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetPhotonView().isMine)
            {
                thisPlayer = n;
            }
        }
    
        if (canComm == true && thisPlayer != null)
        {

            GameObject commObj = PhotonNetwork.Instantiate(pulse.name, thisPlayer.transform.position, Quaternion.identity, 0);
            commObj.GetPhotonView().RPC("ChangeColour", PhotonTargets.All, thisPlayer.GetComponent<MovementScript>().playerNum);
            canComm = false;
            gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            Debug.Log(commObj.name);

            thisPlayer.GetComponent<PlayerStatTracker>().timesActivatingComms += 1;
            Analytics.CustomEvent("Player Communicated", new Dictionary<string, object>
        {
             { "player communicated", "player username: " + thisPlayer.GetComponent<MovementScript>().publicUsername },
             { "Crate ID: ", crateID.ToString() },
             { "Timestamp: ", System.DateTime.Now.ToString()},
        });
            StartCoroutine(CommCooldown(silenceTime));
        }

        thisPlayer.GetComponent<PlayerStatTracker>().timesPressingCommsButton += 1;
    }

    IEnumerator CommCooldown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canComm = true;
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        isPlayerColour = false;
        colourChangeTimer = defaultColourChangeTimer;
    }

    void ChangeButtonColour()
    {
        foreach (GameObject n in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (n.GetPhotonView().isMine)
            {
                if (!isPlayerColour && n.GetComponent<PlayerStatTracker>().timeSinceLastNearFuelCrate < 1.0f)
                {
                    gameObject.GetComponent<Image>().color = n.GetComponent<MovementScript>().myColour;
                    isPlayerColour = true;
                    colourChangeTimer = colourBlinkTime;
                }
                else
                {
                    gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    isPlayerColour = false;
                    colourChangeTimer = defaultColourChangeTimer;
                }
            }
        }
    }

    public void PlayerTagged(string playerUsername)
    {
        Analytics.CustomEvent("Player Seen Comm", new Dictionary<string, object>
        {
             { "player " + thisPlayer.GetComponent<MovementScript>().publicUsername + " communicated", "player " + playerUsername + " has seen" },
             { "Timestamp: ", System.DateTime.Now.ToString()},
        });
    }
}
