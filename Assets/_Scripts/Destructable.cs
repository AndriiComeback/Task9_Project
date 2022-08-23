using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
	[SerializeField] private float hitPoints;
	public float HitPoints { get { return hitPoints; } }
	private float hitPointsCurrent;
	public float HitPointsCurrent { get { return hitPointsCurrent; } }
	private void Start() {
		hitPointsCurrent = hitPoints;
	}
	public void Hit(float damage) {
		hitPointsCurrent -= damage;
		if (hitPointsCurrent <= 0) {
			Die();
		}
	}
	private void Die() {
		Destroy(gameObject);
	}
}
