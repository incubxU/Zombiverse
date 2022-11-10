using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransientFloor : MonoBehaviour
{

    Collider _collider;

    float cooldownToReturnCollision = 0;

    bool _isPlayerInside;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownToReturnCollision > 0) cooldownToReturnCollision -= Time.deltaTime;
        if ((Input.GetAxis("Vertical") < 0 && Input.GetKey(KeyCode.Space)) || _isPlayerInside)
        {

            cooldownToReturnCollision = 1;
            _collider.enabled = false;
            return;
        }

        if (cooldownToReturnCollision <= 0)
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
