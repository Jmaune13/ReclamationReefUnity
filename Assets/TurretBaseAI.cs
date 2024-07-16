using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretBaseAI : MonoBehaviour
{

    public GameObject targetObj;
    [SerializeField] GameObject SelectionRing;

    //Some turrets are connected to a generator, if the generator is destroyed linked turrets will shut off using this
    //private Boolean powerUp;
    
    // Start is called before the first frame update
    void Start()
    {
        //powerUp = true;
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPowerOff()
    {
        //powerUp = false;
    }

    private void OnMouseEnter()
    {
        SelectionRing.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnMouseExit()
    {
        SelectionRing.GetComponent<SpriteRenderer>().enabled = false;
    }
}
