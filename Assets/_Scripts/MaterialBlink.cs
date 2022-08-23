using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialBlink : MonoBehaviour
{
	[SerializeField]
	private Material material1;

	[SerializeField]
	private Material material2;

	private Renderer _renderer;
	bool isFirstMaterial;

	private float time;
	private void Awake() {
		_renderer = GetComponent<Renderer>();
		time = 0f;
		isFirstMaterial = true;
	}
	void Update()
    {
		time += Time.deltaTime;
		if (time > 0.25f) {
			_renderer.material = isFirstMaterial ? material2 : material1;
			isFirstMaterial = !isFirstMaterial;
			time = 0f;
		}
	}
}
