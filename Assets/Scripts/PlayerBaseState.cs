using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player, GameObject radius);

    public abstract void UpdateState(PlayerStateManager player, GameObject radius);

    public abstract void OnCollisionEnter(PlayerStateManager player, Collision collision);
    //abstract state
    // Start is called before the first frame update

}
