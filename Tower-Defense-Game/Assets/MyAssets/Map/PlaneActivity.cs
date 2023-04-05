using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GameInit;

public class PlaneActivity : MonoBehaviour
{
    // All details from gameSystem
    private List<Turret> turrets;
    private GameObject gameSystem;

    // All details from UI
    // Display selected turret before creation
    public string selectedTurretName = null;
    public GameObject tempSelectedTurret;

    // Plane details
    private Renderer rend;
    public Color originalColor;
    public Color hoverColor;
    public Color disabledColor;

    // Top = 0, Left = 1, Bottom = 2, Right = 3
    public int turretInitDiretionCode = 0;
    public Boolean turretCreateAvailability = true;

    // Created turret
    private GameObject currentTurret;
    private string currentTurretName;
    private Boolean turretCreated = false;
   

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponentInChildren<Renderer>();

        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");
        turrets = gameSystem.GetComponent<GameInit>().turrets;
        PlaneMarking(gameSystem.GetComponent<GameInit>().debugMode);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaneMarking(DebugMode debugMode)
    {
        if (!turretCreateAvailability)
        {
            rend.material.color = disabledColor;
        }

        // Create turret on all plane to determine the direction facing
        if (debugMode.showAllTurret)
        {
            Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0, 0);
            currentTurret = Instantiate(turrets[0].turretPrefab, turretPosition, Quaternion.Euler(new Vector3(0, (turretInitDiretionCode * 90), 0)));
        }
    }

    private void OnMouseEnter()
    {
        // Add hover color and show the expected turret when game is not paused or ended
        if (turretCreateAvailability && Time.timeScale != 0)
        {
            rend.material.color = hoverColor;
            showSelectedTurret();
        }
    }

    private void OnMouseExit()
    {
        // Remove hover color and the expected turret
        if (turretCreateAvailability)
        {
            rend.material.color = originalColor;
            Destroy(tempSelectedTurret);
        }
    }

    private void OnMouseDown()
    {
        // Stop turret creation when game is paused or ended
        if(Time.timeScale != 0) { 
            if (turretCreateAvailability)
            {
                Destroy(tempSelectedTurret);
                createSelectedTurret();
            }
            else
            {
                // Turret existed on top of plane
                if (turretCreated)
                    Debug.Log("Turret does created on top of plane, kindly remove it first.");
                // Plane has been disabled for turret building
                else
                    Debug.Log("Turret not able to build on this plane.");
            }
        }
    }

    public void showSelectedTurret()
    {
        selectedTurretName = gameSystem.GetComponent<GameActivity>().selectedTurretName;
        if(selectedTurretName != null)
        {
            foreach (Turret turret in turrets)
            {
                // Show the turret on top of plane without the turret script
                if (selectedTurretName.Equals(turret.name))
                {
                    Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0, 0);
                    tempSelectedTurret = Instantiate(turret.turretPrefab, turretPosition, Quaternion.Euler(new Vector3(0, (turretInitDiretionCode * 90), 0)));
                    tempSelectedTurret.GetComponent<TurretActivity>().enabled = false;
                    break;
                }
            }
        }
    }

    public void createSelectedTurret()
    {
        if (selectedTurretName != null && turretCreateAvailability)
        {
            foreach (Turret turret in turrets)
            {
                // Create the selected turret on top of plane
                if (selectedTurretName.Equals(turret.name))
                {
                    Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0, 0);
                    currentTurret = Instantiate(turret.turretPrefab, turretPosition, Quaternion.Euler(new Vector3(0, (turretInitDiretionCode * 90), 0)));
                    currentTurretName = selectedTurretName;
                    turretCreated = true;
                    turretCreateAvailability = false;
                    rend.material.color = disabledColor;

                    // Reset the selected turret details in gameSystem
                    GameActivity gameActivityScript = gameSystem.GetComponent<GameActivity>();
                    int turretResourcesCost = turrets.Find(turret => (turret.name == gameActivityScript.selectedTurretName)).turretResourcesCost;

                    gameActivityScript.selectedTurretName = "";
                    gameActivityScript.resources -= turretResourcesCost;

                    // Decrease the text count value for the selected turretUI
                    Transform selectedTurretUI = gameActivityScript.selectedTurretUI.transform;
                    int count = int.Parse(selectedTurretUI.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);
                    count--;
                    selectedTurretUI.Find("AvailableCount/Count").GetComponent<TMP_Text>().text = count.ToString();

                    break;
                }
            }
        }
    }
}
