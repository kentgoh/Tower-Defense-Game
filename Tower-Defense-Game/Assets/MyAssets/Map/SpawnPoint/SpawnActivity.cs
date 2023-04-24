using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalPredefinedModel;

public class SpawnActivity : MonoBehaviour
{
    private List<Wave> waves;
    private int totalWave;
    private int currentWaveIndex;
    void Start()
    {
        waves = GameInit.Instance.waves;
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

                GameActivity.Instance.UpdateTimeValueRelatedToWave(wave.intervalBeforeNextWave, totalInterval);

                for (int j = 0; j < wave.enemySpawns.Count; j++)
                {
                    EnemySpawn enemySpawn = wave.enemySpawns[j];
                    float secondsPerSpawn = enemySpawn.interval / enemySpawn.count;

                    StartCoroutine(EnemySpawnByWave(enemySpawn, secondsPerSpawn));
                    yield return new WaitForSeconds(enemySpawn.interval);
                }

                GameActivity.Instance.WaveSpawnCompleted();

                // Delay before next wave spawn
                yield return new WaitForSeconds(wave.intervalBeforeNextWave);

                currentWaveIndex++;
                // Add wave count if it is not last wave
                if (currentWaveIndex < totalWave) {
                    GameActivity.Instance.StartNewWaveSpawn();
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
                Enemy enemy = GameInit.Instance.enemies.Find(x => x.enemyType.Equals(enemySpawn.enemyType));
                if (enemy.enemyPrefab != null) {
                    Instantiate(enemy.enemyPrefab, gameObject.transform.position, Quaternion.identity);
                    enemyCount--;
                }
            }

            yield return new WaitForSeconds(secondPerSpawn);
        }
    }

}
