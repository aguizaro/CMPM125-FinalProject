using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    public Animator animator;


    void Start()
    {
        GameManager.Instance.attack = false;
    }

    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }*/

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
        else if (message.Equals("Explodone"))
        {
            GameManager.Instance.explosive = false;
        }
    }

}
