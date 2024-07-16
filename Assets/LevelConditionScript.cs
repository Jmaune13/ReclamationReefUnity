using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConditionScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject objective;

    [SerializeField] GameObject SpawnPoint1;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] GameObject SpawnPoint3;

    [SerializeField] private GameObject Scout;
    [SerializeField] private GameObject Missile;
    [SerializeField] private GameObject Heavy;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject clicker;

    //Bonus objectives
    [SerializeField] private GameObject bonusUnit1;
    [SerializeField] private GameObject bonusUnit2;
    [SerializeField] private GameObject BonusPoint;

    //Music
    [SerializeField] private AudioSource music1;
    [SerializeField] private AudioSource music2;


    public List<GameObject> units = new List<GameObject>();

    GameObject cond;
    GameObject victory;
    GameObject defeat;

    bool gstart;

    bool bonusComplete;
    bool musicflag;

    void Start()
    {
        gstart = false;
        handleSpawning(SelectionUI.SelectionNum);
        cond = canvas.transform.GetChild(2).gameObject;
        victory = cond.transform.GetChild(0).gameObject;
        defeat = cond.transform.GetChild(1).gameObject;
        //Debug.Log(victory.name);
        //Debug.Log(defeat.name);
        victory.GetComponent<TextMeshProUGUI>().enabled = false;
        defeat.GetComponent<TextMeshProUGUI>().enabled = false;
        Debug.Log("Unit Count: " + units.Count);

        bonusComplete = false;

        music1.Play();
        musicflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(objective == null)
        {
            StartCoroutine(WinScript());
        }
        if(units.Count == 0 && gstart == true)
        {
            StartCoroutine(LoseScript());
        }
        if(bonusUnit1 == null && bonusUnit2 == null && bonusComplete == false)
        {
            bonusComplete = true;
            spawnBonusUnit();
        }
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Unit Count: " + units.Count);
            Debug.Log(gstart);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WinScript());
        }
        */
        if(music1.isPlaying == false && musicflag == false)
        {
            Debug.Log("playing music 2");
            music2.Play();
            musicflag = true;
        }
        if(music2.isPlaying == false && musicflag == true)
        {
            Debug.Log("playing music 1");
            music1.Play();
            musicflag = false;
        }
    }

    IEnumerator WinScript()
    {
        victory.GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator LoseScript()
    {
        defeat.GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    private void handleSpawning(List<int> list)
    {
        if (list.Count == 3)
        {
            getType(list[0], SpawnPoint1.transform);
            getType(list[1], SpawnPoint2.transform);
            getType(list[2], SpawnPoint3.transform);
            gstart = true;
        }
    }

    private void getType(int i, Transform transform)
    {
        GameObject temp;
        if(i == 1)
        {
            temp = Instantiate(Scout, transform.position, transform.rotation);
            temp.GetComponent<HealthManager>().setCanvas(canvas, clicker);
            temp.GetComponent<HealthManager>().LevelCond = gameObject;
        } else if (i == 2)
        {
            temp = Instantiate(Missile, transform.position, transform.rotation);
            temp.GetComponent<HealthManager>().setCanvas(canvas, clicker);
            temp.GetComponent<HealthManager>().LevelCond = gameObject;
        } else
        {
            temp = Instantiate(Heavy, transform.position, transform.rotation);
            temp.GetComponent<HealthManager>().setCanvas(canvas, clicker);
            temp.GetComponent<HealthManager>().LevelCond = gameObject;
        }
        units.Add(temp);
        
    }

    private void spawnBonusUnit()
    {
        GameObject temp;
        temp = Instantiate(Scout, BonusPoint.transform.position, BonusPoint.transform.rotation);
        temp.GetComponent<HealthManager>().setCanvas(canvas, clicker);
        temp.GetComponent<HealthManager>().LevelCond = gameObject;
        units.Add(temp);

    }
    


    
}
