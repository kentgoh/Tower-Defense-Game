using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalPredefinedModel;

public class EnemyActivity : MonoBehaviour
{
    // Enemy properties
    public EnemyType enemyType;
    private Animator animator;

    public float speed;
    public int maxHealthPoint;
    public int healthPoint;
    private GameObject healthBar;
    private Boolean isDamaged = false;

    private Renderer[] rends;
    private Color originalColor;
    private Color onHitColor;
    public GameObject explosionEffect;

    // WayPoint for enemy movement
    public int destinatedWayPointIndex;
    private Transform destinatedWayPoint;
    private GameObject wayPointList;

    // Damage deal
    public int damageDeal;

    // Collider
    public List<DPSBulletCollider> DPSBulletColliders;

    // Start is called before the first frame update
    void Awake()
    {
        if (transform.Find("Canvas/HealthBar"))
        {
            healthBar = transform.Find("Canvas/HealthBar").gameObject;
            GameObject healthBarFill = healthBar.transform.Find("Fill").gameObject;
        }
        if (gameObject.GetComponent<Animator>())
            animator = gameObject.GetComponent<Animator>();

        InitWayPoint();
        InitEnemyProperties();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealthPoint();
        AutoMove();
    }
    
    public void InitWayPoint()
    {
        // Get all available wayPoints
        wayPointList = GameObject.FindGameObjectWithTag("WayPointList");

        // The index of the destinatedWayPoint will always start with 1
        destinatedWayPointIndex = 1;
        destinatedWayPoint = wayPointList.transform.Find(destinatedWayPointIndex.ToString());
    }

    public void InitEnemyProperties()
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
        // Determine the distance between enemy and destinatedWayPoint
        Vector3 dir = destinatedWayPoint.position - transform.position;
        Vector3 posAfterTranslate = transform.position + (dir.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(posAfterTranslate, destinatedWayPoint.position) > (speed * Time.deltaTime))
        {
            // The enemy won't exceed the wayPoint position after translate, move normally
            transform.Translate(dir.normalized * speed * Time.deltaTime);
            transform.Find("Model").LookAt(destinatedWayPoint.position);
        }
        else {
            // Move the enemy to destinatedWayPoint position
            transform.position = destinatedWayPoint.position; 
            
            // Change to nextWayPoint
            WayPointValue wayPointValueScript = destinatedWayPoint.GetComponent<WayPointValue>();

            // It is possible there is more than one next wayPoint
            int randomIndexValue = UnityEngine.Random.Range(0, wayPointValueScript.nextWayPoint.Count);
            destinatedWayPointIndex = wayPointValueScript.nextWayPoint[randomIndexValue];
            destinatedWayPoint = wayPointList.transform.Find(destinatedWayPointIndex.ToString());

            transform.Find("Model").LookAt(destinatedWayPoint.position);
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        var targetTag = collider.transform.tag;

        // Collide with endPoint
        if (targetTag == "EndPoint")
            StartCoroutine("DestroyEnemyAfterHittingEndPoint");

        // Collide with bullet
        if (targetTag == "Bullet")
            DealDamageOnEnemyByBulletType(collider);
    }

    private void OnTriggerStay(Collider collider)
    {
        foreach (DPSBulletCollider c in DPSBulletColliders)
        {
            if (c.collider == collider)
            {
                c.collidedTime += Time.deltaTime;

                if(c.collidedTime >= 1)
                    DealDPSDamageOnEnemy(c);
                break;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Remove the bullet from collider list when it no more collide with current enemy
        foreach(DPSBulletCollider c in DPSBulletColliders)
        {
            if (c.collider == collider)
            {
                DPSBulletColliders.Remove(c);
                break;
            }
        }
    }

    IEnumerator OnHit()
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
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.Explosion);
        if (animator != null)
            animator.SetBool("IsDestroy", true);
        if (explosionEffect != null) 
            explosionEffect.SetActive(true);

        Destroy(healthBar);
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }

    public IEnumerator DestroyEnemyAfterHittingEndPoint()
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.Explosion);
        if (animator != null)
            animator.SetBool("IsDestroy", true);
        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        Destroy(healthBar);
        yield return new WaitForSeconds(1);
        GameActivity.Instance.DecreaseEndpointHealth(damageDeal);
        Destroy(gameObject);
    }

    public void DealDamageOnEnemyByBulletType(Collider collider)
    {
        // Get BulletActivity script to deal required damage on enemy
        if (collider.GetComponentInParent<BulletActivity>() != null) {
            BulletActivity bulletActivityScript = collider.GetComponentInParent<BulletActivity>();
            int damageDealByBullet = bulletActivityScript.bulletDamage;
            BulletDamageType type = bulletActivityScript.bulletDamageType;

            if (type != BulletDamageType.DPS)
            {
                // Single target bullet need to confirm the target of the bullet is current enemy
                if (type == BulletDamageType.SingleTarget)
                {
                    if (bulletActivityScript.target == gameObject)
                        DealDamageOnEnemy(damageDealByBullet);
                }
                else
                    DealDamageOnEnemy(damageDealByBullet);

            }
            else {
                DPSBulletCollider temp = new DPSBulletCollider(collider, 0, damageDealByBullet);
                DPSBulletColliders.Add(temp);
                DealDPSDamageOnEnemy(temp);
            }


        }
    }

    public void DealDamageOnEnemy(int damage)
    {
        // Display healthbar for damaged 
        if (!isDamaged)
            isDamaged = true;

        healthPoint -= damage;

        if (healthPoint <= 0)
            StartCoroutine("DestroyEnemy");
        else
            StartCoroutine(OnHit());
    }

    public void DealDPSDamageOnEnemy(DPSBulletCollider c)
    {
        DealDamageOnEnemy(c.damage);
        // Reset the collided time
        c.collidedTime = 0;
    }
}
