using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Input Configuration")]
    [SerializeField] InputAction rotateAction;
    [SerializeField] InputAction accelerationAction;
    
    [Space]
    
    [Header("Movement Configuration")]
    [SerializeField] float max_velocity = 10f;
    [SerializeField] float min_velocity = 0f;
    [SerializeField] float accelerationTime = 0.3f;                                         // in secords
    [SerializeField] float rotationMultiplier = 10f;

    [Space]

    [Header("Audio Configuration")]
    [SerializeField] private AudioClip accelerationAudioClip;

    bool isAccelerationSoundPlaying = false;

    float velocity = 0f;
    float velocityAccelerationReference = 0f;

    Rigidbody2D rb;
    float rotationAmount = 0f;
    Vector3 rotationVec = new Vector3();
    float acceleration = 0f;
    bool isGrounded = false;

    AudioSource accelerationAudioSource;
    ParticleSystem accelerationParticles;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        accelerationParticles = GetComponent<ParticleSystem>();
        accelerationAudioSource = GetComponent<AudioSource>();
        accelerationAudioSource.clip = accelerationAudioClip;
    }

    public void Update() {
        ReadInput();
        HandleAcceleration();
        HandleRotation();
        HandleParticleSystem();
        HandleAccelerationAudio();
    }   
    
    public void FixedUpdate() {
        rb.AddForce(transform.up * velocity * Time.fixedDeltaTime, ForceMode2D.Impulse);

        IsGrounded();
    }

    private void HandleAccelerationAudio() {
        if (acceleration > 0) {
            if (!isAccelerationSoundPlaying) {
                accelerationAudioSource.Play();
                isAccelerationSoundPlaying = true;
            }
        }
        else if (isAccelerationSoundPlaying) {
            accelerationAudioSource.Stop();
            isAccelerationSoundPlaying = false;
        }
    }

    private void IsGrounded() {
        int playerLayerIndex = 6;
        int raycastLayerMask = ~(1 << playerLayerIndex);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1.4f, raycastLayerMask);
        Debug.DrawRay(transform.position, -transform.up * 1.4f, Color.green);
        isGrounded = (hit.collider != null);
    }

    private void ReadInput() {
        rotationAmount = rotateAction.ReadValue<float>();
        rotationVec = new Vector3(0.0f, 0.0f, rotationAmount);
        acceleration = accelerationAction.ReadValue<float>();
    }

    private void HandleAcceleration() {
        if (acceleration == 0)
            velocity = Mathf.SmoothDamp(velocity, min_velocity, ref velocityAccelerationReference, acceleration);
        else
            velocity = Mathf.SmoothDamp(velocity, max_velocity, ref velocityAccelerationReference, accelerationTime) * acceleration;
    }

    private void HandleRotation() {
        float rotAngle = acceleration * -rotationAmount * rotationMultiplier * Time.deltaTime;
        rb.angularVelocity += rotAngle;
    }

    private void HandleParticleSystem() {
        if (acceleration >= 0.1f) {
            if (!accelerationParticles.isEmitting && !isGrounded) {
                accelerationParticles.Play();
            }
        }
        else if (accelerationParticles.isEmitting) {
            accelerationParticles.Stop();
        }
    }


    public void OnEnable() {
        rotateAction.Enable();
        accelerationAction.Enable();
    }

    public Vector2 GetInputVector() {
        return new Vector2(rotationAmount, acceleration);
    }


    public void OnDisable() {
        rotateAction.Disable();
        accelerationAction.Disable();
    }

    public Vector2 MovementDirection {
        get => transform.up;
    }
}
