using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectList : MonoBehaviour
{
    // Start is called before the first frame update
    public static UnitSelectList Instance;

    public List<GameObject> units;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createList(GameObject objGame)
    {
        units.Add(objGame);   
    }

    void dropList()
    {
        units.Clear();
    }
}
