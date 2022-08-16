using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showHealthTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject hp;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            hp.SetActive(true);
            Destroy(gameObject);
        }
    }
}
