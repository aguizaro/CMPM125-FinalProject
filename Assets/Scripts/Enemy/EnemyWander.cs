using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float speed;
    public int health;
    public int strength;
    public int swordDamage = 2;
    public int explosionDamage = 10;
    private bool sees = false;
    private GameObject target;
    private NavMeshAgent agent;
    private GameManager _gameManager;
    private UIManager _uiManager;
    private float timer;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        timer = wanderTimer;
        SetNewRandomDestination();
        target = GameObject.FindWithTag("Player");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        //set enemy properties from game manager state
        speed = _gameManager.CurrentState.currentWaveParams.enemySpeed;
        health = _gameManager.CurrentState.currentWaveParams.enemyHealth;
        strength = _gameManager.CurrentState.currentWaveParams.enemyStrength;
        swordDamage = _gameManager.CurrentState.swordDamage;
    }

    void Update()
    {

        //if game is paused, stop the enemy
        if (!GameManager.Instance.isActive)
        {
            agent.isStopped = true;
            return;
        } //otherwise resume
        else if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        if (health <= 0)
        {
            //update the game manager
            _gameManager.CurrentState.playerKillCount++;
            _uiManager.UpdateKillCount(_gameManager.CurrentState.playerKillCount);

            _gameManager.CurrentState.enemiesRemaining--;
            _uiManager.UpdateEnemiesRemaining(_gameManager.CurrentState.enemiesRemaining);

            // --
            //play a death animation or sound or something here
            // --

            Destroy(gameObject); //destroy self if health is 0
            return;
        }

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
            //Debug.DrawRay(transform.position, rayDirection, Color.red, Mathf.Infinity);

            if (hit.collider.CompareTag("Player"))
            {
                sees = true;
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


    // Collision detection ---------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enemy collided with: " + collision.gameObject.name + " tag: " + collision.gameObject.tag);
        //Debug.Log("enemy collided with: " + collision.gameObject.name + " tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && !_gameManager.CurrentState.playerAttacked) // avoid causing player more dammage if player already taking damage
        {
            _gameManager.CurrentState.playerAttacked = true;
        }
        else if (collision.gameObject.CompareTag("Sword") && GameManager.Instance.hit) // only take damage if player is attacking
        {
            health -= swordDamage;
            Debug.Log("player hit enemy for " + swordDamage + " damage. Enemy is now at " + health + " health");

            //
            // maybe add a sound or animation here for enemy took damage
            //

        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            health -= explosionDamage;

            Debug.Log("player blew up enemy for " + explosionDamage + " damage. Enemy is now at " + health + " health");
        }

        //explosion damage is instant death handled by DestroyEnemiesInRadius.cs
    }
}
