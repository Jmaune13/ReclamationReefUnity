using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // General use manager health script that all units use to reduce specific script calls. All friendly and enemy units will utilize this script on their parent game object. 

    //Unit health and bar updates
    [SerializeField] private Gradient gradient; 
    [SerializeField] private float MaxHealth = 100f;
    public float currHealth = 0;
    [SerializeField] private Image healthbar;

    //For deletion
    [SerializeField] private GameObject clicker;

    //For UI Elements
    [SerializeField] private GameObject can;
    //private GameObject can;

    //Unit Particle Systems
    public ParticleSystem explosion;

    //Sound
    [SerializeField] private AudioSource explosionAudio;

    [SerializeField] public GameObject LevelCond;

    void Start()
    {
        currHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (currHealth - damage <= 0f)
        {
            onKill();
        }
        else
        {
            currHealth -= damage;
            updateHealthBar();

        }
    }

    private void onKill()
    {
        //Destroy(Instantiate(explosion.gameObject), explosion.main.startLifetimeMultiplier);
        GameObject deathClone = Instantiate(explosion.gameObject, gameObject.transform.position + Vector3.up, Quaternion.identity);
        Destroy(deathClone, 2);
        if(gameObject.TryGetComponent<Movement>(out Movement move))
        {
            if (gameObject.tag == "ally" && move.isSelected == true)
            {
                clicker.GetComponent<Clicker>().removeSpecific(gameObject);
            }
        } else if(gameObject.TryGetComponent<MovementPro>(out MovementPro pro))
        {
            if (gameObject.tag == "ally" && pro.isSelected == true)
            {
                clicker.GetComponent<Clicker>().removeSpecific(gameObject);
            }
        }
        /*
        if(gameObject.tag == "ally" && (gameObject.GetComponent<Movement>().isSelected == true || gameObject.GetComponent<MovementPro>().isSelected == true))
        {
            clicker.GetComponent<Clicker>().removeSpecific(gameObject);
        }
        */

        if(gameObject.tag == "ally")
        {
            LevelCond.GetComponent<LevelConditionScript>().units.Remove(gameObject);
        }

        Destroy(gameObject);
    }

    private void updateHealthBar()
    {
        healthbar.fillAmount = currHealth / MaxHealth;
        healthbar.color = gradient.Evaluate(healthbar.fillAmount);
        if(can != null)
        {
            //Debug.Log("Calling update to panel ui");
            updateUI();
            //can.transform.GetChild(0).GetComponent<PanelHotbar>().updateImgStats(gameObject, healthbar.fillAmount, healthbar.color);
            //Debug.Log("Finished Calling Update");
        } else
        {
            Debug.Log("Canvas is null");
        }
    }

    public void addHealth(float addh)
    {
        if(currHealth < MaxHealth)
        {
            Debug.Log("Healing");
            currHealth += addh;
            updateHealthBar();
        }
    }

    public float getPartial()
    {
        return currHealth / MaxHealth;
    }

    public UnityEngine.Color getGrad()
    {
        return gradient.Evaluate(healthbar.fillAmount);
    }

    public void updateUI()
    {
        Debug.Log("NOW calling update to panel ui");
        float fil = healthbar.fillAmount;
        Color col = healthbar.color;
        can.transform.GetChild(0).GetComponent<PanelHotbar>().updateImgStats(gameObject, fil, col);
    }

    public void setCanvas(GameObject cant, GameObject click)
    {
        Debug.Log("Setting cnavas reference: "+cant);
        can = cant;
        clicker = click;
    }


}
