using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public float panSpeed = 20f;
    public float xLimit = 1080f;
    public float zLimit = 1080f;
    public float yMax = 100f;
    [SerializeField] private float yMin = 10f;

    [SerializeField] private float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        /*
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= (10f))
        {
            pos.x -= panSpeed * Time.deltaTime;
        } 
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= (Screen.width - 10f))
        {
            pos.x += panSpeed * Time.deltaTime;
        } 
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= (Screen.height - 10f))
        {
            pos.z += panSpeed * Time.deltaTime;
        } 
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= (10f))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        */

        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        pos.z = Mathf.Clamp(pos.z, -zLimit, zLimit);

        transform.position = pos;
    }
}
