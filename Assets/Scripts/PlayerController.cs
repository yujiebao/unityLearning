using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller; 
    // private Rigidbody rigidbody;
    // [SerializeField]
    // public int aa;
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField] 
    private float mouseSensitivity =100f;
    private float eyesAngleLimit ; 
    //[SerializeField]
    private Camera playerEyes;
    private Vector3 jumpVector3;
    [SerializeField]
    private float jumpForce = 10f;
    private float gravity =  -9.8f;
    private Boolean isOnGraund = true;
    [SerializeField] private GameObject grandCheck;
    [SerializeField] private LayerMask layerMask;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerEyes =FindObjectOfType<Camera>();
        // rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //获取轴线
           float horizontalinput = Input.GetAxis("Horizontal");    //x轴 左右
           float verticalinput = Input.GetAxis("Vertical");   //z轴 前后
        
        //控制移动
           //  tra. nsform.Translate(new Vector3(horizontalinput,0,verticalinput) * Time.deltaTime * 10);       无法检测到碰撞
           //  rigidbody.AddForce(new Vector3(horizontalinput,0,verticalinput) * Time.deltaTime * 10*aa);      具有较大的惯性难以控制
        
           //  controller.Move(new Vector3(horizontalinput,0,verticalinput) * Time.deltaTime * 10);    直接将参数声明在变量中
           Vector3 moveDirection = transform.forward * verticalinput + transform.right * horizontalinput;
           controller.Move(moveDirection * Time.deltaTime * moveSpeed);
        
        //控制角色旋转
           float mouseX = Input.GetAxis("Mouse X");   //左右旋转
           transform.Rotate(Vector3.up * mouseX * Time.deltaTime * mouseSensitivity);  
           float mouseY = Input.GetAxis("Mouse Y");     //上下旋转
        //    playerEyes.transform.Rotate(Vector3.left * mouseY * Time.deltaTime * mouseSensitivity);    //有问题:边界问题
           eyesAngleLimit -= mouseY * Time.deltaTime * mouseSensitivity; 
           eyesAngleLimit = Mathf.Clamp(eyesAngleLimit, -80f, 80f);
           // playerEyes.transform.Rotate(Vector3.right * eyesAngleLimit * Time.deltaTime * mouseSensitivity);   错误写法--- eyesAngleLimit为累加值若使用Rotate表示一直旋转 eyesAngleLimit的角会一直旋转         
           playerEyes.transform.localRotation = Quaternion.Euler(eyesAngleLimit,0,0);
        
        //检查落地
           isOnGraund = Physics.CheckSphere(grandCheck.transform.position,0.3f,layerMask);     //通过图层遮罩检测是否在空中
         
         //   controller.Move(jumpVector3 * Time.deltaTime); 
         //   isOnGraund = controller.isGrounded;    //使用Controller来检测落地，但注意此方法先运动后检测
           
        //跳跃
           if(Input.GetButtonDown("Jump")&&isOnGraund)
           {
               jumpVector3.y = jumpForce;
               
           }
           else 
           {
               jumpVector3.y += gravity * Time.deltaTime;   //玩家下落时考虑重力加速度
           }
           //下落
            controller.Move(jumpVector3 * Time.deltaTime); 


     }
}
