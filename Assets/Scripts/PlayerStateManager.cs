using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //code idea from https://www.youtube.com/watch?v=Vt8aZDPzRjI&ab_channel=iHeartGameDev
    PlayerBaseState currentState;
    public PlayerBaseState IdleState = new PlayerIdleState();
    public PlayerAttackingState AttackingState = new PlayerAttackingState();
    public PlayerDashState DashState = new PlayerDashState();
    public PlayerExplosionState ExplosionState = new PlayerExplosionState();
    public GameObject radius;
    // Start is called before the first frame update
    void Start()
    {
        radius.SetActive(false);
        currentState = IdleState;
        currentState.EnterState(this, radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }
    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, radius);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this, radius);
    }
}
