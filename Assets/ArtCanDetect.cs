using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtCanDetect : MonoBehaviour
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
        if (other.gameObject.tag == "ally" && transform.parent.gameObject.GetComponent<ArtCan>().target == null)
        {
            Debug.Log("Artillery Truck has detected an enemy of " + other.name + ", preparing to target");
            transform.parent.gameObject.GetComponent<ArtCan>().setTarget(other.gameObject);
        }
    }
}
