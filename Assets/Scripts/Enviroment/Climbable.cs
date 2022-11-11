
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public float secondsToReturnCollision = 1;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
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



