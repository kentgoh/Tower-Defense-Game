using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;

public class GameActivity : MonoBehaviour
{
    public GA_Wave ga_Wave;
    public GA_Time ga_Time;
    public GA_Resource ga_Resource;
    public GA_Turret ga_Turret;

    // end Point Health
    public int endPointHealth;
    // time scale before pause
    private float tempTimeScale;

    // Add manually
    public GameObject pauseGameUI;
    public GameObject loseGameUI;
    public GameObject winGameUI;

    void Awake()
    {
        InitParameters();
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
                (ga_Wave.currentWave == ga_Wave.totalWave) 
                && ga_Wave.waveSpawnCompleted
                && (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
             )
                WinGame();

            AddTime();
            CountDownTimeForThisWave();
        }
    }

    public void AddTime()
    {
        ga_Time.time += Time.deltaTime;
    }

    public void CountDownTimeForThisWave()
    {
        if(ga_Time.timeForThisWave > 0)
            ga_Time.timeForThisWave -= Time.deltaTime;
    }

    public IEnumerator AddResources()
    {
        while (true)
        {
            if (Time.timeScale != 0)
            {
                yield return new WaitForSeconds(1);
                ga_Resource.resources += ga_Resource.resourcesPerSecond;
            }
        }
    }

    public void InitParameters()
    {
        Time.timeScale = 1;

        // Initialize some parameter through value in gameInitScript
        GameInit gameInitScript = transform.GetComponent<GameInit>();       
        endPointHealth = gameInitScript.endPointStartingHealth;
        ga_Wave = new GA_Wave(gameInitScript.waves.Count, false);
        ga_Time = new GA_Time();
        ga_Resource = new GA_Resource(gameInitScript.startingResources, gameInitScript.resourcesPerSecond);
        ga_Turret = new GA_Turret(gameInitScript.turrets);

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
