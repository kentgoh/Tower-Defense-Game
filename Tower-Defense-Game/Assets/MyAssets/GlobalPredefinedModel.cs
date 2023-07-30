using System;
using System.Collections.Generic;
using UnityEngine;

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
    public class DPSCollider
    {
        public Collider collider;
        public string name;
        public float collidedTime;
        public int damage;

        public DPSCollider(Collider collider, float collidedTime, int damage)
        {
            this.collider = collider;
            this.name = collider.name;
            this.collidedTime = collidedTime;
            this.damage = damage;
        }
    }

    [Serializable]
    public class Spell
    {
        public SpellType spellType;
        public float maxCooldown;
        public float currentCooldown;
        public Sprite UI;
        public GameObject prefab;
        public GameObject effectPrefab;

        public Spell(SpellType spellType, float maxCooldown, Sprite UI, GameObject prefab, GameObject effectPrefab)
        {
            this.spellType = spellType;
            this.maxCooldown = maxCooldown;
            this.currentCooldown = 0;
            this.UI = UI;
            this.prefab = prefab;
            this.effectPrefab = effectPrefab;
        }
    }

    [Serializable]
    public class SpellEffect
    {
        public GameObject spell;
        public GameObject effect;
        public SpellType spellType;

        public SpellEffect(GameObject spell, GameObject effect, SpellType spellType)
        {
            this.spell = spell;
            this.effect = effect;
            this.spellType = spellType;
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

    public enum SpellType
    {
        Ice,
        Lightning
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

    [Serializable]
    public struct GA_Spell
    {
        public List<Spell> spells;
        public Spell selectedSpell;

        public GA_Spell(List<SpellType> availableSpellTypes, List<Spell> allSpellsData)
        {
            spells = new List<Spell>();
            selectedSpell = null;

            // Add available spell to ga_spell according to the data in allSpellsData
            foreach(SpellType spellType in availableSpellTypes)
            {
                spells.Add(
                    new Spell(
                        spellType,
                        allSpellsData[(int) spellType].maxCooldown,
                        allSpellsData[(int) spellType].UI,
                        allSpellsData[(int) spellType].prefab,
                        allSpellsData[(int)spellType].effectPrefab
                    )
                );

            }
        }
    }
}
