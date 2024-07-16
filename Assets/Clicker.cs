using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    //Layer masks for clicking
    public LayerMask clickable;
    //public LayerMask ground;

    //Material and List for selection
    public Material select;
    public Material defMat;
    public List<GameObject> unitList;
    public GameObject canvas;

    //UI component

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(UnitList);
        createList();
    }

    // Update is called once per frame
    void Update()
    {
        //Updated Selection
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable.value))
            {
                Debug.Log("Output:" + hit.collider.gameObject);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    chainedSelection(hit);
                }
                else
                {
                    singleSelection(hit);
                }
            }
            else
            {
                //Debug.Log("Output:"+ hit.collider.gameObject);
                Debug.Log("Removing Unit");
                clearList();
            }
            
        }

        //Basic MoveOrder
        if (Input.GetMouseButtonDown(1))
        {
            //Checking for movement or attack
            RaycastHit hit;
            Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (unitList.Count != 0 && (Physics.Raycast(pos, out hit, Mathf.Infinity)))
            {
                if (hit.collider.gameObject.tag == "enemy" || hit.collider.gameObject.tag == "enemyWall")
                {
                    //Attacks, first is regular attack while second moves the target in range to attack
                    //Debug.Log("Attack Command");
                    //Else, lets look for an enemy to attack
                    //if (hit.collider.gameObject.tag == "enemy" || hit.collider.gameObject.tag == "enemyWall")
                    
                        foreach (var x in unitList)
                        {
                            Debug.Log("Setting target");
                            //STOPPED HERE, CONTINUE >>>
                            GameObject child = x.transform.GetChild(0).gameObject;
                            child.GetComponent<MouseTesting>().setTarget(hit.collider.gameObject);

                            float detect = child.GetComponentInChildren<SphereCollider>().radius;

                            //float detect = x.GetComponent<SphereCollider>().radius;

                            //Checks to see if the target is out of range, will move by calling the bottom part (parent) to begin movement
                            if(Vector3.Distance(x.transform.position, hit.collider.gameObject.transform.position) > detect)
                            {
                                if (x.GetComponent<Movement>() != null)
                                {
                                    x.GetComponent<Movement>().attackMove(hit.collider.gameObject, detect);
                                }
                                else if (x.GetComponent<MovementPro>() != null)
                                {
                                    x.GetComponent<MovementPro>().attackMove(hit.collider.gameObject, detect);
                                }
                                else
                                {

                                }
                            //x.GetComponent<Movement>().attackMove(hit.collider.gameObject, detect);
                            }
                        }
                    

                } else
                {
                    //Move Command
                    //Debug.Log("Move Command");
                    foreach (var x in unitList)
                    {
                        if (x.GetComponent<Movement>() != null)
                        {
                            x.GetComponent<Movement>().unitMove(pos);
                        } else
                        {
                            x.GetComponent<MovementPro>().unitMove(pos);
                        }
                        //x.GetComponent<Movement>().unitMove(pos);
                    }
                }
            }
        }

        //Testing Selection Components
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var x in unitList)
            {
                Debug.Log("In Selection: "+x.ToString());
            }
        }
    }

    void createList()
    {
        unitList = new List<GameObject>();
    }

    void clearList()
    {
        removeUnitUI();
        foreach (var x in unitList)
        {
             removeSelected(x);

        }
        if(unitList.Count != 0)
        {
            unitList.Clear();
        }
        //unitList.Clear();
    }

    //Testing Selection Colors
    void setSelected(GameObject objS)
    {
        if(objS.GetComponent<Movement>() != null)
        {
            Debug.Log("Setting selected bool in movement script");
            objS.GetComponent<Movement>().isSelected = true;
            objS.GetComponent<Movement>().setSelection(true);
        } else if (objS.GetComponent<MovementPro>() != null)
        {
            Debug.Log("Setting selected bool in movementPRO script");
            objS.GetComponent<MovementPro>().isSelected = true;
            objS.GetComponent<MovementPro>().setSelection(true);
        } else
        {

        }
        //objS.GetComponent<Movement>().isSelected = true;    
    }

    void removeSelected(GameObject objS)
    {
        if (objS.GetComponent<Movement>() != null)
        {
            Debug.Log("Setting selected bool to false in movement script");
            objS.GetComponent<Movement>().isSelected = false;
            objS.GetComponent<Movement>().setSelection(false);
        }
        else if (objS.GetComponent<MovementPro>() != null)
        {
            Debug.Log("Setting selected bool to false in movementPRO script");
            objS.GetComponent<MovementPro>().isSelected = false;
            objS.GetComponent<MovementPro>().setSelection(false);
        }
        else
        {

        }

    }


    //These two functions handle chained and single selection. Single selection only applies when left shift is NOT held down. Chained is when left shift is. Chained will allow multiple units to be selected
    void singleSelection(RaycastHit hit)
    {
        Debug.Log("Single Selection"+ hit);
        if (hit.collider.gameObject.transform.parent == null)
        {
            if (!unitList.Contains(hit.collider.gameObject))
            {
                clearList();
                addUnitToUI(hit.collider.gameObject);
                unitList.Add(hit.collider.gameObject);
                Debug.Log("Adding Unit Base" + hit.collider.gameObject.name);
                setSelected(hit.collider.gameObject);
            }
            else
            {
                clearList();
                addUnitToUI(hit.collider.gameObject);
                Debug.Log("Already added, removing");
                unitList.Remove(hit.collider.gameObject);
                removeSelected(hit.collider.gameObject);
            };
        }
        else
        {

            if (!unitList.Contains(hit.collider.gameObject.transform.parent.gameObject))
            {
                clearList();
                addUnitToUI(hit.collider.gameObject);
                unitList.Add(hit.collider.gameObject.transform.parent.gameObject);
                Debug.Log("Adding Unit Turret" + hit.collider.gameObject.transform.root.name);
                setSelected(hit.collider.gameObject.transform.parent.gameObject);
            }
            else
            {
                clearList();
                addUnitToUI(hit.collider.gameObject);
                Debug.Log("Already added, removing");
                unitList.Remove(hit.collider.gameObject.transform.parent.gameObject);
                removeSelected(hit.collider.gameObject.transform.parent.gameObject);
            }
        }
    }

    void chainedSelection(RaycastHit hit)
    {
        if (hit.collider.gameObject.transform.parent == null)
        {
            if (!unitList.Contains(hit.collider.gameObject))
            {
                addUnitToUI(hit.collider.gameObject);
                unitList.Add(hit.collider.gameObject);
                Debug.Log("Adding Unit Base" + hit.collider.gameObject.name);
                setSelected(hit.collider.gameObject);
            }
            else
            {
                addUnitToUI(hit.collider.gameObject);
                Debug.Log("Already added, removing");
                unitList.Remove(hit.collider.gameObject);
                removeSelected(hit.collider.gameObject);
            };
        }
        else
        {

            if (!unitList.Contains(hit.collider.gameObject.transform.parent.gameObject))
            {
                addUnitToUI(hit.collider.gameObject);
                unitList.Add(hit.collider.gameObject.transform.parent.gameObject);
                Debug.Log("Adding Unit Turret" + hit.collider.gameObject.transform.root.name);
                setSelected(hit.collider.gameObject.transform.parent.gameObject);
            }
            else
            {
                addUnitToUI(hit.collider.gameObject);
                Debug.Log("Already added, removing");
                unitList.Remove(hit.collider.gameObject.transform.parent.gameObject);
                removeSelected(hit.collider.gameObject.transform.parent.gameObject);
            }
        }
    }

    void addUnitToUI(GameObject obj)
    {
        canvas.transform.GetChild(0).GetComponent<PanelHotbar>().addUnitImage(obj);
    }

    void removeUnitUI()
    {
        canvas.transform.GetChild(0).GetComponent<PanelHotbar>().deleteUnitImage();
    }

    public void removeSpecific(GameObject obj)
    {
        int index = unitList.IndexOf(obj);
        canvas.transform.GetChild(0).GetComponent<PanelHotbar>().deleteSpecUnit(index);
        removeSelected(obj);
        unitList.Remove(obj);
    }
}
