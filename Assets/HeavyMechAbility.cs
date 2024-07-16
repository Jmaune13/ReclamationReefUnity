using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MovementPro;

public class HeavyMechAbility : MonoBehaviour
{
    public GameObject unitBase;
    [SerializeField] private Image image;

    private float AbilityRate = 3.0f;
    private float nextFire = 0f;


    void Start()
    {
        image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Debug.Log("Starting Lockdown at state: " + unitBase.GetComponent<MovementPro>().getState());
            //Debug.Log("With Times: " + Time.time+" : "+nextFire);
            //Debug.Log("And Selected: " + unitBase.GetComponent<MovementPro>().isSelected);
            if (unitBase.GetComponent<MovementPro>().isSelected == true && Time.time > nextFire && (unitBase.GetComponent<MovementPro>().getState() == HeavyStates.Active || unitBase.GetComponent<MovementPro>().getState() == HeavyStates.LockDown))
            {
                //Debug.Log("Ability Activated: LockDown");
                gameObject.GetComponent<MovementPro>().stopUnit();
                StartCoroutine(ChangeForm());
            }
          
        }

        if(Time.time < nextFire)
            {
            //Debug.Log("Filling image: " + (nextFire - Time.time));
            image.fillAmount = (nextFire - Time.time) / AbilityRate;
        }
    }

    IEnumerator ChangeForm()
    {
        HeavyStates lastState = unitBase.GetComponent<MovementPro>().getState();
        unitBase.GetComponent<MovementPro>().setState(HeavyStates.Transition);
        nextFire = Time.time + AbilityRate;
        yield return new WaitForSeconds(3f);
        if (lastState == HeavyStates.Active)
        {
            unitBase.GetComponent<MovementPro>().setState(HeavyStates.LockDown);
        } else
        {
            unitBase.GetComponent<MovementPro>().setState(HeavyStates.Active);
        }
    }

    
}
