using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankTurret : MonoBehaviour
{
    public GameObject targetObj;

    //Unit Attack stats
    private float attackRange = 35f;
    private int baseDamage = 40;
    private float stopTargetingThreshold = 40f;


    [SerializeField] private ParticleSystem laserEffect;

    [SerializeField] private LineRenderer lineLaser;

    [SerializeField] GameObject SelectionRing;

    Boolean checkOthers;
    //Used for firing
    private float RoF = 4.0f;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        checkOthers = false;
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRange, Color.green);

        if (targetObj != null)
        {
            var rotation = Quaternion.LookRotation(targetObj.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);


            if (Vector3.Distance(targetObj.transform.position, transform.position) <= attackRange && Time.time > nextFire)
            {
                //Adds on time plus the ROF offset, probably very ineffecient and should change eventually
                nextFire = Time.time + RoF;
                attackTarget();
            }

            if (Vector3.Distance(targetObj.transform.position, transform.position) > stopTargetingThreshold)
            {
                //Debug.Log("Stopping Targeting");
                targetObj = null;
            }

        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, Time.deltaTime * 2f);
            if (checkOthers == false)
            {
                CheckForOthers();
            }
        }
    }

    public void setTarget(GameObject obj)
    {
        targetObj = obj;
    }

    void attackTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (targetObj.transform.position - transform.position), out hit, attackRange))
        {
        
            //Debug.Log("Starting attack on ally unit:"+targetObj.name);
            //Debug.Log("Hit on ally unit:" + targetObj.name+ " on "+hit.transform.name);

            if (hit.transform == targetObj.transform && targetObj != null && targetObj.GetComponent<HealthManager>())
            {
                lineLaser.SetPosition(0, transform.position);
                //Debug.Log("We have hit the target!");
                targetObj.GetComponent<HealthManager>().takeDamage(baseDamage);

                GameObject laserClone = Instantiate(laserEffect.gameObject, gameObject.transform.position + (Vector3.forward), Quaternion.identity);
                Destroy(laserClone, 0.5f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);
                lineLaser.SetPosition(1, hit.transform.position);
                StartCoroutine(ShootLaser());

            }
            else if (hit.transform == (targetObj.transform.parent || targetObj.transform.GetChild(0)) && targetObj != null && targetObj.transform.parent.GetComponent<HealthManager>())
            {
                lineLaser.SetPosition(0, transform.position);
                //Debug.Log("We have hit the new target");
                targetObj.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);

                GameObject laserClone = Instantiate(laserEffect.gameObject, gameObject.transform.position + Vector3.forward, Quaternion.identity);
                Destroy(laserClone, 0.5f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);
                lineLaser.SetPosition(1, hit.transform.position);
                StartCoroutine(ShootLaser());
            } else
            {
                Debug.Log("Failed to hit target: "+targetObj+" at "+targetObj.transform.position+" on coord "+hit.transform.position+" hit instead "+hit.transform.name+" with parent "+hit.transform);
            }
            
        }
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

    IEnumerator ShootLaser()
    {
        lineLaser.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineLaser.enabled = false;
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
