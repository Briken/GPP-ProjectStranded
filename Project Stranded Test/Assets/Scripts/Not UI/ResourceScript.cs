using UnityEngine;
using System.Collections;

public class ResourceScript : MonoBehaviour {

    PlayerResource playerResource;

    public float resourceDistance = 10.0f;

    public int large, medium, small;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update ()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, resourceDistance);
		int i = 0;
		while (i < hitColliders.Length)
		{
            if (hitColliders.Length >= large && this.tag == "Large")
            {
                if (hitColliders[i].gameObject.tag == "Player")
                {
                    playerResource = hitColliders[i].GetComponent<PlayerResource>();
                    AddResource();
                }
            }
            if (hitColliders.Length >= medium && this.tag == "Medium")
            {
                if (hitColliders[i].gameObject.tag == "Player")
                {
                    playerResource = hitColliders[i].GetComponent<PlayerResource>();
                    AddResource();
                }
            }
            if (hitColliders.Length >= small && this.tag == "Small")
            {
                if (hitColliders[i].gameObject.tag == "Player")
                {
                    playerResource = hitColliders[i].GetComponent<PlayerResource>();
                    AddResource();
                }
            }
        }
	}

    public void AddResource()
    {
        if (this.tag == "Large")
        {
            playerResource.resource += large; 
        }

        if (this.tag == "Medium")
        {
            playerResource.resource += medium;
        }

        if (this.tag == "Small")
        {
            playerResource.resource += small;
        }
    }
}
