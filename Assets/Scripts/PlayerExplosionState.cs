using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosionState : PlayerBaseState
{
    // Start is called before the first frame update
    public Animator animator;
   // private GameObject radius;
    public override void EnterState(PlayerStateManager player, GameObject radius)
    {
        Debug.Log("Hello from the Explosion state");
        GameManager.Instance.movable = false;
        //radius = GameObject.FindWithTag("Explosion");
        radius.SetActive(true);
        animator = radius.GetComponent<Animator>();
        Exploding();
        GameManager.Instance.explosive = true;
    }
    void Exploding()
    {
        animator.SetTrigger("Explode");
    }
    //public void AlertObservers(string message)
    //{
        //if (message.Equals("Explodone"))
      //  {
           // done = true;
            //pc_anim.SetBool("attack", false);
            // Do other things based on an attack ending.
    //    }
  //  }
    public override void UpdateState(PlayerStateManager player, GameObject radius)
    {
        if(GameManager.Instance.explosive == false)
        {
            radius.SetActive(false);
            player.SwitchState(player.IdleState);
            
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
