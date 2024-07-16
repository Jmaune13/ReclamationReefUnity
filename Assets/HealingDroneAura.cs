using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingDroneAura : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeTilDeath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.tag == "ally")
        {
            
            if (other.gameObject.TryGetComponent<HealthManager>(out HealthManager health))
            {
                health.addHealth(0.1f);
            }
        }
    }
    
    IEnumerator TimeTilDeath()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject.transform.parent.gameObject);
    }
    
}
