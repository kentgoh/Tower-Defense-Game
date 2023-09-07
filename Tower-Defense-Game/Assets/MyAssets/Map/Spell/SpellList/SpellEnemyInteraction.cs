using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;

public abstract class SpellEnemyInteraction : MonoBehaviour
{
    public SpellSO spellSO;
    public List<SpellEnemyBinding> spellEnemyBindingList;

    public abstract void FirstCollisionWithEnemy(GameObject enemy);
    public abstract void StopCollisionWithEnemy(GameObject enemy);

    public abstract IEnumerator SpellLifecycle(float duration);

    public void Awake()
    {
        spellEnemyBindingList = new List<SpellEnemyBinding>();
        StartCoroutine(SpellLifecycle(spellSO.duration));
    }

    public void RemoveBindingOnBothSide(EnemyActivity enemyActivityScript, SpellEnemyBinding spellSideBinding, SpellEnemyBinding enemySideBinding)
    {
        // Remove binding info on both side
        GameObject effect = spellSideBinding.spellEffect;
        spellEnemyBindingList.Remove(spellSideBinding);
        enemyActivityScript.RemoveSpellEnemyBinding(enemySideBinding);

        // Destroy the spell effect
        Destroy(effect);

    }
}
