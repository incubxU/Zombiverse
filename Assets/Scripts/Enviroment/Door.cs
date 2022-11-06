
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField]
    public Door connectedDoor;

    public float coolDownTimeInSec;

    private GameObject _player;
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
        _currentCoolDown = 0;
        _player = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentCoolDown > 0) _currentCoolDown -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.E) && _player != null && _currentCoolDown <= 0)
        {
            Debug.Log("Teleported");
            _player.GetComponent<CharacterController>().enabled = false;
            _player.transform.position = connectedDoor.GetDoorPosition();
            _player.GetComponent<CharacterController>().enabled = true;
            _player = null;
            connectedDoor.setCoolDown(coolDownTimeInSec);
            _currentCoolDown = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            _player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovementController>() != null)
        {
            _player = null;
        }

    }


}
