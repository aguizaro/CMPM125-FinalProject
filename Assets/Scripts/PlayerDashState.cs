using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class PlayerDashState : PlayerBaseState
{
  //code from: https://www.youtube.com/watch?v=QRYGrCWumFw&t=663s
  //public float dashForce = 10f;

  //private Rigidbody rb;
  /// <summary>
  /// [Header("References")]
  /// </summary>
  //  public Transform orientation;
  /// public Transform playerCam;
  // private Rigidbody rb;
  // private PlayerController pm;
  // private Vector3 _previousPos;

  // [Header("Dashing")]
  //  public float dashForce;
  // public float dashUpwardForce;
  //  public float maxDashYSpeed;
  //  public float dashDuration;
  /// <summary>
  /// 
  //  [Header("Cooldown")]
  //  public float dashCd;
  //  private float dashCdTimer;
  //private Vector3 _currentPos;
  /// </summary>
  //private Vector3 _previousPos;
  //private Vector3 _currentPos;

  // Somewhere in update

  // Start is called before the first frame update
  //  private CharacterController controller;
  //private float playerSpeed = 10.0f;
  /// <summary>
  //public Vector3 move;
  /// </summary>
  //Vector3 moveDirection = Vector3.zero;
  /// <param name="player"></param>
  //public Vector3 moveDirection;
  //public float maxDashTime = 1.0f;
  //public float dashSpeed = 1.0f;
  //public float dashStoppingSpeed = 0.1f;
  //private float currentDashTime;
  public override void EnterState(PlayerStateManager player, GameObject radius)
  {
    //  rb = player.GetComponent<Rigidbody>();
    // pm = player.GetComponent<PlayerController>();
    //Debug.Log("I got through");
    GameManager.Instance.movable = true;
    GameManager.Instance.dashing = true;
    //controller = player.GetComponent<CharacterController>();
    // controller.enabled = false;
    //move = Vector3.forward;
    // currentDashTime = maxDashTime;
    //currentDashTime = 0.0f;
    // player.GetComponent<Rigidbody>().AddForce(10.0f);
    //player.GetComponent<GameController>
  }
  //private void Dash()
  //{
  //Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;
  //rb.AddForce(forceToApply,ForceMode.Impulse);
  //  Invoke(nameof(ResetDash), dashDuration);
  //}
  //private void ResetDash()
  // {

  //   }
  public override void UpdateState(PlayerStateManager player, GameObject radius)
  {
    if (!GameManager.Instance.dashing)
    {
      player.SwitchState(player.IdleState);
    }
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      player.SwitchState(player.AttackingState);
    }
    //  rb.AddForce(0, 0, dashForce, ForceMode.Impulse);
    //_previousPos = _currentPos;
    //Debug.Log("call me asparagus");
    //_currentPos = player.transform.position;
    // rb.AddForce(MoveDirection * dashForce, ForceMode.Impulse);

    // if (Input.GetKeyDown(KeyCode.LeftShift))
    //{
    ////     Cast();
    //  }
    //  if (currentDashTime < maxDashTime)
    //  {
    //      moveDirection = new Vector3(0, 0, dashSpeed);
    //      currentDashTime += dashStoppingSpeed;
    //  }
    //  else
    // {
    //     moveDirection = Vector3.zero;
    //     controller.Move(moveDirection * Time.deltaTime);
    //    Debug.Log("I am out");
    //player.SwitchState(player.IdleState);
    //  }
    //   controller.Move(moveDirection * Time.deltaTime);
    //controller.Move(move * playerSpeed * Time.deltaTime);
  }
  // public Vector3 MoveDirection
  //{
  //  get
  //{
  //  return (_currentPos - _previousPos).normalized; // The order might be incorrect, swap if positions if it is
  //}
  //}
  public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
  {

  }

}
