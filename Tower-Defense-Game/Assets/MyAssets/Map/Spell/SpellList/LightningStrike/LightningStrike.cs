using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalPredefinedModel;

public class LightningStrike : SpellEnemyInteraction
{
    public float stunDuration;

    public override void FirstCollisionWithEnemy(GameObject enemy)
    {
        GameObject effect = Instantiate(spellSO.effectPrefab, enemy.transform);
        effect.name = spellSO.abnormalEffect.ToString();

        if (enemy.transform.Find("Model")) { 
            effect.transform.position = enemy.transform.Find("Model").position;
            effect.transform.SetParent(enemy.transform.Find("Model").parent);
        }

        // Stun the enemy
        if (enemy.GetComponent<EnemyActivity>()) { 
            EnemyActivity enemyActivityScript = enemy.GetComponent<EnemyActivity>();
            enemyActivityScript.Stunned(stunDuration);
            enemyActivityScript.DealDamageOnEnemy((int)spellSO.damage);

            SpellEnemyBinding spellEnemyBinding = new SpellEnemyBinding(gameObject, effect, AbnormalEffect.Stun);
            enemyActivityScript.AddNewSpellEnemyBinding(spellEnemyBinding);
        }

    }

    public override void StopCollisionWithEnemy(GameObject enemy)
    {}

    public override IEnumerator SpellLifecycle(float duration)
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.LightningStrikeSE);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
