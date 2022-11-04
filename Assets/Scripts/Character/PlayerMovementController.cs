using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpSpeed = 12.0f;
    public float minFall = -1.5f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10f;
    public float fallAcceleration = 5f;

    private CharacterController _characterController;
    private float vertSpeed;
    private ControllerColliderHit contact;


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

        if (CheckGroundHit())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            vertSpeed += gravity * fallAcceleration * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }

            if (_characterController.isGrounded)
            {
                if (Vector3.Dot(movement, contact.normal) < 0)
                {
                    movement = contact.normal * speed;
                }
                else
                {
                    movement += contact.normal * speed;
                }
            }
        }
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

    private float GetHorizontalMovement()
    {
        return Input.GetAxis("Horizontal") * speed;
    }

    private bool CheckGroundHit()
    {
        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_characterController.height + _characterController.radius) / _characterController.height / 1.9f;
            hitGround = hit.distance <= check;
        }

        return hitGround;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }
}
