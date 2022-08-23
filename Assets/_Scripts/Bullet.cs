using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage = 1;

    public float Damage { get { return damage; } }
    private void OnTriggerEnter(Collider other) {
		Destructable target = other.gameObject.GetComponent<Destructable>();
		if (target != null) {
			target.Hit(Damage);
		}
		Destroy(gameObject);
	}
}
