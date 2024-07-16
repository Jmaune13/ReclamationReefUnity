using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetect : MonoBehaviour
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
        if (other.gameObject.tag == "ally" && transform.parent.gameObject.GetComponent<TurretHead>().targetObj == null)
        {
            transform.parent.gameObject.GetComponent<TurretHead>().setTarget(other.gameObject);
        }
    }
}
