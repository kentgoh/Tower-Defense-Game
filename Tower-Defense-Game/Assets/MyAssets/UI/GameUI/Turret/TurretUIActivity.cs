using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalPredefinedModel;
using Random = UnityEngine.Random;

public class TurretUIActivity : MonoBehaviour
{
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
        turrets = GameActivity.Instance.ga_Turret.turrets;

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
            UpdateTurretUIAvailabilityAndCount();
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

            createdNextTurretUI = Instantiate(turrets[randomTurretIndex].UI, nextTurretUIParent.transform);
            createdNextTurretUI.transform.SetAsFirstSibling();
            
            // Set the available count and border to not active
            if(createdNextTurretUI.transform.Find("AvailableCount"))
                createdNextTurretUI.transform.Find("AvailableCount").gameObject.SetActive(false);
            if (createdNextTurretUI.transform.Find("Border"))
                createdNextTurretUI.transform.Find("Border").gameObject.SetActive(false);

            // Start loading the cooldown
            nextTurretUICooldown = turrets[randomTurretIndex].UICooldown;
            nextTurretUILoader.SetActive(true);
        }
        if(nextTurretUICooldown >= 0)
            nextTurretUICooldown -= Time.deltaTime;

        // Push the turretUI to available turretUI
        if (BufferTurretUI(turrets[randomTurretIndex].UICooldown, nextTurretUICooldown, nextTurretUILoader))
        {
            AddTurretUICount(createdNextTurretUI);
            Destroy(createdNextTurretUI);
        }

    }
    public bool BufferTurretUI(int maxTurretUICooldown, float currentTurretUICooldown, GameObject turretUILoader)
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
        // TurretAUI (Clone) -> TurretAUI
        string turretUINameWithoutClone = createdNextTurretUI.name.Replace("(Clone)", "");

        if (availableTurretUIParent.transform.Find(turretUINameWithoutClone))
            GameActivity.Instance.AddTurretCountByUIName(turretUINameWithoutClone);

    }
    
    public void UpdateTurretUIAvailabilityAndCount()
    {
        GameActivity gameActivityScript = GameActivity.Instance;

        foreach (Transform child in availableTurretUIParent.transform)
        {
            // Get resourcesNeeded and count value from turretUI
            string turretNameWithoutUI = child.name.Replace("UI", "");
            Turret turret = turrets.Find((turret) => (turret.name.Equals(turretNameWithoutUI)));

            if(turret != null) {
                // Get resources value from GameActivity script
                int currentResources = gameActivityScript.ga_Resource.resources;

                // Get color string from GameInit script
                Color color;
                TurretUIColor turretUIColor = TurretUIColor.notAvailable;

                if (gameActivityScript.ga_Turret.selectedTurret != null && child.name.Contains(gameActivityScript.ga_Turret.selectedTurret.name))
                    turretUIColor = TurretUIColor.selected;
                else if (currentResources >= turret.resourcesCost && turret.count > 0)
                    turretUIColor = TurretUIColor.available;

                // Get color by hexastring
                ColorUtility.TryParseHtmlString(
                    GameInit.Instance.GetTurretUIColor(turretUIColor),
                    out color
                );
                child.Find("Border").GetComponent<Image>().color = color;
                child.Find("AvailableCount/Count").GetComponent<TMP_Text>().text = turret.count.ToString();
            }
            else
            {
                Debug.Log("Can't find turret from TurretUI with name: " + turretNameWithoutUI);
            }
        }
    }
}
