using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ScoutMechHead : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    //public Transform parent;
    //public LayerMask clickable;

    //Unit stats
    private float attackRange = 15f;
    private int baseDamage = 10;
    private float stopTargetingThreshold = 20f;

    //Used for firing
    private float RoF = 2.0f;
    private float nextFire = 0f;

    //Detection recheck 
    bool checkOthers;
    void Start()
    {

    }

    void Update()
    {
        //If a target is selected enter this phase where top will angle towards the target, else angle back to the bottom position.
        if (target != null)
        {

            var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.blue);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward, Color.red);
            }

            if (Vector3.Distance(target.position, transform.position) <= attackRange && Time.time > nextFire)
            {
                //Adds on time plus the ROF offset, probably very ineffecient and should change eventually
                nextFire = Time.time + RoF;
                attackTarget();
            }

            if (Vector3.Distance(target.position, transform.position) > stopTargetingThreshold)
            {
                Debug.Log("Stopping Targeting");
                target = null;
            }

        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, Time.deltaTime * 2f);

        }

    }


    /*
    float AngleBetweenPoint(Vector2 A, Vector2 B)
    {
        return Mathf.Atan2(A.y - B.y, A.x - B.x) * Mathf.Rad2Deg;
    }
    */

    public void setTarget(GameObject obj)
    {
        target = obj.transform;
    }

    //Maybe not needed for now?
    /*
    //After setting target, move towards if out of range determined by Clicker object
    public void setTargetMove(GameObject obj)
    {
        transform.parent.GetComponent<Movement>().attackMove(obj, attackRange);
    }
    */

    void attackTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, attackRange))
        {
            if (hit.transform == target && target != null && target.GetComponent<enemyTestScript>())
            {
                Debug.Log("We have hit the target!");
                target.GetComponent<enemyTestScript>().health -= baseDamage;
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
}
