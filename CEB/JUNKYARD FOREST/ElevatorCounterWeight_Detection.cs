using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Ensure Collider is present
public class ElevatorDetection : MonoBehaviour
{
    private ElevatorMovement parentMovementController; // Reference to the script on the parent

    void Start()
    {
        // Find the ElevatorMovement script on the parent GameObject
        parentMovementController = GetComponentInParent<ElevatorMovement>();

        // Validate that the parent script was found
        if (parentMovementController == null)
        {
            Debug.LogError("ElevatorDetection: Could not find ElevatorMovement script on parent GameObject!", this);
            enabled = false; // Disable script
            return;
        }

        // Validate that this collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogError("ElevatorDetection: The Collider2D on this GameObject MUST be set to 'Is Trigger'!", this);
            enabled = false;
        }
    }

    // Called by Unity when a Collider2D enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if parent controller reference is valid
        if (parentMovementController == null) return;

        // Check tags and inform the parent controller
        if (other.CompareTag("Player"))
        {
            parentMovementController.SetPlayerDetected(true);
        }
        else if (other.CompareTag("Crow"))
        {
            // Pass the transform of the crow that entered
            parentMovementController.AddCrow(other.transform);
        }
    }

    // Called by Unity when a Collider2D exits this trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if parent controller reference is valid
        if (parentMovementController == null) return;

        // Check tags and inform the parent controller
        if (other.CompareTag("Player"))
        {
            parentMovementController.SetPlayerDetected(false);
        }
        else if (other.CompareTag("Crow"))
        {
            // Pass the transform of the crow that exited
            parentMovementController.RemoveCrow(other.transform);
        }
    }
}
