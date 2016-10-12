using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.left, 66 * Time.deltaTime);
	}

    public void Quit()
    {
        Application.Quit();
    }

}
