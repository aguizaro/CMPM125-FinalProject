using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private int health = 1;
    //public GameObject sword;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            GameManager.Instance.hit = true;
        }
        else if (collision.gameObject.CompareTag("Sword"))
        {
            Destroy(enemy);
        }
    }

}
