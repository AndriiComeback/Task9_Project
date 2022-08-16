using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Explodable : MonoBehaviour
{
	[SerializeField]
	public float radius = 5.0F;

	[SerializeField]
	public float power = 10.0F;

	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Bullet")) {
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
			foreach (Collider hit in colliders) {
				Rigidbody rb = hit.GetComponent<Rigidbody>();

				if (rb != null)
					rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
			}
			Destroy(gameObject);
		}
	}
}
