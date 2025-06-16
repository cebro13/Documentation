using UnityEngine;

public class ParticleManager : CoreComponent
{
    const string PARTICLE_CONTAINER_TAG = "ParticleContainer";
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();
        particleContainer = GameObject.FindGameObjectWithTag(PARTICLE_CONTAINER_TAG).transform;
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(particlePrefab, position, rotation, particleContainer);
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        return Instantiate(particlePrefab, transform.position, randomRotation);
    }
}
