using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DetectionCheck : MonoBehaviour
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
        if(other.gameObject.tag == "enemy" && transform.parent.gameObject.GetComponent<MouseTesting>().target == null)
        {
            transform.parent.gameObject.GetComponent<MouseTesting>().setTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == transform.parent.gameObject.GetComponent<MouseTesting>().target)
        {
            UnityEngine.Debug.Log("Exiting targeting radius");
            transform.parent.gameObject.GetComponent<MouseTesting>().setTarget(null);
        }
    }
}
