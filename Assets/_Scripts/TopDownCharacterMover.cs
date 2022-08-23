using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterMover : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float moveSpeed = 3f;
    private Camera cam;
    [SerializeField] private float rotateSpeed;
    private void Awake() {
        _input = GetComponent<InputHandler>();
        cam = Camera.main;
	}
    private void Update() {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        Vector3 movementVector = MoveTowardTarget(targetVector);

        RotateTowardMovementVector(movementVector);

        LookAtTarget();

	}

    private Vector3 MoveTowardTarget(Vector3 targetVector) {
        float speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, cam.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;

	}

    private void RotateTowardMovementVector(Vector3 movementVector) {
        if (movementVector.magnitude == 0) {
            return;
        }
        Quaternion rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }
	private void LookAtTarget() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		Plane plane = new Plane(Vector3.up, transform.position);
		float distance;

		if (plane.Raycast(ray, out distance)) {
			Vector3 position = ray.GetPoint(distance);
			transform.LookAt(position);
		}
	}
}
