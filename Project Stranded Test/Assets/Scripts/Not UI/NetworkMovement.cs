using UnityEngine;
using System.Collections;
using Photon;

public class NetworkMovement : Photon.MonoBehaviour {


    private Vector3 correctPPos;
    private Quaternion correctPRot;
    // Use this for initialization
    void Start() {
      
    }

    // Update is called once per frame
    void Update() {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPPos, Time.deltaTime*5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPRot, Time.deltaTime*5);
        }
    }


    public void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            this.transform.position = (Vector3) stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}