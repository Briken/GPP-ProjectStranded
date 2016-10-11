using UnityEngine;
using System.Collections;
    /*
     * 
     * 
     * DO NOT TOUCH THIS SCRIPT. IM SERIOUS. DONT.
     * 
     * 
     */

public class NetworkingCode : MonoBehaviour {

    public GameObject terrainprefab;

    string typeName = "ProjectStrandedTestGame";
    string gameName = "ProjectStrandedTestRoom";
    HostData[] hostList;
    string ipAddress = "xxx.xxx.xxx.xxx";
	
    
    // Use this for initialization
	void Start ()
    {
        MasterServer.ipAddress = ipAddress;
	}
	
	//Update is called once per frame
	void Update () {
	
	}

    public void StartServer()
    {
        if (!Network.isServer && !Network.isClient)
        {
            Network.InitializeServer(6, 25000, 
                !Network.HavePublicAddress());
            MasterServer.RegisterHost(typeName, gameName);
        }
    }

    void OnServerInitialised()
    {
        Debug.Log("server initialised");
    }

    public void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            hostList = MasterServer.PollHostList();
            foreach (HostData hd in hostList)
            {
                if (hd.gameName == gameName)
                {
                    Network.Connect(hd);
                }
            }
        }
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
        SpawnPlayer();
    }

    void SpawnPlayer()
    {

    }
}
