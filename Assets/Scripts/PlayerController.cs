using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used unity's example code: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public Vector3 move;
    private bool groundedPlayer;
    private PlayerDash dash;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerBoost = 20f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityValue = -120f;
    [SerializeField] private float rotationSensitivity = 10f;


    //dash movement chain
    private float currentSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private bool keepMomentum;
    public float dashSpeedChangeFactor;
    //private MovementState lastState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        controller = gameObject.AddComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        dash = GetComponent<PlayerDash>();
        keepMomentum = false;
        GameManager.Instance.indashed = false;

    }

    void Update()
    {
        if (GameManager.Instance.movable == true)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            //rotate ur mom

            //rotate player left right (Y axis)
            float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // apply boost on shift
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerBoost : playerSpeed;
            desiredMoveSpeed = currentSpeed;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // local space to world space
            if (GameManager.Instance.dashing == true) //activate dash
            {
                dash.StartDash();
            }
            move = transform.TransformDirection(move);
            bool desiredMoveSpeedHasChange = desiredMoveSpeed != lastDesiredMoveSpeed;
            if (GameManager.Instance.indashed == true) {
                keepMomentum = true; speedChangeFactor = dashSpeedChangeFactor;
            }
            if (desiredMoveSpeedHasChange)
            {
                if (keepMomentum)
                {
                    Debug.Log("we made it>");
                    StopAllCoroutines();
                    StartCoroutine(SmoothlyLerpMoveSpeed());
                }
                else
                {
                    StopAllCoroutines();
                    currentSpeed = desiredMoveSpeed;
                }
            }
            controller.Move(move * currentSpeed * Time.deltaTime);


            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            // if (Input.GetMouseButtonDown(0))
            // {
            //attack logic
            //}

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            lastDesiredMoveSpeed = desiredMoveSpeed;
        }
    }
    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        Debug.Log("here here here");
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - currentSpeed);
        float startValue = currentSpeed;

        float boostFactor = speedChangeFactor;
        while (time < difference)
        {
            desiredMoveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }
        currentSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
}