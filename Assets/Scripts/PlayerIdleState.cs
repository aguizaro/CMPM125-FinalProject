using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    //public GameObject radius;

    public const float SwingCost = 5.0f;
    public const float ExplosionCost = 30.0f;

    public override void EnterState(PlayerStateManager player, GameObject radius)
    {
        //radius = GameObject.FindWithTag("Explosion");
        //radius.SetActive(false);
        GameManager.Instance.movable = true;
    }

    public override void UpdateState(PlayerStateManager player, GameObject radius)
    {   // allow attack if heat is greater than 20
        if (Input.GetKeyDown(KeyCode.Mouse0) && (GameManager.Instance.heat >= SwingCost))
        {
            player.SwitchState(player.AttackingState);
            GameManager.Instance.heat -= SwingCost;

        }
        // allow dash - has cooldown 
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !GameManager.Instance.indashed)
        {
            player.SwitchState(player.DashState);
        }
        // allow explosion if heat is greater than 50
        else if (Input.GetKeyDown(KeyCode.E) && (GameManager.Instance.heat >= ExplosionCost))
        {
            // radius.SetActive(true);
            player.SwitchState(player.ExplosionState);
            GameManager.Instance.heat -= ExplosionCost;
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
