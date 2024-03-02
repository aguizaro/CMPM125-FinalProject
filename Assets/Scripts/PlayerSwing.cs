using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    public Animator animator;
    private bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.attack = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!attacking)
        //{
        if (!GameManager.Instance.attack) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                GameManager.Instance.attack = true;
                GameManager.Instance.hit = true;
              //  attacking = true;
            }
        }
        // (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Placeholder"))
      //  {
      //     Debug.Log("backto-");
        //    GameManager.Instance.attack = true;
            // Avoid any reload.
       // }
       // else
      //  {
      //      GameManager.Instance.attack = false;
       // }
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        //{
        //  GameManager.Instance.attack = false;
        //      attacking = false;
        //}
        //else
        //{
        //  GameManager.Instance.attack = false;
        // }
    }
    // IEnumerator MyCoroutine(float time)
    //{

    //}
    //Solution by https://gamedev.stackexchange.com/questions/117423/unity-detect-animations-end
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            GameManager.Instance.attack = false;
            GameManager.Instance.hit = false;
            //pc_anim.SetBool("attack", false);
            // Do other things based on an attack ending.
        }
    }
    //Made in china

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
