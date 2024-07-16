using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererSpecifics : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startFire(Transform target)
    {
        line.SetPosition(0, gameObject.transform.position);
        line.SetPosition(1, target.position);
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        line.enabled = true;
        yield return new WaitForSeconds(0.2f);
        line.enabled = false;
    }
}
