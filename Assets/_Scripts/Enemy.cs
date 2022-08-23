using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }
    private ScoreLabel scoreLabel;

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            agent.SetDestination(target.position);
        }
	}
    private void OnDestroy() {
        scoreLabel = GameObject.FindWithTag("Score Label").GetComponent<ScoreLabel>();
		scoreLabel.Score += 25;
    }
}
