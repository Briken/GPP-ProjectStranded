using UnityEngine;
using System.Collections;

public class TeamScript : MonoBehaviour {

    GameObject[] team1 = new GameObject[4];
    GameObject[] team2 = new GameObject[4];
    public bool teamToggle = true;
    int firstplayernum = 0;
    int secondplayernum = 0;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void TeamAdd(GameObject player, bool teamone)
    {
        
        if (teamone)
        {
            if (team1[0] == null)
            {
                team1[0] = player;
                Debug.Log(team1[0] + "added to team 1");
            }
            else
            {
                 firstplayernum++;
                team1[firstplayernum] = player;
            }
        }
        if (teamone == false)
        {
            if (team2[0] == null)
            {
                team2[0] = player;
                Debug.Log(team2[0] + "added to team 2");
            }
            else
            {
                secondplayernum++;
                team2[secondplayernum] = player;
                Debug.Log(team2[secondplayernum]);
            }
        }
    }

}
