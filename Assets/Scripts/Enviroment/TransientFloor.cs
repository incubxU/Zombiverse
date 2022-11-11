using UnityEngine;

public class TransientFloor : MonoBehaviour
{

    Collider _collider;

    public float secondsToReturnCollision = 1;

    private float currentTimer = 0;


    bool _isPlayerInside;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimer > 0) currentTimer -= Time.deltaTime;

        if ((Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.Space)) || _isPlayerInside)
        {

            currentTimer = secondsToReturnCollision;
            _collider.enabled = false;
            return;
        }

        if (currentTimer <= 0)
        {
            _collider.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            _isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            _isPlayerInside = false;
        }
    }
}
