using UnityEngine;
using System.Collections;
using Photon;

public class CommScript : PunBehaviour {

    public GameObject ellipsisPrefab;
    public GameObject exclamationPrefab;
    public GameObject questionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.tag == "Ellipsis")
                {
                    PhotonNetwork.Instantiate(ellipsisPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                }

                if (hit.collider.gameObject.tag == "Exclamation")
                {
                    PhotonNetwork.Instantiate(exclamationPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                }
                if (hit.collider.gameObject.tag == "Question Mark")
                {
                    PhotonNetwork.Instantiate(questionPrefab.name, hit.collider.gameObject.transform.position, Quaternion.identity, 0);
                }
            }
        }
    }
}
