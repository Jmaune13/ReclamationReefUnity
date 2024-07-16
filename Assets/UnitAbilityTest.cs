using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityTest : MonoBehaviour
{
    //[SerializeField] private float AbilityTime;
    public GameObject unitBase;
    [SerializeField] SphereCollider field;

    //Store ability timer image
    [SerializeField] private UnityEngine.UI.Image image;

    private float RoF = 4.0f;
    private float nextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(unitBase.GetComponent<Movement>().isSelected == true && Time.time > nextFire && field.enabled == false) {
                //Debug.Log("Ability Activated: PDS");
                field.enabled = true;
                image.fillAmount = 1;
                StartCoroutine(ActiveAbility());
            }
        }

        if (Time.time < nextFire)
        {
            //Debug.Log("Filling image: " + (nextFire - Time.time));
            image.fillAmount = (nextFire - Time.time) / RoF;
        }
    }

    IEnumerator ActiveAbility()
    {
        yield return new WaitForSeconds(10f);
        nextFire = Time.time + RoF;
        field.enabled = false;
        //Debug.Log("Ability PDS out of time, cooldown begin");
    }
}
