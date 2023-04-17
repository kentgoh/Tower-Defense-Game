using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalPredefinedModel
{
    // ==================== enum ====================
    public enum TurretUIColor
    {
        notAvailable,
        available,
        selected
    }

    public enum EnemyType
    {
        Raider,
        Drone,
        Boulder
    }

    // ==================== struct ====================
    [Serializable]
    public struct DebugMode
    {
        public Boolean mode;
        public Boolean showAllTurret;

        public DebugMode(Boolean mode, Boolean showAllTurret)
        {
            this.mode = mode;
            this.showAllTurret= showAllTurret;
        }
    }

    [Serializable]
    public struct Turret
    {
        public string name;
        public int resourcesCost;
        public int count;

        public GameObject prefab;

        public GameObject UI;
        public int UICooldown;

        public GameObject detailsUI;
    }

    [Serializable]
    public struct Enemy
    {
        public EnemyType enemyType;
        public GameObject enemyPrefab;
    }

    [Serializable]
    public struct EnemySpawn
    {
        public EnemyType enemyType;
        public int count;
        public int interval;
    }

    [Serializable]
    public struct Wave
    {
        public List<EnemySpawn> enemySpawns;
        public int intervalBeforeNextWave;
    }

    [Serializable]
    public struct GA_Wave
    {
        public int totalWave;
        public int currentWave;
        public Boolean waveSpawnCompleted;

        public GA_Wave(int totalWave, Boolean waveSpawnCompleted)
        {
            this.totalWave = totalWave;
            this.currentWave = 1;
            this.waveSpawnCompleted = false;
        }
    }

    [Serializable]
    public struct GA_Time
    {
        public float time;
        public float timeBeforeNextWave;
        public float timeForThisWave;
    }

    [Serializable]
    public struct GA_Resource
    {
        public int resources;
        public int resourcesPerSecond;

        public GA_Resource(int resources, int resourcesPerSecond)
        {
            this.resources = resources;
            this.resourcesPerSecond = resourcesPerSecond;
        }
    }

    [Serializable]
    public struct GA_Turret
    {
        public string selectedTurretName;
        public GameObject selectedTurretUI;
        public List<Turret> turrets;

        public GA_Turret(List<Turret> turrets)
        {
            this.selectedTurretName = "";
            this.selectedTurretUI = null;
            this.turrets = turrets;
        }
    }
}
