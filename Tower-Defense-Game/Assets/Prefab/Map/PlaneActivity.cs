using System;
using System.Collections;
using System.Collections.Generic;
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
    public Color hoverColor;
    public Color originalColor;

    public enum TurretInitDirection
    {
        Top = 1,
        Right = 2,
        Bottom = 3,
        Left = 4
    }
    public int turretInitDiretionCode;
    public Boolean turretCreateAvailability = true;

    // Created turret
    private GameObject currentTurret;
    private string currentTurretName;
    private Boolean turretCreated;
   

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");
        turrets = gameSystem.GetComponent<GameInit>().turrets;
        PlaneMarking();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaneMarking()
    {
        if (!turretCreateAvailability)
        {
            rend.material.color = Color.red;
        }
    }

    private void OnMouseEnter()
    {
        // Add hover color and show the expected turret 
        if (turretCreateAvailability)
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
        if (turretCreateAvailability)
        {
            createSelectedTurret();
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
                    Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0.25f, 0);
                    tempSelectedTurret = Instantiate(turret.turretPrefab, turretPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
                    tempSelectedTurret.GetComponent<TurretActivity>().enabled = false;
                    break;
                }
            }
        }
    }

    public void createSelectedTurret()
    {
        if (selectedTurretName != null)
        {
            foreach (Turret turret in turrets)
            {
                // Create the selected turret on top of plane
                if (selectedTurretName.Equals(turret.name))
                {
                    Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0.25f, 0);
                    currentTurret = Instantiate(turret.turretPrefab, turretPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
                    currentTurretName = selectedTurretName;
                    break;
                }
            }
        }
    }
}
