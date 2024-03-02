using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    // Start is called before the first frame update
    //damage: 1 -> 5
    //enemy health: 3 -> 8
    //health bar
    private bool attacking = false;
    public GameObject Sword;
    public Animator animator;
    public override void EnterState(PlayerStateManager player)
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        Sword = GameObject.FindWithTag("Sword");
        animator = Sword.GetComponent<Animator>();
        Attack();
        //animator.GetComponent.Animation<>();
        GameManager.Instance.attack = true;
        GameManager.Instance.hit = true;
        GameManager.Instance.movable = false;
       // }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
    }
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

    public override void UpdateState(PlayerStateManager player)
    {
        if(GameManager.Instance.attack == false)
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
