using UnityEngine;
using System.Collections;
using Photon;

public class FreezingDebuff : Photon.PunBehaviour {
    
    public MovementScript targetMove;
    public ResourceScript checkPlayers;

	// Use this for initialization
	void Start ()
    {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("muthafucka you pressed fire");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Oh shit you hit something");
                    if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject != this.gameObject)
                    {
                        targetMove = hit.collider.gameObject.GetComponent<MovementScript>();
                        int targetNum = targetMove.playerNum;
                        Debug.Log("target is player: " + targetNum.ToString());
                        targetMove.photonView.RPC("SetPlayerFrozen", PhotonTargets.All, targetMove.playerNum);
                    }
                }
            }
        }
    }
    
}
