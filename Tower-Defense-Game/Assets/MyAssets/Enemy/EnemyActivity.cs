using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameInit;

public class EnemyActivity : MonoBehaviour
{
    private GameObject gameSystem;

    // Enemy properties
    public EnemyType enemyType;
    private Animator animator;

    public int maxHealthPoint;
    public int healthPoint;
    private GameObject healthBar;
    private Boolean isDamaged = false;

    private Renderer[] rends;
    private Color originalColor;
    private Color onHitColor;

    // Enemy movement 
    public float speed;
    public int destinatedWayPointIndex;
    private Transform destinatedWayPoint;
    private GameObject wayPointList;

    // Damage deal
    public int damageDeal;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");

        // Initialize waypoint
        destinatedWayPointIndex = 1;
        wayPointList = GameObject.FindGameObjectWithTag("WayPointList");

        if (transform.Find("Canvas/HealthBar"))
        {
            healthBar = transform.Find("Canvas/HealthBar").gameObject;
            GameObject healthBarFill = healthBar.transform.Find("Fill").gameObject;
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
                    maxHealthPoint = 10;
                    healthPoint = 10;
                    speed = 0.2f;
                    damageDeal = 2;
                    break;
                }
            case EnemyType.Drone:
                {
                    maxHealthPoint = 5;
                    healthPoint = 5;
                    speed = 0.5f;
                    damageDeal = 1;
                    break;
                }
            case EnemyType.Boulder:
                {
                    maxHealthPoint = 30;
                    healthPoint = 30;
                    speed = 0.1f;
                    damageDeal = 5;
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
        transform.Find("Model").LookAt(destinatedWayPoint.position);

    }

    private void OnTriggerEnter(Collider collider)
    {
        var targetTag = collider.transform.tag;

        // Collide with endPoint, removed
        if (targetTag == "EndPoint")
        {
            StartCoroutine("DestroyEnemy");
            ReduceEndPointHealth();
        }

        if (targetTag == "Bullet")
        {
            if (!isDamaged)
                isDamaged = true;

            healthPoint--;
            // If no health point left
            //      Destroy object
            // Else
            //      Trigger on hit effect
            if (healthPoint == 0)
                StartCoroutine("DestroyEnemy");
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
        if (healthBar != null && isDamaged)
        {
            healthBar.SetActive(true);

            Image fill = healthBar.transform.Find("Fill").GetComponent<Image>();
            float fillRange = Mathf.InverseLerp(maxHealthPoint, 0, healthPoint);
            fill.fillAmount = Mathf.Lerp(1, 0, fillRange);
        }
    }

    public IEnumerator DestroyEnemy()
    {
        if (animator != null)
            animator.SetBool("IsDestroy", true);

        Destroy(healthBar);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void ReduceEndPointHealth()
    {
        gameSystem.GetComponent<GameActivity>().endPointHealth -= damageDeal;
    }
}
