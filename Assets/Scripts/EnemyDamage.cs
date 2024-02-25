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
        GameManager.Instance.hot = false;
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
            if (GameManager.Instance.hot == true)
            {
                if (GameManager.Instance.attack == true)
                {
                    Destroy(enemy);
                    GameManager.Instance.attack = false;
                    //GameManager.Instance.attack = false;
                }
            }
        }
    }

}
