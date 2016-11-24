using UnityEngine;
using System.Collections;

public class ParticleDestroyEffect : MonoBehaviour {

    public GameObject particleEffectPrefab;
    public float timeUntilDestroy = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        GameObject particleEffectObject = (GameObject)Instantiate(particleEffectPrefab, gameObject.transform.position, Random.rotation);
        Destroy(particleEffectObject, timeUntilDestroy);
    }
}
