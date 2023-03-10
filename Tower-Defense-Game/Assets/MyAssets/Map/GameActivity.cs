using System;
using System.Collections;
using UnityEngine;

public class GameActivity : MonoBehaviour
{
    public GameObject pauseUI;

    // Timer
    public float time = 0;
    public int currentWave = 1;
    public float timeBeforeNextWave;
    public float timeForThisWave;
    public Boolean waveSpawnCompleted = false;

    // Turret
    public string selectedTurretName;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            AddTime();
            CountDownTimeForThisWave();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Pause game
            if(Time.timeScale == 1)
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
}
