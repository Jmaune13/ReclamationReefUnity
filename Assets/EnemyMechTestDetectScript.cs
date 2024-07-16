using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechTestDetectScript : MonoBehaviour
{
    [SerializeField] private GameObject mainParent;
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
        if (other.gameObject.tag == "ally" && transform.parent.gameObject.GetComponent<EnemyMechTest>().target == null)
        {
            transform.parent.gameObject.GetComponent<EnemyMechTest>().setTarget(other.gameObject);
        }
    }



    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == transform.parent.gameObject.GetComponent<MouseTesting>().target)
        {
            UnityEngine.Debug.Log("Exiting targeting radius");
            transform.parent.gameObject.GetComponent<MouseTesting>().setTarget(null);
        }
    }
    */
}
