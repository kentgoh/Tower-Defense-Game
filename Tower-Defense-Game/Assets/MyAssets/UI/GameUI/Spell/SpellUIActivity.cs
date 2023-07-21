using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalPredefinedModel;

public class SpellUIActivity : MonoBehaviour
{
    public List<GameObject> spellUISlot;

    void Awake()
    {
        InitSpellList();
    }

    void Update()
    {
        UpdateSpellUIAvailability();
    }


    public void InitSpellList()
    {
        List<Spell> availableSpells = GameActivity.Instance.ga_Spell.spells;

        for (int i = 0; i < spellUISlot.Count; i++)
        {
            try
            {
                if (spellUISlot[i].transform.Find("Image")) { 
                    spellUISlot[i].transform.Find("Image").GetComponent<Image>().sprite = availableSpells[i].spellUI;
                    spellUISlot[i].transform.name = availableSpells[i].spellType.ToString();
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                spellUISlot[i].transform.name = "N/A";

                foreach(Transform child in spellUISlot[i].transform) {
                    // Make border, image and counter of the unuse spellUI to be invisible when there is less than 3 spells
                    if (child.GetComponent<Image>()) { 
                        child.GetComponent<Image>().raycastTarget = false;
                        child.GetComponent<Image>().color = Color.clear;
                    }
                    if (child.GetComponent<TMP_Text>())
                    {
                        child.GetComponent<TMP_Text>().text = "";
                    }
                }

            }
                    
        }
    }

    public void UpdateSpellUIAvailability()
    {
        foreach (GameObject spellUI in spellUISlot)
        {
            Spell currentSpell = GameActivity.Instance.ga_Spell.spells.Find(x => x.spellType.ToString().Equals(spellUI.name));
            // Display cooldown text
            if (currentSpell != null && spellUI.transform.Find("Counter").GetComponent<TMP_Text>())
            {
                // Only show counter when the spell is in cooldown
                if (currentSpell.currentCooldown > 0)
                {
                    currentSpell.currentCooldown -= Time.deltaTime;
                    spellUI.transform.Find("Counter").GetComponent<TMP_Text>().text = Mathf.Ceil(currentSpell.currentCooldown).ToString();
                }
                else
                    spellUI.transform.Find("Counter").GetComponent<TMP_Text>().text = "";

                if(GameActivity.Instance.ga_Spell.selectedSpell == null && spellUI.transform.Find("Border").GetComponent<Image>())
                {
                    Image border = spellUI.transform.Find("Border").GetComponent<Image>();
                    border.color = new Color(border.color.r, border.color.g, border.color.b, 0f);
                }
 

            }
        }
    }
}
