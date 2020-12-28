using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorMove : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private ParticleSystem.Particle[] m_Particles;
    private int numParticlesAlive;
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (!GetComponent<Transform>()) GetComponent<Transform>();
    }

    private void Update()
    {
        m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
        numParticlesAlive = ps.GetParticles(m_Particles);
        var step = speed * Time.deltaTime;
        for (var i = 0; i < numParticlesAlive; i++)
            m_Particles[i].position = Vector3.MoveTowards(m_Particles[i].position, target.position, step);
        ps.SetParticles(m_Particles, numParticlesAlive);
    }
}