using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used unity's example code: https://docs.unity3d.com/ScriptReference/CharacterController.Move.html

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityValue = -120f;
    [SerializeField] private float rotationSensitivity = 10f;


    private void Start()
    {
        Application.targetFrameRate = 60;
        controller = gameObject.AddComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //rotate player left right (Y axis)
        float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        Vector3 move = new Vector3(0, 0, Input.GetAxis("Vertical"));
        // local space to world space
        move = transform.TransformDirection(move);
        controller.Move(move * playerSpeed * Time.deltaTime);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //attack logic
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}