using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    private void Awake()
    {
        Application.targetFrameRate = 60;
        controller = gameObject.AddComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        dash = GetComponent<PlayerDash>();

    }

    void Update()
    {
        if (!GameManager.Instance.isActive) return;


        if (GameManager.Instance.movable == true)
        {

            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            //rotate player left right (Y axis)
            float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // apply boost on shift
            var currentSpeed = Input.GetKey(KeyCode.LeftShift) ? playerBoost : playerSpeed;

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // local space to world space
            move = transform.TransformDirection(move);
            controller.Move(move * currentSpeed * Time.deltaTime);


            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            if (GameManager.Instance.dashing == true) //activate dash
            {
                dash.StartDash();
            }
            // if (Input.GetMouseButtonDown(0))
            // {
            //attack logic
            //}

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}