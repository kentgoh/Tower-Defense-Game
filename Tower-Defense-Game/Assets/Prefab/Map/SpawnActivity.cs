using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivity : MonoBehaviour
{
    public GameObject[] enemies;
    public float timer;

    public int currentPhase = 0;

    public float phaseOneSpawnInterval = 1.0f;
    public float phaseOneSpawnEnd = 10;
    
    public float phaseTwoSpawnInterval = 2.0f;
    public float phaseTwoSpawnEnd = 20;
    
    public float phaseThreeSpawnInterval = 5.0f;
    public float phaseThreeSpawnEnd = 40;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < phaseOneSpawnEnd && currentPhase == 0){
            InvokeRepeating("PhaseOneSpawn", 0, phaseOneSpawnInterval);
            currentPhase = 1;
        }
        if(timer > phaseOneSpawnEnd && currentPhase == 1)
        {
            CancelInvoke("PhaseOneSpawn");
            InvokeRepeating("PhaseTwoSpawn", 0, phaseTwoSpawnInterval);
            currentPhase = 2;
        }
        if (timer > phaseTwoSpawnEnd && currentPhase == 2)
        {
            CancelInvoke("PhaseTwoSpawn");
            InvokeRepeating("PhaseThreeSpawn", 0, phaseThreeSpawnInterval);
            currentPhase = 3;
        }
        if (timer > phaseThreeSpawnEnd && currentPhase == 3)
        {
            CancelInvoke("PhaseThreeSpawn");
            currentPhase = 4;
        }
    }

    void PhaseOneSpawn()
    {
        Instantiate(enemies[0], gameObject.transform.position, Quaternion.identity);
    }
    void PhaseTwoSpawn()
    {
        Instantiate(enemies[1], gameObject.transform.position, Quaternion.identity);
    }

    void PhaseThreeSpawn()
    {
        Instantiate(enemies[2], gameObject.transform.position, Quaternion.identity);
    }
}
