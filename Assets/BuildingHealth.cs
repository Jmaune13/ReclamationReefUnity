using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    private float MaxHealth = 100f;
    public float currHealth = 0;

    //Unit Particle Systems
    public ParticleSystem explosion;


    void Start()
    {
        currHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (currHealth - damage <= 0f)
        {
            Debug.Log("Building Destroyed");
            onKill();
        }
        else
        {
            Debug.Log("Building subtract Health");
            currHealth -= damage;

        }
    }

    private void onKill()
    {
        //Destroy(Instantiate(explosion.gameObject), explosion.main.startLifetimeMultiplier);
        GameObject deathClone = Instantiate(explosion.gameObject, gameObject.transform.position + Vector3.up, Quaternion.identity);
        Destroy(deathClone, 2);
        Destroy(gameObject);
    }
}
