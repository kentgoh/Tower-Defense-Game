using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using static GameInit;
using Random = UnityEngine.Random;

public class TurretUIActivity : MonoBehaviour
{
    private GameObject gameSystem;
    private List<Turret> turrets;

    // Decoration
    public GameObject bigGearUI;
    public GameObject smallGearUI;

    // Next Turret
    public GameObject nextTurretUIParent;
    public GameObject nextTurretUILoader;
    public TMP_Text nextTurretUILoaderText;
    public float nextTurretUICooldown;
    public GameObject createdNextTurretUI;
    public int randomTurretIndex;

    // Available Turret
    public GameObject availableTurretUIParent;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");
        turrets = gameSystem.GetComponent<GameInit>().turrets;

        // Decoration animation
        if (transform.Find("Decoration/BigGear/Image").gameObject)
            bigGearUI = transform.Find("Decoration/BigGear/Image").gameObject;
        if (transform.Find("Decoration/SmallGear/Image").gameObject)
            smallGearUI = transform.Find("Decoration/SmallGear/Image").gameObject;

        // Turret UI activity
        if (transform.Find("NextTurret/Turret").gameObject)
            nextTurretUIParent = transform.Find("NextTurret/Turret").gameObject;
        if (transform.Find("AvailableTurret/Turrets").gameObject)
            availableTurretUIParent = transform.Find("AvailableTurret/Turrets").gameObject;
        if (transform.Find("NextTurret/Turret/TurretUILoader").gameObject) { 
            nextTurretUILoader = transform.Find("NextTurret/Turret/TurretUILoader").gameObject;

            if (nextTurretUILoader.transform.Find("Counter"))
                nextTurretUILoaderText = nextTurretUILoader.transform.Find("Counter").GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            DecorationRotation();
            GenerateNewTurretUI();
            UpdateTurretUIAvailability();
        }
    }

    public void DecorationRotation() {
        bigGearUI.transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        smallGearUI.transform.Rotate(Vector3.back, 50 * Time.deltaTime);
    }
    public void GenerateNewTurretUI()
    {
        // Set the turretUI as the child of nextTurretUI
        if (nextTurretUIParent.transform.childCount < 2) {
            // Get random turretUI
            randomTurretIndex = Random.Range(0, turrets.Count);

            createdNextTurretUI = Instantiate(turrets[randomTurretIndex].turretUI, nextTurretUIParent.transform);
            createdNextTurretUI.transform.SetAsFirstSibling();
            
            // Set the available count and border to not active
            if(createdNextTurretUI.transform.Find("AvailableCount"))
                createdNextTurretUI.transform.Find("AvailableCount").gameObject.SetActive(false);
            if (createdNextTurretUI.transform.Find("Border"))
                createdNextTurretUI.transform.Find("Border").gameObject.SetActive(false);

            // Start loading the cooldown
            nextTurretUICooldown = turrets[randomTurretIndex].turretUICooldown;
            nextTurretUILoader.SetActive(true);
        }
        if(nextTurretUICooldown >= 0)
            nextTurretUICooldown -= Time.deltaTime;

        // Push the turretUI to available turretUI
        if (BufferTurretUI(turrets[randomTurretIndex].turretUICooldown, nextTurretUICooldown, nextTurretUILoader))
        {
            AddTurretUICount(createdNextTurretUI);
            Destroy(createdNextTurretUI);
        }

    }
    public Boolean BufferTurretUI(int maxTurretUICooldown, float currentTurretUICooldown, GameObject turretUILoader)
    {
        // Update the loader image
        float imageFillRange = Mathf.InverseLerp(0, maxTurretUICooldown, currentTurretUICooldown);
        turretUILoader.GetComponent<Image>().fillAmount = imageFillRange; 

        // Update the loader text
        nextTurretUILoaderText.text = Mathf.Ceil(currentTurretUICooldown).ToString();
        return (currentTurretUICooldown <= 0);
    }

    public void AddTurretUICount(GameObject createdNextTurretUI)
    {
        string turretNameWithoutClone = createdNextTurretUI.name.Replace("(Clone)", "");

        if (availableTurretUIParent.transform.Find(turretNameWithoutClone))
        {
            // Increase the text count value for the selected turretUI
            Transform turretUI = availableTurretUIParent.transform.Find(turretNameWithoutClone);
            int count = int.Parse(turretUI.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);
            count++;
            turretUI.Find("AvailableCount/Count").GetComponent<TMP_Text>().text = count.ToString();
        }
    }
    
    public void UpdateTurretUIAvailability()
    {
        GameInit gameInitScript = gameSystem.GetComponent<GameInit>();
        GameActivity gameActivityScript = gameSystem.GetComponent<GameActivity>();

        foreach (Transform child in availableTurretUIParent.transform)
        {
            // Get resourcesNeeded and count value from turretUI
            int turretUIResourcesNeeded = int.Parse(child.Find("ResourcesNeeded").GetComponent<TMP_Text>().text);
            int turretUIAvailableCount = int.Parse(child.Find("AvailableCount/Count").GetComponent<TMP_Text>().text);

            // Get resources value from GameActivity script
            int currentResources = gameActivityScript.resources;

            // Get color string from GameInit script
            Color color;
            TurretUIColor turretUIColor = TurretUIColor.notAvailable;

            if((gameActivityScript.selectedTurretName.Length > 0) && child.name.Contains(gameActivityScript.selectedTurretName))
                turretUIColor = TurretUIColor.selected;
            else if (currentResources >= turretUIResourcesNeeded && turretUIAvailableCount > 0)
                turretUIColor = TurretUIColor.available;

            // Get color by hexastring
            ColorUtility.TryParseHtmlString(
                gameInitScript.GetTurretUIColor(turretUIColor),
                out color
            );
            child.Find("Border").GetComponent<Image>().color = color;
        }
    }
}
