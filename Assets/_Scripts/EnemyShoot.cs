using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
	[SerializeField] Transform gunPoint;
	[SerializeField] float shotsPerSecond = 0.5f;
	[SerializeField] float shootPower = 500f;
	private Transform target;
	private bool seeTarget;
	private float time;

	private void Start() {
		target = GameObject.FindWithTag("Player").transform;
		time = 0f;
	}
	private void Update()
    {
		time += Time.deltaTime;
		CheckTargetVisibility();
		if (seeTarget && time > 1f / shotsPerSecond) {
			time = 0;
			Shoot();
		}
	}
    private void CheckTargetVisibility() {
		if (target == null) {
			target = GameObject.FindWithTag("Player").transform;
		}
		Vector3 targetDirection = target.position - gunPoint.transform.position;
		Ray ray = new Ray(gunPoint.transform.position, targetDirection);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform == target) {
				seeTarget = true;
				return;
			}
		}
		seeTarget = false;
	}
	private void Shoot() {
		GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
		Vector3 targetDirection = target.position - gunPoint.transform.position;
		targetDirection.Normalize();
		newBullet.GetComponent<Rigidbody>().AddForce(targetDirection * shootPower);
		Destroy(newBullet, 5);
	}
}
