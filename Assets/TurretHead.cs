using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretHead : MonoBehaviour
{

    public GameObject targetObj;

    [SerializeField] GameObject SelectionRing;


    //Unit Attack stats
    private float attackRange = 30f;
    private int baseDamage = 10;
    private float stopTargetingThreshold = 35f;

    //Used for firing
    private float RoF = 1.0f;
    private float nextFire = 0f;

    [SerializeField] LineRenderer lineLaser;
    [SerializeField] private ParticleSystem laserEffect;

    [SerializeField] AudioSource soundFX;

    // Start is called before the first frame update
    void Start()
    {
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging laser
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * attackRange, Color.green);

        if(targetObj != null)
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

        } else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.rotation, Time.deltaTime * 2f);
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
            //Debug.Log("Starting attack on enemy"+targetObj.name+" with hit on "+hit.transform.name);

            if (hit.transform == targetObj.transform && targetObj != null && targetObj.GetComponent<HealthManager>())
            {
                if(hit.transform.parent != null)
                {
                    if (hit.transform.tag.Equals("ally"))
                    {
                        soundFX.Play();
                        lineLaser.SetPosition(0, transform.position);
                        //Debug.Log("We have hit the target!");
                        targetObj.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);

                        lineLaser.SetPosition(1, hit.transform.position);
                        //Used for bullet end particle, need to find the correct positioning
                        GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                        Destroy(laserCloneEnd, 0.5f);

                        StartCoroutine(ShootLaser());
                    }
                } else
                {
                    if (hit.transform.tag.Equals("ally"))
                    {
                        soundFX.Play();
                        lineLaser.SetPosition(0, transform.position);
                        //Debug.Log("We have hit the target!");
                        targetObj.GetComponent<HealthManager>().takeDamage(baseDamage);

                        lineLaser.SetPosition(1, hit.transform.position);
                        //Used for bullet end particle, need to find the correct positioning
                        GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                        Destroy(laserCloneEnd, 0.5f);

                        StartCoroutine(ShootLaser());
                    }
                }

            }
            else if (hit.transform == (targetObj.transform.parent || targetObj.transform.GetChild(0)) && targetObj != null && targetObj.transform.parent.GetComponent<HealthManager>())
            {
                soundFX.Play();
                lineLaser.SetPosition(0, transform.position);
                //Debug.Log("We have hit the new target");
                targetObj.transform.parent.GetComponent<HealthManager>().takeDamage(baseDamage);

                lineLaser.SetPosition(1, hit.transform.position);
                //Used for bullet end particle, need to find the correct positioning
                GameObject laserCloneEnd = Instantiate(laserEffect.gameObject, hit.point, Quaternion.identity);
                Destroy(laserCloneEnd, 0.5f);

                StartCoroutine(ShootLaser());
            } else
            {
                Debug.Log("Failed to hit target: "+targetObj+" at "+targetObj.transform.position+" on coord "+hit.transform.position+" hit instead "+hit.transform.name+" with parent "+hit.transform);
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
        lineLaser.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineLaser.enabled = false;
    }
}
