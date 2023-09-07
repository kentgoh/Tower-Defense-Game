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
    public List<SpellSO> spellSOList;     

    public DebugMode debugMode = new DebugMode(false, false);

    // ========== Set manually =========
    public int endPointStartingHealth;
    public int startingResources;
    public int resourcesPerSecond;

    public List<SpellName> startingSpells;

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
            return "#FE7000";
        else
            return "#8C0A0A";

    }
}
