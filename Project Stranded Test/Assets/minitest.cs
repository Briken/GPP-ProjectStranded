using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class minitest : MonoBehaviour {

    public Text debug;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (/*Input.GetButtonDown("Fire1")*/ Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            debug.text = "fire1 pressed";
            Vector3 screenPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            Ray ray = Camera.main.ScreenPointToRay(worldPos);
            RaycastHit hit = new RaycastHit();

            debug.text = (worldPos.ToString());

            if (Physics.Raycast(ray, out hit))
            {
                debug.text = "rayhit";
                if (hit.collider.gameObject.tag == "Player")
                {
                    debug.text = "STOP TOUCHING MEEEEEEE";
                }
            }
        }

    }

}
