using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    public Transform m_player;
    public float m_attractionRange = 5f;
    public float m_attractionStrength = 10f;

    private ParticleSystem m_ps;
    private ParticleSystem.Particle[] m_particles;

    void Start()
    {
        m_ps = GetComponent<ParticleSystem>();
        m_particles = new ParticleSystem.Particle[m_ps.main.maxParticles];
    }

    void LateUpdate()
    {
        int aliveParticles = m_ps.GetParticles(m_particles);
        Vector3 m_playerPos = m_player.position;

        for (int i = 0; i < aliveParticles; i++)
        {
            Vector3 directionToPlayer = m_playerPos - m_particles[i].position;
            float distance = directionToPlayer.magnitude;

            if (distance < m_attractionRange)
            {
                Vector3 force = directionToPlayer.normalized * (m_attractionStrength * Time.deltaTime);
                m_particles[i].velocity += force;
            }
        }

        m_ps.SetParticles(m_particles, aliveParticles);
    }
}
