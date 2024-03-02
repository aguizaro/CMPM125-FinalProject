using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    // Start is called before the first frame update
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello from the Idle state");
        GameManager.Instance.movable = true;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            player.SwitchState(player.AttackingState);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            player.SwitchState(player.DashState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
