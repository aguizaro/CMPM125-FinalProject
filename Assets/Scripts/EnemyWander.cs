using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewRandomDestination();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetNewRandomDestination();
            timer = wanderTimer;
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
    }
}
