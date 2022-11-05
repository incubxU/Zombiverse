
using UnityEngine;

public class Climbable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.CanClimb(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.CanClimb(false);
        }
    }
}
