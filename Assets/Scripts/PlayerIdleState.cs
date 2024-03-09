using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    //public GameObject radius;
    // Start is called before the first frame update
    public override void EnterState(PlayerStateManager player, GameObject radius)
    {
        //radius = GameObject.FindWithTag("Explosion");
        //radius.SetActive(false);
        Debug.Log("Hello from the Idle state");
        GameManager.Instance.movable = true;
    }

    public override void UpdateState(PlayerStateManager player, GameObject radius)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (GameManager.Instance.heat >=20.0f))
        {
            player.SwitchState(player.AttackingState);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            player.SwitchState(player.DashState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && (GameManager.Instance.heat >= 50.0f))
        {
           // radius.SetActive(true);
            player.SwitchState(player.ExplosionState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
