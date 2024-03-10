using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
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
            Destroy(other.gameObject);
            _gameManager.CurrentState.playerKillCount++;
            _gameManager.CurrentState.enemiesRemaining--;
            Debug.Log("Explosion killed enemy\nTotal: " + _gameManager.CurrentState.playerKillCount + " enemies killed, " + _gameManager.CurrentState.enemiesRemaining + " enemies remaining");
        }
    }
}
