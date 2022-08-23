using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private float movementSpeed;
	private Vector3 center;

	private void Update() {
        MoveCamera();
	}
    void MoveCamera() {
		if (target == null) {
			target = GameObject.FindWithTag("Player").transform;
		}
		transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, movementSpeed * Time.deltaTime);
	}
}
