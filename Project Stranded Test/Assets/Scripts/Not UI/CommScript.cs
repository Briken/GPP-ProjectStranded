using UnityEngine;
using System.Collections;
using Photon;

public class CommScript : PunBehaviour {

    public GameObject ellipsisPrefab;
    public GameObject exclamationPrefab;
    public GameObject questionPrefab;

    public GameObject pulse;

    UISpiderButton menu;

    bool canComm = true;
    public float silenceTime = 5.0f;

	// Use this for initialization
	void Start ()
    {
        menu = GetComponent<UISpiderButton>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1") && canComm)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.tag == "Ellipsis")
                {
                    GameObject elips = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                    elips.GetComponent<SpriteRenderer>().color = Color.blue;
                    canComm = false;
                    StartCoroutine(CommCooldown(silenceTime));
                    elips.transform.SetParent(this.transform);
                    menu.ToggleSpiderButtons();
                }

                if (hit.collider.gameObject.tag == "Exclamation")
                {
                    GameObject excl = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                    excl.GetComponent<SpriteRenderer>().color = Color.red;
                    canComm = false;
                    StartCoroutine(CommCooldown(silenceTime));
                    excl.transform.SetParent(this.transform);
                    menu.ToggleSpiderButtons();
                }
                if (hit.collider.gameObject.tag == "Question Mark")
                {
                    GameObject ques = PhotonNetwork.Instantiate(pulse.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                    ques.GetComponent<SpriteRenderer>().color = Color.green;
                    canComm = false;
                    StartCoroutine(CommCooldown(silenceTime));
                    ques.transform.SetParent(this.transform);
                    menu.ToggleSpiderButtons();
                }
            }
        }
    }

    IEnumerator CommCooldown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canComm = true;
    }
}
