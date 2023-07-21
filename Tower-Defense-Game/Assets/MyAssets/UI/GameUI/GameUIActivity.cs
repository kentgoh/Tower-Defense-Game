using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GlobalPredefinedModel;

public class GameUIActivity : MonoBehaviour, IPointerClickHandler
{
    public static GameUIActivity Instance;

    // Set manually
    public GameObject turretDetailsUI;
    public GameObject spellDetailsUI;
    public GameObject dialogUI;
    public GameObject spellIndicatorPrefab;

    // Camera moving related
    public bool mouseReleasedOnCameraMoveButton = true;
    int directionIndex = 0;

    private new Camera camera;
    private GameObject spellIndicator;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        camera = Camera.main;
        spellDetailsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MouseHoverDetection();
        ZoomCamera();
        MoveCamera();
        ShowSpellIndicator();

    }

    // ======================= MOUSE ACTIVITY ======================= 
    // Mouse hover activity
    public void MouseHoverDetection()
    {
        // Get current pointer details by mousePosition
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Get raycast result by pointer details
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Mouse pointer does point to more than one gameObject
        if (results.Count > 0)
        {
            Boolean tagMatched = false;
            foreach (RaycastResult result in results)
            {
                // Spell UI mouse hover activity
                if (result.gameObject.CompareTag("SpellUI"))
                {
                    List<Spell> spells = GameActivity.Instance.ga_Spell.spells;

                    // The parent name of the pointed image is the spell name
                    string pointedSpellName = result.gameObject.transform.parent.name;

                    EnablePointedSpellDetailsUI(pointedSpellName);
                    tagMatched = true;
                    break;

                }

                // Turret UI mouse hover activity
                foreach (Turret turret in GameInit.Instance.turrets)
                {
                    // disable all UI first, then enable the selected one only
                    if (result.gameObject.name.Equals(turret.name))
                    {
                        DisableAllTurretDetailsUI();
                        EnablePointedTurretDetailsUI(turret);
                        tagMatched = true;
                        break;
                    }
                }
            }

            if (!tagMatched)
            {
                // Mouse pointer does point to more than one gameObject but not spellUI or turretUI
                DisableAllSpellDetailsUI();
                DisableAllTurretDetailsUI();
            }

        }
        else
        {
            DisableAllSpellDetailsUI();
            DisableAllTurretDetailsUI();
        }
    }

    // Mouse click activity
    public void OnPointerClick(PointerEventData eventData)
    {
        // Left click
        if(eventData.button == PointerEventData.InputButton.Left) {
            GameObject selectedUI = eventData.pointerCurrentRaycast.gameObject;

            // Check selected turret UI
            if (selectedUI.CompareTag("TurretUI"))
                CheckTurretUISelection(selectedUI);
            // Check selected spell UI
            else if (selectedUI.CompareTag("SpellUI"))
                CheckSpellUISelection(selectedUI);
        }
        // Right click
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            GameActivity.Instance.ResetSelectedSpell();
            GameActivity.Instance.ResetSelectedTurret();
            DisableAllSpellDetailsUI();
            DisableAllTurretDetailsUI();
        }

    }

    // ==================== Turret UI Related ====================
    // Enable the pointed turret details
    public void EnablePointedTurretDetailsUI(Turret turret)
    {
        turretDetailsUI.SetActive(true);
        turret.detailsUI.SetActive(true);
    }

    // Disable all the turret details
    public void DisableAllTurretDetailsUI()
    {
        turretDetailsUI.SetActive(false);

        foreach (Turret turret in GameInit.Instance.turrets)
            turret.detailsUI.SetActive(false);
    }

    public void CheckTurretUISelection(GameObject selectedUI)
    {
        GameObject parentOfSelectedUI = selectedUI.transform.parent.gameObject;

        // Get resources value from gameActivityScript
        int resources = GameActivity.Instance.ga_Resource.resources;

        // Get turretResourcesCost from gameInitScript
        int turretResourcesCost = GameInit.Instance.turrets.Find(turret => (turret.name == selectedUI.name)).resourcesCost;
        // Get turretCount from the turretUI
        int turretCount = int.Parse(selectedUI.transform.parent.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);

        if (resources >= turretResourcesCost)
        {
            if (turretCount > 0)
            {
                GameActivity.Instance.SetSelectedTurretByTurretName(selectedUI.name, parentOfSelectedUI);
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUISelected);
            }
            else
            {
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                CoroutineDisplayDialog("Turret not available to build.");
            }
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
            CoroutineDisplayDialog("You don't have sufficient coin to build this turret.");
        }
        GameActivity.Instance.ResetSelectedSpell();

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
        directionIndex = i;
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

    // ==================== Spell UI Related ====================
    public void DisableAllSpellDetailsUI()
    {
        spellDetailsUI.SetActive(false);
    }

    public void EnablePointedSpellDetailsUI(string spellName)
    {
        spellDetailsUI.SetActive(true);

        foreach (Transform child in spellDetailsUI.transform)
        {
            child.gameObject.SetActive(true);

            // Deactivate all spellDetailsUI not same name as pointed spellUI
            if (!child.name.Equals(spellName))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void CheckSpellUISelection(GameObject selectedUI)
    {
        // The parent name of the pointed image is the spell name
        string selectedSpellName = selectedUI.transform.parent.name;

        Spell selectedSpell = GameActivity.Instance.ga_Spell.spells.Find(x => x.spellType.ToString().Equals(selectedSpellName));
        if (!selectedSpell.Equals(null))
        {
            if(selectedSpell.currentCooldown == 0)
            {
                GameActivity.Instance.ga_Spell.selectedSpell = selectedSpell;
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUISelected);
                GameActivity.Instance.ResetSelectedTurret();
                HighlightSelectedSpellUI(selectedUI);

            }
            else
            {
                // Spell in cooldown, can't select
                AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.TurretUIError);
                CoroutineDisplayDialog("Spell is still in cooldown.");
            }
        }
        else
            DisableAllSpellDetailsUI();

    }

    public void HighlightSelectedSpellUI(GameObject selectedUI)
    {
        // Make others spell border color to transparent
        Transform spellList = selectedUI.transform.parent.parent;

        foreach(Transform child in spellList)
        {
            if (child.Find("Border").GetComponent<Image>())
            {
                // Make the border of the selected spellUI not transparent while others remain transparent
                Image border = child.Find("Border").GetComponent<Image>();
                if (child.Equals(selectedUI.transform.parent))
                    border.color = new Color(border.color.r, border.color.g, border.color.b, 255f);
                else
                    border.color = new Color(border.color.r, border.color.g, border.color.b, 0f);

            }
        }

    }

    private void ShowSpellIndicator()
    {
        if (GameActivity.Instance.ga_Spell.selectedSpell != null) { 
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Map")))
            {
                if (raycastHit.collider.CompareTag("Base"))
                {
                    Vector3 pointedBaseLocation = raycastHit.point;
                    pointedBaseLocation.y = 0.5f;
                    // The x and z value will have the value of 0.5 according to the position of the mouse pointed on the base
                    // Example:
                    //      0.2 => 0.5
                    //      -1.7 => -1.5
                    pointedBaseLocation.x = Mathf.Sign(pointedBaseLocation.x) * (Mathf.Floor(Mathf.Abs(pointedBaseLocation.x)) + 0.5f);
                    pointedBaseLocation.z = Mathf.Sign(pointedBaseLocation.z) * (Mathf.Floor(Mathf.Abs(pointedBaseLocation.z)) + 0.5f);

                    // Create spellIndicator if it does not exist yet
                    if(spellIndicator == null)
                        spellIndicator = Instantiate(spellIndicatorPrefab, pointedBaseLocation, Quaternion.identity);
                    // Only destroy old spellIndicator if the instantiate position need to be changed
                    else if (!pointedBaseLocation.Equals(spellIndicator.transform.position)) {
                        Destroy(spellIndicator);
                        spellIndicator = Instantiate(spellIndicatorPrefab, pointedBaseLocation, Quaternion.identity);
                    }
                }
                else
                    Destroy(spellIndicator);

            }
        }
        else
            Destroy(spellIndicator);
    }

}
