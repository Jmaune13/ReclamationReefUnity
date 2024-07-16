using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTraj : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AnimationCurve curve;
    private float missileSpeed = 10f;
    private float maxHeight = 10f;

    public ParticleSystem explosion;
    [SerializeField] private SphereCollider radius;

    Vector3 endPoint;
    Boolean endSet = false;

    IEnumerator follow = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endSet == true)
        {
            follow = beginCurve();
            StartCoroutine(follow);
            endSet = false;
        }
    }

    IEnumerator beginCurve()
    {
        Vector3 pathPosition = endPoint - transform.position;
        float totalDistance = pathPosition.magnitude;
        pathPosition.Normalize();

        float distanceTraveled = 0f;
        float missileRadius = transform.localScale.y / 2.0f;

        //totalDistance = ((totalDistance * 0.05f) * 25f) / 25f;

        Vector3 newPos = transform.position;

        while (distanceTraveled <= totalDistance)
        {
            Vector3 deltaPath = pathPosition * (missileSpeed * totalDistance * Time.deltaTime);
            newPos += deltaPath;
            distanceTraveled += deltaPath.magnitude;

            newPos.y = missileRadius + (maxHeight * curve.Evaluate(distanceTraveled / totalDistance));

            transform.position = newPos;

            yield return null;
        }

        follow = null;
        BeginExplosion();
        //Debug.Log("Beginning Explosion");
    }

    public void setEndPoint(Vector3 point)
    {
        endPoint = point;
        endSet = true;
    }

    private void BeginExplosion()
    {
        //Debug.Log("Beginning Explosion");
        GameObject exp = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
        Destroy(exp, 0.5f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var Collider in colliders)
        {
            GameObject temp = Collider.gameObject;
            if (temp.tag == "ally")
            {
                if (temp.TryGetComponent<HealthManager>(out HealthManager health))
                {
                    Debug.Log("Applying Damage To Unit");
                    health.takeDamage(20f);
                }
            }
        }
        //Debug.Log("Destroying game object");
        Destroy(gameObject);
    }

    public void EarlyExplosion()
    {
        Debug.Log("Beginning Explosion from PDS");
        GameObject exp = Instantiate(explosion.gameObject, transform.position, Quaternion.identity);
        Destroy(exp, 0.5f);
        Debug.Log("Destroying game object from PDS");
        Destroy(gameObject);
    }
}
