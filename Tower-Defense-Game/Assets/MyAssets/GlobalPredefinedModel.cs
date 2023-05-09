using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;

public static class GlobalPredefinedModel
{
    // ==================== class ====================
    [Serializable]
    public class Turret
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
    public class DPSBulletCollider
    {
        public Collider collider;
        public float collidedTime;
        public int damage;

        public DPSBulletCollider(Collider collider, float collidedTime, int damage)
        {
            this.collider = collider;
            this.collidedTime = collidedTime;
            this.damage = damage;
        }
    }

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

    public enum BulletDamageType
    {
        SingleTarget,
        Explosion,
        DPS
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
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

    // Not serializable because selectedTurret and selectedTurretUI might be null
    public struct GA_Turret
    {
        public List<Turret> turrets;
        public Turret selectedTurret;
        public GameObject selectedTurretUI;

        public GA_Turret(List<Turret> turrets)
        {
            selectedTurret = null;
            selectedTurretUI = null;
            this.turrets = turrets;
        }
    }
}
