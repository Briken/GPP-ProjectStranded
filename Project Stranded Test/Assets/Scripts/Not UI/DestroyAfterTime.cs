using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    public float timeUntilDestroy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timeUntilDestroy < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timeUntilDestroy -= Time.deltaTime;
        }
	
	}
}
