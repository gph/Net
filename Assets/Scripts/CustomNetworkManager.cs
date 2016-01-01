using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public void Host()
    {
        StartHost();
    }
	public void Join () {
        StartClient();
	}
}
