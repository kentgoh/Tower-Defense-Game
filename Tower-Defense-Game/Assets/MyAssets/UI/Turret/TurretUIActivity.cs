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

        if (transform.Find("Decoration/BigGear/Image").gameObject)
            bigGearUI = transform.Find("Decoration/BigGear/Image").gameObject;
        if (transform.Find("Decoration/SmallGear/Image").gameObject)
            smallGearUI = transform.Find("Decoration/SmallGear/Image").gameObject;

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
        if (Time.timeScale == 1)
        {
            DecorationRotation();
            GenerateNewTurret();
        }
    }

    public void DecorationRotation() {
        bigGearUI.transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        smallGearUI.transform.Rotate(Vector3.back, 50 * Time.deltaTime);
    }
    public void GenerateNewTurret()
    {
        // Set the turretUI as the child of nextTurretUI
        if (nextTurretUIParent.transform.childCount < 2) {
            // Get random turretUI
            randomTurretIndex = Random.Range(0, turrets.Count);

            createdNextTurretUI = Instantiate(turrets[randomTurretIndex].turretUI, nextTurretUIParent.transform);
            createdNextTurretUI.transform.SetAsFirstSibling();
            
            // Start loading the cooldown
            nextTurretUICooldown = turrets[randomTurretIndex].turretUICooldown;
            nextTurretUILoader.SetActive(true);
        }
        if(nextTurretUICooldown >= 0)
            nextTurretUICooldown -= Time.deltaTime;

        // Push the turretUI to available turretUI
        if (BufferTurretUI(turrets[randomTurretIndex].turretUICooldown, nextTurretUICooldown, nextTurretUILoader))
        {
            createdNextTurretUI.transform.SetParent(availableTurretUIParent.transform);
            nextTurretUILoader.SetActive(false);
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
}
