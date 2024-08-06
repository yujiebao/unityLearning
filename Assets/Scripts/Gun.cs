using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private float shootRate = 10f;
    [SerializeField] private float shootRange = 20f;
    [SerializeField] private float shootForce  = 150f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject Muzzle;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    private Camera eye;
    private InputAction shootAction;
    private float nextTimeToShoot = 0f;
    // Start is calletd before the first frame update
    void Start()
    {
      //新输入系统绑定按钮
        shootAction = new InputAction("Shoot",binding:"<mouse>/leftButton");
        shootAction .AddBinding("<Gamepad>/x");
        shootAction.Enable(); 

        eye = FindAnyObjectByType<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
      if(shootAction.IsPressed()&&Time.time > nextTimeToShoot)
      {
        audioSource.PlayOneShot(shootSound);
        nextTimeToShoot = Time.time + 1f/shootRate;  //shootRate 用于计算时间间隔
         muzzleFlash.Play();
         RaycastHit hitInfo;
         if(Physics.Raycast(eye.transform.position,eye.transform.forward,out hitInfo,shootRange))
          {
            if(hitInfo.rigidbody)
            {
                hitInfo.rigidbody.AddForce(Muzzle.transform.forward * shootForce);  //施加力先获取刚体
            }
            Instantiate(impactEffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal),hitInfo.transform);   //平面垂直于法线
            // Instantiate(impactEffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal),hitInfo.transform); 
            // // 使用三个参数的 Instantiate 方法实例化对象
            //  GameObject Hole = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            // // 将实例化对象设置为 hitInfo.transform 的子对象
            // Hole.transform.SetParent(hitInfo.transform);   或者 Hole.transform.parent = hitInfo.transform;
          }
                  
      
      }  
    }
}
