using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float speed = 2.0f;

    private bool sees = false;
    private GameObject target;
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        timer = wanderTimer;
        SetNewRandomDestination();
        target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (sees)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {

            if (timer <= 0f)
            {
                SetNewRandomDestination();
                timer = wanderTimer;
            }
        }
        RaycastHit hit;
        var rayDirection = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            Debug.DrawRay(transform.position, rayDirection, Color.red, Mathf.Infinity);

            if (hit.collider.CompareTag("Player")) //(hit.transform == player)
            {
                sees = true;
                // Debug.Log("found you");
                agent.SetDestination(target.transform.position);
                //Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                sees = false;
            }
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
