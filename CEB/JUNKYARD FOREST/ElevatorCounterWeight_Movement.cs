using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Needed for List

// =====================================================
// ElevatorMovement.cs (Attach to Parent GameObject)
// =====================================================
public class ElevatorMovement : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("Assign the child Transform for the elevator platform.")]
    public Transform elevator;
    [Tooltip("Assign the child Transform for the counterweight.")]
    public Transform counterWeight;
    [Tooltip("Assign the Transform defining the highest point.")]
    public Transform elevatorTopPoint;
    [Tooltip("Assign the Transform defining the lowest point.")]
    public Transform elevatorBottomPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 1f;

    [Header("Crow Settings")]
    public int crowWeightThreshold = 3;

    // --- State managed by this script (updated by ElevatorDetection) ---
    private List<Transform> crowsOnPlatform = new List<Transform>();
    private bool playerInElevator = false;
    // ---

    private Vector3 mirrorPivot;

    void Start()
    {
        // Validate assigned transforms
        if (elevator == null || counterWeight == null || elevatorTopPoint == null || elevatorBottomPoint == null)
        {
            Debug.LogError("ElevatorMovement: One or more required Transforms are not assigned in the Inspector!", this);
            enabled = false; // Disable script
            return;
        }

        // Calculate the pivot point
        mirrorPivot = (elevatorTopPoint.position + elevatorBottomPoint.position) / 2f;
        Debug.Log("Elevator system initialized with midpoint: " + mirrorPivot);
    }

    void Update()
    {
        // Decide movement based on current state
        if (playerInElevator)
        {
            // Player presence scares crows and moves elevator up
            if (crowsOnPlatform.Count > 0)
            {
                Debug.Log("Player detected, clearing crows.");
                // We don't need the specific transforms to clear, but having the list confirms crows were present.
                crowsOnPlatform.Clear();
            }
            MoveElevator(Vector3.up);
        }
        else if (crowsOnPlatform.Count >= crowWeightThreshold)
        {
            // Enough crows (and no player) moves elevator down
            MoveElevator(Vector3.down);
        }
        // Otherwise, no movement command is issued, elevator stops.
    }

    // Handles the actual movement of elevator, counterweight, and crows
    void MoveElevator(Vector3 direction)
    {
        float targetY = direction == Vector3.down
            ? elevatorBottomPoint.position.y
            : elevatorTopPoint.position.y;

        float step = moveSpeed * Time.deltaTime;
        Vector3 positionBeforeMove = elevator.position;

        // Check if already at the target
        if (Mathf.Approximately(elevator.position.y, targetY)) return;

        // Move elevator
        Vector3 elevatorPos = elevator.position;
        elevatorPos.y = Mathf.MoveTowards(elevatorPos.y, targetY, step);
        elevator.position = elevatorPos;

        // Calculate movement delta
        Vector3 movementDelta = elevator.position - positionBeforeMove;

        // Move any crows on the platform along with it
        // (Requires crows NOT be parented to elevator)
        if (movementDelta != Vector3.zero && crowsOnPlatform.Count > 0)
        {
            // Use a standard for loop for safety if list might be modified elsewhere (though unlikely here)
            for (int i = 0; i < crowsOnPlatform.Count; i++)
            {
                Transform crowTransform = crowsOnPlatform[i];
                if (crowTransform != null)
                {
                    crowTransform.position += movementDelta;
                }
                // Optional: clean up null entries if crows can be destroyed
                // else { crowsOnPlatform.RemoveAt(i); i--; }
            }
        }

        // Move counterweight
        float offsetY = elevator.position.y - mirrorPivot.y;
        Vector3 counterPos = counterWeight.position;
        counterPos.y = mirrorPivot.y - offsetY;
        counterWeight.position = counterPos;
    }

    // --- Public Methods Called by ElevatorDetection Script ---

    public void SetPlayerDetected(bool isPlayerPresent)
    {
        if (playerInElevator != isPlayerPresent) // Only log change
        {
            playerInElevator = isPlayerPresent;
            Debug.Log("ElevatorMovement: Player detected status updated to " + isPlayerPresent);
        }
    }

    public void AddCrow(Transform crowTransform)
    {
        if (crowTransform != null && !crowsOnPlatform.Contains(crowTransform))
        {
            crowsOnPlatform.Add(crowTransform);
            Debug.Log("ElevatorMovement: Crow added. Count: " + crowsOnPlatform.Count);
        }
    }

    public void RemoveCrow(Transform crowTransform)
    {
        if (crowTransform != null && crowsOnPlatform.Contains(crowTransform))
        {
            crowsOnPlatform.Remove(crowTransform);
            Debug.Log("ElevatorMovement: Crow removed. Count: " + crowsOnPlatform.Count);
        }
    }
    // --- End Public Methods ---
}
