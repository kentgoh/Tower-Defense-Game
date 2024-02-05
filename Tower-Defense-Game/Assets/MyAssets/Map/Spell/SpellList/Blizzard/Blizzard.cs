using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static GlobalPredefinedModel;

public class Blizzard : SpellEnemyInteraction
{
    public float slowRatio = 0.5f;

    public override void FirstCollisionWithEnemy(GameObject enemy)
    {
        GameObject effect = Instantiate(spellSO.effectPrefab, enemy.transform);
        effect.name = "Slow";

        if (enemy.transform.Find("Model")) { 
            effect.transform.position = enemy.transform.Find("Model").position;
            effect.transform.SetParent(enemy.transform.Find("Model").parent);
        }

        // Slow the enemy
        if (enemy.GetComponent<EnemyActivity>()) { 
            EnemyActivity enemyActivityScript = enemy.GetComponent<EnemyActivity>();
            enemyActivityScript.DecreaseMovementSpeed(slowRatio);

            // Add binding info on spell side
            SpellEnemyBinding spellEnemyBinding = new SpellEnemyBinding(enemy, effect, AbnormalEffect.Slow);
            spellEnemyBindingList.Add(spellEnemyBinding);

            // Add binding info on enemy side
            spellEnemyBinding = new SpellEnemyBinding(gameObject, effect, AbnormalEffect.Slow);
            enemyActivityScript.AddNewSpellEnemyBinding(spellEnemyBinding);
        }

    }

    public override void StopCollisionWithEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyActivity>()) {
            EnemyActivity enemyActivityScript = enemy.GetComponent<EnemyActivity>();
            Boolean isSpellSideBinding = false;
            Boolean isEnemySideBinding = false;

            SpellEnemyBinding spellSideBinding = spellEnemyBindingList.Find(x => (x.target == enemy));
            SpellEnemyBinding enemySideBinding = enemyActivityScript.GetSpellEnemyBindingBySpell(gameObject);
            if (spellSideBinding != null)
                isSpellSideBinding = true;
            if (enemySideBinding != null)
                isEnemySideBinding = true;

            if(isSpellSideBinding && isEnemySideBinding)
                RemoveBindingOnBothSide(enemyActivityScript, spellSideBinding, enemySideBinding);
            
        }
    }

    public override IEnumerator SpellLifecycle(float duration)
    {
        AudioSource audioSource = AudioManager.Instance.PlayLoopSound(AudioManager.AudioSourceType.Spell, "BlizzardSE");
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
        AudioManager.Instance.RemoveFromLoopingOneShotAudioList(audioSource);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foreach (SpellEnemyBinding spellSideBinding in spellEnemyBindingList.ToList())
        {
            Boolean isSpellSideBinding = false;
            Boolean isEnemySideBinding = false;

            if (spellSideBinding.target != null) {
                EnemyActivity enemyActivityScript = spellSideBinding.target.GetComponent<EnemyActivity>();

                SpellEnemyBinding enemySideBinding = enemyActivityScript.GetSpellEnemyBindingBySpell(gameObject);

                if (spellSideBinding != null)
                    isSpellSideBinding = true;
                if (enemySideBinding != null)
                    isEnemySideBinding = true;

                if (isSpellSideBinding && isEnemySideBinding)
                    RemoveBindingOnBothSide(enemyActivityScript, spellSideBinding, enemySideBinding);

            }
        }
    }

}
