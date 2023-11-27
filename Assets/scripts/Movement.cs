using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    
    Rigidbody rb;
    AudioSource auS;

    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody>();
      auS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      ProcessThrust();
      ProcessRotation();
    }

    void ProcessThrust()
    {
      if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
      else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            LeftRotating();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RightRotating();
        }
        else
        {
            ParticleStopper();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!auS.isPlaying)
        {
            auS.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    void StopThrusting()
    {
        auS.Stop();
        mainEngineParticles.Stop();
    }


    void LeftRotating()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }

    void RightRotating()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }
    void ParticleStopper()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation so the physics system can take over
    }
}
