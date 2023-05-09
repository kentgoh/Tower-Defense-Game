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

    // Camera moving related
    public bool mouseReleasedOnCameraMoveButton = true;
    int directionIndex = 0;

    private new Camera camera;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        camera = Camera.main;
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
        ZoomCamera();
        MoveCamera();

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
                foreach (Turret turret in GameInit.Instance.turrets)
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

        foreach (Turret turret in GameInit.Instance.turrets)
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
                if (turretCount > 0) {
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

    // ==================== Camera moving related ====================
    public void ZoomCamera()
    {
        float zoomSpeed = 10.0f;

        if (camera != null)
        {
            float temp = camera.fieldOfView;
            temp += (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed) * (-1);

            // FOV 10~70
            if (temp >= 10 && temp <= 70)
                camera.fieldOfView = temp;
        }

    }

    public void MoveCamera()
    {
        float cameraMoveSpeed = 2.0f;

        if (!mouseReleasedOnCameraMoveButton)
        {
            if (directionIndex == (int)Direction.Up)
            {
                if (camera.transform.position.z <= -1)
                    camera.transform.Translate(new Vector3(0, 0, cameraMoveSpeed * Time.fixedDeltaTime), Space.World);
            }
            else if (directionIndex == (int)Direction.Down)
            {
                if (camera.transform.position.z >= -11)
                    camera.transform.Translate(new Vector3(0, 0, cameraMoveSpeed * (-1) * Time.fixedDeltaTime), Space.World);
            }
            else if (directionIndex == (int)Direction.Right)
            {
                if (camera.transform.position.x <= 5)
                    camera.transform.Translate(new Vector3(cameraMoveSpeed * Time.fixedDeltaTime, 0, 0), Space.World);
            }
            else if (directionIndex == (int)Direction.Left)
            {
                if (camera.transform.position.x >= -5)
                    camera.transform.Translate(new Vector3(cameraMoveSpeed * (-1) * Time.fixedDeltaTime, 0, 0), Space.World);
            }
        }
    }

    public void StartMoveCamera(int i)
    {
        directionIndex= i;
        mouseReleasedOnCameraMoveButton = false;
    }

    public void EndMoveCamera()
    {
        mouseReleasedOnCameraMoveButton = true;
    }

    public void InitCameraPosition()
    {
        camera.transform.position = new Vector3(0, 12, -5);
        camera.fieldOfView = 70;
    }
    

}
