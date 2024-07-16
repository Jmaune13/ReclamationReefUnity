using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoutAbilityScript : MonoBehaviour
{
    public GameObject unitBase;
    [SerializeField] GameObject healerDrone;

    [SerializeField] private Image image;

    private float AbilityRate = 40.0f;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (unitBase.GetComponent<Movement>().isSelected == true && Time.time > nextFire)
            {
                //Debug.Log("Ability Activated: Healing Drone");
                CreateDrone();
            }
        }
        if(Time.time < nextFire)
        {
            image.fillAmount = (nextFire - Time.time) / AbilityRate;
        }
    }

    private void CreateDrone()
    {
        Instantiate(healerDrone, transform.position + (Vector3.up*3), Quaternion.identity);
        nextFire = Time.time + AbilityRate;
    }

    public void startButtonAbility()
    {
        Debug.Log("Pressed Button");
        if (unitBase.GetComponent<Movement>().isSelected == true && Time.time > nextFire)
        {
            Debug.Log("Ability Activated: Healing Drone");
            CreateDrone();
        }
    }
}
