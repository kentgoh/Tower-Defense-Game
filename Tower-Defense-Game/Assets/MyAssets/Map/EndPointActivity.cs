using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPointActivity : MonoBehaviour
{
    private GameObject gameSystem;

    public int health;
    private GameObject healthUI;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");

        if (transform.Find("Canvas/Health").gameObject)
            healthUI = transform.Find("Canvas/Health").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        health = gameSystem.GetComponent<GameActivity>().endPointHealth;
        healthUI.GetComponent<TMP_Text>().text = health.ToString();
    }

}
