using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCardScript : MonoBehaviour
{
    GameObject unitHolder; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetGameObject()
    {
        return unitHolder;
    }

    public void setGameObj(GameObject obj)
    {
        unitHolder = obj;
    }
}
