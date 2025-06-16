using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindColliderReceiver : MonoBehaviour
{
    [Header("Vent")]
    public float windForce = 10f;
    public Vector2 windDirection = Vector2.left;
    public float pushDurationPerParticle = 0.5f;

    [Header("Zone d'effet")]
    public BoxCollider2D pushZone;

    [Header("Détection d'abris")]
    public LayerMask windBlockingLayers; // Coche le layer de tes abris dans l'inspecteur

    private HashSet<int> activeParticles = new HashSet<int>();
    private bool windActive = false;

    public void NotifyParticleHit(int id)
    {
        if (!activeParticles.Contains(id))
        {
            activeParticles.Add(id);

            if (!windActive)
            {
                windActive = true;
                StartCoroutine(ApplyWind());
            }

            StartCoroutine(RemoveAfterDelay(id, pushDurationPerParticle));
        }
    }

    private IEnumerator ApplyWind()
    {
        while (windActive)
        {
            if (pushZone == null)
            {
                Debug.LogWarning("Push zone non assignée !");
                yield break;
            }

            Collider2D[] colliders = Physics2D.OverlapBoxAll(pushZone.bounds.center, pushZone.bounds.size, 0f);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    // Raycast depuis le vent vers le joueur
                    Vector2 playerPos = col.bounds.center;
                    float rayStartOffset = 100f; // Distance derrière le joueur, à ajuster selon ta scène

                    Vector2 origin = playerPos - windDirection.normalized * rayStartOffset;
                    Vector2 direction = (playerPos - origin).normalized;
                    float distance = Vector2.Distance(origin, playerPos);

                    RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, windBlockingLayers);

                    // Debug (vert = libre, rouge = bloqué)
                    Debug.DrawLine(origin, playerPos, hit.collider != null ? Color.red : Color.green, 0.1f);

                    if (hit.collider != null)
                    {
                        // Un obstacle bloque le vent (abri détecté)
                        continue;
                    }

                    // Pas d'obstacle, appliquer la poussée
                    Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(windDirection.normalized * windForce);
                    }
                }
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator RemoveAfterDelay(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeParticles.Remove(id);

        if (activeParticles.Count == 0)
        {
            windActive = false;
        }
    }
}
