using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject turretDetailsBackground;
    [Serializable]
    public struct Turret
    {
        public string name;
        public GameObject turretPrefab;
        public GameObject turretDetailsUI;
        public GameObject turretUI;
    }
    public List<Turret> turrets;

    public List<int> turretDisabledPlaneIndex;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAllPlane();
    }

    public void InitializeAllPlane()
    {
        GameObject[] allPlanes = GameObject.FindGameObjectsWithTag("Plane");

        for(int i = 0; i < allPlanes.Length; i++)
        {
            if (turretDisabledPlaneIndex.Contains(i))
                allPlanes[i].GetComponent<PlaneActivity>().turretCreateAvailability = false;
        }
    }
}
