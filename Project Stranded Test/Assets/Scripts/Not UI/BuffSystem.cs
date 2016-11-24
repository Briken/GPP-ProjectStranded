using UnityEngine;
using System.Collections;

public class BuffSystem : MonoBehaviour {

    bool isRunning;
    public float buffTimer = 15;
    public int buff;
    public int[] buffs;
    FreezingDebuff freeze;
    SpeedBuff speedB;
    bool spent = false;

    // Use this for initialization
    void Start ()
    {
        freeze = GetComponent<FreezingDebuff>();
        speedB = GetComponent<SpeedBuff>();
        isRunning = false;
        buffs = new int[2];

        for (int x = 0; x < buffs.GetLength(0); x++)
        {
            buffs[x] = x;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(SetBuff());
            freeze.enabled = false;
            speedB.enabled = false;
        }

        if (buff == 0 && !spent)
        {
            freeze.enabled = true;
            spent = true;
        }
        
        if (buff == 1 && !spent)
        {
            speedB.enabled = true;
            spent = true;
        }

    }

    IEnumerator SetBuff()
    {
        yield return new WaitForSeconds(buffTimer);
        buff = buffs[(int)Random.Range(0, buffs.GetLength(0))];
        isRunning = false;
        spent = false;
    }
}
