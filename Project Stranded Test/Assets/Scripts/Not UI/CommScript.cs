using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class CommScript : PunBehaviour {

    public GameObject ellipsisPrefab;
    public GameObject exclamationPrefab;
    public GameObject thisPlayer;

    public GameObject pulse;

    UISpiderButton menu;

    bool canComm = true;
    public float silenceTime = 5.0f;

	// Use this for initialization
	void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetButton("Fire1") && canComm)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    RaycastHit hit = new RaycastHit();

        //    if (Physics.Raycast(ray, out hit))
        //    {

        //        if (hit.collider.gameObject.tag == "Ellipsis")
        //        {
        //            GameObject elips = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
        //            elips.GetComponent<SpriteRenderer>().color = Color.green;
        //            canComm = false;
        //            StartCoroutine(CommCooldown(silenceTime));
        //            elips.transform.SetParent(this.transform);
        //            menu.ToggleSpiderButtons();
        //        }

        //        if (hit.collider.gameObject.tag == "Exclamation")
        //        {
        //            GameObject excl = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
        //            excl.GetComponent<SpriteRenderer>().color = Color.red;
        //            canComm = false;
        //            StartCoroutine(CommCooldown(silenceTime));
        //            excl.transform.SetParent(this.transform);
        //            menu.ToggleSpiderButtons();
        //        }
        //        if (hit.collider.gameObject.tag == "Question Mark")
        //        {
        //            GameObject ques = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
        //            ques.GetComponent<SpriteRenderer>().color = Color.blue;
        //            canComm = false;
        //            StartCoroutine(CommCooldown(silenceTime));
        //            ques.transform.SetParent(this.transform);
        //            menu.ToggleSpiderButtons();
        //        }
        //    }
        //}
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

            StartCoroutine(CommCooldown(silenceTime));
        }

        thisPlayer.GetComponent<PlayerStatTracker>().timesPressingCommsButton += 1;
    }

    IEnumerator CommCooldown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canComm = true;
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
