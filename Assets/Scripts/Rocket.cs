using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //properties
    Rigidbody rb;
    AudioSource audioSrc;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;

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

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK");    //todo remove
                break;
            case "Fuel":
                print("Fuel");  //todo remove
                // add fuel
                break;
            default:
                print("Dead"); //todo remove
                // kill rocket
                break;
        }
    }

    /**
     * Handles key presses.
     * @author Sidney Goossens
     */
    private void ProcessInput()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust);
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

    private void Rotate()
    {
        rb.freezeRotation = true;   // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rb.freezeRotation = false;  // resume physics control of rotation
    }
}
