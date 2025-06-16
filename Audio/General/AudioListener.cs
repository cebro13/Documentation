using UnityEngine;

public class AudioListener : MonoBehaviour
{
    void Update()
    {
        Vector3 worldPosition = transform.position;  // Get the world position
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);  // Set the z to 0 in world space
    }
}
