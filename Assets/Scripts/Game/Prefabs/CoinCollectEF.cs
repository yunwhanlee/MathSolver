using UnityEngine;
using UnityEngine.UI;

public class CoinCollectEF : MonoBehaviour
{
    public RectTransform target;

    private ParticleSystem system;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[30];
    private int count;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();

        if (system == null)
        {
            Debug.LogWarning("ParticleSystem component not found.");
            enabled = false;
        }
        else
        {
            system.Play();
        }
    }

    private void Update()
    {
        count = system.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = target.position;

            float remainingLifetime = particle.remainingLifetime;
            float startLifetime = particle.startLifetime;

            // Calculate the normalized remaining lifetime
            float normalizedLifetime = remainingLifetime / startLifetime;

            // Calculate the interpolated position using Slerp
            Vector3 tarPosi = Vector3.Lerp(v1, v2, 1 - normalizedLifetime);

            particle.position = system.transform.InverseTransformPoint(tarPosi);
            particles[i] = particle;

            // Check if the particle has reached the target position
            if (particles[i].position.y >= target.position.y) {
                particle.remainingLifetime = 0f; // Set remaining lifetime to 0 to deactivate the particle
                particles[i] = particle;
            }
        }

        system.SetParticles(particles, count);
    }
}