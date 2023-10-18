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

    float velocity = 0f;
    float velocityAccelerationReference = 0f;

    Rigidbody2D rb;
    float rotationAmount = 0f;
    Vector3 rotationVec = new Vector3();
    float acceleration = 0f;

    ParticleSystem accelerationParticles;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        accelerationParticles = GetComponent<ParticleSystem>();
    }

    public void Update() {
        ReadInput();
        HandleAcceleration();
        HandleRotation();
        HandleParticleSystem();
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

    public void FixedUpdate() {
        rb.AddForce(transform.up * velocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void OnEnable() {
        rotateAction.Enable();
        accelerationAction.Enable();
    }

    public Vector2 GetInputVector() {
        return new Vector2(rotationAmount, acceleration);
    }

    private void HandleParticleSystem() {
        Debug.Log("Current Acceleration: " + acceleration);
        if (acceleration >= 0.1f) {
            Debug.Log("SHOULD Play PS");
            if (!accelerationParticles.isEmitting) {
                Debug.Log("Play PS");
                accelerationParticles.Play();
            }
        }
        else if (accelerationParticles.isEmitting) {
            Debug.Log("Stop PS");
            accelerationParticles.Stop();
        }
    }

    public void OnDisable() {
        rotateAction.Disable();
        accelerationAction.Disable();
    }

    public Vector2 MovementDirection {
        get => transform.up;
    }
}
