using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ScoutMechMovement : MonoBehaviour
{
    public Rigidbody myBody;
    public NavMeshAgent agent;

    //Used to calculating when to stop an attack move
    public float moveRange;
    public GameObject targetObj;

    //Unit health and bar updates
    private float MaxHealth = 100f;
    private float currHealth = 0;
    [SerializeField] private Image healthbar;

    // Start is called before the first frame update
    void Start()
    {
        moveRange = 0;
        targetObj = null;
        currHealth = MaxHealth;
        //updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObj != null)
        {
            if (Vector3.Distance(transform.position, targetObj.transform.position) <= moveRange)
            {
                Debug.Log("STOPPING MOVEMENT");
                agent.isStopped = true;
                targetObj = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            takeDamage(15f);
        }
    }

    public void unitMove(Ray pos)
    {
        if (Physics.Raycast(pos, out var hitInfo))
        {
            agent.SetDestination(hitInfo.point);
            if (agent.isStopped == true)
            {
                agent.isStopped = false;
            }
        }
    }

    public void attackMove(GameObject obj, float range)
    {
        Vector3 target = obj.transform.position;

        agent.SetDestination(target);

        //Look at stoppingDistance, maybe its far easier?
        //agent.stoppingDistance()

        //Sets local target and range
        targetObj = obj.gameObject;
        moveRange = range;
        if (agent.isStopped == true)
        {
            agent.isStopped = false;
        }

    }

    public void takeDamage(float damage)
    {
        if (currHealth - damage <= 0f)
        {
            onKill();
        }
        else
        {
            currHealth -= damage;
            updateHealthBar();

        }
    }

    public void onKill()
    {
        Destroy(gameObject);
    }

    public void updateHealthBar()
    {
        healthbar.fillAmount = currHealth / MaxHealth;
    }
}

