using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class BulletActivity : MonoBehaviour
{
    public GameObject target;
    public string turretName;

    [Header("Turret = (A,C)")]
    public int speed;

    [Header("Turret = (B, C)")]
    public GameObject bulletPoint;
    private ConstraintSource constraintSource;

    [Header("Turret = (B)")]
    public float laserExistTime;
    public Boolean lockOn;

    // Sound effect
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (turretName.Equals("TurretA"))
        {
            // Make the bullet rotate towards the target
            Vector3 dir = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (turretName.Equals("TurretB"))
        {
            // Make the laser follow and rotate around the turret
            FollowBulletPoint();
            StartCoroutine("LaserCountdown", laserExistTime);

            if (transform.GetComponent<AudioSource>())
            {
                audioSource = transform.GetComponent<AudioSource>();
                
                if(AudioManager.Instance.soundOn)
                    audioSource.PlayOneShot(audioSource.clip);
            }
        }
        else if (turretName.Equals("TurretC"))
        {
            lockOn = true;
            if (transform.GetComponent<AudioSource>())
                audioSource = transform.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TurretA bullet will destroy on impact
        if (turretName.Equals("TurretA"))
        {
            float singleSpeed = speed * Time.deltaTime;
            transform.Translate(transform.forward * singleSpeed * 2, Space.World);

            // Self destruct if the target has already been destroyed
            if (target == null)
                Destroy(gameObject);
        }
        if (turretName.Equals("TurretC") && lockOn)
        {
            if(target != null) { 
                Vector3 Vo = CalculateCatapult(target.transform.position, transform.position, 1);

                transform.GetComponent<Rigidbody>().velocity = Vo;
                lockOn = false;
            }
        }


    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.tag == "Enemy")
        {
            if(turretName.Equals("TurretA"))
                Destroy(gameObject);
        }
        if(collider.transform.tag == "Base")
        {
            if (turretName.Equals("TurretC"))
            {
                StartCoroutine("Explosion");
            }
        }

    }
    IEnumerator LaserCountdown(float countDown)
    {
        while (Time.timeScale >= 1)
        {
            yield return new WaitForSeconds(1);
            countDown--;
            if (countDown == 0) { 
                // Stop the soundEffect
                if(audioSource != null)
                    audioSource.Stop();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Explosion()
    {
        GameObject explosionRange = transform.Find("ExplosionRange").gameObject;
        explosionRange.SetActive(true);

        if (AudioManager.Instance.soundOn)
            audioSource.PlayOneShot(audioSource.clip);

        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.Find("Bullet").gameObject.SetActive(false);
        
        // Deactivate explosionRange after 0.1s
        yield return new WaitForSeconds(0.1f);
        explosionRange.SetActive(false);

        // Remove the gameObject of the bullet after 0.9s (Ensure the audio trigger completely)
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);

    }

    public void FollowBulletPoint()
    {
        constraintSource.sourceTransform = bulletPoint.transform;
        constraintSource.weight = 1;
        ParentConstraint parentConstraint = gameObject.AddComponent<ParentConstraint>();
        parentConstraint.constraintActive = true;
        parentConstraint.AddSource(constraintSource);
        parentConstraint.rotationAxis = Axis.Y;
        parentConstraint.translationAxis = (Axis.X | Axis.Z);
    }

    Vector3 CalculateCatapult(Vector3 target, Vector3 origen, float time)
    {
        Vector3 distance = target - origen;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
