using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Sprite defSprite;
    [SerializeField] private Sprite Scout;
    [SerializeField] private Sprite Missile;
    [SerializeField] private Sprite Heavy;

    [SerializeField] private GameObject panel;

    [SerializeField] private Button ReadyUp;

    GameObject obj;

    public static List<int> SelectionNum = new List<int>();
    void Start()
    {
        ReadyUp.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach(int x in SelectionNum)
            {
                Debug.Log("Selection List values: " +x);
            }
        } 
        
        if (SelectionNum.Count == 3)
        {
            ReadyUp.interactable = true;
        } else
        {
            ReadyUp.interactable = false;
        }
    }

    public void addToArray(int i)
    {
        if(!(SelectionNum.Count > 2))
        {
            Debug.Log("Adding " + i + " to list");
            SelectionNum.Add(i);
            obj = new GameObject();
            obj.AddComponent<Image>();
            HandleImageSpecs(i, obj.GetComponent<Image>());
            obj.transform.SetParent(panel.transform);
        } else
        {
            Debug.Log("Cannot add");
        }
    }

    public List<int> getSelectionList()
    {
        return SelectionNum;
    }

    public void deleteLastUnit()
    {
        if((SelectionNum.Count > 0))
        {
            Debug.Log("Deleting...");
            SelectionNum.RemoveAt(SelectionNum.Count - 1);
            Destroy(panel.transform.GetChild((panel.transform.childCount - 1)).gameObject);
        } else
        {
            Debug.Log("Cannot Delete");
        }
       
    }

    private void HandleImageSpecs(int i, Image image)
    {
        switch (i)
        {
            case 1:
                image.sprite = Scout;
                break;
            case 2:
                image.sprite = Missile;
                break;
            case 3:
                image.sprite = Heavy;
                break;
            default:
                Debug.Log("Could not source image");
                image.sprite = defSprite;
                break;
        }
    }

    public void SceneSwap()
    {
        Debug.Log("Swapping Scenes");
        SceneManager.LoadScene(2);
    }


}
