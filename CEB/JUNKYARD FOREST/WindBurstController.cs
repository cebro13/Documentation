using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindBurstController : MonoBehaviour
{
    [Header("Rythme du vent")]
    public float windInterval = 3f;           // Délai entre deux rafales
    public float burstDuration = 1.5f;        // Durée d’une rafale
    public float delayBeforeParticles = 0f;   // Décalage optionnel avant d’activer les particules

    [Header("Systèmes de particules")]
    public List<ParticleSystem> windParticles = new();

    [Header("Déclencheur de vent (optionnel)")]
    public WindColliderReceiver windReceiver;
    public bool notifyReceiver = false;

    private void Start()
    {
        StartCoroutine(WindBurstLoop());
    }

    private IEnumerator WindBurstLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(windInterval);

            if (delayBeforeParticles > 0)
                yield return new WaitForSeconds(delayBeforeParticles);

            ActivateParticles();

            if (notifyReceiver && windReceiver != null)
            {
                // Simule une rafale (id fictif = -1)
                windReceiver.NotifyParticleHit(-1);
            }

            yield return new WaitForSeconds(burstDuration);
            DeactivateParticles();
        }
    }

    private void ActivateParticles()
    {
        foreach (var ps in windParticles)
        {
            if (ps != null) ps.Play();
        }
    }

    private void DeactivateParticles()
    {
        foreach (var ps in windParticles)
        {
            if (ps != null) ps.Stop();
        }
    }
}
