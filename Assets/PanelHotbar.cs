using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelHotbar : MonoBehaviour
{
    public GameObject imgO;
    public GameObject imgH;

    public GameObject Panel;
    public Sprite iconDef;
    [SerializeField] private GameObject icon;

    public Sprite iconScout;
    public Sprite iconMissile;
    public Sprite iconHeavy;

    private List<GameObject> imgList;

    // Start is called before the first frame update
    void Start()
    {
        imgList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addUnitImage(GameObject obj)
    {
        Debug.Log("Adding child ui element");
        //imgO = new GameObject();
        GameObject objT;
        //RectTransform rect = imgO.AddComponent<RectTransform>();
        //rect.transform.SetParent(Panel.transform);

        if(obj.transform.parent != null)
        {
            objT = obj.transform.parent.gameObject;
        } else
        {
            objT = obj;
        }

        //Debug.Log("Object T should be:" + objT.name);

        GameObject newIcon = Instantiate(icon);

        newIcon.transform.SetParent(Panel.transform);
        // = Panel.transform

        newIcon.GetComponent<UnitCardScript>().setGameObj(objT);
        Image image = newIcon.GetComponent<Image>();

        string holder = "";

        if (objT.GetComponent<Movement>() != null)
        {
            holder = objT.GetComponent<Movement>().UnitName;
        } else if (objT.GetComponent<MovementPro>() != null)
        {
            holder = objT.GetComponent<MovementPro>().UnitName;
        } else
        {
            holder = objT.GetComponent<MovementPro>().UnitName;
        }

        //imgO.transform.parent = Panel.transform;
        //imgO.AddComponent<RectTransform>();
        //RectTransform rect = imgO.GetComponent<RectTransform>();

        //rect.localPosition = Vector3.zero;
        //rect.localPosition = Vector2.one;

        //Image image = imgO.AddComponent<Image>();
        //imgO.AddComponent<UnitCardScript>();

        //imgO.GetComponent<UnitCardScript>().setGameObj(objT);

        Debug.Log("Setting Images icons up, using object:"+objT.name+" with name set as: "+holder);

        switch (holder)
        {
            case "MissileMech":
                //Debug.Log("Setting Test Mech Icon");
                image.sprite = iconMissile;
                break;
            case "TurretTop":
                //Debug.Log("Setting Test Mech Icon");
                image.sprite = iconMissile;
                break;
            case "ScoutMech":
                image.sprite = iconScout;
                break;
            case "HeadTop":
                image.sprite = iconScout;
                break;
            case "HeavyMech":
                image.sprite = iconHeavy;
                break;
            default:
                Debug.Log("Setting to default");
                image.sprite = iconDef;
                break;
        }

        //imgH = new GameObject();
        //imgH.AddComponent<Image>();

        //Image hbar = imgH.GetComponent<Image>();
        //RectTransform rectC = imgH.GetComponent<RectTransform>();
        //rectC.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, rect.offsetMax, rect.offsetMax);
        //rectC.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);

        //hbar.type = Image.Type.Filled;
        //hbar.fillMethod = Image.FillMethod.Horizontal;

        //hbar.fillOrigin = (int) Image.OriginHorizontal.Left;

        GameObject imgH = newIcon.transform.GetChild(0).gameObject;
        Image hbar = imgH.GetComponent<Image>();

        hbar.fillAmount = 1;
        hbar.fillAmount = objT.GetComponent<HealthManager>().getPartial();
        hbar.color = objT.GetComponent<HealthManager>().getGrad();

        //imgH.transform.parent = imgO.transform;
        //imgH.transform.SetParent(imgO.transform);

        //Add the img with ref to the list
        //imgList.Add(imgO.gameObject);
        imgList.Add(newIcon);

        //image.sprite = iconDef;
    }

    public void deleteUnitImage()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //Used to clear the img list cache
        if(imgList.Count != 0)
        {
            imgList.Clear();
        }
        //imgList.Clear();
    }

    public void deleteSpecUnit(int index)
    {
        Debug.Log("Destroying unit at index: " + index);
        Transform obj = transform.GetChild(index);
        //Used to clear a specific unit from the list
        imgList.Remove(obj.gameObject);
        Destroy(obj.gameObject);
        //Probably reorder list herre after
    }

    public void updateImgStats(GameObject obj, float newHealth, Color color)
    {
        Debug.Log("Inside updateImgStats with list size of: "+imgList.Count);
        foreach(GameObject temp in imgList)
        {
            //Debug.Log("Checking item: " + temp.name + " to compare to target: " + obj.name);
            if (obj == temp.GetComponent<UnitCardScript>().GetGameObject())
            {
                //Debug.Log("Found!");
                //Updates health bars, this is VERY slow and will cause issues on a larger scale more than likely
                //Debug.Log("Color before: " + temp.transform.GetComponentInChildren<Image>().color);
                temp.transform.GetChild(0).GetComponent<Image>().color = color;
                //Debug.Log("Fill percent: " + newHealth+" from "+ temp.transform.GetChild(0).GetComponent<Image>().fillAmount);
                temp.transform.GetChild(0).GetComponent<Image>().fillAmount = newHealth;
                //Debug.Log("Color after: " + temp.transform.GetComponentInChildren<Image>().color);
            }
            
        }
    }
}
