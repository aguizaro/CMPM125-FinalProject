using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerDash : MonoBehaviour
{
  //main code by:https://www.youtube.com/watch?v=vTNWUbGkZ58
  PlayerController moveScript;
  private CharacterController controlls;
  //[Header("References")]
  //  public Transform orientation;
  //  public Transform playerCam;
  //  private Rigidbody rb;
  // private PlayerController pm;
  // private Vector3 _previousPos;
  public float dashSpeed;
  public float dashTime;
  public bool indashed = false;

  //  [Header("Dashing")]
  //  public float dashForce;
  //  public float dashUpwardForce;
  //  public float maxDashYSpeed;
  //  public float dashDuration;
  /// <summary>
  //  /// 
  //  [Header("Cooldown")]
  //   public float dashCd;
  //  private float dashCdTimer;

  private void Start()
  {
    // rb = GetComponent<Rigidbody>();
    //pm = GetComponent<PlayerController>();

    GameManager.Instance.dashing = false;

  }
  //private void Dash()
  //{
  //  Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;
  //rb.AddForce(forceToApply, ForceMode.Impulse);
  //Invoke(nameof(ResetDash), dashDuration);
  //GameManager.Instance.dashing = false;
  //}

  public void StartDash()
  {
    GameManager.Instance.dashing = false;
    controlls = GetComponent<CharacterController>();
    moveScript = GetComponent<PlayerController>();

    indashed = true;
    StartCoroutine(Dash());
    StartCoroutine(Cooldown(3));
  }

  IEnumerator Dash()
  {
    float startTime = Time.time;
    //indashed = true;
    while (Time.time < startTime + dashTime)
    {
      // moveScript.controller.Move(moveScript.move * dashSpeed * Time.deltaTime);
      //code from @lamnguyentung3227 on Youtube
      //  transform.Translate(Vector3.forward * dashSpeed);

      Vector3 forward = transform.TransformDirection(Vector3.forward);

      controlls.Move(forward * dashSpeed);
      yield return null;//new WaitForSeconds(time);
                        //indashed = false;
                        //  GameManager.Instance.dashing = false;
    }
  }
  IEnumerator Cooldown(float Time)
  {

    yield return new WaitForSeconds(Time);
    indashed = false;

  }
  //private void ResetDash()
  //{

  // }
}
