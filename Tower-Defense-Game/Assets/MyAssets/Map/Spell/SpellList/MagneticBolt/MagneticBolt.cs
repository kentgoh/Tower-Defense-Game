using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static GlobalPredefinedModel;

public class MagneticBolt : SpellEnemyInteraction
{
    public override void FirstCollisionWithEnemy(GameObject enemy)
    {
        GameObject effect = Instantiate(spellSO.effectPrefab, enemy.transform);
        effect.name = "Weak";

        if (enemy.transform.Find("Model")) { 
            effect.transform.position = enemy.transform.Find("Model").position;
            effect.transform.SetParent(enemy.transform.Find("Model").parent);
        }

        // Weaken the enemy
        if (enemy.GetComponent<EnemyActivity>())
        {
            EnemyActivity enemyActivityScript = enemy.GetComponent<EnemyActivity>();
            enemyActivityScript.Weaken(2);

            // Add binding info on spell side
            SpellEnemyBinding spellEnemyBinding = new SpellEnemyBinding(enemy, effect, AbnormalEffect.Weak);
            spellEnemyBindingList.Add(spellEnemyBinding);

            // Add binding info on enemy side
            spellEnemyBinding = new SpellEnemyBinding(gameObject, effect, AbnormalEffect.Weak);
            enemyActivityScript.AddNewSpellEnemyBinding(spellEnemyBinding);
        }

    }

    public override void StopCollisionWithEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyActivity>())
        {
            EnemyActivity enemyActivityScript = enemy.GetComponent<EnemyActivity>();
            Boolean isSpellSideBinding = false;
            Boolean isEnemySideBinding = false;

            SpellEnemyBinding spellSideBinding = spellEnemyBindingList.Find(x => (x.target == enemy));
            SpellEnemyBinding enemySideBinding = enemyActivityScript.GetSpellEnemyBindingBySpell(gameObject);
            if (spellSideBinding != null)
                isSpellSideBinding = true;
            if (enemySideBinding != null)
                isEnemySideBinding = true;

            if (isSpellSideBinding && isEnemySideBinding)
                RemoveBindingOnBothSide(enemyActivityScript, spellSideBinding, enemySideBinding);

        }
    }

    public override IEnumerator SpellLifecycle(float duration)
    {
        AudioSource audioSource = AudioManager.Instance.PlayLoopSound(AudioManager.AudioSourceType.Spell, "MagneticBoltSE");
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

            if (spellSideBinding.target != null)
            {
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
