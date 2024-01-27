using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalPredefinedModel;

public class PlaneActivity : MonoBehaviour
{
    public GameObject tempSelectedTurret;

    // Plane details
    private Renderer rend;
    private Color originalColor;
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
        originalColor = rend.material.color;

        PlaneMarking(GameInit.Instance.debugMode);
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
                currentTurret = Instantiate(GameActivity.Instance.ga_Turret.turrets[0].prefab, turretPosition, gameObject.transform.rotation);
            }
        }
    }

    private void OnMouseEnter()
    {
        if(Time.timeScale != 0) {
            // Add hover color and show the expected turret when game is not paused or ended
            if (turretCreateAvailability && !GameActivity.Instance.ga_MouseState.IsOnUI())
            {
                GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.Plane_Map);
                ShowSelectedTurret();
            }
        }

    }

    private void OnMouseExit()
    {
        if(Time.timeScale != 0) { 
            // Remove hover color and the expected turret
            if (turretCreateAvailability)
                ResetPlane();

            if (GameActivity.Instance.ga_MouseState.mouseState == MouseState.Plane_Map)
                    GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.None);
        }

    }

    private void OnMouseDown()
    {
        // Stop turret creation when game is paused or ended
        if(Time.timeScale != 0) {
            if (!GameActivity.Instance.ga_MouseState.IsOnUI()) { 
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
                        GameUIActivity.Instance.CoroutineDisplayDialog("Turret does created on top of plane, kindly remove it first.");
                    }
                    // Plane has been disabled for turret building
                    else
                    {
                        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                        GameUIActivity.Instance.CoroutineDisplayDialog("Turret not able to build on this plane.");
                    }
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if(Time.timeScale != 0) { 
            // Only enable the hover function in Plane when there's no UI on top of it
            if (!GameActivity.Instance.ga_MouseState.IsOnUI())
            {
                GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.Plane_Map);
                if (turretCreateAvailability && tempSelectedTurret == null)
                    ShowSelectedTurret();
            }
            else
            {
                if(turretCreateAvailability)
                    ResetPlane();
            }
        }

    }

    public void ShowSelectedTurret()
    {
        // Change plane color to hover color
        rend.material.color = hoverColor;

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

            GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.None);

        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
            GameUIActivity.Instance.CoroutineDisplayDialog("No turret has been selected.");
        }
    }

    public void ResetPlane()
    {
        rend.material.color = originalColor;
        
        if(tempSelectedTurret != null)
            Destroy(tempSelectedTurret);
    }

}
