using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GlobalPredefinedModel;

public class PlaneActivity : MonoBehaviour
{
    public GameObject tempSelectedTurret;

    // Plane details
    private Renderer rend;
    public Color originalColor;
    public Color hoverColor;
    public Color disabledColor;
    public Boolean turretCreateAvailability = true;

    // Created turret
    private GameObject currentTurret;
    private string currentTurretName;
    private Boolean turretCreated = false;
   

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponentInChildren<Renderer>();

        PlaneMarking(GameInit.Instance.debugMode);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaneMarking(DebugMode debugMode)
    {
        if (!turretCreateAvailability)
            rend.material.color = disabledColor;

        // Create turret on all plane to determine the direction facing
        if (debugMode.showAllTurret)
        {
            if (turretCreateAvailability) { 
                Vector3 turretPosition = gameObject.transform.position;
                currentTurret = Instantiate(GameInit.Instance.turrets[0].prefab, turretPosition, gameObject.transform.rotation);
            }
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
                CreateSelectedTurret();
            }
            else
            {
                // Turret existed on top of plane
                if (turretCreated)
                {
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                    Debug.Log("Turret does created on top of plane, kindly remove it first.");
                }
                // Plane has been disabled for turret building
                else
                {
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                    Debug.Log("Turret not able to build on this plane.");
                }
            }
        }
    }

    public void showSelectedTurret()
    {
        Turret turret = GameActivity.Instance.ga_Turret.selectedTurret;
        if(turret != null)
        {   
            Vector3 turretPosition = gameObject.transform.position ;
            tempSelectedTurret = Instantiate(turret.prefab, turretPosition, gameObject.transform.rotation);
            tempSelectedTurret.GetComponent<TurretActivity>().enabled = false;

        }
    }

    public void CreateSelectedTurret()
    {
        Turret turret = GameActivity.Instance.ga_Turret.selectedTurret;
        if (turret != null)
        {
            Vector3 turretPosition = gameObject.transform.position + new Vector3(0, 0, 0);
            currentTurret = Instantiate(turret.prefab, turretPosition, gameObject.transform.rotation);
            turretCreated = true;
            turretCreateAvailability = false;
            rend.material.color = disabledColor;

            GameActivity.Instance.ResetSelectedTurretAfterCreated();
            AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretBuild);

        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
            Debug.Log("No turret has been selected");
        }
    }
}
