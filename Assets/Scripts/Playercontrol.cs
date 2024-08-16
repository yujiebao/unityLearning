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
    [SerializeField]private Camera playerEyes;
    private Vector3 jumpVector3;
    [SerializeField]
    private float jumpForce = 6f;
    private float gravity = -9.8f;
    private bool isOnGround = true;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask layerMask;
    private MyInputActions inputActions;    //玩家移动
    private InputAction scrollAction;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] guns;
    private int selectedWeapon = 0;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        // playerEyes = FindObjectOfType<Camera>();
        inputActions = new MyInputActions();
        inputActions.Enable();

        scrollAction = new InputAction("scroll",binding:"<Mouse>/scroll");
        scrollAction.Enable();

    }

    void Update()
    {
        animator = guns[selectedWeapon].GetComponent<Animator>();   //使用数组确定使用的武器使用的动画  也可写在SwitchWeapon中
        float horizontalInput = inputActions.Player.Move.ReadValue<Vector2>().x;
        float verticalInput = inputActions.Player.Move.ReadValue<Vector2>().y;
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput; 
        animator.SetFloat("Speed",Math.Abs(horizontalInput)+Math.Abs(verticalInput));
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

        // /// <summary>
        // /// 滑动滚轮        
        // /// </summary>
        // float scrollValue = scrollAction.ReadValue<Vector2>().y;
        // if(scrollValue > 0)   //向下滚动
        // {
        //     selectedWeapon = (selectedWeapon+1)%guns.Length;
        //     // Debug.Log("    " + selectedWeapon);
        //     // selectedWeapon++;
        //     // if(scrollValue == guns.Length)
        //     // {
        //     //     selectedWeapon = 0 ;
        //     // }
        // //    SwitchWeapon();
          
        // }
        // else if(scrollValue < 0)//向上滚动
        // {
        //      selectedWeapon = (selectedWeapon+guns.Length-1)%guns.Length;
        //     //  Debug.Log("    " + selectedWeapon);
        //     // selectedWeapon--;
        //     // if(scrollValue == -1)
        //     // {
        //     //     selectedWeapon = guns.Length - 1 ;
        //     // }
        //     //  SwitchWeapon();
        // }
        SwitchWeapon();
    } 
    
    void SwitchWeapon()
    {
        
       /// <summary>
        /// 滑动滚轮        
        /// </summary>
        float scrollValue = scrollAction.ReadValue<Vector2>().y;
        
        if(scrollValue != 0)
        {
            Gun nowGun = guns[selectedWeapon].GetComponent<Gun>();
            nowGun.isReloading = false;
        }
        
        if(scrollValue > 0)   //向下滚动
        {
            selectedWeapon = (selectedWeapon+1)%guns.Length;
            // Debug.Log("    " + selectedWeapon);
            // selectedWeapon++;
            // if(scrollValue == guns.Length) 
            // {
            //     selectedWeapon = 0 ;
            // }
        //    SwitchWeapon();
        }
        else if(scrollValue < 0)//向上滚动
        {
             selectedWeapon = (selectedWeapon+guns.Length-1)%guns.Length;
            //  Debug.Log("    " + selectedWeapon);
            // selectedWeapon--;
            // if(scrollValue == -1)
            // {
            //     selectedWeapon = guns.Length - 1 ;
            // }
            //  SwitchWeapon();
        }
        for(int i = 0; i < guns.Length; i++)
        {
            if(i == selectedWeapon)
            {
                guns[i].SetActive(true);
            }
            else
            {
                guns[i].SetActive(false);
            }
            // guns[i].SetActive(false);
        }
        // guns[selectedWeapon].SetActive(true);
        // animator = guns[selectedWeapon].GetComponent<Animator>();
    }
}