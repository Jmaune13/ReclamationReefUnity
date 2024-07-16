using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class detectMissiles : MonoBehaviour
{
    [SerializeField] private LineRenderer lineLaser;
    [SerializeField] private SphereCollider field;

    // Start is called before the first frame update
    void Start()
    {
        field.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemyMissile") {
            Debug.Log("Missile Spotted, taking out");
            lineLaser.SetPosition(0, transform.position);
            DrawLaser(other.gameObject.transform);
            other.GetComponent<enemyTraj>().EarlyExplosion();
        }
    }

    private void DrawLaser(Transform pos)
    {
        lineLaser.SetPosition(1, pos.position);
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        lineLaser.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineLaser.enabled = false;
    }
}
