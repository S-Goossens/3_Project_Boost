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
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSrc.Stop();
        audioSrc.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);    // parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSrc.Stop();
        audioSrc.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);    // parameterise time
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {

        SceneManager.LoadScene(1);
    }

    private void ProcessInput()
    {
        // todo somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            Rotate();
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSrc.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSrc.isPlaying)    // so the sound doesn't layer
        {
            audioSrc.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
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
