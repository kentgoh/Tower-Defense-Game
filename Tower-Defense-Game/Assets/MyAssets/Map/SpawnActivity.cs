using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;

public class SpawnActivity : MonoBehaviour
{
    // All details from gameSystem
    private GameObject gameSystem;
    private List<Enemy> enemies;
    private List<Wave> waves;
    private int totalWave;
    private int currentWaveIndex;

    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");
        enemies = gameSystem.GetComponent<GameInit>().enemies;
        waves = gameSystem.GetComponent<GameInit>().waves;
        totalWave = waves.Count;
        currentWaveIndex = 0;

        StartCoroutine(EnemySpawn());
    }

    public IEnumerator EnemySpawn()
    {
        // End this Coroutine when all wave has been completed
        while (currentWaveIndex < totalWave) {
            if (Time.timeScale != 0)
            {
                Wave wave = waves[currentWaveIndex];

                // Get total interval for this wave
                float totalInterval = 0;
                wave.enemySpawns.ForEach(enemySpawn =>
                {
                    totalInterval += enemySpawn.interval;
                });
                totalInterval += wave.intervalBeforeNextWave;

                GameActivity gameActivityScript = gameSystem.GetComponent<GameActivity>();
                gameActivityScript.ga_Time.timeBeforeNextWave = wave.intervalBeforeNextWave;
                gameActivityScript.ga_Time.timeForThisWave = totalInterval;

                for (int j = 0; j < wave.enemySpawns.Count; j++)
                {
                    EnemySpawn enemySpawn = wave.enemySpawns[j];
                    float secondsPerSpawn = enemySpawn.interval / enemySpawn.count;

                    StartCoroutine(EnemySpawnByWave(enemySpawn, secondsPerSpawn));
                    yield return new WaitForSeconds(enemySpawn.interval);
                }

                gameActivityScript.ga_Wave.waveSpawnCompleted = true;

                // Delay before next wave spawn
                yield return new WaitForSeconds(wave.intervalBeforeNextWave);

                currentWaveIndex++;
                // Stop adding wave count if it is last wave
                if (currentWaveIndex < totalWave) {
                    gameActivityScript.ga_Wave.waveSpawnCompleted = false;
                    gameActivityScript.ga_Wave.currentWave = currentWaveIndex + 1;
                }

            }
        }
    }

    public IEnumerator EnemySpawnByWave(EnemySpawn enemySpawn, float secondPerSpawn)
    {
        int enemyCount = enemySpawn.count;

        // End this Coroutine when all enemy has been spawned for this enemySpawn in this wave
        while(enemyCount > 0) {
            if (Time.timeScale != 0)
            {
                Enemy enemy = enemies.Find(x => x.enemyType.Equals(enemySpawn.enemyType));
                if (enemy.enemyPrefab != null) {
                    Instantiate(enemy.enemyPrefab, gameObject.transform.position, Quaternion.identity);
                    enemyCount--;
                }
            }

            yield return new WaitForSeconds(secondPerSpawn);
        }
    }

}
