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

    public float cayoteTime = 1.0f;

    private CharacterController _characterController;
    private float vertSpeed;


    void Start()
    {
        vertSpeed = minFall;
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        float horizontalMovement = GetHorizontalMovement();
        movement.x = horizontalMovement;


        vertSpeed = GetVerticalSpeed();
        movement.y = vertSpeed;

        //TODO correct rotation mechanism
        Vector3 pScale = transform.localScale;
        if (!Mathf.Approximately(horizontalMovement, 0))
        {
            pScale.x = Mathf.Sign(horizontalMovement) * Mathf.Abs(pScale.x);
            transform.localScale = pScale;
        }


        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }

    private float GetVerticalSpeed()
    {
        return CheckGrounded() ? PerformJumpIfPressed() : CalculateFallSpeed();
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
}
