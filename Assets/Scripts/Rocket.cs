using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //properties
    Rigidbody rb;
    public int rcsThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    /**
     * Handles key presses.
     * @author Sidney Goossens
     */
    private void ProcessInput()
    {
        //thrust
        if(Input.GetKey(KeyCode.Space))
        {
            print("Space pressed");
            rb.AddRelativeForce(Vector3.up);
        } 
        
        //rotation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("D pressed");
            transform.Rotate(-Vector3.forward);
        }
    }
}
