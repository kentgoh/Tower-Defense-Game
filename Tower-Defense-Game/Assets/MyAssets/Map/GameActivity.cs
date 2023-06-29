using System.Collections;
using UnityEngine;
using static GlobalPredefinedModel;

public class GameActivity : MonoBehaviour
{
    public static GameActivity Instance { get; private set; }

    public GA_Wave ga_Wave;
    public GA_Time ga_Time;
    public GA_Resource ga_Resource;
    public GA_Turret ga_Turret;
    public GA_Spell ga_Spell;

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
        if (!Instance)
            Instance = this;

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
            if (endPointHealth < 1)
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
        GameInit gameInitScript = GameInit.Instance;     
        endPointHealth = gameInitScript.endPointStartingHealth;
        ga_Wave = new GA_Wave(gameInitScript.waves.Count, false);
        ga_Time = new GA_Time();
        ga_Resource = new GA_Resource(gameInitScript.startingResources, gameInitScript.resourcesPerSecond);
        ga_Turret = new GA_Turret(gameInitScript.turrets);
        ga_Spell = new GA_Spell(gameInitScript.startingSpellTypes, gameInitScript.spells);

    }

    public void DecreaseEndpointHealth(int enemyDamageDeal)
    {
        endPointHealth -= enemyDamageDeal;
    }

    public void SetSelectedTurretByTurretName(string turretName, GameObject turretUI)
    {
        Turret turret = ga_Turret.turrets.Find(turret => (turret.name.Equals(turretName)));
        if (turret != null) { 
            ga_Turret.selectedTurret = turret;
            ga_Turret.selectedTurretUI = turretUI;
        }

    }

    public void ResetSelectedTurretAfterCreated()
    {
        int count = 0;
        ga_Resource.resources -= ga_Turret.selectedTurret.resourcesCost;

        // Decrease the count for the selectedTurret
        foreach (Turret turret in ga_Turret.turrets)
        {
            if (turret.name.Equals(ga_Turret.selectedTurret.name))
            {
                turret.count--;
                count = turret.count;
                break;
            }
        }

        // Unselect turret if count less than 1 or insufficient resources
        if(count < 1 || (ga_Resource.resources < ga_Turret.selectedTurret.resourcesCost))
            ga_Turret.selectedTurret = null;
    }

    public void UpdateTimeValueRelatedToWave(float timeBeforeNextWave, float timeRemainingForThisWave)
    {
        ga_Time.timeBeforeNextWave = timeBeforeNextWave;
        ga_Time.timeForThisWave = timeRemainingForThisWave;
    }

    public void WaveSpawnCompleted()
    {
        ga_Wave.waveSpawnCompleted = true;
    }

    public void StartNewWaveSpawn()
    {
        ga_Wave.waveSpawnCompleted = false;
        ga_Wave.currentWave++;
    }

    public void AddTurretCountByUIName(string turretUIName)
    {
        // TurretAUI -> TurretA
        string turretNameWithoutUI = turretUIName.Replace("UI", "");

        foreach (Turret turret in ga_Turret.turrets)
        {
            if (turret.name.Equals(turretNameWithoutUI)) {
                turret.count++;
                break;
            }
        }
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
        loseGameUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.WinGame);
        winGameUI.SetActive(true);
        Time.timeScale = 0;
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
