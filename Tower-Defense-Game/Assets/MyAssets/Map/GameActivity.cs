using System;
using System.Collections;
using UnityEngine;


public class GameActivity : MonoBehaviour
{
    // ActivityUI
    // Add manually
    public GameObject pauseGameUI;
    public GameObject loseGameUI;
    public GameObject winGameUI;
    private float tempTimeScale;

    // End Point Health
    public int endPointHealth;

    // Timer
    public int totalWave;
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

    void Awake()
    {
        Time.timeScale = 1;
        InitValueFromGameInitScript();
        StartCoroutine("AddResources");

        AudioManager.Instance.PlayBGM();

    }

    // Update is called once per frame
    void Update()
    {
        // Add time if not paused
        if (Time.timeScale != 0)
        {
            // Lose game if endPointHealth = 0
            if (endPointHealth < 0)
            LoseGame();

            // Win game if no enemy left after final wave
            if(
                (currentWave == totalWave) 
                && waveSpawnCompleted 
                && (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
             )
                WinGame();

            AddTime();
            CountDownTimeForThisWave();
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

    public void InitValueFromGameInitScript()
    {
        GameInit gameInitScript = transform.GetComponent<GameInit>();

        endPointHealth = gameInitScript.endPointStartingHealth;
        totalWave = gameInitScript.waves.Count;
    }

    // Activity UI
    public void PauseGame()
    {
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0;
        pauseGameUI.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = tempTimeScale;
        pauseGameUI.SetActive(false);
    }
    public void LoseGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.LoseGame);
        Time.timeScale = 0;
        loseGameUI.SetActive(true);
    }

    public void WinGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.WinGame);
        Time.timeScale = 0;
        winGameUI.SetActive(true);
    }

    public void RestartGame()
    {
        ScenesManager.Instance.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
