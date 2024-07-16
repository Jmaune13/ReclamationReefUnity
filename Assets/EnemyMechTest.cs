using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class EnemyMechTest : MonoBehaviour
{
    Vector3 guardPoint;
    public GameObject target;
    [SerializeField] Boolean ChaseMode;
    [SerializeField] private NavMeshAgent agent;

    //Ranges for determining attack range and chase range
    private float MaxRange = 50f;
    private float AttackRange = 20f;
    private float baseDamage = 5f;


    Boolean checkOthers;

    //Rate of fire
    //Used for firing
    private float RoF = 1.0f;
    private float nextFire = 0f;

    //Selection Ring
    [SerializeField] GameObject SelectionRing;
    public ParticleSystem laserEffect;
    [SerializeField] LineRenderer line;

    [SerializeField] AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        //currHealth = MaxHealth;
        guardPoint = gameObject.transform.position;
        ChaseMode = false;
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
        checkOthers = false;
    }

    // Update is called once per frame
    void Update()

        
    {
        if(target != null)
        {
            //If the target leaves attack range, start following it
            if (Vector3.Distance(target.transform.position, gameObject.transform.position) >= AttackRange && ChaseMode == false)
            {
                Debug.Log("Target out of range, chasing target now...");
                ChaseMode = true;
                //chaseTarget();
            }

            if(ChaseMode == false)
            {
                var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            }

            //If chasing a target, keep updating its position to follow it unless its too close to it
            if(ChaseMode == true)
            {
                agent.SetDestination(target.transform.position);
            }

            //If the unit gets to far from its guarded point it will start heading back
            if (Vector3.Distance(guardPoint, gameObject.transform.position) >= MaxRange && ChaseMode == true)
            {
                Debug.Log("Target beyond guard range, returning now...");
                ChaseMode = false;
                returnToPoint();
            }

            
            //Prevents the mech from trying to ram itself into other units
            if ((Vector3.Distance(target.transform.position, gameObject.transform.position) <= 5f))
            {
                agent.ResetPath();
            }

            if (Vector3.Distance(target.transform.position, transform.position) <= AttackRange && Time.time > nextFire)
            {
                //Adds on time plus the ROF offset, probably very ineffecient and should change eventually
                nextFire = Time.time + RoF;
                attackTarget();
            }

        } else
        {
            if (checkOthers == false)
            {
                CheckForOthers();
            }
        }

    }
    void chaseTarget()
    {
        agent.SetDestination(target.transform.position);
    }
    

    void returnToPoint()
    {
        target = null;
        agent.SetDestination(guardPoint);
    }

    
    public void setTarget(GameObject obj)
    {
        Debug.Log("Setting new target for enemy mech...");
        if (obj.transform.parent != null)
        {
            target = obj.transform.parent.gameObject;
        } else
        {
            target = obj;
        }
        Debug.Log("ENEMY UNIT SETTING TARGET: " + target.transform.name);
    }
    

    void attackTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, (target.transform.position - transform.position), out hit, AttackRange))
        {
            Debug.Log("Starting attack on friendly unit"+target.name);
            if (hit.transform == target.transform && target != null && target.GetComponent<HealthManager>())
            {
                Debug.Log("Attack hit on"+hit.transform.name+", taking health off the friendly unit");
                //Debug.Log("We have hit the target!");

                line.SetPosition(0, transform.position);
                target.transform.GetComponent<HealthManager>().takeDamage(baseDamage);

                GameObject laserClone = Instantiate(laserEffect.gameObject, gameObject.transform.position, Quaternion.identity);
                Destroy(laserClone, 0.5f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);
                line.SetPosition(1, hit.transform.position);
                StartCoroutine(ShootLaser());

                if (audioS != null)
                {
                    audioS.Play();
                }

            } else if (!(hit.transform == target.transform)) {
                Debug.Log("Did not hit the target, instead hit: "+hit.transform);
            } else
            {
                Debug.Log("Failed to hit target: " + target + " at " + target.transform.position + " on coord " + hit.transform.position);
            }
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

    IEnumerator ShootLaser()
    {
        line.enabled = true;
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;
    }

    void CheckForOthers()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, 10f);
        if (others[0] != null)
        {
            foreach (Collider col in others)
            {
                if (col.gameObject.tag == "ally")
                {
                    setTarget(col.gameObject);
                    break;
                }
            }
        }
        else
        {
            checkOthers = true;
        }
    }


}
