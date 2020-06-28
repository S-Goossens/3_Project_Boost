using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //properties
    Rigidbody rb;
    AudioSource audioSrc;
    //public int rcsThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
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
        HandleThrust();

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

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up);
            if (!audioSrc.isPlaying)    // so the sound doesn't layer
            {
                audioSrc.Play();
            }
        }
        else
        {
            audioSrc.Stop();
        }
    }
}
