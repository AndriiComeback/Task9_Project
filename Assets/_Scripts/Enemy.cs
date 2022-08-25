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
        GameObject scoreLabelObject = GameObject.FindWithTag("Score Label");
        if (scoreLabelObject != null) {
			scoreLabel = scoreLabelObject.GetComponent<ScoreLabel>();
            if (scoreLabel != null) {
                scoreLabel.Score += 25;
            }
        }
    }
}
