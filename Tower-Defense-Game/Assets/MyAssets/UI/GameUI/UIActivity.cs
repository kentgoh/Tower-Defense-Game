using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalPredefinedModel;

public class UIActivity : MonoBehaviour, IPointerClickHandler
{
    private GameObject turretDetailsUIBackground;
    private List<Turret> turrets;
    private string selectedTurretName;
    private GameObject gameSystem;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");
        turrets = gameSystem.GetComponent<GameInit>().turrets;
        turretDetailsUIBackground = gameSystem.GetComponent<GameInit>().turretDetailsBackground;
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

        displayTurretDetailsUI(results);
       
    }

    // ======================= MOUSE ACTIVITY ======================= 
    // Mouse hover activity
    public void displayTurretDetailsUI(List<RaycastResult> results)
    {
        // Mouse pointer does point to more than one gameObject
        if (results.Count > 0)
        {
            Boolean nameMatched = false;
            foreach (RaycastResult result in results)
            {
                foreach(Turret turret in turrets)
                {
                    // disable all UI first, then enable the selected one only
                    if (result.gameObject.name.Equals(turret.name))
                    {
                        disableAllTurretDetailsUI();
                        enablePointedTurretDetailsUI(turret);
                        nameMatched = true;
                        break;
                    }
                }
                // Mouse pointer does point to more than one gameObject but no turretDetails matched
                if (!nameMatched)
                {
                    disableAllTurretDetailsUI();
                }
            }
        }
        else
        {
            disableAllTurretDetailsUI();
        }
    }

    // Enable the pointed turret details
    public void enablePointedTurretDetailsUI(Turret turret) {
        turretDetailsUIBackground.SetActive(true);
        turret.detailsUI.SetActive(true);
    }

    // Disable all the turret details
    public void disableAllTurretDetailsUI()
    {
        turretDetailsUIBackground.SetActive(false);

        foreach(Turret turret in turrets)
        {
            turret.detailsUI.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject selectedUI = eventData.pointerCurrentRaycast.gameObject;

        // Check selected turret UI
        displayTurretUISelection(selectedUI);
    }

    public void displayTurretUISelection(GameObject selectedUI)
    {
        GameObject parentOfSelectedUI = selectedUI.transform.parent.gameObject;

        if (selectedUI.tag.Equals("TurretUI"))
        {
            // Get resources value from gameActivityScript
            GameActivity gameActivityScript = gameSystem.GetComponent<GameActivity>();
            int resources = gameActivityScript.ga_Resource.resources;
            
            // Get turretResourcesCost from gameInitScript
            int turretResourcesCost = turrets.Find(turret => (turret.name == selectedUI.name)).resourcesCost;
            // Get turretCount from the turretUI
            int turretCount = int.Parse(selectedUI.transform.parent.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);

            if (resources >= turretResourcesCost)
            {
                if(turretCount > 0) { 
                    gameActivityScript.ga_Turret.selectedTurretName = selectedUI.name;
                    gameActivityScript.ga_Turret.selectedTurretUI = parentOfSelectedUI;
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUISelected);
                }
                else {
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                    Debug.Log("Turret not available to build");
                }
            }
            else {
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                Debug.Log("You don't have sufficient coin to build this turret");
            }
        }

    }
}
