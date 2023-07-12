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

    public float ladderImpulse = 15;

    public float climbSpeed = 10f;

    public float cayoteTime = 1.0f;

    public float ladderStickingLength = 3;

    private CharacterController _characterController;
    private Animator _animator;

    public float _vertSpeed;
    private bool _canClimb;
    private bool _isClimbing;



    private float _xPosition;
    private bool _impulseToGetUp;

    public void CanClimb(bool canClimb)
    {
        this._canClimb = canClimb;
        if (!canClimb && _isClimbing)
        {
            AdjustClimbingPosition();
            _isClimbing = false;
            _animator.SetBool("Climbing", false);
        }
    }

    private void AdjustClimbingPosition()
    {
        Vector3 newPoint = transform.position;
        if (_isClimbing)
        {
            transform.LookAt(transform.position + Vector3.forward);
            newPoint.x = _xPosition;
        }
        else
        {
            transform.LookAt(transform.position + Vector3.left);
            newPoint.x = _xPosition - ladderStickingLength;
        }
        teleportToPoint(newPoint, false);
    }

    public void teleportToPoint(Vector3 point, bool saveXPosition = true)
    {
        _characterController.enabled = false;
        transform.position = point;
        if (saveXPosition)
        {
            _xPosition = point.x;
        }
        _characterController.enabled = true;
    }

    void Start()
    {
        _isClimbing = false;
        _canClimb = false;
        _vertSpeed = minFall;
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = Vector3.zero;

        float horizontalMovement = GetHorizontalMovement();
        movement.z = horizontalMovement;

        _vertSpeed = GetVerticalMovement();
        if (_impulseToGetUp) _vertSpeed += ladderImpulse;
        movement.y = _vertSpeed;

        //TODO correct rotation mechanism
        if (!Mathf.Approximately(horizontalMovement, 0) && !_isClimbing)
        {
            transform.LookAt(transform.position + Vector3.forward * horizontalMovement);
        }


        _animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        _animator.SetFloat("VerticalSpeed", _vertSpeed);
        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }

    private float GetVerticalMovement()
    {
        float climbing = Input.GetAxis("Vertical") * climbSpeed;

        bool isGrounded = CheckGrounded();
        _animator.SetBool("InAir", !isGrounded);
        if (_canClimb)
        {
            if (isGrounded && climbing < 0)
            {
                if (_isClimbing)
                {
                    AdjustClimbingPosition();
                    _isClimbing = false;
                    _animator.SetBool("Climbing", false);
                }
            }
            else
            {
                if (!Mathf.Approximately(climbing, 0))
                {
                    if (!_isClimbing)
                    {
                        AdjustClimbingPosition();
                        _isClimbing = true;
                        _animator.SetBool("Climbing", true);
                    }
                    return climbing;
                }
            }
        }

        if (!_isClimbing)
        {
            _impulseToGetUp = false;
            return isGrounded ? PerformJumpIfPressed() : CalculateFallSpeed();
        }

        return 0f;
    }

    private float CalculateFallSpeed()
    {
        checkHitHead();

        float fallSpeed = _vertSpeed + gravity * fallAcceleration * Time.deltaTime;
        _vertSpeed = fallSpeed;
        if (fallSpeed < terminalVelocity)
        {
            fallSpeed = terminalVelocity;
        }
        return fallSpeed;
    }

    private void checkHitHead()
    {
        float rayCastDistance = (_characterController.height + _characterController.radius) * transform.localScale.y / 1.9f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, rayCastDistance))// && hit.collider.GetComponent<TransientFloor>() == null)
        {
            _vertSpeed = minFall;
        }
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
        return _vertSpeed < 0 && IsGroundIsUnderFoots(rayCastDistance);
    }

    private bool IsGroundIsUnderFoots(float rayCastDistance)
    {
        Vector3 footOffset = Vector3.forward * _characterController.radius * cayoteTime;
        return Physics.Raycast(transform.position + footOffset, Vector3.down, rayCastDistance) ||
            Physics.Raycast(transform.position - footOffset, Vector3.down, rayCastDistance);
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
