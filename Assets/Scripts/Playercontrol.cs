using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playercontrol : MonoBehaviour
{
    private CharacterController controller; 
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField] 
    private float mouseSensitivity = 10f;
    private float eyesAngleLimit; 
    private Camera playerEyes;
    private Vector3 jumpVector3;
    [SerializeField]
    private float jumpForce = 6f;
    private float gravity = -9.8f;
    private bool isOnGround = true;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask layerMask;
    private MyInputActions inputActions;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerEyes = FindObjectOfType<Camera>();
        inputActions = new MyInputActions();
        inputActions.Enable();
    }

    void Update()
    {
        float horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        float verticalInput = inputActions.Player.Move.ReadValue<Vector2>().y;
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;  
        controller.Move(moveDirection * Time.deltaTime * moveSpeed);

        float mouseX = inputActions.Player.Look.ReadValue<Vector2>().x;   
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * mouseSensitivity);   
        float mouseY = inputActions.Player.Look.ReadValue<Vector2>().y;
        eyesAngleLimit -= mouseY * Time.deltaTime * mouseSensitivity; 
        eyesAngleLimit = Mathf.Clamp(eyesAngleLimit, -80f, 80f);
        playerEyes.transform.localRotation = Quaternion.Euler(eyesAngleLimit, 0, 0);

        isOnGround = Physics.CheckSphere(groundCheck.transform.position, 0.3f, layerMask);     

        if (inputActions.Player.Jump.triggered && isOnGround)
        {
            jumpVector3.y = jumpForce;
        }
        else 
        {
            jumpVector3.y += gravity * Time.deltaTime;   
        }

        controller.Move(jumpVector3 * Time.deltaTime); 
    }
}