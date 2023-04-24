using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalPredefinedModel;

public class GameUIActivity : MonoBehaviour, IPointerClickHandler
{
    public static GameUIActivity Instance;
    public GameObject turretDetailsUI;
    public GameObject dialogUI;

    void Awake()
    {
        if (!Instance)
            Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        // Get current pointer details by mousePosition
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Get raycast result by pointer details
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        DisplayTurretDetailsUI(results);
       
    }

    // ======================= MOUSE ACTIVITY ======================= 
    // Mouse hover activity
    public void DisplayTurretDetailsUI(List<RaycastResult> results)
    {
        // Mouse pointer does point to more than one gameObject
        if (results.Count > 0)
        {
            Boolean nameMatched = false;
            foreach (RaycastResult result in results)
            {
                foreach(Turret turret in GameInit.Instance.turrets)
                {
                    // disable all UI first, then enable the selected one only
                    if (result.gameObject.name.Equals(turret.name))
                    {
                        DisableAllTurretDetailsUI();
                        EnablePointedTurretDetailsUI(turret);
                        nameMatched = true;
                        break;
                    }
                }
                // Mouse pointer does point to more than one gameObject but no turretDetails matched
                if (!nameMatched)
                {
                    DisableAllTurretDetailsUI();
                }
            }
        }
        else
        {
            DisableAllTurretDetailsUI();
        }
    }

    // Enable the pointed turret details
    public void EnablePointedTurretDetailsUI(Turret turret) {
        turretDetailsUI.SetActive(true);
        turret.detailsUI.SetActive(true);
    }

    // Disable all the turret details
    public void DisableAllTurretDetailsUI()
    {
        turretDetailsUI.SetActive(false);

        foreach(Turret turret in GameInit.Instance.turrets)
        {
            turret.detailsUI.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject selectedUI = eventData.pointerCurrentRaycast.gameObject;

        // Check selected turret UI
        DisplayTurretUISelection(selectedUI);
    }

    public void DisplayTurretUISelection(GameObject selectedUI)
    {
        GameObject parentOfSelectedUI = selectedUI.transform.parent.gameObject;

        if (selectedUI.tag.Equals("TurretUI"))
        {
            // Get resources value from gameActivityScript
            int resources = GameActivity.Instance.ga_Resource.resources;
            
            // Get turretResourcesCost from gameInitScript
            int turretResourcesCost = GameInit.Instance.turrets.Find(turret => (turret.name == selectedUI.name)).resourcesCost;
            // Get turretCount from the turretUI
            int turretCount = int.Parse(selectedUI.transform.parent.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);

            if (resources >= turretResourcesCost)
            {
                if(turretCount > 0) {
                    GameActivity.Instance.SetSelectedTurretByTurretName(selectedUI.name, parentOfSelectedUI);
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUISelected);
                }
                else {
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                    CoroutineDisplayDialog("Turret not available to build.");
                }
            }
            else {
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                CoroutineDisplayDialog("You don't have sufficient coin to build this turret.");
            }
        }

    }

    public void CoroutineDisplayDialog(string message)
    {
        StartCoroutine(DisplayDialog(message));
    }

    public IEnumerator DisplayDialog(string message)
    {
        dialogUI.GetComponent<TMP_Text>().text = message;

        yield return new WaitForSecondsRealtime(1.5f);

        dialogUI.GetComponent<TMP_Text>().text = "";
    }
}
