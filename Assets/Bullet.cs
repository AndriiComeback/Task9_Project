using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    public int Damage { get { return damage; } }
    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
