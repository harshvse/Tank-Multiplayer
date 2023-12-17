using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ManagerServer : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void JoinServer(){
        NetworkManager.Singleton.StartClient();
    } 
}
