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
        public SpellName spellName;
        public string name;
        public float maxCooldown;
        public float currentCooldown;
        public float damage;

        public Sprite UI;
        public GameObject prefab;
        public GameObject effectPrefab;

        public Spell(SpellSO spellSO)
        {
            spellName = spellSO.spellName;
            name = spellSO.actualName;

            maxCooldown = spellSO.cooldown;
            currentCooldown = 0;
            damage = spellSO.damage;

            UI = spellSO.UI;
            prefab = spellSO.prefab;

            if (prefab.GetComponent<SpellEnemyInteraction>())
                prefab.GetComponent<SpellEnemyInteraction>().spellSO = spellSO;
        }
    }

    [Serializable]
    public class SpellEffect
    {
        public GameObject spell;
        public GameObject effect;
        public SpellName spellName;

        public SpellEffect(GameObject spell, GameObject effect, SpellName spellName)
        {
            this.spell = spell;
            this.effect = effect;
            this.spellName = spellName;
        }
    }

    [Serializable]
    public class SpellEnemyBinding
    {
        public GameObject target;
        public GameObject spellEffect;
        public AbnormalEffect abnormalEffect;

        public SpellEnemyBinding(GameObject target, GameObject spellEffect, AbnormalEffect abnormalEffect)
        {
            this.target = target;
            this.spellEffect = spellEffect;
            this.abnormalEffect = abnormalEffect;
        }
    }

    [Serializable]
    public class GA_MouseState
    {
        public MouseState mouseState;

        public GA_MouseState()
        {
            mouseState = MouseState.None;
        }
        public void UpdateMouseState(MouseState mouseState)
        {
            if (!(this.mouseState == mouseState)) {   
                //Debug.Log(String.Format("Update Mouse State from {0} to {1}", this.mouseState.ToString(), mouseState.ToString()));
                this.mouseState = mouseState;
            }
        }

        public bool IsOnUI()
        {
            return mouseState.ToString().Contains("UI");
        }

        public bool IsOnMap()
        {
            return mouseState.ToString().Contains("Map");
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

    public enum SpellName
    {
        Blizzard,
        LightningStrike,
        MagneticBolt,
        IcePillar
    }

    public enum AbnormalEffect
    {
        None,
        Weak,
        Slow,
        Stun
    }

    public enum MouseState
    {
        None,
        Spell_UI,
        Turret_UI,
        Button_UI,
        Plane_Map,
        Base_Map
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

        public GA_Spell(List<SpellName> availableSpellNames, List<SpellSO> allSpellSO)
        {
            spells = new List<Spell>();
            selectedSpell = null;

            // Add available spell to ga_spell according to the data in allSpellsData
            foreach(SpellName spellName in availableSpellNames)
            {
                SpellSO spellSO = allSpellSO.Find(x => x.spellName.Equals(spellName));

                if(spellSO != null)
                    spells.Add(new Spell(spellSO));

            }
        }
    }
}
