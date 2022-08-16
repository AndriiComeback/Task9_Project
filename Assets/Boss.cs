using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int health;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TMPro.TMP_Text text;

    private void Awake() {
        health = maxHealth;
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            TakeDamage(other.GetComponent<Bullet>().Damage);
        }
    }

    private void TakeDamage(int damage) {
        health -= damage;
        if (health < 0) {
            health = 0;
        }
        hpBar.value = health / (float)maxHealth;
        if (health == 0) {
            text.text = "NO!!! I SHALL RETURN, MEATBAGS!!!";
            hpBar.gameObject.SetActive(false);

            Rigidbody[] parts = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody part in parts) {
                part.isKinematic = false;
            }
			Destroy(gameObject, 5);
		}
    }
}
