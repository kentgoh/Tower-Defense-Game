using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalPredefinedModel;

public class SpellActivity : MonoBehaviour
{
    public List<GameObject> spellUISlot;

    void Update()
    {
        UpdateSpellList();
    }

    public void UpdateSpellList()
    {
        List<Spell> availableSpells = GameActivity.Instance.ga_Spell.spells;

        for (int i = 0; i < spellUISlot.Count; i++)
        {
            try
            {
                spellUISlot[i].transform.GetComponent<Image>().sprite = availableSpells[i].spellUI;
                spellUISlot[i].transform.name = availableSpells[i].spellType.ToString();
            }
            catch(ArgumentOutOfRangeException)
            {
                spellUISlot[i].transform.GetComponent<Image>().raycastTarget = false;
                spellUISlot[i].transform.name = "N/A";
                spellUISlot[i].transform.GetComponent<Image>().color = Color.clear;
            }
                    
        }
    }
}
