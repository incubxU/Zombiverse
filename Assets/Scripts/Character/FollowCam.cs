using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform target;
	public float smoothTime = 0.2f;

	private Vector3 _velocity = Vector3.zero;
	private Vector3 offset;

	// Use this for initialization
	void Start() {
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void LateUpdate() {
		Vector3 targetPosition = new Vector3(transform.position.x, target.position.y + offset.y, target.position.z + offset.z);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
	}
}
