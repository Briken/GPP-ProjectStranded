using UnityEngine;
using System.Collections;
using Photon;

public class CommScript : PunBehaviour {

    public GameObject ellipsisPrefab;
    public GameObject exclamationPrefab;
    public GameObject questionPrefab;
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
                    GameObject elips = PhotonNetwork.Instantiate(ellipsisPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                    canComm = false;
                    StartCoroutine(CommCooldown(silenceTime));
                    elips.transform.SetParent(this.transform);
                    menu.ToggleSpiderButtons();
                }

                if (hit.collider.gameObject.tag == "Exclamation")
                {
                    GameObject excl = PhotonNetwork.Instantiate(exclamationPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                    canComm = false;
                    StartCoroutine(CommCooldown(silenceTime));
                    excl.transform.SetParent(this.transform);
                    menu.ToggleSpiderButtons();
                }
                if (hit.collider.gameObject.tag == "Question Mark")
                {
                    GameObject ques = PhotonNetwork.Instantiate(questionPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
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
