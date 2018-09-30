using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightMech : MonoBehaviour
{
    //Light Mechanics
    public float mouseSpeed = 5.0f;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Receive Mouse input for flashlight mechanics
        Vector2 lightDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(lightDirection.y, lightDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, mouseSpeed * Time.deltaTime);
    }
}
