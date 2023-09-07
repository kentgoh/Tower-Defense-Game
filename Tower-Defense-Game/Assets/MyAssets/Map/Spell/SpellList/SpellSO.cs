using UnityEngine;
using static GlobalPredefinedModel;

[CreateAssetMenu(menuName = "SpellSO")]
public class SpellSO : ScriptableObject
{
    // ========== Basic Information ==========
    public SpellName spellName;
    public string actualName;
    public float cooldown;
    public float damage;
    public float duration;
    public AbnormalEffect abnormalEffect;

    // ========== Map ==========
    public GameObject prefab;
    public GameObject effectPrefab;

    // ========== UI ==========
    // Spell Icon 
    public Sprite UI;

    // Spell Details
    public Color backgroundColor;
    public Color contentBackgroundColor;
    public string description;

    public SpellEnemyInteraction spellEnemyInteraction;

}
