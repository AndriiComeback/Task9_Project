using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private float offsetX = 0f;

	[SerializeField]
	private float offsetY = 8f;

	[SerializeField]
	private float offsetZ = 7f;

	public float turnSpeed = 4.0f;
	public Transform player;

	private Vector3 offset;

	void Start() {
		offset = new Vector3(player.position.x + offsetX, player.position.y + offsetY, player.position.z + offsetZ);
	}

	void LateUpdate() {
		offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
		transform.position = player.position + offset;
		transform.LookAt(player.position + Vector3.up * 2);
	}
}
