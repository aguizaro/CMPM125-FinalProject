using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemiesInRadius : MonoBehaviour
{
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("explosion collided with: " + other.gameObject.name + " tag: " + other.gameObject.tag);

            _gameManager.CurrentState.playerKillCount++;
            _gameManager.CurrentState.enemiesRemaining--;

            Destroy(other.gameObject);
            Debug.Log("Explosion killed enemy\nTotal: " + _gameManager.CurrentState.playerKillCount + " enemies killed, " + _gameManager.CurrentState.enemiesRemaining + " enemies remaining");
        }
    }
}
