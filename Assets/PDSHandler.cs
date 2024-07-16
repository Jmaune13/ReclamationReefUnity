using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDSHandler : MonoBehaviour
{
    [SerializeField] private SphereCollider field;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void acivateShield()
    {
        field.enabled = true;
    }

    public void deactivateShield()
    {
        field.enabled = false;
    }
}
