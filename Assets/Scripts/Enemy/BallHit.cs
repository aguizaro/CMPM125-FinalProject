using System.Collections;
using UnityEngine;

public class BallHit : MonoBehaviour {

}
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ball collided with: " + collision.gameObject.name + " tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") && !_gameManager.CurrentState.playerAttacked) // avoid causing player more dammage if player already taking damage
        {
            _gameManager.CurrentState.playerAttacked = true;
            Destroy(gameObject)
        }
    }