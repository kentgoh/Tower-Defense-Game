using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIActivity : MonoBehaviour
{
    // Timer UI Components
    private GameObject currentWaveUI;
    private GameObject nextWaveUI;
    private GameObject lastWaveUI;

    private TMP_Text timeText;
    private TMP_Text currentWaveTimeText;
    private Text nextWaveTimeText;
    private Image nextWaveTimeDial;

    // Speed Controller Components
    private TMP_Text speedControllerText;

    void Start()
    {
        if (transform.Find("Wave/CurrentWave").gameObject)
            currentWaveUI = transform.Find("Wave/CurrentWave").gameObject;
        if (transform.Find("Wave/NextWave").gameObject)
            nextWaveUI = transform.Find("Wave/NextWave").gameObject;
        if (transform.Find("Wave/LastWave").gameObject)
            lastWaveUI = transform.Find("Wave/LastWave").gameObject;

        if (transform.Find("Time/Time").GetComponent<TMP_Text>())
            timeText = transform.Find("Time/Time").GetComponent<TMP_Text>();
        if (transform.Find("Wave/CurrentWave/Counter").GetComponent<TMP_Text>())
            currentWaveTimeText = transform.Find("Wave/CurrentWave/Counter").GetComponent<TMP_Text>();
        if (transform.Find("Wave/NextWave/Counter/TimeDial").GetComponent<Image>())
            nextWaveTimeDial = transform.Find("Wave/NextWave/Counter/TimeDial").GetComponent<Image>();
        if (transform.Find("Wave/NextWave/Counter/TimeRemaining").GetComponent<Text>())
            nextWaveTimeText = transform.Find("Wave/NextWave/Counter/TimeRemaining").GetComponent<Text>();

        if (transform.Find("SpeedController/Button/Text").GetComponent<TMP_Text>())
            speedControllerText = transform.Find("SpeedController/Button/Text").GetComponent<TMP_Text>();
    }

    void Update()
    {
        DisplayTime();
        DisplayWave();
        DisplaySpeed();
    }

    public void DisplayTime()
    {
        timeText.text = Mathf.FloorToInt(GameActivity.Instance.ga_Time.time).ToString();
    }

    public void DisplayWave()
    {
        int maxWave = GameActivity.Instance.ga_Wave.totalWave;
        int currentWave = GameActivity.Instance.ga_Wave.currentWave;
        Boolean waveSpawnCompleted = GameActivity.Instance.ga_Wave.waveSpawnCompleted;

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
                nextWaveTimeText.text = Mathf.CeilToInt(GameActivity.Instance.ga_Time.timeForThisWave).ToString();

                // Image display
                float intervalBeforeNextWave = GameActivity.Instance.ga_Time.timeBeforeNextWave;
                float currentInterval = GameActivity.Instance.ga_Time.timeForThisWave;

                float timeRangeClamped = Mathf.InverseLerp(intervalBeforeNextWave, 0, currentInterval);
                nextWaveTimeDial.fillAmount = Mathf.Lerp(1, 0, timeRangeClamped);
            }
        }
    }

    public void DisplaySpeed()
    {
        if(Time.timeScale > 0)
            speedControllerText.text = Time.timeScale.ToString("F1");
    }

    public void ChangeTimeScale()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 1.5f;
        else if (Time.timeScale == 1.5f)
            Time.timeScale = 2.0f;
        else if (Time.timeScale == 2.0f)
            Time.timeScale = 5.0f;
        else if (Time.timeScale == 5.0f)
            Time.timeScale = 1.0f;
    }

}
