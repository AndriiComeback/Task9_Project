using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	[SerializeField] private float damage = 1;
	[SerializeField] GameObject explosionPrefab;

	public float Damage { get { return damage; } }
	private void OnTriggerEnter(Collider other) {
		Destructable target = other.gameObject.GetComponent<Destructable>();
		if (target != null) {
			target.Hit(Damage);
		}
		if (explosionPrefab != null) {
			Explosion.Create(transform.position, explosionPrefab);
		}
		ParticleSystem trail = gameObject.GetComponentInChildren<ParticleSystem>();
		if (trail != null) {
			Destroy(trail.gameObject, trail.startLifetime);
			trail.Stop();
			trail.transform.SetParent(null);
		}
		Destroy(gameObject);
	}
}
