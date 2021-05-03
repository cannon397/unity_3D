using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.Scenes;
using Mono.Data.Sqlite;
using System;

public class Player : MonoBehaviour
{
    
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
        // rigid.AddForce(new Vector3(Input.GetAxis("Horizontal"),
        //0,
        //Input.GetAxis("Vertical")), ForceMode.Impulse);


    }
    void OnCollisionEnter(Collision collision)
    {



    }
    public void LookAround(Transform cameraArm, Transform camera, float camera_dstc, float mouse_dpi)
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * mouse_dpi, Input.GetAxis("Mouse Y") * mouse_dpi);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f) {x = Mathf.Clamp(x, -1f, 70f);}
        else {x = Mathf.Clamp(x, 290f, 361f);}

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

        RaycastHit hitinfo;
        if (Physics.Linecast(cameraArm.position, camera.position, out hitinfo))//레이케스트 성공시
        {
            //point로 옮긴다.
            camera.transform.position = hitinfo.point;
        }
        else
        {
            camera.localPosition = Vector3.Lerp(camera.localPosition, new Vector3(0, -1f, -4f).normalized * camera_dstc, Time.deltaTime * 3);
        }

    }
    public void Move(Transform cameraArm, float hAxis, float vAxis, bool wDown, Transform characterBody, Animator animator, bool jumpStatus, float speed, Rigidbody rigid, float jumpPower)
    {
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");

        

        Vector2 moveInput = new Vector2(hAxis, vAxis);
        bool isMove = moveInput.magnitude != 0;
        if (isMove)
        {
            //카메라 전면
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            //캐릭터 카메라 주시방향
            characterBody.forward = moveDir;
            characterBody.transform.position += moveDir * speed * (wDown ? 1f : 0.3f) * Time.deltaTime;
        }

        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //transform.position += moveVec * speed * (wDown ? 1f : 0.3f) * Time.deltaTime;

        animator.SetBool("isWalk", moveInput != Vector2.zero);
        animator.SetBool("isRun", wDown);
        //카메라
        
        //transform.LookAt(transform.position + moveVec);

        if (Input.GetButtonDown("Jump") && !jumpStatus)
        {
            jumpStatus = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
    //충돌
    public void CollisionFromChild(Collision collision)
    {
       
        //Debug.Log("호출여부");
        if (collision.gameObject.tag == "Floor")
        {
            //jumpStatus = false;
        }
    }
    void LateUpdate()
    {
        
    }
    private void PointOfView(bool viewPoint, string[] key_custom_arry, int viewPointFlag)
    {
        viewPoint = Input.GetButtonDown(key_custom_arry[0]);
        if (viewPoint)
        {
            if(viewPointFlag == 1)
            {
                
                viewPointFlag = 0;
            }
            else 
            {
                viewPointFlag = 1;
            }
            //viewPointFlag == 1 ? viewPointFlag = 0 : viewPointFlag = 1;
            
            Debug.Log("v 누름");
            Debug.Log(viewPointFlag);
        }
    }

    
}
