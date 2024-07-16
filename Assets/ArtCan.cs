using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArtCan : MonoBehaviour
{

    public GameObject target;

    //Missile Projectile
    [SerializeField] private GameObject proj;

    [SerializeField] GameObject SelectionRing;

    //Ranges for determining attack range and chase range
    private float MaxRange = 25f;
    private float AttackRange = 20f;

    //Rate of fire
    Boolean checkOthers;
    //Used for firing
    private float RoF = 2.0f;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //Check others is a method to check for additional targets inside the units range if a target has been destroyed
        checkOthers = false;
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            //rotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

            if (Vector3.Distance(target.transform.position, transform.position) <= AttackRange && Time.time > nextFire)
            {
                //Adds on time plus the ROF offset, probably very ineffecient and should change eventually
                nextFire = Time.time + RoF;
                attackTarget();
            }

            if (Vector3.Distance(target.transform.position, transform.position) > MaxRange)
            {
                //Debug.Log("Stopping Targeting");
                target = null;
            }

        } 
            else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, (transform.parent.rotation * Quaternion.Euler(0, 90, 0)), Time.deltaTime * 2f);
            if(checkOthers == false)
            {
                CheckForOthers();
            }
        }
    }

    public void setTarget(GameObject targ)
    {
        if(target == null) {
            target = targ;
        }
        checkOthers = false;
    }

    void attackTarget()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            //For future: Call upon a cached storage of them
            GameObject newProjectile = Instantiate(proj, gameObject.transform.position + Vector3.up, Quaternion.identity);
            newProjectile.GetComponent<enemyTraj>().setEndPoint(target.transform.position);
        }
        else
        {
            Debug.Log("Target out of range or another error has occured");
        }
    }

    void CheckForOthers()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, 10f);
        if (others[0] != null)
        {
            foreach(Collider col in others)
            {
                if(col.gameObject.tag == "ally")
                {
                    setTarget(col.gameObject);
                    break;
                }
            }
        } else
        {
            checkOthers = true;
        }
    }

    private void OnMouseEnter()
    {
        SelectionRing.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseExit()
    {
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }
}
