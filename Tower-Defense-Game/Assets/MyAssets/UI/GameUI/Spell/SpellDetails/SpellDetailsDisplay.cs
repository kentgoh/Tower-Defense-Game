using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellDetailsDisplay : MonoBehaviour
{
    public SpellSO spellSO;
    public new TMP_Text name;
    public Image background;

    public Image contentBackground;
    public TMP_Text damageAndCooldown;

    public Image descriptionBackground;
    public TMP_Text description;

    void Awake()
    {
        DisplaySpellDetail();
    }

    private void DisplaySpellDetail()
    {
        name.text = spellSO.actualName;
        name.color = spellSO.contentBackgroundColor;
        background.color = spellSO.backgroundColor;

        contentBackground.color = spellSO.contentBackgroundColor;
        damageAndCooldown.text = spellSO.damage.ToString() + "<br>" + spellSO.cooldown.ToString();

        descriptionBackground.color = spellSO.contentBackgroundColor;
        description.text = spellSO.description;
    }

}
