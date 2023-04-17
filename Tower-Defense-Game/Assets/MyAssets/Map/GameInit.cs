using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;
public class GameInit : MonoBehaviour
{
    public static GameInit Instance;

    public List<Enemy> enemies;
    public List<Wave> waves;
    public List<Turret> turrets;

    public DebugMode debugMode = new DebugMode(false, false);

    // ========== Set manually =========
    public GameObject turretDetailsBackground;
    public int endPointStartingHealth;
    public int startingResources;
    public int resourcesPerSecond;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        CheckDebugMode();
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

    public string GetTurretUIColor(TurretUIColor turretUIColor)
    {
        if (turretUIColor == TurretUIColor.available)
            return "#19870F";
        else if (turretUIColor == TurretUIColor.selected)
            return "#00FF22";
        else
            return "#8C0A0A";

    }
}
