using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //properties
    Rigidbody rb;
    AudioSource audioSrc;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

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
        if (state != State.Alive) { return; } // ignore collisions when dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);    // parameterise time
                break;
            default:
                // kill rocket
                print("hit something deadly");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);    // parameterise time
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {

        SceneManager.LoadScene(1);
    }

    /**
     * Handles key presses.
     * @author Sidney Goossens
     */
    private void ProcessInput()
    {
        // todo somewhere stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
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
