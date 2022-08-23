using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Destructable owner;

	Image healthBar;
    bool rotateBar;

	void Start()
    {
        healthBar = gameObject.GetComponent<Image>();
		rotateBar = owner.gameObject.GetComponent<CharacterController>() == null;
	}

	// Update is called once per frame
	void Update()
    {
        healthBar.fillAmount = Mathf.InverseLerp(0.0f, owner.HitPoints, owner.HitPointsCurrent);
		if (rotateBar) {
			//transform.forward = Camera.main.transform.position - transform.position;
			transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.down);
		}
	}
}
