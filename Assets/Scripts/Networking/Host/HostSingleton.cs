using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;
    
    private HostGameManager _gameManager;
    
    public static HostSingleton Instance
    {
        get
        {
            if (instance != null) return instance;
            
            instance = FindObjectOfType<HostSingleton>();
            if (instance == null)
            {
                Debug.Log("No host singleton in the scene");
                return null;
            }
            
            return instance;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        _gameManager = new HostGameManager();
    }   
}
