using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpSpeed = 12.0f;
    public float minFall = -1.5f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10f;
    public float fallAcceleration = 5f;

    public float climbSpeed = 10f;

    public float cayoteTime = 1.0f;

    private CharacterController _characterController;
    public float vertSpeed;


    private bool _canClimb;
    private bool _isClimbing;
    private bool _impulseToGetUp;

    public void CanClimb(bool canClimb)
    {
        _canClimb = canClimb;
        if (!canClimb && _isClimbing)
        {
            transform.LookAt(transform.position + Vector3.forward, Vector3.forward);
            _characterController.Move(new Vector3(0,0,-2));
        }
    }

    void Start()
    {
        Debug.Log("Here");
        _isClimbing = false;
        _canClimb = false;
        vertSpeed = minFall;
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = Vector3.zero;

        float horizontalMovement = GetHorizontalMovement();
        movement.x = horizontalMovement;

        vertSpeed = GetVerticalMovement();
        if (_impulseToGetUp) vertSpeed += 10f;
        movement.y = vertSpeed;

        //TODO correct rotation mechanism
        Vector3 pScale = transform.localScale;
        if (!Mathf.Approximately(horizontalMovement, 0) && !_isClimbing)
        {
            pScale.x = Mathf.Sign(horizontalMovement) * Mathf.Abs(pScale.x);
            transform.localScale = pScale;
        }


        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }

    private float GetVerticalMovement()
    {
        float climbing = Input.GetAxis("Vertical") * climbSpeed;
        if (_canClimb && !Mathf.Approximately(climbing, 0))
        {
            if (!_isClimbing)
            {
                transform.LookAt(transform.position + Vector3.left, Vector3.right);
                _characterController.Move(new Vector3(0, 0, 2));
                _isClimbing = true;
            }
            return climbing;
        }

        if (!_canClimb)
        {
            _isClimbing = false;
        }

        if (!_isClimbing)
        {
            _impulseToGetUp = false;
            return CheckGrounded() ? PerformJumpIfPressed() : CalculateFallSpeed();
        }

        return 0f;
    }

    private float CalculateFallSpeed()
    {
        float fallSpeed = vertSpeed + gravity * fallAcceleration * Time.deltaTime;
        vertSpeed = fallSpeed;
        if (fallSpeed < terminalVelocity)
        {
            fallSpeed = terminalVelocity;
        }
        return fallSpeed;
    }

    private float PerformJumpIfPressed()
    {
        return Input.GetKeyDown(KeyCode.Space) ? jumpSpeed : minFall;
    }

    private float GetHorizontalMovement()
    {
        return Input.GetAxis("Horizontal") * speed;
    }

    private bool CheckGrounded()
    {
        float rayCastDistance = (_characterController.height + _characterController.radius) / 1.9f;
        return vertSpeed < 0 && IsGroundIsUnderFoots(rayCastDistance);
    }

    private bool IsGroundIsUnderFoots(float rayCastDistance)
    {
        return Physics.Raycast(transform.position + Vector3.left * _characterController.radius * cayoteTime, Vector3.down, rayCastDistance) ||
            Physics.Raycast(transform.position + Vector3.right * _characterController.radius * cayoteTime, Vector3.down, rayCastDistance);
    }

    private void OnTriggerExit(Collider other)
    {
        TransientFloor floor = other.gameObject.GetComponent<TransientFloor>();
        if (floor != null && other.transform.position.y < transform.position.y)
        {
            _impulseToGetUp = true;
        }
    }
}
