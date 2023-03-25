using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameInit;

public class TimerUIActivity : MonoBehaviour
{
    // All details from gameSystem
    private GameObject gameSystem;

    // Timer UI Components
    public GameObject currentWaveUI;
    public GameObject nextWaveUI;
    public GameObject lastWaveUI;

    public TMP_Text timeText;
    public TMP_Text currentWaveTimeText;
    public Text nextWaveTimeText;
    public Image nextWaveTimeDial;


    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");

        if (transform.Find("Wave/CurrentWave").gameObject)
            currentWaveUI = transform.Find("Wave/CurrentWave").gameObject;
        if (transform.Find("Wave/NextWave").gameObject)
            nextWaveUI = transform.Find("Wave/NextWave").gameObject;
        if (transform.Find("Wave/LastWave").gameObject)
            lastWaveUI = transform.Find("Wave/LastWave").gameObject;

        if (transform.Find("Time").GetComponent<TMP_Text>())
            timeText = transform.Find("Time").GetComponent<TMP_Text>();
        if (transform.Find("Wave/CurrentWave/Counter").GetComponent<TMP_Text>())
            currentWaveTimeText = transform.Find("Wave/CurrentWave/Counter").GetComponent<TMP_Text>();
        if (transform.Find("Wave/NextWave/Counter/TimeDial").GetComponent<Image>())
            nextWaveTimeDial = transform.Find("Wave/NextWave/Counter/TimeDial").GetComponent<Image>();
        if (transform.Find("Wave/NextWave/Counter/TimeRemaining").GetComponent<Text>())
            nextWaveTimeText = transform.Find("Wave/NextWave/Counter/TimeRemaining").GetComponent<Text>();
    }

    void Update()
    {
            DisplayTime();
            DisplayWave();
    }

    public void DisplayTime()
    {
        timeText.text = Mathf.FloorToInt(gameSystem.GetComponent<GameActivity>().time).ToString();
    }

    public void DisplayWave()
    {
        int maxWave = gameSystem.GetComponent<GameInit>().waves.Count;
        int currentWave = gameSystem.GetComponent<GameActivity>().currentWave;
        Boolean waveSpawnCompleted = gameSystem.GetComponent<GameActivity>().waveSpawnCompleted;

        // Current wave is last wave
        if (maxWave == currentWave)
        {
            currentWaveUI.SetActive(false);
            nextWaveUI.SetActive(false);
            lastWaveUI.SetActive(true);
        }
        else {
            // Current wave is spawning
            if (!waveSpawnCompleted)
            {
                currentWaveUI.SetActive(true);
                nextWaveUI.SetActive(false);
                lastWaveUI.SetActive(false);

                currentWaveTimeText.text = currentWave.ToString();
            }
            // Wait for next wave
            else {
                currentWaveUI.SetActive(false);
                nextWaveUI.SetActive(true);
                lastWaveUI.SetActive(false);

                // Text display
                nextWaveTimeText.text = Mathf.CeilToInt(gameSystem.GetComponent<GameActivity>().timeForThisWave).ToString();

                // Image display
                float intervalBeforeNextWave = gameSystem.GetComponent<GameActivity>().timeBeforeNextWave;
                float currentInterval = gameSystem.GetComponent<GameActivity>().timeForThisWave;

                float timeRangeClamped = Mathf.InverseLerp(intervalBeforeNextWave, 0, currentInterval);
                nextWaveTimeDial.fillAmount = Mathf.Lerp(1, 0, timeRangeClamped);
            }
        }
    }
}
