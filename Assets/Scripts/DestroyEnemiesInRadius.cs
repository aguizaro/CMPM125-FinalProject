using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemiesInRadius : MonoBehaviour
{
    private GameManager _gameManager;
    private UIManager _uiManager;
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("explosion collided with: " + other.gameObject.name + " tag: " + other.gameObject.tag);
            Destroy(other.transform.parent.gameObject);
            _gameManager.CurrentState.playerKillCount++;
            _gameManager.CurrentState.enemiesRemaining--;
            _uiManager.UpdateKillCount(_gameManager.CurrentState.playerKillCount);
            _uiManager.UpdateEnemiesRemaining(_gameManager.CurrentState.enemiesRemaining);

            // Debug.Log("Explosion killed enemy\nTotal: " + _gameManager.CurrentState.playerKillCount + " enemies killed, " + _gameManager.CurrentState.enemiesRemaining + " enemies remaining");
        }
        else if (other.gameObject.CompareTag("Snowball")) {
            Destroy(other.gameObject);
        }
    }
}
