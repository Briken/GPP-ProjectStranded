using UnityEngine;
using System.Collections;
using Photon;

public class SpeedBuff : PunBehaviour {
    public MovementScript targetMove;
    public ResourceScript checkPlayers;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject != this.gameObject)
                    {
                        targetMove = hit.collider.gameObject.GetComponent<MovementScript>();
                        int targetNum = targetMove.playerNum;
                        Debug.Log("target is player: " + targetNum.ToString());
                        targetMove.photonView.RPC("SetSpeedBuff", PhotonTargets.All, targetMove.playerNum);
                        this.enabled = false;
                    }
                }
            }
        }
    }
}
