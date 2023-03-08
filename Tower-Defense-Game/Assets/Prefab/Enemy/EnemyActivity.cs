using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivity : MonoBehaviour
{
    // Enemy properties
    public int type;

    private int healthPoint;
    private Renderer rend;
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

        // Initialize enemy properties by type
        initEnemyProperties();
    }

    // Update is called once per frame
    void Update()
    {
        autoMove();
    }

    
    void initEnemyProperties()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        onHitColor = Color.yellow;

        // Define enemy health point and speed by type
        switch (type)
        {
            case 1:
                { 
                    healthPoint = 5;
                    speed = 0.5f;
                    break;
                }
            case 2:
                {
                    healthPoint = 10;
                    speed = 0.8f;
                    break;
                }
            case 3:
                {
                    healthPoint = 50;
                    speed = 0.3f;
                    break;
                }
        }

    }

    void autoMove()
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
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        var targetTag = collider.transform.tag;

        // Collide with endPoint, removed
        if (targetTag == "EndPoint")
        {
            Destroy(gameObject);
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
                Destroy(gameObject);
            }
            else
                StartCoroutine(onHit());
        }
    }

    IEnumerator onHit()
    {
        rend.material.color = onHitColor;
        yield return new WaitForSeconds(0.2f);
        rend.material.color = originalColor;
    }
}
