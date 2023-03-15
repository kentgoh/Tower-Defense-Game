using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameInit;

public class EnemyActivity : MonoBehaviour
{
    public GameObject cameraLocation;

    // Enemy properties
    public EnemyType enemyType;
    private Animator animator;

    public int healthPoint;
    private TMP_Text healthPointUI;

    private Renderer[] rends;
    private Color originalColor;
    private Color onHitColor;

    // Enemy movement 
    public float speed;
    public int destinatedWayPointIndex;
    private Transform destinatedWayPoint;
    private GameObject wayPointList;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize waypoint
        destinatedWayPointIndex = 1;
        wayPointList = GameObject.FindGameObjectWithTag("WayPointList");
        cameraLocation = GameObject.FindGameObjectWithTag("MainCamera");

        if (gameObject.GetComponentInChildren<TMP_Text>()){
            healthPointUI = gameObject.GetComponentInChildren<TMP_Text>();
        }
        if (gameObject.GetComponent<Animator>())
            animator = gameObject.GetComponent<Animator>();

        // Initialize enemy properties by type
        initEnemyProperties();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealthPoint();
        AutoMove();
    }

    
    public void initEnemyProperties()
    {
        rends = gameObject.GetComponentsInChildren<Renderer>();
        originalColor = rends[0].material.color;
        onHitColor = Color.yellow;

        // Define enemy health point and speed by type
        switch (enemyType)
        {
            case EnemyType.Raider:
                { 
                    healthPoint = 5;
                    speed = 0.5f;
                    break;
                }
            case EnemyType.Drone:
                {
                    healthPoint = 10;
                    speed = 0.8f;
                    break;
                }
            case EnemyType.Boulder:
                {
                    healthPoint = 50;
                    speed = 0.3f;
                    break;
                }
        }

    }

    public void AutoMove()
    {
        // Get destinated wayPoint
        WayPointValue[] wayPointValueScriptList = wayPointList.GetComponentsInChildren<WayPointValue>();
        foreach(WayPointValue wayPointValueScript in wayPointValueScriptList)
        {
            if(wayPointValueScript.currentWayPoint == destinatedWayPointIndex)
            {
                destinatedWayPoint = wayPointValueScript.GetComponentInParent<Transform>();
                break;
            }
        }

        Vector3 dir = destinatedWayPoint.position - transform.position;
        if (Vector3.Distance(transform.position, destinatedWayPoint.position) <= 0.01f)
        {
            destinatedWayPointIndex = destinatedWayPoint.gameObject.GetComponent<WayPointValue>().destinatedWayPoint;
        }

        // Move object to wayPoint
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(destinatedWayPoint.position);

    }

    private void OnTriggerEnter(Collider collider)
    {
        var targetTag = collider.transform.tag;

        // Collide with endPoint, removed
        if (targetTag == "EndPoint")
        {
            StartCoroutine("DestroyEnemy");
        }

        if (targetTag == "Bullet")
        {  
            healthPoint--;
            // If no health point left
            //      Destroy object
            // Else
            //      Trigger on hit effect
            if (healthPoint == 0)
            {

                StartCoroutine("DestroyEnemy");
            }
            else
                StartCoroutine(onHit());
        }
    }

    IEnumerator onHit()
    {
        foreach(Renderer rend in rends) {
            rend.material.color = onHitColor;
        }
        yield return new WaitForSeconds(0.2f);

        foreach (Renderer rend in rends)
        {
            rend.material.color = originalColor;
        }
    }

    public void DisplayHealthPoint()
    {
        if(healthPointUI != null) { 
            healthPointUI.text = healthPoint.ToString();
            healthPointUI.transform.rotation = Quaternion.LookRotation(healthPointUI.transform.position - cameraLocation.transform.position);
        }
    }

    public IEnumerator DestroyEnemy()
    {
        if (animator != null)
            animator.SetBool("IsDestroy", true);

        Destroy(healthPointUI);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
