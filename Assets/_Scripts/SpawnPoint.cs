using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	[SerializeField] float spawnDelay = 3f;
	[SerializeField] List<Enemy> enemyPrefabs;
	private void Start() {
		StartCoroutine(SpawnEnemies());
	}
	public void Spawn() {
		int index = Random.Range(0, enemyPrefabs.Count);
		Enemy enemyPrefab = enemyPrefabs[index];
		Enemy newEnemy = Instantiate(enemyPrefab, transform.position,
		Quaternion.identity) as Enemy;
		newEnemy.Target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	IEnumerator SpawnEnemies() {
		for (int i = 0; i < 300; i++) {
			Spawn();
			yield return new WaitForSeconds(spawnDelay);
		}
	}
}
