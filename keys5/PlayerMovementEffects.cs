// PlayerMovementEffects.cs
using UnityEngine;

public class PlayerMovementEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem movementParticles;
    [SerializeField] private float minSpeedForParticles = 0.1f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > minSpeedForParticles)
        {
            if (!movementParticles.isPlaying)
            {
                movementParticles.Play();
            }
        }
        else
        {
            if (movementParticles.isPlaying)
            {
                movementParticles.Stop();
            }
        }
    }
}