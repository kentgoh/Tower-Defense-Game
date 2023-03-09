using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    // Debug Mode
    [Serializable]
    public class DebugMode
    {
        public Boolean mode = false;
        public Boolean showAllTurret = false;
    }
    public DebugMode debugMode;

    // Turret
    [Serializable]
    public struct Turret
    {
        public string name;
        public GameObject turretPrefab;
        public GameObject turretDetailsUI;
    }
    public List<Turret> turrets;
    public GameObject turretDetailsBackground;

    // Disable plane by index
    public List<int> turretDisabledPlaneIndex;
    // Set facing direction by index
    public List<int> turretInitDirectionRightIndex;
    public List<int> turretInitDirectionBottomIndex;
    public List<int> turretInitDirectionLeftIndex;

    void Start()
    {
        CheckDebugMode();
        InitializeAllPlane();
    }

    public void InitializeAllPlane()
    {
        GameObject[] allPlanes = GameObject.FindGameObjectsWithTag("Plane");


        for(int i = 0; i < allPlanes.Length; i++)
        {
            // Add index value on top of plane
            if (debugMode.mode)
            {
                allPlanes[i].transform.Find("Index").gameObject.GetComponent<TMP_Text>().text = i.ToString();
            }

            // Set those plane that has been disabled (the plane is not beside the enemy route/map special setup)
            if (turretDisabledPlaneIndex.Contains(i))
                allPlanes[i].GetComponent<PlaneActivity>().turretCreateAvailability = false;

            // Set the initial direction of the turret when created
            if(turretInitDirectionRightIndex.Contains(i))
                allPlanes[i].GetComponent<PlaneActivity>().turretInitDiretionCode= 1;
            if (turretInitDirectionBottomIndex.Contains(i))
                allPlanes[i].GetComponent<PlaneActivity>().turretInitDiretionCode = 2;
            if (turretInitDirectionLeftIndex.Contains(i))
                allPlanes[i].GetComponent<PlaneActivity>().turretInitDiretionCode = 3;
        }

    }

    // SetActive() to false for plane index
    public void CheckDebugMode()
    {
        if (!debugMode.mode)
        {
            GameObject[] debugElements = GameObject.FindGameObjectsWithTag("DebugElement");
            foreach(GameObject gameObject in debugElements)
            {
                gameObject.SetActive(false);
            }
        }
    }
}