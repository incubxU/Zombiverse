using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 4.5f;
	public float jumpSpeed = 15.0f;
	public float minFall = -1.5f;
	public float gravity = -9.8f;
	public float terminalVelocity = -10f;


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
		float deltaZ = Input.GetAxis("Horizontal") * speed;
		movement = Vector3.forward * deltaZ;
		


		if (_characterController.isGrounded)
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
			vertSpeed += gravity * 5 * Time.deltaTime;
			if (vertSpeed < terminalVelocity)
			{
				vertSpeed = terminalVelocity;
			}
		}
		movement.y = vertSpeed;


		Vector3 pScale = Vector3.one;
		if (!Mathf.Approximately(deltaZ, 0))
		{
			transform.localScale = new Vector3(Mathf.Sign(deltaZ) / pScale.x, 1 / pScale.y, 1);
		}


		movement = movement *= Time.deltaTime;
		_characterController.Move(movement);
	}
}
