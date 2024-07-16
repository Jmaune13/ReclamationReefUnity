using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ally" && transform.parent.gameObject.GetComponent<EnemyTankTurret>().targetObj == null)
        {
            //Debug.Log("SETTING TANK TARGET");
            transform.parent.gameObject.GetComponent<EnemyTankTurret>().setTarget(other.gameObject);
        }
    }
}
