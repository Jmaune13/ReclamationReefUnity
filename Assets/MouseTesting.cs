using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class MouseTesting : MonoBehaviour
{
    // Used for firing mechanisms
    public Transform target;
    public ParticleSystem laserEffect;
    [SerializeField] private LineRenderer lineLaser; 
    [SerializeField] private GameObject proj;
    private string mName;

    //Sound Effects
    [SerializeField] private AudioSource audioWeapon;


    //Only used in heavy mech
    [SerializeField] private GameObject firingPort1;
    [SerializeField] private GameObject firingPort2;

    //Unit stats
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float stopTargetingThreshold = 20f;

    //Used for firing
    [SerializeField] private float RoF = 2.0f;
    private float nextFire = 0f;


    //Detection recheck 
    bool checkOthers;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRange, Color.green);
        //If a target is selected enter this phase where top will angle towards the target, else angle back to the bottom position.
        if (target != null) {

            var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.blue);
            } else
            {
                Debug.DrawRay(transform.position, transform.forward, Color.red);
            }

            if(Vector3.Distance(target.position, transform.position) <= attackRange && Time.time > nextFire)
            {
                //Adds on time plus the ROF offset, probably very ineffecient and should change eventually
                nextFire = Time.time + RoF;
                attackTarget();
            }

            if(Vector3.Distance(target.position, transform.position) > stopTargetingThreshold)
            {
                target = null;
            }

        } else
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
        target = obj.transform;
        Debug.Log("FRIENDLY UNIT SETTING TARGET: " + target.transform.name);
    }

    void attackTarget()
    {
        //Picks target based on name of prefab

        if (gameObject.transform.parent.GetComponent<Movement>())
        {
            //Used for scout and missile mech
            mName = gameObject.transform.parent.GetComponent<Movement>().UnitName;
        } else
        {
            //Used for grabbing heavy mech
            mName = gameObject.transform.parent.GetComponent<MovementPro>().UnitName;
        }

        if(mName == "MissileMech")
        {
            ProjAttack();
        } else if (mName == "ScoutMech")
        {
            RayCastAttack();
        } else if (mName == "HeavyMech")
        {
            if (MovementPro.HeavyStates.LockDown == gameObject.transform.parent.GetComponent<MovementPro>().getState())
            {
                HeavyAttack();
            } else
            {
                Debug.Log("Not in Lockdown");
            }
            //HeavyAttack();
        } else
        {
            Debug.Log("Attack Name not found, cannot initiate attack: "+mName);
        }
    }

    void RayCastAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, attackRange))
        {
            //lineLaser.SetPosition(0, transform.position);
            Debug.Log("Starting attack on enemy" + target.transform.name + " with a hit on " + hit.transform.name);
            if (hit.transform == target.transform && target != null && (target.GetComponent<HealthManager>() || target.GetComponent<BuildingHealth>()))
            {
                lineLaser.SetPosition(0, transform.position);
                if (target.transform.parent == true)
                {
                    if (target.transform.parent.tag == "enemy")
                    {
                        target.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);
                    } else
                    {
                        target.transform.parent.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                    }

                }
                else
                {
                    if (target.tag == "enemy")
                    {
                        target.GetComponent<HealthManager>().takeDamage(baseDamage);
                    } else
                    {
                        target.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                    }

                }

                if (audioWeapon != null)
                {
                    audioWeapon.Play();
                }

                GameObject laserClone = Instantiate(laserEffect.gameObject, gameObject.transform.position, Quaternion.identity);
                Destroy(laserClone, 0.5f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);
                lineLaser.SetPosition(1, hit.transform.position);
                StartCoroutine(ShootLaser());

            }
            else if (hit.transform == (target.transform.parent || target.transform.GetChild(0)) && target != null && (target.transform.parent.GetComponent<HealthManager>() || target.transform.parent.GetComponent<BuildingHealth>()))
            {
                lineLaser.SetPosition(0, transform.position);
                //Debug.Log("We have hit the new target");
                if (target.transform.parent.tag == "enemy")
                {
                    target.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);
                } else
                {
                    target.transform.parent.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                }

                if (audioWeapon != null)
                {
                    audioWeapon.Play();
                }

                GameObject laserClone = Instantiate(laserEffect.gameObject, gameObject.transform.position, Quaternion.identity);
                Destroy(laserClone, 0.5f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point + (Vector3.forward * 2) + Vector3.up, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);
                lineLaser.SetPosition(1, hit.transform.position);
                StartCoroutine(ShootLaser());
            }
            else
            {
                Debug.Log("Failed to hit target: " + target + " at " + target.transform.position + " on coord " + hit.transform.position + " on hit " + hit.transform.name);
            }
        }
    }

    void ProjAttack()
    {
        if(Vector3.Distance(transform.position, target.transform.position) <= attackRange && Vector3.Distance(transform.position, target.transform.position) > 10f)
        {
            GameObject newProjectile = Instantiate(proj, gameObject.transform.position + Vector3.up, Quaternion.identity);
            newProjectile.GetComponent<Trajectory>().setEndPoint(target.transform.position);
        } else
        {
            Debug.Log("Target out of range or another error has occured");
        }
    }

    void HeavyAttack()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, attackRange))
        {
            Debug.Log("Starting attack on enemy" + target.transform.name + " with a hit on " + hit.transform.name);
            if(hit.transform == target.transform && target != null && (target.GetComponent<HealthManager>() || target.GetComponent<BuildingHealth>()))
            {
                if (target.transform.parent == true)
                {
                    if (target.transform.parent.tag == "enemy")
                    {
                        target.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);
                    }
                    else
                    {
                        target.transform.parent.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                    }
                }
                else
                {
                    if (target.tag == "enemy")
                    {
                        target.GetComponent<HealthManager>().takeDamage(baseDamage);
                    }
                    else
                    {
                        target.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                    }
                }
                firingPort1.GetComponent<LineRendererSpecifics>().startFire(hit.transform);
                firingPort2.GetComponent<LineRendererSpecifics>().startFire(hit.transform);

                if (audioWeapon != null)
                {
                    audioWeapon.Play();
                }

                GameObject laserClone = Instantiate(laserEffect.gameObject, firingPort1.transform.position, Quaternion.identity);
                Destroy(laserClone, 1f);
                GameObject laserClone2 = Instantiate(laserEffect.gameObject, firingPort2.transform.position, Quaternion.identity);
                Destroy(laserClone2, 1f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 1f);

            }
            else if (hit.transform == (target.transform.parent || target.transform.GetChild(0)) && target != null && (target.transform.parent.GetComponent<HealthManager>() || target.transform.parent.GetComponent<BuildingHealth>()))
            {
                //Debug.Log("We have hit the new target");
                if (target.transform.parent.tag == "enemy")
                {
                    target.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);
                }
                else
                {
                    target.transform.parent.GetComponent<BuildingHealth>().takeDamage(baseDamage);
                }
                firingPort1.GetComponent<LineRendererSpecifics>().startFire(hit.transform);
                firingPort2.GetComponent<LineRendererSpecifics>().startFire(hit.transform);

                if(audioWeapon != null)
                {
                    audioWeapon.Play();
                }


                GameObject laserClone = Instantiate(laserEffect.gameObject, firingPort1.transform.position, Quaternion.identity);
                Destroy(laserClone, 1f);
                GameObject laserClone2 = Instantiate(laserEffect.gameObject, firingPort2.transform.position, Quaternion.identity);
                Destroy(laserClone2, 1f);

                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point + (Vector3.forward * 2) + Vector3.up, Quaternion.identity);
                Destroy(laserCloneEnd, 1f);

            }
            else
            {
                Debug.Log("Failed to hit target: " + target + " at " + target.transform.position + " on coord " + hit.transform.position + " on hit " + hit.transform.name);
            }
        }
    }

    IEnumerator ShootLaser()
    {
        lineLaser.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineLaser.enabled = false;
    }

    void CheckForOthers()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, 10f);
        if (others[0] != null)
        {
            foreach (Collider col in others)
            {
                if (col.gameObject.tag == "enemy")
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
