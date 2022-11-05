using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransientFloor : MonoBehaviour
{

    Collider _collider;

    bool _isPlayerInside;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(Input.GetAxis("Vertical"), 0) || _isPlayerInside)
        {
            _collider.isTrigger = true;
        }
        else
        {
            _collider.isTrigger = false;

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
