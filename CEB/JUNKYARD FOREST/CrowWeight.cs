using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowBehavior : MonoBehaviour
{
    [Header("Flight Destinations")]
    public Transform destinationPoint;
    private Vector3 originalPosition;

    [Header("Crow Settings")]
    public float flightSpeed = 5f;
    public Animator animator;

    private enum CrowState { Idle, FlyingToDest, Landed, Returning }
    private CrowState state = CrowState.Idle;

    private bool hasFlown = false;
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Start()
    {
        originalPosition = transform.position;
        animator.Play("Idle");
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (state == CrowState.Idle && !hasFlown)
        {
            StartCoroutine(FlyTo(destinationPoint.position, CrowState.Landed));
            hasFlown = true;
        }
        else if (state == CrowState.Landed)
        {
            StartCoroutine(FlyTo(originalPosition, CrowState.Idle));
            hasFlown = false;
        }
    }

    // --- MODIFICATION START ---
    public void ForceReturnToOrigin()
    {
        // Check if the crow is actually at the destination or flying towards it
        if (state == CrowState.Landed || state == CrowState.FlyingToDest)
        {
            // Stop any existing flight coroutine immediately to prevent conflicts
            StopAllCoroutines(); // Or be more specific if needed

            // Start flying back to the original position
            StartCoroutine(FlyTo(originalPosition, CrowState.Idle));
            hasFlown = false; // Ensure it can be triggered to fly to the destination again later
        }
        // If it's already Idle or Returning, we might not need to do anything,
        // or you could add logic here if needed.
    }
    // --- MODIFICATION END ---

    IEnumerator FlyTo(Vector3 destination, CrowState nextState)
    {
        isMoving = false;
        animator.Play("TakeFlight");
        yield return new WaitForSeconds(0.5f); // optional: match takeoff animation timing

        state = CrowState.FlyingToDest;
        targetPosition = destination;
        animator.Play("Fly");
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            yield return null;
        }

        isMoving = false;
        animator.Play("Land");
        yield return new WaitForSeconds(0.5f);

        animator.Play("Idle");
        state = nextState;
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * flightSpeed * Time.deltaTime;

        // Optional: flip crow depending on flight direction
        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }
}
