using System;
using System.Collections;
using UnityEngine;

public class GameActivity : MonoBehaviour
{
    private GameObject pauseUI;
    private GameObject endGameUI;

    // End Point Health
    public int endPointHealth;

    // Timer
    public float time = 0;
    public int currentWave = 1;
    public float timeBeforeNextWave;
    public float timeForThisWave;
    public Boolean waveSpawnCompleted = false;

    // Resources
    public int resources = 0;
    public int resourcesPerSecond = 0;

    // Turret
    public string selectedTurretName;
    public GameObject selectedTurretUI;

    void Start()
    {
        Time.timeScale = 1;
        InitUI();
        InitEndPointHealth();
        StartCoroutine("AddResources");

    }

    // Update is called once per frame
    void Update()
    {
        // End game if endPointHealth = 0
        if(endPointHealth == 0)
            EndGame();

        // Add time if not paused
        if (Time.timeScale != 0)
        {
            AddTime();
            CountDownTimeForThisWave();
        }

        // Pause game when space key pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale != 0)
            {
                Debug.Log("Pause Triggered");
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
            else
            {
                Debug.Log("Resume Triggered");
                Time.timeScale = 1;
                pauseUI.SetActive(false);
            }

        }
    }

    public void AddTime()
    {
        time += Time.deltaTime;
    }

    public void CountDownTimeForThisWave()
    {
        if(timeForThisWave > 0)
            timeForThisWave -= Time.deltaTime;
    }

    public IEnumerator AddResources()
    {
        while (true)
        {
            if (Time.timeScale != 0)
            {
                yield return new WaitForSeconds(1);
                resources += resourcesPerSecond;
            }
        }
    }

    public void InitUI()
    {
        if (GameObject.Find("Canvas/PauseUI")) { 
            pauseUI = GameObject.Find("Canvas/PauseUI").gameObject;
            pauseUI.SetActive(false);
        }
        if (GameObject.Find("Canvas/EndGameUI")) { 
            endGameUI = GameObject.Find("Canvas/EndGameUI").gameObject;
            endGameUI.SetActive(false);
        }

        if (pauseUI == null || endGameUI == null)
            Debug.Log("InitUI Failed");

    }

    public void InitEndPointHealth()
    {
        endPointHealth = transform.GetComponent<GameInit>().endPointStartingHealth;
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        endGameUI.SetActive(true);
    }

}
