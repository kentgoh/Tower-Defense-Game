using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static TurretAI;

public class TurretActivity : MonoBehaviour
{
    public string turretName;
    public Transform rotatedTurretPart;
    public float turretYRotationDegreePerSecond;
    public Animator animator;

    public Transform bulletPoint;
    public GameObject bullet;
    public float bulletCooldown;

    public GameObject currentTarget;
    public float distAway;
    public float bulletCurrentCooldown;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckForEnemy",0, 0.5f);

        bulletCurrentCooldown = 0;
        InvokeRepeating("BulletCooldownCounter", 0, 1.0f);
        if (transform.GetChild(0).GetComponent<Animator>())
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            FollowEnemy();

            if(bulletCurrentCooldown == 0)
            {
                ShootEnemy(currentTarget);
            }
        }
    }

    public void CheckForEnemy()
    {
        // Find all object with collider within the radius
        Collider[] colls = Physics.OverlapSphere(transform.position, 1.0f);
        distAway = Mathf.Infinity;

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Enemy")
            {
                // Find Enemy closest to the turret
                float dist = Vector3.Distance(transform.position, colls[i].transform.position);
                if (dist < distAway)
                {
                        currentTarget = colls[i].gameObject;
                        distAway = dist;
                 }
              }
         }

        // Deselect target when it is out of range
        if (distAway == Mathf.Infinity)
        {
            currentTarget = null;
        }
    }

    public void FollowEnemy()
    {
        Vector3 targetDir = currentTarget.transform.position - rotatedTurretPart.position;
        targetDir.y = 0;
        rotatedTurretPart.transform.rotation = Quaternion.RotateTowards(rotatedTurretPart.rotation, Quaternion.LookRotation(targetDir), turretYRotationDegreePerSecond * Time.deltaTime);

    }

    public void ShootEnemy(GameObject target)
    {
        GameObject createdBullet = Instantiate(bullet, bulletPoint.transform.position, bulletPoint.rotation);
        BulletActivity bulletActivity = createdBullet.GetComponent<BulletActivity>();
        bulletActivity.turretName = turretName;
        bulletActivity.target = target;
        bulletActivity.bulletPoint = bulletPoint.gameObject;
        
        if(animator != null)
            animator.SetTrigger("Fire");

        bulletCurrentCooldown = bulletCooldown;
    }

    public void BulletCooldownCounter()
    {
        if (bulletCurrentCooldown != 0)
        {
            bulletCurrentCooldown--;
        }
    }
}
