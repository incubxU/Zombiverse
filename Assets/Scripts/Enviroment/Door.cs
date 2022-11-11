
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField]
    public Door connectedDoor;

    public float coolDownTimeInSec;

    private PlayerMovementController _player;
    private float _currentCoolDown;


    public void setCoolDown(float coolDown)
    {
        this._currentCoolDown = coolDown;
    }

    public Vector3 GetDoorPosition()
    {
        return transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _currentCoolDown = 0;
        _player = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentCoolDown > 0) _currentCoolDown -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.E) && _player != null && _currentCoolDown <= 0)
        {
            _player.teleportToPoint(connectedDoor.GetDoorPosition());
            _player = null;
            connectedDoor.setCoolDown(coolDownTimeInSec);
            _currentCoolDown = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerMovementController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            _player = null;
        }

    }


}
