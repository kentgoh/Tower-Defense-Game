using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystemManager : MonoBehaviour
{
    private static bool mainSystemExist = false;
    void Awake()
    {
        // Whole game only will have one mainSystem throughout all scenes
        if (!mainSystemExist) { 
            DontDestroyOnLoad(gameObject);
            mainSystemExist = true;
        }
        else 
            Destroy(gameObject);
      
    }

}
