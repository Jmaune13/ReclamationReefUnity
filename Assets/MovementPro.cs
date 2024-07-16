using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MovementPro : MonoBehaviour
{
    public Rigidbody myBody;
    public NavMeshAgent agent;

    //Boolean for if the unit is selected
    public Boolean isSelected;

    //Used to calculating when to stop an attack move
    public float moveRange;
    public GameObject targetObj;

    //Used for handling the sprite renderer for selection
    [SerializeField] private GameObject rend;

    public enum HeavyStates {
        LockDown,
        Active,
        Transition
    }

    HeavyStates state;

    [SerializeField] public String UnitName;

    // Start is called before the first frame update
    void Start()
    {
        moveRange = 0;
        targetObj = null;
        isSelected = false;
        state = HeavyStates.Active;
        rend.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObj != null)
        {
            if (Vector3.Distance(transform.position, targetObj.transform.position) <= moveRange)
            {
                //Debug.Log("STOPPING MOVEMENT");
                agent.isStopped = true;
                targetObj = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.GetComponent<HealthManager>().takeDamage(15f);
        }
    }

    public void unitMove(Ray pos)
    {
        if (Physics.Raycast(pos, out var hitInfo) && state == HeavyStates.Active)
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
        if (state == HeavyStates.Active) {
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

    }

    public void setState(HeavyStates newState)
    {
        state = newState;
    }

    public HeavyStates getState()
    {
        return state;
    }

    public void stopUnit()
    {
        agent.isStopped = true;
    }


    public void setSelection(Boolean value)
    {
        rend.GetComponent<SpriteRenderer>().enabled = value;
    }
}

